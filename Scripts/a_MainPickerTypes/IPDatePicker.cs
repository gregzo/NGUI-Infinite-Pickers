//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class IPDatePicker : IPPickerLabelBase 
{
	public DateTimeLocalization.Language language = DateTimeLocalization.Language.English;
	
	public Date pickerMinDate = new Date ( 1, 1, 1900 ),
				pickerMaxDate = new Date ( 31, 12, 2199 ),
				initDate; //if day == 0, init at today
	
	public string dateFormat = "ddd d MMM yyyy"; //please see http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx for details on format specifiers
	
	public DateTime CurrentDate { get; private set; }

	DateTime _minDate;
	
	
	
	public bool TryResetPickerAtDate ( Date date )
	{	
		if ( TryResetPickerAtDateTime ( date.GetDateTime () ) == false )
		{
			return false;
		}
		
		return true;
	}
	
	public bool TryResetPickerAtDateTime ( DateTime dateTime )
	{
		int index = GetIndexForDateTime ( dateTime );
		
		if ( index < 0 || index >= _nbOfVirtualElements )
		{
			return false;
		}

		ResetPickerAtIndex ( index );
		
		return true;
	}
	
	public void SetNewLanguage ( DateTimeLocalization.Language newLanguage )
	{
		if ( DateTimeLocalization.SetLanguage ( newLanguage ) )
		{
			language = newLanguage;
			ResetPickerAtIndex ( _selectedIndex );	
		}
	}
	
	public void SetNewLanguage ( string languageString ) // Call via sendmessage
	{
		if ( DateTimeLocalization.TrySetLanguage ( languageString ) )
		{
			ResetPickerAtIndex ( _selectedIndex );
		}
		
		language = DateTimeLocalization.CurrentLanguage;
	}
		
	// Init overriden : date requires language initialization
	protected override void Init ()
	{
		UpdateVirtualElementsCount ();
		_selectedIndex = GetInitIndex ();
		SetNewLanguage ( language );
	}
	
	#region compulsory_overrides

	protected override int GetInitIndex ()
	{
		if ( initDate.day == 0 || initDate.month == 0 || initDate.year == 0 )
		{
			CurrentDate = DateTime.Now;
		}
		else
		{
			CurrentDate = initDate.GetDateTime();
		}
		
		return GetIndexForDateTime ( CurrentDate );
	}
	
	protected override void UpdateCurrentValue ()
	{
		CurrentDate = GetDateTimeForIndex ( _selectedIndex );
	}
	
	public override void UpdateVirtualElementsCount ()
	{
		_minDate = pickerMinDate.GetDateTime();
		DateTime tempMaxDate = pickerMaxDate.GetDateTime();
		TimeSpan timeSpan = tempMaxDate.Subtract ( _minDate );
		
		_nbOfVirtualElements = (int) timeSpan.Days;
	}
	
	public override void UpdateWidget ( int widgetIndex, int contentIndex )
	{
		uiLabels[widgetIndex].text =  GetDateTimeForIndex ( contentIndex ).Date.ToString ( dateFormat, DateTimeLocalization.Culture );
	}
	
	#endregion
	
	#region helper_methods
	
	DateTime GetDateTimeForIndex ( int index )
	{
		return _minDate.AddDays ( (double) index );
	}
	
	int GetIndexForDateTime ( DateTime dateTime )
	{
		TimeSpan timeSpan = dateTime.Subtract ( _minDate );
		
		return (int)timeSpan.Days;
	}
	
	#endregion
	
	//helper class for serialization in inspector
	[System.Serializable]
	public class Date
	{
		public int 	day,
					month,
					year;
		
		public DateTime GetDateTime ()
		{
			return new DateTime ( year, month, day );
		}
		
		public Date ( int iday, int imonth, int iyear )
		{
			day = iday;
			month = imonth;
			year = iyear;
		}
	}
}
