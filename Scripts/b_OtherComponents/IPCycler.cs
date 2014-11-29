//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Cycles an odd number of transforms, horizontally or vertically
/// </summary>
/// 
[ RequireComponent ( typeof ( UIPanel ), typeof ( UIScrollView ) ) ]
[ExecuteInEditMode]
public class IPCycler : MonoBehaviour {
	
	//Inspector
	public 	Direction 	direction; public enum Direction { Vertical, Horizontal }
	
	public float 		spacing = 30f,	// The horizontal or vertical space between cycled transforms ( positive values only )
						dragScale = 1f, //Sets UIDraggablePanel.scale according to the direction of the cycler. If 0, UIDraggablePanel.scale will be left as is.
						recenterSpeedThreshold = .2f, //The minimum movement of the cycler panel before centering on the centermost transform ( high values will cut momentum )
						recenterSpringStrength = 8f;
	public bool			restrictDragToPicker;
	//Read only properties
	/// <summary>
	/// Gets or sets a value indicating whether this instance is moving. 
	/// Use onCyclerStopped delegate to be notified when cycler has finished recentering, 
	/// as IsMoving can be false for a frame between momentum and UICenterOnChildManual.CenterOnChild().
	/// </summary>
	public bool IsMoving { get; private set; }
	public int CenterWidgetIndex { get; private set; }
	public int RecenterTargetWidgetIndex { get; private set; }
	public int NbOfTransforms 
	{ 
		get 
		{ 
			return transform.childCount; 
		} 
	}
	
	public bool ClampIncrement{ get; set; }
	public bool ClampDecrement{ get; set; }
	
	#region Delegates
	public delegate void 	CyclerIndexChangeHandler ( bool increase, int indexToUpdate );
	public 					CyclerIndexChangeHandler onCyclerIndexChange;
	
	public delegate void 	CyclerStoppedHandler ();
	public 					CyclerStoppedHandler onCyclerStopped;
	
	public delegate void 	SelectionStartedHandler ();
	public 					SelectionStartedHandler onCyclerSelectionStarted;
	
	public delegate void 	CenterOnChildStartedHandler ();
	public 					CenterOnChildStartedHandler onCenterOnChildStarted;
	#endregion
	
	#region Private_Members

	public Transform[] 		_cycledTransforms;
	
	UIScrollView	 		_draggablePanel;
	[HideInInspector]
	public IPDragScrollView	dragPanelContents;
	[HideInInspector]
	public IPUserInteraction	userInteraction;
	UIPanel 				_panel;
	UICenterOnChild			_uiCenterOnChild;
	
	BoxCollider 			_pickerCollider; // If no collider is found, the cycler can still be controlled manually ( by PickerScrollButton, for example ). See #region ManualControls for relevant methods.

	int 	_initFrame = -1,
			_lastResetFrame = -1;
	
	float 	_decrementThreshold,
			_incrementThreshold,
			_transformJumpDelta,
			_panelSignificantPosVector,
			_panelPrevPos,
			_deltaPos;
	
	Vector3 _resetPosition;
	bool 	_isInitialized, _hasMovedSincePress; 
	[SerializeField]
	bool	_isHorizontal;
	[SerializeField]
	private float _trueSpacing; // Spacing adjusted according to cycler direction
	
	#endregion
	
	
	/// <summary>
	/// Not meant for public use. Called by IPPickers.
	/// </summary>
	public void Init()
	{
		//Prevent multiple Init in the same frame - for compound pickers
		if ( _initFrame == Time.frameCount ) 
		{
			return;
		}
		_initFrame = Time.frameCount;
		
		//Cache NGUI components
		_draggablePanel = gameObject.GetComponent ( typeof ( UIScrollView ) ) as UIScrollView;
		_panel = gameObject.GetComponent ( typeof ( UIPanel ) ) as UIPanel;
		
		//Make sure the parent is active before getting collider
		NGUITools.SetActive ( transform.parent.gameObject, true ); 
		
		//Look for a collider, and if one is found, add user interaction scripts
		if ( _pickerCollider == null )
			_pickerCollider = transform.parent.GetComponentInChildren ( typeof ( BoxCollider ) ) as BoxCollider;
		
		if ( _pickerCollider != null )
		{
			( dragPanelContents = _pickerCollider.gameObject.AddComponent ( typeof ( IPDragScrollView ) ) as IPDragScrollView ).scrollView = _draggablePanel;
			
			userInteraction = _pickerCollider.gameObject.AddComponent ( typeof ( IPUserInteraction ) ) as IPUserInteraction;
			userInteraction.cycler = this;
			userInteraction.restrictWithinPicker = restrictDragToPicker;
		}
		else Debug.Log ("Could not get collider" );
		
		//Add and subscribe to the recenter component
		_uiCenterOnChild = gameObject.AddComponent ( typeof ( UICenterOnChild ) ) as UICenterOnChild;
		_uiCenterOnChild.enabled = false;
		_uiCenterOnChild.springStrength = recenterSpringStrength;
		_uiCenterOnChild.onFinished = PickerStopped;
		
		//Check if the ScrollWheelEvents singleton is in the scene
		ScrollWheelEvents.CheckInstance ();
			
		//Adapt the DraggablePanel's drag scale to the cycler's direction
		if ( dragScale != 0f )
		{
			_draggablePanel.movement = _isHorizontal ? UIScrollView.Movement.Horizontal : UIScrollView.Movement.Vertical;
		}
		
		//Iniitialize a bunch of variables
		_resetPosition = _panel.cachedTransform.localPosition;
		
		//was clipRange
		_panelPrevPos = SignificantPosVector ( _panel.cachedTransform );
		
		_transformJumpDelta = -_trueSpacing * NbOfTransforms; //how much should the cycled transforms move when re-cycling (incrementing) ? 
		
		CenterWidgetIndex = NbOfTransforms / 2;
		_decrementThreshold = -_trueSpacing / 2;
		_incrementThreshold = _trueSpacing / 2;
		_deltaPos = 0f;
		
		RecenterTargetWidgetIndex = NbOfTransforms / 2;
		
		_isInitialized = true;
	}
	
	/// <summary>
	/// Called by IPPickerBase to reset the cycler - not meant for public use.
	/// </summary>
	public void ResetCycler ()
	{
		if ( _lastResetFrame == Time.frameCount ) //prevent resetting twice in the same frame ( for compound pickers )
			return;
		_lastResetFrame = Time.frameCount;
		
		if ( _panel == null )
		{
			_panel = gameObject.GetComponent ( typeof ( UIPanel ) ) as UIPanel;
		}
		//Reset the panel's position and clipping
		_panel.cachedTransform.localPosition = _resetPosition; 
		_panel.clipOffset = new Vector2( 0f, 0f );
		_panelPrevPos = SignificantPosVector ( _panel.cachedTransform );
		
		//Space the transforms
		PlaceTransforms ();
		
		//Reset various members
		CenterWidgetIndex = NbOfTransforms / 2;
		_decrementThreshold = -_trueSpacing / 2;
		_incrementThreshold = _trueSpacing / 2;
		_deltaPos = 0f;
	}
	
	
	void Update ()
	{		
		if ( !_isInitialized )
		{
			return;
		}
		
		_panelSignificantPosVector = SignificantPosVector ( _panel.cachedTransform );
		
		if ( Mathf.Approximately ( _panelSignificantPosVector, _panelPrevPos ) == false )
		{
			IsMoving = true;
			_hasMovedSincePress = true;
			
			_deltaPos =  _panelSignificantPosVector - _panelPrevPos;
			
			if ( _isHorizontal )
			{
				_deltaPos = -_deltaPos;
			}
			
			if ( _deltaPos > 0 )
			{	
				while ( TryIncrement () )
				{
					
				}
			}
			else
			{
				while ( TryDecrement () )
				{
					
				}
			}
		}
		else if ( IsMoving ) 
		{
			IsMoving = false;
			_deltaPos = 0f;
		}
		
		_panelPrevPos = _panelSignificantPosVector;
	}
	
	//Called by UICenterOnChildAuto.onFinished ( forwarding delegate, notably to IPForwardPickerEvents )
	void PickerStopped ()
	{
		if ( onCyclerStopped != null )
		{
			onCyclerStopped ();
		}
	}
	
	/// <summary>
	///	The following methods can be used to manually handle the cycler.
	/// See PickerScrollButton for an example.
	/// </summary>
	#region Manual_Controls
	public void Scroll ( float delta )
	{
		_draggablePanel.Scroll ( delta );
	}
	
	public void Recenter ()
	{
		CenterOnTransformIndex ( CenterWidgetIndex );
	}
	
	/// <summary>
	/// Automatically scroll the picker to the next element 
	/// </summary>
	public void AutoScrollToNextElement ()
	{
		int targetIndex = ( CenterWidgetIndex + 1 ) % NbOfTransforms;
		CenterOnTransformIndex ( targetIndex );
	}
	
	/// <summary>
	/// Automatically scroll the picker to the previous element
	/// </summary>
	public void AutoScrollToPreviousElement ()
	{
		int targetIndex = ( CenterWidgetIndex - 1 );
		if ( targetIndex < 0 )
			targetIndex += NbOfTransforms;
		
		CenterOnTransformIndex ( targetIndex );
	}
	
	public void AutoScrollBy ( int delta )
	{
		if ( Mathf.Abs ( delta ) > NbOfTransforms / 2 )
		{
			Debug.LogWarning (" Cannot auto-scroll more than NbOfTransforms / 2 at once ");
		}
		delta = Mathf.Clamp ( delta, -NbOfTransforms / 2, NbOfTransforms / 2 );
		
		int targetIndex = ( CenterWidgetIndex + delta ) % NbOfTransforms;
		
		if ( targetIndex < 0 )
			targetIndex += NbOfTransforms;
		
		CenterOnTransformIndex ( targetIndex );
	}
	
	public int GetDeltaIndexFromScreenPos ( Vector2 pos )
	{
		Vector3 onClickTouchPosInWorld = UICamera.currentCamera.ScreenToWorldPoint ( new Vector3 ( pos.x, pos.y, 0f ) );
		Vector3 onClickTouchPosInPicker = transform.parent.InverseTransformPoint ( onClickTouchPosInWorld );
		
		float distanceFromCenter;
		
		if ( direction == Direction.Horizontal )
		{
			distanceFromCenter = onClickTouchPosInPicker.x;
		}
		else
		{
			distanceFromCenter = onClickTouchPosInPicker.y;
		}
		
		distanceFromCenter = distanceFromCenter >= 0 ? distanceFromCenter + spacing / 2 : distanceFromCenter - spacing / 2;
		
		int deltaIndex = (int) distanceFromCenter / ( int )spacing;
		
		if ( direction == Direction.Vertical ) //Thank you loverainstm ;-)
		{
			deltaIndex = -deltaIndex;
		} 
		
		deltaIndex = Mathf.Clamp ( deltaIndex, -NbOfTransforms / 2, NbOfTransforms / 2 );
		
		return deltaIndex;
	}
	
	public int GetIndexFromScreenPos ( Vector2 pos )
	{
		int deltaIndex = GetDeltaIndexFromScreenPos ( pos );
		int index = ( CenterWidgetIndex + deltaIndex ) % NbOfTransforms;
		if ( index < 0 )
			index += NbOfTransforms;
		return index;
	}
	
	#endregion
	
	#region User_Interaction
	/// <summary>
	/// Called by IPUserInteraction to forward press events. Not for public use!
	/// </summary>
	public void OnPress ( bool press )
	{
		if ( press )
		{
			_hasMovedSincePress = false;
			StopAllCoroutines (); // Stop Recenter coroutine on press
			//uiCenterOnChildManual legacy
			//_uiCenterOnChild.Abort ();
			if ( onCyclerSelectionStarted != null ) 
				onCyclerSelectionStarted ();
		}
		else
		{
			if ( _hasMovedSincePress )
			{
				StartCoroutine ( RecenterOnThreshold () ); // Launch Recenter coroutine on release
			}
		}
	}
	
	/// <summary>
	/// Called by IPUserInteraction to forward scrollwheel events. Not for public use!
	/// </summary>
	public void ScrollWheelStartOrStop ( bool start )
	{
		if ( start )
		{
			StopAllCoroutines();
			
			if ( onCyclerSelectionStarted != null )
				onCyclerSelectionStarted ();
		}
		else
		{
			StartCoroutine ( RecenterOnThreshold () );
		}
	}
	
	/// <summary>
	/// Wait until the cycler has slowed down enough before recentering.
	/// </summary>
	IEnumerator RecenterOnThreshold ()
	{
		while ( Mathf.Abs ( _deltaPos ) > recenterSpeedThreshold ) // let the momentum carry on 
		{
			yield return null;
		}
		//
		CenterOnTransformIndex(CenterWidgetIndex); //Recenter
	}
	
	void CenterOnTransformIndex ( int index )
	{
		RecenterTargetWidgetIndex = index;
		_uiCenterOnChild.CenterOn (  _cycledTransforms[index] );
		if ( onCenterOnChildStarted != null )
			onCenterOnChildStarted ();
	}
	
	#endregion

	#region Cycling_Helper_Methods
	float SignificantPosVector ( Transform trans )
	{
		return _isHorizontal ? trans.localPosition.x : trans.localPosition.y ;
	}
	
	void SetWidgetSignificantPos ( Transform trans, float pos )
	{
		if ( !_isHorizontal )
		{
			trans.localPosition = new Vector3 ( 0f, pos , trans.localPosition.z );
		}
		else
		{
			trans.localPosition = new Vector3 ( pos, 0f, trans.localPosition.z );
		}
	}
	
	bool TryIncrement ()
	{
		if( ClampIncrement )
		{
			return false;
		}
		
		if ( _isHorizontal )
		{
			if ( ! ( _panelSignificantPosVector < _incrementThreshold ) )
			{		
				return false;
			}
		}
		else 
		{
			if ( ! ( _panelSignificantPosVector > _incrementThreshold ) )
			{
				return false;
			}
		}
	
		_incrementThreshold += _trueSpacing;
		_decrementThreshold += _trueSpacing;
		
		int firstWidgetIndex;
		Transform firstWidget = FirstWidget ( out firstWidgetIndex );
		SetWidgetSignificantPos ( firstWidget, SignificantPosVector ( firstWidget ) + _transformJumpDelta );
		CenterWidgetIndex = ( CenterWidgetIndex + 1 ) % NbOfTransforms;
		
		if ( onCyclerIndexChange != null )
		{
			onCyclerIndexChange ( true, firstWidgetIndex );
		}
		
		return true;
	}
	
	bool TryDecrement ()
	{
		if( ClampDecrement )
		{
			return false;
		}
		
		if ( _isHorizontal )
		{
			if ( !( _panelSignificantPosVector > _decrementThreshold ) )
			{
				return false;
			}
		}
		else 
		{
			if ( !( _panelSignificantPosVector < _decrementThreshold ) )
			{
				return false;
			}
		}
		
		_incrementThreshold -= _trueSpacing;
		_decrementThreshold -= _trueSpacing;
		
		int lastWidgetIndex;
		Transform lastWidget = LastWidget ( out lastWidgetIndex );
		SetWidgetSignificantPos ( lastWidget, SignificantPosVector ( lastWidget ) - _transformJumpDelta );
		CenterWidgetIndex = ( CenterWidgetIndex - 1 );
		
		if ( CenterWidgetIndex < 0 )
		{
			CenterWidgetIndex += NbOfTransforms;
		}
		
		if ( onCyclerIndexChange != null )
		{
			onCyclerIndexChange ( false, lastWidgetIndex );
		}
			
		return true;
	}
	
	Transform FirstWidget ( out int firstWidgetIndex )
	{
		firstWidgetIndex = CenterWidgetIndex - NbOfTransforms / 2;
		
		if ( firstWidgetIndex < 0 )
		{
			firstWidgetIndex += NbOfTransforms;
		}
		
		return _cycledTransforms[ firstWidgetIndex ];
	}
	
	Transform LastWidget (out int lastWidgetIndex)
	{
		lastWidgetIndex = ( CenterWidgetIndex + NbOfTransforms / 2 ) % NbOfTransforms;
		
		return _cycledTransforms[ lastWidgetIndex ];
	}
	
	#endregion
	
	#region cycler_building
	/// <summary>
	/// Not meant for public use.
	/// Used by editor script for WYSIWYG purposes.
	/// </summary>
	public void EditorInit ()
	{	
		DestroyChildrenOfChildren ();
		if ( _cycledTransforms == null || _cycledTransforms.Length != NbOfTransforms )
		{
			_cycledTransforms = new Transform [ NbOfTransforms ];
		}
		
		for ( int i = 0; i < NbOfTransforms; i++ )
		{
			_cycledTransforms[i] = transform.GetChild ( i );
		}
		
		UpdateTrueSpacing ();
	}
	
	/// <summary>
	/// Not meant for public use.
	/// Used by editor script for WYSIWYG purposes.
	/// </summary>
	public void UpdateTrueSpacing ()
	{
		_isHorizontal = ( direction == Direction.Horizontal );
		
		_trueSpacing = Mathf.Abs ( spacing ); //prevent negative values
		
		if ( _isHorizontal )
		{
			_trueSpacing = -_trueSpacing; //if horizontal, increasing panel's x position moves the cycler backwards. A negative spacing allows for less duplicate code.
		}
		
		PlaceTransforms ();
	}
	
	/// <summary>
	/// Not meant for public use.
	/// Used by editor script for WYSIWYG purposes and by ResetPicker.
	/// </summary>
	public void PlaceTransforms ()
	{
		float transSignificantPos = _trueSpacing * ( NbOfTransforms / 2 ); 
		
		for ( int i = 0; i < NbOfTransforms; i++ )
		{
			SetWidgetSignificantPos ( _cycledTransforms[i], transSignificantPos );
			transSignificantPos -= _trueSpacing;
		}
	}
	/// <summary>
	/// Not meant for public use.
	/// Used by editor script for WYSIWYG purposes.
	/// </summary>
	public void RebuildTransforms ( int newNb )
	{	
		int initNbOfTr = NbOfTransforms;
		int i;
		
		if ( initNbOfTr != 0 )
		{
			Transform[] tmp = new Transform [ initNbOfTr ];
			
			
			for ( i = 0; i < initNbOfTr; i++ )
			{
				tmp[i] = transform.GetChild ( i );
			}
			
			for ( i = 0; i < initNbOfTr; i++ )
			{
				DestroyImmediate ( tmp[i].gameObject );
			}
		}
		
		_cycledTransforms = new Transform[ newNb ];
		
		for ( i = 0; i < newNb; i++ )
		{
			GameObject go = new GameObject();
			go.transform.parent = transform;
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = Vector3.zero;
			go.name = i.ToString();
			_cycledTransforms[i] = go.transform;
		}
		
		PlaceTransforms ();
	}
	
	void DestroyChildrenOfChildren ()
	{
		foreach ( Transform t in transform )
		{
			Transform[] children = new Transform[t.childCount];
			for ( int i = 0; i < t.childCount; i++ )
			{
				children[i] = t.GetChild (i);
			}
			
			for ( int j = 0; j < children.Length; j++ )
			{
				DestroyImmediate ( children[j].gameObject );
			}
		}
	}
	
	/// <summary>
	/// Called by IPPickerBase.GetOrMakeWidgetComponents
	/// Flags and returns the requested widget, or makes and returns it
	/// if it doesn't already exist.
	/// </summary>
	public T[] MakeWidgets < T >  () where T : UIWidget
	{
		T[] ret = new T[ NbOfTransforms ];
		
		for ( int i = 0; i < NbOfTransforms; i++ )
		{
			ret[i] = NGUITools.AddWidget < T > ( _cycledTransforms[i].gameObject );
		}
		
		return ret;
	}
	
	#endregion
}
