using UnityEngine;
using System.Collections;

[RequireComponent ( typeof ( UILabel ) ) ]
public class DisplayPickerText : MonoBehaviour {
	
	public IPTextPicker picker; //The picker to grab text from
	
	UILabel _label;
	
	void Awake()
	{
		_label = gameObject.GetComponent ( typeof ( UILabel ) ) as UILabel;
	}
	
	void DisplayText ()
	{
		_label.text = picker.CurrentLabelText;
	}
}
