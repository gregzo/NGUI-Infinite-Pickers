using UnityEngine;
using System.Collections;

[RequireComponent ( typeof ( UISprite ) ) ]
public class DisplayPickerSprite : MonoBehaviour {
	
	public IPSpritePicker picker; // The picker to retrieve spriteName from

	public DisplayMode displayMode; public enum DisplayMode { MakePixelPerfect, Normalize, KeepScale };
	
	public float normalizedMax; // the maximum horizontal or vertical scale of the sprite, effective only if displayMode is set to Normalize
	
	UISprite _sprite;
	
	void Awake()
	{
		_sprite = gameObject.GetComponent ( typeof ( UISprite ) ) as UISprite;
	}
	
	public void DisplaySprite ()
	{
		_sprite.spriteName = picker.CurrentSpriteName;
		
		if ( displayMode == DisplayMode.Normalize )
		{
			IPTools.NormalizeWidget ( _sprite, normalizedMax );
		}
		else if ( displayMode == DisplayMode.MakePixelPerfect )
		{
			_sprite.MakePixelPerfect ();
		}
	}
}
