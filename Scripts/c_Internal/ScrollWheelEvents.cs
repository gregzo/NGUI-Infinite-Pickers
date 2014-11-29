using UnityEngine;
using System.Collections;

public class ScrollWheelEvents : MonoBehaviour {

	static ScrollWheelEvents _instance;
	
	public delegate void ScrollStartStopHandler ( bool start );
	public static ScrollStartStopHandler onScrollStartOrStop;

	
	bool _isScrolling;
	
	void Awake ()
	{
		if ( _instance != null )
		{
			Debug.LogWarning ( "ScrollWheelEvents is pseudo-singleton, destroying new instance" );
			Destroy ( this );
			return;
		}
		
		_instance = this;
	}
	
	public static void CheckInstance ()
	{
		if ( _instance == null )
		{
			GameObject go = new GameObject ( "ScrollWheelEvents" );
			go.AddComponent ( typeof ( ScrollWheelEvents ) );
		}
	}
	
	void Update ()
	{
		float scrollDelta = Input.GetAxis ( "Mouse ScrollWheel" );
		if ( scrollDelta != 0f ) 
		{
			if ( _isScrolling == false )
			{
				_isScrolling = true;
				if ( onScrollStartOrStop != null )
				{
					onScrollStartOrStop ( true );
				}
			}
		}
		else if ( _isScrolling )
		{
			_isScrolling = false;
			
			if ( onScrollStartOrStop != null )
			{
				onScrollStartOrStop ( false );
			}
		}
	}
}
