//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class IPNumberPicker : IPPickerLabelBase {
	
	public int 	min  = 0,
				max  = 9,
				step = 1,
				initValue;
	
	public string toStringFormat = "D2"; // how to format the int in labels - see http://msdn.microsoft.com/en-us/library/dwhawy9k.aspx for more details
	
	public int CurrentValue
	{
		get;
		private set;
	}
	
	public void ResetPickerAtValue ( int val )
	{
		UpdateVirtualElementsCount ();
		int valIndex = ValueToIndex ( val );
		
		if ( valIndex < 0 || valIndex >= _nbOfVirtualElements )
		{
			Debug.LogError ( "value out of picker range" );
			return;
		}
	
		ResetPickerAtIndex ( valIndex );
	}
	
	#region compulsory_overrides
	
	protected override int GetInitIndex ()
	{
		return ValueToIndex ( initValue );
	}
	
	protected override void UpdateCurrentValue ()
	{
		CurrentValue = VirtualIndexToValue ( _selectedIndex );
	}
	
	public override void UpdateWidget ( int widgetIndex, int contentIndex )
	{
		uiLabels[widgetIndex].text = VirtualIndexToValue ( contentIndex ).ToString ( toStringFormat );
	}
	
	public override void UpdateVirtualElementsCount ()
	{
		if ( max - min < step ) //max is exclusive, avoid division by zero
		{
			max = min + step + 1;
		}	
		
		_nbOfVirtualElements = ( max - min ) / step;
	}
	
	#endregion
	
	#region helper_methods
	int VirtualIndexToValue ( int index )
	{
		return ( min + index * step );
	}
	
	int ValueToIndex ( int val )
	{
		return ( val - min ) / step;
	}
	#endregion

}
