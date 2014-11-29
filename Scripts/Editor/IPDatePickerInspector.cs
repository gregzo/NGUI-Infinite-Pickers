//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEditor;

[ CustomEditor ( typeof ( IPDatePicker ) ) ]
public class IPDatePickerInspector : IPLabelPickerInspector {

	protected SerializedProperty	_language,
									_pickerMinDate,
									_pickerMaxDate,
									_initDate,
									_dateFormat;
		
	
	protected override void GetUniqueProperties ()
	{
		_language = _picker.FindProperty ( "language" );
		_pickerMinDate = _picker.FindProperty ( "pickerMinDate" );
		_pickerMaxDate = _picker.FindProperty ( "pickerMaxDate" );
		_initDate = _picker.FindProperty ( "initDate" );
		_dateFormat = _picker.FindProperty ( "dateFormat" );
	}
	
	public override void OnInspectorGUI ()
	{
		_picker.Update();
		
		DrawLabelProperties ();
		DrawUniqueProperties ();
		DrawBaseProperties ();
		DrawResetButton ();
		
		if ( _picker.ApplyModifiedProperties() )
		{
			( ( IPPickerBase ) target ).ResetWidgets ();
		}
	}
	
	protected override void DrawUniqueProperties ()
	{
		EditorGUILayout.PropertyField ( _language );
		EditorGUILayout.PropertyField ( _pickerMinDate, true );
		EditorGUILayout.PropertyField ( _pickerMaxDate, true );
		EditorGUILayout.PropertyField ( _initDate, true );
		EditorGUILayout.PropertyField ( _dateFormat );
	}
}
