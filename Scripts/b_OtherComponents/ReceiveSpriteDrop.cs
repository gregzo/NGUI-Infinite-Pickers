using UnityEngine;
using System.Collections;

public class ReceiveSpriteDrop : MonoBehaviour {
	
	public GameObject target;
	public string message;
	
	void OnSpriteDrop ( string spriteName )
	{
		target.SendMessage ( message, spriteName );	
	}
}
