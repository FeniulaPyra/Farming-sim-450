using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TogglableMenu : MonoBehaviour
{
	public static int TOP = -148;
	public static int LEFT = -360;
	public static int SLOT_GAP = 10;

	public void Hide()
	{
		gameObject.SetActive(false);
	}
	public void Show()
	{
		gameObject.SetActive(true);
	}
}
