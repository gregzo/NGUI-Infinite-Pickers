//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class IPSpritePicker : IPPickerSpriteBase {
	
	public List < string > spriteNames;
	
	public bool 	normalizeSprites = true;
	public float 	normalizedMax = 50f;
	public int 		initIndex;
	
	public string CurrentSpriteName
	{
		get;
		private set;
	}
	
	public void ResetPickerAtContentIndex ( int index )
	{
		ResetPickerAtIndex ( index );
	}
	
	public void InsertElement ( string spriteName )
	{
		InsertElementAtIndex ( spriteName, _selectedIndex, true );
	}
	
	public void InsertElementAtIndex ( string spriteName, int index, bool resetPicker = true )
	{
		spriteNames.Insert ( index, spriteName );
		if ( resetPicker )
			ResetPickerAtIndex ( _selectedIndex );
	}
	
	public void RemoveElementAtIndex ( int index, bool resetPicker = true )
	{
		if ( _nbOfVirtualElements == 0 )
		{
			return;
		}
		spriteNames.RemoveAt ( index );
		if ( resetPicker )
			ResetPickerAtIndex ( IPTools.GetSelectedIndexAfterRemoveElement ( index, _selectedIndex, _nbOfVirtualElements - 1 ) );
	}
	
	#region compulsory_overrides
	
	protected override int GetInitIndex ()
	{
		return initIndex;
	}
	
	protected override void UpdateCurrentValue ()
	{
		CurrentSpriteName = spriteNames[_selectedIndex];
	}
	
	public override void UpdateWidget ( int widgetIndex, int contentIndex )
	{
		uiSprites[widgetIndex].spriteName = spriteNames[contentIndex];
		if ( normalizeSprites )
			IPTools.NormalizeWidget ( uiSprites[widgetIndex], normalizedMax );
	}
	
	public override void UpdateVirtualElementsCount ()
	{
		_nbOfVirtualElements = spriteNames == null ? 0 : spriteNames.Count;
	}
	
	#endregion
}
