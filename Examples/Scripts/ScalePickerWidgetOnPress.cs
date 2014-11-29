using UnityEngine;
using System.Collections;

/// <summary>
/// Example use of picker.GetWidgetAtScreenPos ( pos )
/// to tween the scale of any widget in the picker.
/// Attach to the picker's collider ( Background object ).
/// </summary>
public class ScalePickerWidgetOnPress : MonoBehaviour {
	
	public IPPickerBase picker;
	public float duration = .5f;
	public float scaleFactor = 1.2f;
	
	UIWidget _widget;
	TweenScale _tweenScale;
	
	void OnPress ( bool press )
	{
		if ( press )
		{
			Vector2 pos = new Vector2 ( Input.mousePosition.x, Input.mousePosition.y );
			_widget = picker.GetWidgetAtScreenPos ( pos );
			
			_tweenScale = _widget.gameObject.GetComponent ( typeof ( TweenScale ) ) as TweenScale;
			if ( _tweenScale == null )
			{
				AddScaleTween ( _widget.cachedGameObject );
			}
			
			_tweenScale.from = _widget.cachedTransform.localScale;
			_tweenScale.to = _tweenScale.from * scaleFactor;
			_tweenScale.Play ( true );
		}
		else
		{
			_tweenScale.Play ( false );
		}
	}
	
	void AddScaleTween ( GameObject go )
	{
		_tweenScale = _widget.gameObject.AddComponent ( typeof ( TweenScale ) ) as TweenScale;
		_tweenScale.enabled = false;
		_tweenScale.duration = duration;
	}
}
