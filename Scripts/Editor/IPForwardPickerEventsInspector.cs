//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections;

[ CustomEditor ( typeof ( IPForwardPickerEvents ) ) ]
public class IPForwardPickerEventsInspector : Editor {
	
	private SerializedObject _forwardPickerEvents;
	
	private SerializedProperty
		_observedPicker,
		
		_notifyOnSelectionChange,
		_onSelectionChangeNotification,
		
		_notifyOnSelectionStarted,
		_onStartedNotification,
		
		_notifyOnCenterOnChildStarted,
		_onCenterOnChildNotification,
	
		_notifyOnPickerStopped,
		_onStoppedNotification,
		
		_notifyOnDragExit,
		_onExitNotification;

	void OnEnable () 
	{
		_forwardPickerEvents = new SerializedObject(target);
		
		_observedPicker = _forwardPickerEvents.FindProperty ( "observedPicker" );
		
		IPForwardPickerEvents forwardEvents = ( IPForwardPickerEvents ) target;
		
		if ( forwardEvents.observedPicker == null )
		{
			forwardEvents.observedPicker = forwardEvents.gameObject.GetComponent ( typeof ( IPPickerBase ) ) as IPPickerBase;
			
			if ( forwardEvents.observedPicker == null )
			{
				Debug.LogWarning ( "No picker script found on " + forwardEvents.gameObject.name + ". Please assign manually." );
			}
		}
		
		_notifyOnSelectionChange = _forwardPickerEvents.FindProperty ( "notifyOnSelectionChange" );
		_onSelectionChangeNotification = _forwardPickerEvents.FindProperty ( "onSelectionChangeNotification" );
		
		_notifyOnSelectionStarted = _forwardPickerEvents.FindProperty ( "notifyOnSelectionStarted" );
		_onStartedNotification = _forwardPickerEvents.FindProperty ( "onStartedNotification" );
		
		_notifyOnCenterOnChildStarted = _forwardPickerEvents.FindProperty ( "notifyOnCenterOnChildStarted" );
		_onCenterOnChildNotification = _forwardPickerEvents.FindProperty ( "onCenterOnChildNotification" );
		
		_notifyOnPickerStopped = _forwardPickerEvents.FindProperty ( "notifyOnPickerStopped" );
		_onStoppedNotification = _forwardPickerEvents.FindProperty ( "onStoppedNotification" );
		
		_notifyOnDragExit = _forwardPickerEvents.FindProperty ( "notifyOnDragExit" );
		_onExitNotification = _forwardPickerEvents.FindProperty ( "onExitNotification" );
	}
	
	public override void OnInspectorGUI ()
	{
		_forwardPickerEvents.Update();

		EditorGUILayout.PropertyField ( _observedPicker );
		EditorGUILayout.Separator ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ( "Notify On Selection Change", GUILayout.Width ( 150f ) );
		_notifyOnSelectionChange.boolValue = EditorGUILayout.Toggle ( _notifyOnSelectionChange.boolValue);
		EditorGUILayout.EndHorizontal ();
		if ( _notifyOnSelectionChange.boolValue )
		{
			EditorGUILayout.PropertyField( _onSelectionChangeNotification, true );
			NGUIEditorTools.DrawSeparator ();
		}
		else EditorGUILayout.Separator ();
		
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ( "Notify On Selection Started", GUILayout.Width ( 150f ) );
		_notifyOnSelectionStarted.boolValue = EditorGUILayout.Toggle ( _notifyOnSelectionStarted.boolValue);
		EditorGUILayout.EndHorizontal ();
		if ( _notifyOnSelectionStarted.boolValue )
		{
			EditorGUILayout.PropertyField( _onStartedNotification, true );
			NGUIEditorTools.DrawSeparator ();
		}
		else EditorGUILayout.Separator ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ( "Notify On Recenter Start", GUILayout.Width ( 150f ) );
		_notifyOnCenterOnChildStarted.boolValue = EditorGUILayout.Toggle ( _notifyOnCenterOnChildStarted.boolValue);
		EditorGUILayout.EndHorizontal ();
		if ( _notifyOnCenterOnChildStarted.boolValue )
		{
			EditorGUILayout.PropertyField( _onCenterOnChildNotification, true );
			NGUIEditorTools.DrawSeparator ();
		}
		else EditorGUILayout.Separator ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ( "Notify On Recenter End", GUILayout.Width ( 150f ) );
		_notifyOnPickerStopped.boolValue = EditorGUILayout.Toggle ( _notifyOnPickerStopped.boolValue);
		EditorGUILayout.EndHorizontal ();
		if ( _notifyOnPickerStopped.boolValue )
		{
			EditorGUILayout.PropertyField( _onStoppedNotification, true );
		}
		else EditorGUILayout.Separator ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ( "Notify On Drag Exit", GUILayout.Width ( 150f ) );
		_notifyOnDragExit.boolValue = EditorGUILayout.Toggle ( _notifyOnDragExit.boolValue);
		EditorGUILayout.EndHorizontal ();
		if ( _notifyOnDragExit.boolValue )
		{
			EditorGUILayout.PropertyField( _onExitNotification, true );
		}
		
		_forwardPickerEvents.ApplyModifiedProperties();
	}
}
