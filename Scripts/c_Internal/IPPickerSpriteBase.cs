//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------
using UnityEngine;
using System.Collections;

public abstract class IPPickerSpriteBase : IPPickerBase {

	public UIAtlas atlas;
	
	[SerializeField]
	protected UISprite[] uiSprites;
	
	public override UIWidget GetCenterWidget ()
	{
		return uiSprites [ cycler.CenterWidgetIndex ];
	}
	
	public override UIWidget GetWidgetAtScreenPos ( Vector2 pos )
	{
		int index = cycler.GetIndexFromScreenPos ( pos );
		return uiSprites [index];
	}

	protected override void InitWidgets ()
	{
		for ( int i = 0; i < uiSprites.Length; i++ )
		{
			uiSprites[i].atlas = atlas;
			
			uiSprites[i].color = widgetsColor;
			uiSprites[i].pivot = widgetsPivot;
			uiSprites[i].cachedTransform.localPosition = widgetOffsetInPicker;
		}
	}
	
	
	protected override void MakeWidgetComponents ()
	{
		uiSprites = cycler.MakeWidgets < UISprite > ();
		
		foreach ( UISprite sprite in uiSprites )
		{
			sprite.depth = widgetsDepth;
		}
		
		_nbOfWidgets = uiSprites.Length;
	}
	
	public override void EnableWidgets (bool enable)
	{
		for ( int i = 0; i < uiSprites.Length; i++ )
		{
			uiSprites[i].enabled = enable;
		}
	}
	
	protected override bool WidgetsNeedRebuild ()
	{
		if ( uiSprites == null || uiSprites.Length == 0 )
			return true;
		
		for ( int i = 0; i < uiSprites.Length; i++ )
		{
			if ( uiSprites[i] == null )
				return true;
		}
		return false;
	}
}
