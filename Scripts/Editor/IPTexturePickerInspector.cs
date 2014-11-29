//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEditor;

[ CustomEditor ( typeof ( IPTexturePicker ) ) ]
public class IPTexturePickerInspector : IPPickerInspector 
{
	protected SerializedProperty	_textures,
									_shader,
									_normalizeSprites,
									_normalizedMax,
									_initIndex;
	
	void OnEnable ()
	{
		_picker = new SerializedObject(target);
		
		GetBaseProperties ();
		GetUniqueProperties ();
	}
	
	protected override void GetUniqueProperties ()
	{
		_textures = _picker.FindProperty ( "textures" );
		_shader = _picker.FindProperty ( "shader" );
		_normalizeSprites = _picker.FindProperty ( "normalizeTextures" );
		_normalizedMax = _picker.FindProperty ( "normalizedMax" );
		_initIndex = _picker.FindProperty ( "initIndex" );
	}
	
	public override void OnInspectorGUI ()
	{
		_picker.Update();
		
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
		EditorGUILayout.PropertyField ( _shader, true );
		EditorGUILayout.PropertyField ( _textures, true );
		EditorGUILayout.PropertyField ( _initIndex );
		EditorGUILayout.PropertyField ( _normalizeSprites );
		
		if ( _normalizeSprites.boolValue )
			EditorGUILayout.PropertyField ( _normalizedMax );
	}
}
