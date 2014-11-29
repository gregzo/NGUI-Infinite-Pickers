//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEditor;

[ CustomEditor ( typeof ( IPPickerBase ) ) ]
public abstract class IPPickerInspector : Editor 
{
	protected SerializedObject 	_picker;
	
	protected SerializedProperty _pickerName,
								 _widgetOffsetInPicker,
								 _initInAwake,
								 _widgetsDepth,
								 _widgetsPivot,
								 _widgetsColor;
	
	private static GUIContent _resetButton = new GUIContent ( "Reset", "Resets the picker" );
	private static GUILayoutOption  _resetButtonWidth = GUILayout.MaxWidth(60f);
	
	protected void GetBaseProperties () 
	{
		_pickerName = _picker.FindProperty ( "pickerName" );
		_initInAwake = _picker.FindProperty ( "initInAwake" );
		_widgetsDepth = _picker.FindProperty ( "widgetsDepth" );
		_widgetsPivot = _picker.FindProperty ( "widgetsPivot" );
		_widgetOffsetInPicker = _picker.FindProperty ( "widgetOffsetInPicker" );
		_widgetsColor = _picker.FindProperty ( "widgetsColor" );
	}
	
	protected void DrawBaseProperties ()
	{
		EditorGUILayout.PropertyField ( _pickerName );
		EditorGUILayout.PropertyField ( _initInAwake );
		EditorGUILayout.PropertyField ( _widgetsDepth );
		EditorGUILayout.PropertyField ( _widgetsPivot );
		EditorGUILayout.PropertyField ( _widgetOffsetInPicker );
		EditorGUILayout.PropertyField ( _widgetsColor );
	}
	
	protected void DrawResetButton ()
	{
		if ( GUILayout.Button ( _resetButton, EditorStyles.toolbarButton, _resetButtonWidth ) )
		{
			IPPickerBase picker = ( IPPickerBase ) target;
			
			if ( picker.cycler == null )
				picker.cycler = picker.gameObject.GetComponentInChildren ( typeof ( IPCycler ) ) as IPCycler;
			
			picker.cycler.EditorInit ();
			picker.cycler.SendMessageUpwards ( "RebuildWidgets", SendMessageOptions.DontRequireReceiver ); //sendmessage upwards from cycler in case there are multiple pickers
		}
	}
	
	protected abstract void GetUniqueProperties ();
	
	protected abstract void DrawUniqueProperties ();
}
