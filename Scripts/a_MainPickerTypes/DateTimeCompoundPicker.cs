using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// An example script that manually manages 3 IPPicker components 
/// </summary>
public class DateTimeCompoundPicker : MonoBehaviour {
	
	public IPDatePicker 	datePicker;
	public IPNumberPicker 	hourPicker,
							minutesPicker;
	
	public bool initAtNow;
	
	public IPDatePicker.Date 	initDate;
	public int 					initHour,
								initMinute;
	
	bool _isInitialized;
	
	public bool IsMoving // Are any of the three pickers currently moving?
	{
		get
		{
			return ( datePicker.IsCyclerMoving || hourPicker.IsCyclerMoving || minutesPicker.IsCyclerMoving );
		}
	}
	
	void Awake ()
	{
		if ( datePicker.initInAwake || hourPicker.initInAwake || minutesPicker.initInAwake )
		{
			Debug.LogError ("All three pickers of DateTimeCompoundPicker should have initInAwake set to false" );
		}
		
		if ( initAtNow )
		{
			DateTime now = DateTime.Now; //Grab the current date 
			initDate = new IPDatePicker.Date ( now.Day, now.Month, now.Year ); //set init values
			initHour = now.Hour;
			initMinute = now.Minute;
		}
		
		//Push init values to the date picker
		datePicker.initDate = initDate; 
	}
	
	void Start ()
	{
		//Setup all pickers
		datePicker.Setup ();
		hourPicker.Setup ();
		hourPicker.ResetPickerAtValue ( initHour ); //hour and minutes picker have to be reset, they are not displayed in the inspector
		minutesPicker.Setup ();
		minutesPicker.ResetPickerAtValue ( initMinute );
		
		_isInitialized = true;
	}
	
	public DateTime GetSelectedDateTime ( out bool isStillMoving ) // out forces the user to be aware if any of the pickers are still moving
	{
		isStillMoving = IsMoving;
		DateTime currentDateTime = datePicker.CurrentDate; //Get the date from the date picker
		currentDateTime = currentDateTime.AddMinutes ( (double) ( minutesPicker.CurrentValue  + hourPicker.CurrentValue * 60 ) ); //Add minutes from the minute and hour pickers
		return currentDateTime;
	}
	
	public void SetSelectedDateTime ( DateTime dateTime )
	{
		if ( datePicker.TryResetPickerAtDateTime ( dateTime ) ) // Set time only if date is valid
		{
			hourPicker.ResetPickerAtValue ( dateTime.Hour );
			minutesPicker.ResetPickerAtValue ( dateTime.Minute );
		}
		else
		{
			Debug.LogError ( "date is out of picker range" );
		}
	}
	
	//Called via SendMessage, by a UIPopupList for example
	IEnumerator SetNewLanguage (string language)
	{
		if ( _isInitialized == false ) //yield if picker hasn't fully initialized ( UIPopupList sends selection message in Start
			yield return null;
		
		datePicker.SetNewLanguage ( language );
	}
}
