using UnityEngine;
using System.Collections;

public class TweenCenterWidgetScaleAndAlpha : MonoBehaviour {

	public IPPickerBase picker;
	
	public float scaleFactor = 1.1f;
	
	public float duration = .5f;
	
	TweenScale scaleTween;
	TweenAlpha alphaTween;
	
	UIWidget currentWidget;

	void Start ()
	{
		Grow ();
	}
	
	void Grow ()
	{
		currentWidget = picker.GetCenterWidget ();
		
		scaleTween = currentWidget.gameObject.GetComponent ( typeof ( TweenScale ) ) as TweenScale;
		if ( scaleTween == null )
		{
			AddTweeners ( );
		}
		else
		{
			alphaTween = currentWidget.gameObject.GetComponent ( typeof ( TweenAlpha ) ) as TweenAlpha;
		}
		
		scaleTween.Play ( true );
		alphaTween.Play ( true );
	}
			
	void Shrink ()
	{
		scaleTween.Play ( false );
		alphaTween.Play ( false );
	}
	
	void AddTweeners ( )
	{
		scaleTween = currentWidget.gameObject.AddComponent ( typeof ( TweenScale ) ) as TweenScale;
		alphaTween = currentWidget.gameObject.AddComponent ( typeof ( TweenAlpha ) ) as TweenAlpha;
		
		scaleTween.from = currentWidget.cachedTransform.localScale;
		scaleTween.to = new Vector3 ( currentWidget.cachedTransform.localScale.x * scaleFactor, currentWidget.cachedTransform.localScale.y * scaleFactor, currentWidget.cachedTransform.localScale.z );
		scaleTween.duration = duration;
		
		alphaTween.to = 1f;
		alphaTween.from = currentWidget.alpha;
		alphaTween.duration = duration;
	}
}
