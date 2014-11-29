//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class IPTexturePicker : IPTexturePickerBase {
	
	public List < Texture > textures;
	
	public bool 	normalizeTextures = true;
	public float 	normalizedMax = 50f;
	public int 		initIndex;
	
	public Texture CurrentTexture
	{
		get;
		private set;
	}
	
	public void ResetPickerAtContentIndex ( int index )
	{
		ResetPickerAtIndex ( index );
	}
	
	public void InsertElement ( Texture texture )
	{
		InsertElementAtIndex ( texture, _selectedIndex, true );
	}
	
	public void InsertElementAtIndex ( Texture texture, int index, bool resetPicker = true )
	{
		textures.Insert ( index, texture );
		if ( resetPicker )
			ResetPickerAtIndex ( _selectedIndex );
	}
	
	public void RemoveElementAtIndex ( int index, bool resetPicker = true )
	{
		if ( _nbOfVirtualElements == 0 )
		{
			return;
		}
		textures.RemoveAt ( index );
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
		CurrentTexture = textures[_selectedIndex];
	}
	
	public override void UpdateWidget ( int widgetIndex, int contentIndex )
	{
		uiTextures[widgetIndex].mainTexture = textures[contentIndex];
		if ( normalizeTextures )
			IPTools.NormalizeWidget ( uiTextures[widgetIndex], normalizedMax );
	}
	
	public override void UpdateVirtualElementsCount ()
	{
		_nbOfVirtualElements = textures == null ? 0 : textures.Count;
	}
	

	
	#endregion
	
	

}
