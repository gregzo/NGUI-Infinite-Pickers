//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEditor;

[ CustomEditor ( typeof ( IPPickerLabelBase ) ) ]
public abstract class IPLabelPickerInspector : IPPickerInspector 
{
	protected SerializedProperty _font;
	
	void OnEnable ()
	{
		_picker = new SerializedObject(target);
		
		GetBaseProperties ();
		GetLabelProperties ();
		GetUniqueProperties ();
	}
	
	protected void GetLabelProperties () 
	{
		_font = _picker.FindProperty ( "font" );
	}
	
	protected void DrawLabelProperties ()
	{
		EditorGUILayout.PropertyField ( _font );
	}
}
