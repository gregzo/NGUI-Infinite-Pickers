//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEditor;

[ CustomEditor ( typeof ( IPPickerSpriteBase ) ) ]
public abstract class IPPickerSpriteBaseInspector : IPPickerInspector 
{
	protected SerializedProperty _atlas;
	
	void OnEnable ()
	{
		_picker = new SerializedObject(target);
		
		GetBaseProperties ();
		GetSpriteProperties ();
		GetUniqueProperties ();
	}
	
	protected void GetSpriteProperties () 
	{
		_atlas = _picker.FindProperty ( "atlas" );
	}
	
	protected void DrawSpriteProperties ()
	{
		EditorGUILayout.PropertyField ( _atlas );
	}
}
