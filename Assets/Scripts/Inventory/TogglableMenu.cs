using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TogglableMenu : MonoBehaviour
{
	public int TOP = 10;
	public int LEFT = -180;
	public int SLOT_GAP = 5;
	public int SLOT_SIZE = 37;

	public virtual void Hide()
	{
		gameObject.SetActive(false);
	}
	public virtual void Show()
	{
		gameObject.SetActive(true);
	}
}
