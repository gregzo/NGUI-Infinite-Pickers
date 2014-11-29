//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------

using UnityEngine;
using System.Collections;

public abstract class IPPickerLabelBase : IPPickerBase 
{
	public UIFont font;
	
	[SerializeField]
	protected UILabel[] uiLabels;
	
	public override UIWidget GetCenterWidget ()
	{
		return uiLabels [ cycler.CenterWidgetIndex ];
	}
	
	public override UIWidget GetWidgetAtScreenPos ( Vector2 pos )
	{
		int index = cycler.GetIndexFromScreenPos ( pos );
		return uiLabels [index];
	}
	
	protected override void InitWidgets ()
	{
		for ( int i = 0; i < uiLabels.Length; i++ )
		{
			uiLabels[i].bitmapFont = font;
			uiLabels[i].MakePixelPerfect ();
			
			uiLabels[i].color = widgetsColor;
			uiLabels[i].pivot = widgetsPivot;
			uiLabels[i].cachedTransform.localPosition = widgetOffsetInPicker;
		}
	}
	
	protected override void MakeWidgetComponents ()
	{
		uiLabels = cycler.MakeWidgets < UILabel > ();
		
		foreach ( UILabel label in uiLabels )
		{
			label.depth = widgetsDepth;
		}
		
		_nbOfWidgets = uiLabels.Length;
	}
	
	public override void EnableWidgets (bool enable)
	{
		for ( int i = 0; i < uiLabels.Length; i++ )
		{
			uiLabels[i].enabled = enable;
		}
	}
	
	protected override bool WidgetsNeedRebuild ()
	{
		if ( uiLabels == null || uiLabels.Length == 0 )
			return true;
		
		for ( int i = 0; i < uiLabels.Length; i++ )
		{
			if ( uiLabels[i] == null )
				return true;
		}
		return false;
	}
}
