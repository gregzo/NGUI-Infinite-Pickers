//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEditor;

[ CustomEditor ( typeof ( IPCycler ) ) ]
public class IPCyclerInspector : Editor {

	private SerializedObject 	_cycler;
	
	private SerializedProperty 	_direction,
								_spacing,
								_recenterSpeedThreshold,
								_recenterSpringStrength,
								_restrictDragToPicker;
	
	private int _nbOfTransforms;
	
	private static GUIContent 	_moreTransformsButton = new GUIContent("+", "add 2 more transforms to cycle"),	
								_lessTransformsButton = new GUIContent("-", "remove 2 transforms to cycle"),
								_resetButton = new GUIContent ( "Reset", "Resets the picker" );
		
	private static GUILayoutOption  _buttonWidth = GUILayout.MaxWidth(30f),
									_resetButtonWidth = GUILayout.MaxWidth(60f);
	
	void OnEnable () 
	{
		_cycler = new SerializedObject(target);
		_cycler.Update();
		IPCycler cycler = ( IPCycler ) target;
		_nbOfTransforms = cycler.NbOfTransforms;
		_spacing = _cycler.FindProperty ( "spacing" );
		_direction = _cycler.FindProperty ( "direction" );
		_recenterSpeedThreshold = _cycler.FindProperty ( "recenterSpeedThreshold" );
		_recenterSpringStrength = _cycler.FindProperty ( "recenterSpringStrength" );
		_restrictDragToPicker = _cycler.FindProperty ( "restrictDragToPicker" );
	}
	
	public override void OnInspectorGUI ()
	{
		_cycler.Update();
		
		EditorGUILayout.BeginHorizontal();
		
		GUILayout.Label ( "Number of transforms to cycle : " );
		GUILayout.Label ( _nbOfTransforms.ToString () );

		if ( GUILayout.Button ( _lessTransformsButton, EditorStyles.miniButtonLeft, _buttonWidth ) )
		{
			if ( _nbOfTransforms > 4 )
			{
				IPCycler cycler = ( IPCycler ) target;
				_nbOfTransforms -= 2;
				if ( _nbOfTransforms % 2 == 0 )
					_nbOfTransforms++;
				
				cycler.RebuildTransforms ( _nbOfTransforms );
				cycler.SendMessageUpwards ( "RebuildWidgets", SendMessageOptions.DontRequireReceiver );
			}
		}
		
		if ( GUILayout.Button ( _moreTransformsButton, EditorStyles.miniButtonRight, _buttonWidth ) )
		{
			IPCycler cycler = ( IPCycler ) target;
			int nbToAdd = _nbOfTransforms % 2 == 0 ? 3 : 2;
			_nbOfTransforms += nbToAdd;
			cycler.RebuildTransforms ( _nbOfTransforms );
			cycler.SendMessageUpwards ( "RebuildWidgets", SendMessageOptions.DontRequireReceiver );
		}
		
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.PropertyField ( _spacing );
		EditorGUILayout.PropertyField ( _direction );
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ( "Recenter Speed Threshold", GUILayout.Width ( 150f ) );
		_recenterSpeedThreshold.floatValue = EditorGUILayout.FloatField ( _recenterSpeedThreshold.floatValue );
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ( "Recenter Spring Strength", GUILayout.Width ( 150f ) );
		_recenterSpringStrength.floatValue = EditorGUILayout.FloatField ( _recenterSpringStrength.floatValue );
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ( "Restrict Drag To Picker", GUILayout.Width ( 150f ) );
		_restrictDragToPicker.boolValue = EditorGUILayout.Toggle ( _restrictDragToPicker.boolValue );
		EditorGUILayout.EndHorizontal ();
		
		if ( GUILayout.Button ( _resetButton, EditorStyles.toolbarButton, _resetButtonWidth ) )
		{
			IPCycler cycler = ( IPCycler ) target;
			cycler.EditorInit ();
			cycler.SendMessageUpwards ( "RebuildWidgets", SendMessageOptions.DontRequireReceiver );
		}
		
		if ( _cycler.ApplyModifiedProperties() )
		{
			( ( IPCycler ) target ).UpdateTrueSpacing ();
		}
	}
}

