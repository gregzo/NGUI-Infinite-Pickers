//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------
using UnityEngine;
using System.Collections;

public class IPUserInteraction : MonoBehaviour {

	public IPCycler cycler;
	
	public bool restrictWithinPicker;
	public float exitRecenterDelay = .1f;
	
	bool _isScrolling;
	
	bool _isPressed;
	
	bool _isDraggingOutsideOfPicker;
	
	public delegate void 	OnPickerClicked ();
	public 					OnPickerClicked onPickerClicked;
	
	public delegate void 	OnDragExit ();
	public 					OnDragExit onDragExit;
	
	void OnPress ( bool press )
	{
		if ( _isPressed != press )
		{
			cycler.OnPress ( press );
			_isPressed = press;
		}
		
		if ( _isDraggingOutsideOfPicker && !press )
		{
			cycler.dragPanelContents.enabled = true;
			_isDraggingOutsideOfPicker = false;
			StopAllCoroutines ();
		}
			
	}
	
	void OnScroll ( float delta )
	{
		if ( _isScrolling == false )
		{
			ScrollWheelEvents.onScrollStartOrStop += ScrollStartOrStop;
			_isScrolling = true;
		}
	}
	
	void ScrollStartOrStop ( bool start )
	{
		if ( !start )
		{
			ScrollWheelEvents.onScrollStartOrStop -= ScrollStartOrStop;
			_isScrolling = false;
		}
		
		cycler.ScrollWheelStartOrStop ( start );
	}
	
	void OnClick ()
	{
		if ( onPickerClicked != null )
		{
			onPickerClicked ();
		}
	}
	
	void OnDrag ()
	{
		if ( restrictWithinPicker )
		{
			if ( !_isDraggingOutsideOfPicker )
			{
				if ( UICamera.currentTouch.current != this.gameObject )
				{
					_isDraggingOutsideOfPicker = true;
					if ( onDragExit != null ) //fire the event, listeners can implement their own delay
						onDragExit ();
					StartCoroutine ( DelayedDragExit () );
				}
			}
		}
	}
	
	IEnumerator DelayedDragExit () //Dont recenter the picker straight away, wait to allow for energetic picking without breaking momentum.
	{
		yield return new WaitForSeconds ( exitRecenterDelay );
		
		if ( !_isDraggingOutsideOfPicker )
			yield break;
			
		cycler.dragPanelContents.enabled = false;
		cycler.OnPress ( false );
		
	}
}
