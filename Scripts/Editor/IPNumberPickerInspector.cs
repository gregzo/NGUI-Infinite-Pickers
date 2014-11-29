//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEditor;

[ CustomEditor ( typeof ( IPNumberPicker ) ) ]
public class IPNumberPickerInspector : IPLabelPickerInspector 
{
	protected SerializedProperty	_min,
									_max,
									_step,
									_initValue,
									_toStringFormat;
	
	protected override void GetUniqueProperties ()
	{
		_min = _picker.FindProperty ( "min" );
		_max = _picker.FindProperty ( "max" );
		_step = _picker.FindProperty ( "step" );
		_initValue = _picker.FindProperty ( "initValue" );
		_toStringFormat = _picker.FindProperty ( "toStringFormat" );
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
		GUILayout.BeginHorizontal ( );
		
		EditorGUILayout.LabelField ( "Min", GUILayout.Width ( 40f ) );
		_min.intValue = EditorGUILayout.IntField ( _min.intValue, GUILayout.Width ( 50f )  );
		EditorGUILayout.LabelField ( "Max", GUILayout.Width ( 40f ) );
		_max.intValue = EditorGUILayout.IntField ( _max.intValue, GUILayout.Width ( 50f )  );
		
		GUILayout.EndHorizontal ();
		
		GUILayout.BeginHorizontal ();
		
		EditorGUILayout.LabelField ( "Step", GUILayout.Width ( 40f ) );
		_step.intValue = EditorGUILayout.IntField ( _step.intValue, GUILayout.Width ( 50f )  );
		EditorGUILayout.LabelField ( "Init Value", GUILayout.Width ( 40f ) );
		_initValue.intValue = EditorGUILayout.IntField ( _initValue.intValue, GUILayout.Width ( 50f )  );
		
		GUILayout.EndHorizontal ();
		
		EditorGUILayout.PropertyField ( _toStringFormat );
	}
}
