using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TogglableMenu : MonoBehaviour
{
	public static int TOP = -59;
	public static int LEFT = -180;
	public static int SLOT_GAP = 5;
	public static int SLOT_SIZE = 37;

	public void Hide()
	{
		gameObject.SetActive(false);
	}
	public void Show()
	{
		gameObject.SetActive(true);
	}
}
