using UnityEngine;
using System.Collections;

public class PickerScrollButton : MonoBehaviour {

	public IPCycler targetCycler; //The cycler to scroll
	public float 	scrollSpeed = 1f;
	
	public ScrollDirection scrollDirection; //Increase or decrease picker index?
		public enum ScrollDirection { Increase, Decrease }
	
	bool _pressed;
	
	void Awake ()
	{
		if ( targetCycler == null )
		{
			Debug.LogError ( "PickerScrollButton needs a target picker ( component type : WidgetPicker ) ");
			return;
		}
		
		scrollSpeed = Mathf.Abs ( scrollSpeed ); //Make sure scroll speed is positive before adjusting it according to scrollDirection

		if ( targetCycler.direction == IPCycler.Direction.Horizontal )
		{
			if ( scrollDirection == ScrollDirection.Decrease )
			{
				scrollSpeed = -scrollSpeed;
			}
		}
		else
		{
			if ( scrollDirection == ScrollDirection.Increase )
			{
				scrollSpeed = -scrollSpeed;
			}
		}
	}
	
	void OnPress ( bool press )
	{
		_pressed = press;
		
		if ( !press ) //Recenter on release
		{
			targetCycler.Recenter ();
		}
	}
	
	void OnClick () // AutoScroll
	{
		if (scrollDirection == ScrollDirection.Increase ) 
		{
			targetCycler.AutoScrollToNextElement ();
		} 
		else
		{
			 targetCycler.AutoScrollToPreviousElement ();
		}
	}
	
	void Update ()
	{
		if ( _pressed ) //Scroll when pressed
		{
			targetCycler.Scroll ( scrollSpeed );
		}
	}
}
