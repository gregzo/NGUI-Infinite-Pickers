//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class IPTools {
	
	public static int GetSelectedIndexAfterRemoveElement ( int removedIndex, int selectedIndex, int nbOfElements )
	{
		int newSelectedIndex = selectedIndex;
		
		if ( removedIndex <= selectedIndex )
		{
			newSelectedIndex = ( selectedIndex == 0 ) ? ( nbOfElements - 1 ) : ( selectedIndex - 1 );
			
			if ( newSelectedIndex < 0 )
			{
				newSelectedIndex = 0;
			}
		}
		
		return newSelectedIndex;
	}
	
	public static void NormalizeWidget ( UIWidget widget, float normalizedMax )
	{
		widget.MakePixelPerfect ();
		
		float ratio;
		
		if ( widget.height >= widget.width )
		{
			ratio = normalizedMax / widget.height;
		}
		else
		{
			ratio = normalizedMax / widget.width;
		}
		
		//widget.cachedTransform.localScale = new Vector3 ( widget.cachedTransform.localScale.x * ratio, widget.cachedTransform.localScale.y * ratio, 1f );
		widget.width  = ( int )( widget.width  * ratio );
		widget.height = ( int )( widget.height * ratio );
	}
}
