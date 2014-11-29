
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;

public static class DateTimeLocalization 
{
	static Language _currentLanguage = Language.None;
		
	static string[] _languageCodes = new string[] { "nl-NL", "en-US", "fr-FR", "de-DE", "pt-PT", "es-ES" };

	public enum Language
	{
		Dutch,
		English,
		French,
		German,
		Portuguese,
		Spanish,
		None
	}
	
	public static CultureInfo Culture
	{
		get;
		private set;
	}
	
	public static Language CurrentLanguage
	{
		get
		{
			return _currentLanguage;
		}
	}
	
	public static bool SetLanguage ( Language language )
	{
		if ( language == _currentLanguage )
			return false;
		
		_currentLanguage = language;
		
		Culture = new CultureInfo ( _languageCodes [ (int) language ] ); 
		
		//System.Threading.Thread.CurrentThread.CurrentCulture = Culture; // needs JIT compilation, which isn't supported on mobiles

		return true;
	}
	
	/// <summary>
	/// Get all 7 day names in the current language. Be careful, index 0 is Sunday.
	/// </param>
	public static string[] GetDayNames ( bool abbreviated ) 
	{
		if ( _currentLanguage == Language.None )
		{
			Debug.LogError ("No Language set !");
			return null;
		}

		string[] names = new string[7];
		
		for ( int i = 0; i < 7; i++ )
		{
			names[i] = abbreviated ? Culture.DateTimeFormat.AbbreviatedDayNames[i] : Culture.DateTimeFormat.DayNames[i];
		}
		
		return names;
	}
	
	/// <summary>
	/// Get all 12 month names in the current language. Index 0 is January.
	/// </param>
	public static string[] GetMonthNames ( bool abbreviated )
	{
		if ( _currentLanguage == Language.None )
		{
			Debug.LogError ("No Language set !");
			return null;
		}
		
		string[] names = new string[12];
		
		for ( int i = 0; i < 12; i++ )
		{
			names[i] = abbreviated ? Culture.DateTimeFormat.AbbreviatedMonthGenitiveNames[i] : Culture.DateTimeFormat.MonthNames[i];
		}
		
		return names;
	}
	
	/// <summary>
	/// Tries to set the current language by finding the corresponding index in the Language enum.
	/// Useful with UIPopupList, which sends out a string OnSelectionChange.
	/// </summary>

	public static bool TrySetLanguage ( string languageString ) 
	{
		bool ret = false;
		Language lang;
		for ( int i = 0; i < 6; i++ )
		{
			lang = (Language)i;
			
			if ( lang.ToString () == languageString && lang != CurrentLanguage )
			{
				SetLanguage ( lang );
				ret = true;
				break;
			}
		}
		
		return ret;
	}
	
}


