//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class IPTextPicker : IPPickerLabelBase {
	
	public List < string > labelsText;
	
	public int initIndex;
	
	public string CurrentLabelText
	{
		get;
		private set;
	}
	
	
	public void InsertElement ( string text )
	{
		InsertElementAtIndex ( text, _selectedIndex, true );
	}
	
	public void InsertElementAtIndex ( string text, int index, bool resetPicker = true )
	{
		labelsText.Insert ( index, text );
		if ( resetPicker )
			ResetPickerAtIndex ( _selectedIndex );
	}
	
	public void RemoveElementAtIndex ( int index, bool resetPicker = true )
	{
		if ( _nbOfVirtualElements == 0 )
		{
			return;
		}
		
		labelsText.RemoveAt ( index );
		
		if ( resetPicker )
			ResetPickerAtIndex ( IPTools.GetSelectedIndexAfterRemoveElement ( index, _selectedIndex, _nbOfVirtualElements - 1 ) );
	}
	
	public void ResetPickerAtText ( string labelText )
	{
		int index = labelsText.IndexOf ( labelText );
		if ( index < 0 )
		{
			Debug.LogError ( "string not in picker" );
			return;
		}
		ResetPickerAtIndex ( index );
	}
	
	public void ResetPickerAtContentIndex ( int index )
	{	
		ResetPickerAtIndex ( index );
	}
	
	#region compulsory_overrides
	
	protected override int GetInitIndex ()
	{
		return initIndex;
	}
	
	protected override void UpdateCurrentValue ()
	{
		CurrentLabelText = labelsText [_selectedIndex];
	}
	
	public override void UpdateWidget ( int widgetIndex, int contentIndex )
	{
		uiLabels[widgetIndex].text = labelsText[contentIndex];
	}
	
	public override void UpdateVirtualElementsCount ()
	{
		_nbOfVirtualElements = labelsText == null ? 0 : labelsText.Count;
	}
	
	#endregion
	
}
