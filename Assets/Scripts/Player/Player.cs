using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	Inventory inv;

	int HeldSlot;
	int Hotbar;

	Item heldItem;

	public double MouseScrollDeadzone;
	public double currentMouseScroll;

    // Start is called before the first frame update
    void Start()
    {
    }
}
