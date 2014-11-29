//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEditor;

[ CustomEditor ( typeof ( IPTextPicker ) ) ]
public class IPTextPickerInspector : IPLabelPickerInspector 
{
	protected SerializedProperty	_labelsText,
									_initIndex;
	
	protected override void GetUniqueProperties ()
	{
		_labelsText = _picker.FindProperty ( "labelsText" );
		_initIndex = _picker.FindProperty ( "initIndex" );
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
		EditorGUILayout.PropertyField ( _labelsText, true );
		EditorGUILayout.PropertyField ( _initIndex );
	}
}
