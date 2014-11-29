using UnityEngine;
using System.Collections;

public class IPDragScrollView : UIDragScrollView {

	void OnPress (bool pressed)
	{
		if (scrollView && enabled && NGUITools.GetActive(gameObject))
		{
			scrollView.Press(pressed);
		}
	}
}
