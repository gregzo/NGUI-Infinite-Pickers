//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class IPPickerBase : MonoBehaviour {
	
	public string 			pickerName;
	public bool 			initInAwake = true;
	
	public int				widgetsDepth = 3;
	public Vector3 			widgetOffsetInPicker; 
	public UIWidget.Pivot 	widgetsPivot = UIWidget.Pivot.Center;
	public Color			widgetsColor = Color.white;
		
	public delegate void 	PickerValueUpdatedHandler ();
	public 					PickerValueUpdatedHandler onPickerValueUpdated;
	
	public IPCycler 		cycler;
	
	[SerializeField]
	protected int 			_nbOfWidgets;
	
	protected int			_nbOfVirtualElements,
						  	_selectedIndex;
	
	/// <summary>
	/// Gets the index of the selected value. Not relevant in dynamicaly updated pickers ( IPDatePicker, IPNumberPicker ).
	/// </summary>
	public int SelectedIndex 
	{
		get
		{
			return _selectedIndex;
		}
	}
	
	public int VirtualElementsCount
	{
		get
		{
			return _nbOfVirtualElements;
		}
	}
	
	public bool IsCyclerMoving 
	{
		get 
		{
			return cycler.IsMoving;
		}
	}
	
	protected virtual void Awake ()
	{	
		if ( Application.isPlaying )
		{
			if ( initInAwake )
			{	
				Setup ();
			}	
		}
		else
		{
			if ( initInAwake )
			{	
				Init ();
			}
		}
	}
	
	void OnDestroy ()
	{
		if ( Application.isPlaying )
		{
			cycler.onCyclerIndexChange -= CyclerIndexChange;
		}
	}

	//Called in awake if initInAwake. Else, you can call Setup manually.
	public void Setup ()
	{
		cycler.Init ();
		cycler.onCyclerIndexChange += CyclerIndexChange;
		Init ();
	}
	
	protected virtual void Init ()
	{
		if ( cycler == null )
		{
			cycler = GetComponentInChildren ( typeof ( IPCycler ) ) as IPCycler;
		}
		
		if ( WidgetsNeedRebuild () )
			MakeWidgetComponents ();
		
		UpdateVirtualElementsCount ();
		
		if ( _nbOfVirtualElements == 0 )
		{
			EnableWidgets ( false );
			return;
		}
			
		_selectedIndex = GetInitIndex ();
		UpdateCurrentValue ();
		ResetWidgetsContent ();
	}
	
	//derived classes provide wrappers to this method ( ResetPickerAtValue / date / etc...
	protected void ResetPickerAtIndex ( int index )
	{
		_selectedIndex = index;
		
		int prevNbOfElements = _nbOfVirtualElements;
		
		UpdateVirtualElementsCount ();
		
		if ( prevNbOfElements == 0 )
		{
			if ( _nbOfVirtualElements > 0 )
				EnableWidgets ( true );
		}
		else if (  _nbOfVirtualElements == 0 )
		{
			EnableWidgets ( false );
			return;
		}
		
		UpdateCurrentValue ();
		
		cycler.ResetCycler ();
		
		ResetWidgetsContent();
	}
	
	public void ResetPicker ()
	{
		ResetPickerAtIndex ( _selectedIndex );
	}
	//Called by delegate cycler.onCyclerIndexChange
	void CyclerIndexChange ( bool increment, int widgetIndex ) //called by cycler.onPickerIndexChange delegate
	{
		if ( _nbOfVirtualElements == 0 )
			return;
		
		if ( increment )
		{
			_selectedIndex = ( _selectedIndex + 1 ) % _nbOfVirtualElements;
		}
		else
		{
			_selectedIndex--;
			if ( _selectedIndex < 0 )
			{
				_selectedIndex += _nbOfVirtualElements;
			}
		}
		
		CycleWidgets ( increment, widgetIndex );
		
		UpdateCurrentValue ();
		
		if ( onPickerValueUpdated != null )
		{
			onPickerValueUpdated ();
		}
	}
	
	
	void CycleWidgets ( bool indexIncremented, int widgetIndex )
	{
		int newContentIndex;
		
		if ( indexIncremented )
		{
			newContentIndex = ( _selectedIndex + _nbOfWidgets / 2 ) % _nbOfVirtualElements;
		}
		else
		{
			newContentIndex = ( _selectedIndex - _nbOfWidgets / 2 ) % _nbOfVirtualElements;
			
			if ( newContentIndex < 0 )
			{
				newContentIndex += _nbOfVirtualElements;
			}
		}
		
		UpdateWidget ( widgetIndex, newContentIndex );
	}
	
	#region abstract methods
	public abstract UIWidget GetCenterWidget ();
	
	public abstract UIWidget GetWidgetAtScreenPos ( Vector2 pos );
	
	public abstract void EnableWidgets ( bool enable );
	
	public abstract void UpdateVirtualElementsCount ();
	
	public abstract void UpdateWidget ( int widgetIndex, int newContentIndex );
	
	protected abstract void MakeWidgetComponents (); 
	
	protected abstract void InitWidgets ();
	
	protected abstract int GetInitIndex ();
	
	protected abstract void UpdateCurrentValue ();
	
	protected abstract bool WidgetsNeedRebuild ();
	
	#endregion
	
	#region picker_building_methods
	//Called by editor script (IPCyclerInspector) via sendmessage to rebuild widgets when nb of transforms in picker changes
	protected void RebuildWidgets () 
	{
		if ( cycler == null )
		{
			cycler = GetComponentInChildren ( typeof ( IPCycler ) ) as IPCycler;
		}
		
		MakeWidgetComponents ();
		ResetWidgets ();
	}
	
	/// <summary>
	/// Not meant for public use.
	/// Used by editor script for WYSIWYG purposes.
	/// </summary>
	public void ResetWidgets ()
	{
		UpdateVirtualElementsCount ();
		if ( _nbOfVirtualElements == 0 )
			return;
		_selectedIndex  = GetInitIndex ();
		InitWidgets ();
		ResetWidgetsContent ();
	}
	
	// Updates all widgets, uset by editor scripts for WYSIWYG and by ResetPickerAtIndex. Use if you need to update the visible content of the picker.
	public void ResetWidgetsContent ()
	{
		int contentIndex = _selectedIndex - _nbOfWidgets / 2;
		
		if ( contentIndex < 0 )
		{
			contentIndex += _nbOfVirtualElements;
		}
		
		if ( contentIndex < 0 )
		{
			contentIndex = 0;
		}
		
		for ( int i = 0; i < _nbOfWidgets; i++ )
		{
			UpdateWidget ( i, contentIndex );
			contentIndex = ( contentIndex + 1 ) % _nbOfVirtualElements;
		}
	}
	
	#endregion
	
}
