using UnityEngine;
using System.Collections;

public class TweenPanelClipSoftness : UITweener {

	public Vector2 from;
	public Vector2 to;

	UIPanel _panel;
	
	
	void Awake ()
	{
		_panel = gameObject.GetComponent ( typeof ( UIPanel ) ) as UIPanel;
		
		if ( _panel == null )
			Debug.LogError ( "TweenPanelClipRange needs a UIPanel!" );
	}
	
	protected override void OnUpdate (float factor, bool isFinished)
	{
		_panel.clipSoftness = from * (1f - factor) + to * factor;
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenScale Begin (GameObject go, float duration, Vector3 scale)
	{
		TweenScale comp = UITweener.Begin<TweenScale>(go, duration);
		comp.from = comp.value;
		comp.to = scale;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}
