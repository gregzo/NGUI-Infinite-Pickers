using UnityEngine;
using System.Collections;

public class CyclerClamper : MonoBehaviour {
	
	public IPPickerBase observedPicker;
	IPCycler _cycler;
	
	void Awake()
	{
		_cycler = gameObject.GetComponent( typeof( IPCycler ) ) as IPCycler;
		
		if( _cycler == null )
		{
			Debug.LogError( "CyclerClamper must be placed on the same GameObject as IPCycler." );
			Destroy( this );
			return;
		}
	}
	
	void Start()
	{
		OnPickerUpdated();
	}
	
	void OnEnable()
	{
		observedPicker.onPickerValueUpdated += OnPickerUpdated;
	}
	
	void OnDisable()
	{
		observedPicker.onPickerValueUpdated -= OnPickerUpdated;
	}
	
	void OnPickerUpdated()
	{
		_cycler.ClampDecrement = observedPicker.SelectedIndex <= _cycler.NbOfTransforms / 2;
		_cycler.ClampIncrement = observedPicker.SelectedIndex >= observedPicker.VirtualElementsCount - _cycler.NbOfTransforms / 2 - 1;
	}
}
