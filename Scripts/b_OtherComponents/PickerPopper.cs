//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------
using UnityEngine;
using System.Collections;

[RequireComponent ( typeof ( BoxCollider ) )]
public class PickerPopper : MonoBehaviour 
{
	public GameObject pickerColliderObject;
	public IPPickerBase picker;
	public UIWidget pickerBackground;
	public UIPanel cyclerPanel;
	
	public Vector2 openCloseScaleTweenDuration = new Vector2 ( 1f, 1f );
	public Vector2 openCloseClippingTweenDuration = new Vector2 ( 1.2f, .8f );
	
	public UIWidget widgetForBlinkConfirmation;
	public Color blinkColor;
	public float confirmBlinkDuration;
	
	public GameObject notifyWhenCollapsing;
	public string	  message = "OnPickerCollapsed";
	
	
	IPDragScrollView	 	_dragContents;
	IPUserInteraction 		_pickerInteraction;
	IPCycler 				_cycler;
	TweenPanelClipRange 	_clipRangeTween;
	TweenPanelClipSoftness _softnessTween;
	TweenHeight 			_scaleTween;
	
	BoxCollider _pickerCollider,
				_thisCollider;
	
	bool 		_waitBeforeClosing,
				_clippingIsTweening;
	
	Color 		_initBlinkColor;
	
	Vector3 _cyclerCachedPos;
	
	
	void Start ()
	{
		if ( widgetForBlinkConfirmation != null )
		{
			_initBlinkColor = widgetForBlinkConfirmation.color;
		}
		
		
		_cycler = cyclerPanel.gameObject.GetComponent ( typeof ( IPCycler ) ) as IPCycler;
		
		_dragContents = pickerColliderObject.GetComponent ( typeof ( IPDragScrollView ) ) as IPDragScrollView;
		_pickerInteraction = pickerColliderObject.GetComponent ( typeof ( IPUserInteraction ) ) as IPUserInteraction;
		
		_pickerCollider = pickerColliderObject.GetComponent ( typeof ( BoxCollider ) ) as BoxCollider;
		_thisCollider = gameObject.GetComponent ( typeof ( BoxCollider ) ) as BoxCollider;
		
		_clipRangeTween = cyclerPanel.gameObject.GetComponent ( typeof ( TweenPanelClipRange ) ) as TweenPanelClipRange;
		_softnessTween = cyclerPanel.gameObject.GetComponent ( typeof ( TweenPanelClipSoftness ) ) as TweenPanelClipSoftness;
		
		_scaleTween = pickerBackground.gameObject.GetComponent ( typeof ( TweenHeight ) ) as TweenHeight;
		
		cyclerPanel.baseClipRegion = _clipRangeTween.from;
		cyclerPanel.clipSoftness = _softnessTween.from;
		pickerBackground.height = ( int )_scaleTween.from;
		
		_dragContents.enabled = false;
		_pickerCollider.enabled = false;
	}
	
	public IEnumerator OpenPicker ()
	{
		_thisCollider.enabled = false;
		_scaleTween.duration = openCloseScaleTweenDuration.x;
		_scaleTween.Play ( true );
		
		EventDelegate.Set ( _clipRangeTween.onFinished, OnTweenClipRangeFinished );

		_clippingIsTweening = true;
		_clipRangeTween.duration = openCloseClippingTweenDuration.x;
		_clipRangeTween.Play ( true );
		
		if ( _softnessTween != null )
		{
			_softnessTween.duration = openCloseClippingTweenDuration.x;
			_softnessTween.Play ( true );
		}
		
		while ( _clippingIsTweening )
			yield return null;
		
		_dragContents.enabled = true;
		_pickerCollider.enabled = true;
		_pickerInteraction.onPickerClicked += PickerClicked;
		_cycler.onCyclerSelectionStarted += OnPickerSelectionStarted;
		_cycler.onCyclerStopped += OnPickerStopped;
	}
	
	public IEnumerator ClosePicker ()
	{
		_pickerCollider.enabled = false;
		_dragContents.enabled = false;
		_pickerInteraction.onPickerClicked -= PickerClicked;
		
		if ( widgetForBlinkConfirmation != null )
		{
			StartCoroutine ( BlinkWidget ( 2 ) );
		}
		
		yield return null;
		
		if ( _cycler.transform.localPosition == _cyclerCachedPos )
			_waitBeforeClosing = false;
		
		while ( _waitBeforeClosing )
		{
			yield return null;
			//Debug.Log ("waiting");
		}
			
		picker.ResetPicker ();
	
		_cycler.onCyclerSelectionStarted -= OnPickerSelectionStarted;
		_cycler.onCyclerStopped -= OnPickerStopped;
		
		_thisCollider.enabled = true;
		
		_scaleTween.duration = openCloseScaleTweenDuration.y;
		
		_scaleTween.Play ( false );
		_clipRangeTween.duration = openCloseClippingTweenDuration.y;
		_clipRangeTween.Play ( false );
		
		if ( _softnessTween != null )
		{
			_softnessTween.duration = openCloseClippingTweenDuration.y;
			_softnessTween.Play ( false );
		}
	}
	
	void OnClick ()
	{
		StartCoroutine ( OpenPicker () );
	}
	
	void PickerClicked ()
	{
		StartCoroutine ( ClosePicker() );
		
		if ( notifyWhenCollapsing != null )
			notifyWhenCollapsing.SendMessage ( message );
	}
	
	void OnPickerSelectionStarted ()
	{
		_cyclerCachedPos = _cycler.transform.localPosition;
		_waitBeforeClosing = true;
	}
	
	void OnPickerStopped ()
	{
		_waitBeforeClosing = false;
	}
	
	void OnTweenClipRangeFinished ()
	{
		_clippingIsTweening = false;
	}
	
	IEnumerator BlinkWidget( int nbOfBlinks )
	{
		for ( int i = 0; i < nbOfBlinks; i++ )
		{
			yield return StartCoroutine ( LerpBackgroundColor ( blinkColor, confirmBlinkDuration / 2 ) );
			yield return StartCoroutine ( LerpBackgroundColor ( _initBlinkColor, confirmBlinkDuration / 2 ) );
		}
	}
	
	IEnumerator LerpBackgroundColor ( Color targetColor, float duration )
	{
		Color initValue = widgetForBlinkConfirmation.color;
		float i = 0f;
		
		float rate = 1f / ( duration );
		
		while ( i < 1f )
		{
			i += Time.deltaTime * rate;
			widgetForBlinkConfirmation.color = Color.Lerp ( initValue, targetColor, i );
			yield return null;
		}
	}
}
