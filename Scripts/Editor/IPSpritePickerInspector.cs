//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEditor;

[ CustomEditor ( typeof ( IPSpritePicker ) ) ]
public class IPSpritePickerInspector : IPPickerSpriteBaseInspector 
{
	protected SerializedProperty	_spriteNames,
									_normalizeSprites,
									_normalizedMax,
									_initIndex;
	
	
	protected override void GetUniqueProperties ()
	{
		_spriteNames = _picker.FindProperty ( "spriteNames" );
		_normalizeSprites = _picker.FindProperty ( "normalizeSprites" );
		_normalizedMax = _picker.FindProperty ( "normalizedMax" );
		_initIndex = _picker.FindProperty ( "initIndex" );
	}
	
	public override void OnInspectorGUI ()
	{
		_picker.Update();
		
		DrawSpriteProperties ();
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
		EditorGUILayout.PropertyField ( _spriteNames, true );
		EditorGUILayout.PropertyField ( _initIndex );
		EditorGUILayout.PropertyField ( _normalizeSprites );
		
		if ( _normalizeSprites.boolValue )
			EditorGUILayout.PropertyField ( _normalizedMax );
	}
}
