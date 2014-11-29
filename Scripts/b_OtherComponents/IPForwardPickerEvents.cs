//----------------------------------------------
//            NGUI Infinite Pickers
// 		Copyright © 2013 Gregorio Zanon
//----------------------------------------------

/// <summary>
/// Forwards picker events via SendMessage.
/// Drop on a GameObject that has an IPPicker component.
/// </summary>
using UnityEngine;
using System.Collections;

public class IPForwardPickerEvents : MonoBehaviour {
	
	public IPPickerBase observedPicker; //Will be automatically grabbed in Awake if null. Can be manually set in the inspector if needed ( if there are multiple IPPicker components on the same gameObject, for instance )
	
	public bool notifyOnSelectionChange;
	public GameObjectAndMessage onSelectionChangeNotification;
	
	public bool notifyOnSelectionStarted;
	public GameObjectAndMessage onStartedNotification;
	
	public bool notifyOnCenterOnChildStarted;
	public GameObjectAndMessage onCenterOnChildNotification;
	
	public bool notifyOnPickerStopped;
	public GameObjectAndMessage onStoppedNotification;
	
	public bool notifyOnDragExit;
	public GameObjectAndMessage onExitNotification;
	
	IPCycler _cycler;

	void Start ()
	{
		if ( observedPicker == null )
		{
			observedPicker = gameObject.GetComponent ( typeof ( IPPickerBase ) ) as IPPickerBase;
		}
		
		if ( observedPicker == null )
		{
			Debug.LogError ( "Needs an IPPicker!");
			return;
		}
		
		_cycler = observedPicker.cycler;
		if ( _cycler == null )
			Debug.LogError ("No Cycler!");
		
		SetDelegates ();
		
		if ( notifyOnSelectionChange && onSelectionChangeNotification.notifyInStart )
		{
			Notify ( onSelectionChangeNotification );
		}
		
		if ( notifyOnSelectionStarted && onStartedNotification.notifyInStart )
		{
			Notify ( onStartedNotification );
		}
		
		if ( notifyOnCenterOnChildStarted && onCenterOnChildNotification.notifyInStart )
		{
			Notify ( onCenterOnChildNotification );
		}
		
		if ( notifyOnPickerStopped && onStoppedNotification.notifyInStart )
		{
			Notify ( onStoppedNotification );
		}
		
		if ( notifyOnDragExit && onExitNotification.notifyInStart )
		{
			Notify ( onExitNotification );
		}
	}
	
	void SetDelegates ()
	{
		if ( notifyOnSelectionChange )
			observedPicker.onPickerValueUpdated += OnPickerSelectionChange;
		
		if ( notifyOnSelectionStarted )
			_cycler.onCyclerSelectionStarted += OnCyclerSelectionStarted;
		
		if ( notifyOnCenterOnChildStarted )
			_cycler.onCenterOnChildStarted += OnCenterOnChildStarted;
		
		if ( notifyOnPickerStopped )
			_cycler.onCyclerStopped += OnCyclerStopped;
		
		if ( notifyOnDragExit )
			_cycler.userInteraction.onDragExit += OnDragExit;
	}
	
	void OnEnable ()
	{
		if ( _cycler != null )
			SetDelegates ();
	}
	
	void OnDisable ()
	{
		if ( notifyOnSelectionChange )
			observedPicker.onPickerValueUpdated -= OnPickerSelectionChange;
		
		if ( notifyOnSelectionStarted )
			_cycler.onCyclerSelectionStarted -= OnCyclerSelectionStarted;
			
		if ( notifyOnCenterOnChildStarted )
			_cycler.onCenterOnChildStarted -= OnCenterOnChildStarted;
		
		if ( notifyOnPickerStopped )
			_cycler.onCyclerStopped -= OnCyclerStopped;
		
		if ( notifyOnDragExit )
			_cycler.userInteraction.onDragExit -= OnDragExit;
	}
	
	void OnCyclerSelectionStarted ()
	{
		Notify ( onStartedNotification );
	}
	
	void OnCenterOnChildStarted ()
	{
		Notify ( onCenterOnChildNotification );
	}
	
	void OnCyclerStopped ()
	{
		Notify ( onStoppedNotification );
	}
	
	void OnPickerSelectionChange ()
	{
		Notify ( onSelectionChangeNotification );
	}
	
	void OnDragExit ()
	{
		Notify ( onExitNotification );
	}
	
	void Notify ( GameObjectAndMessage goAndMessage )
	{
		if ( goAndMessage.gameObject != null )
		{
			goAndMessage.gameObject.SendMessage ( goAndMessage.message );
		}
	}
	
	[System.Serializable]
	public class GameObjectAndMessage
	{
		public GameObject gameObject;
		public string message;
		public bool notifyInStart;
	}
}
