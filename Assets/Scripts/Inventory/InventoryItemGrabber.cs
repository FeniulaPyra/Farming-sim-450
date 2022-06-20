using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryItemGrabber : TogglableMenu
{
	//public GameObject selectedItem;
	public Item item;
	public int amount;

	Image icon;
	Text label;

	//overall menu object
	public GameObject menu;

	// Start is called before the first frame update
	void Start()
    {

		icon = transform.GetChild(0).GetComponent<Image>();
		label = transform.GetChild(1).GetComponent<Text>();
	}

    // Update is called once per frame
    void Update()
    {
		if (item != null && amount > 0)
		{
			label.text = "" + amount;
			icon.enabled = true;
			icon.sprite = item.spr;
		}
		else
		{
			label.text = "";
			icon.sprite = null;
			icon.enabled = false;
		}


		//https://stackoverflow.com/questions/43802207/position-ui-to-mouse-position-make-tooltip-panel-follow-cursor
		Vector2 pos;
        /*RectTransformUtility.ScreenPointToLocalPointInRectangle(
			menu.transform as RectTransform, 
			Input.mousePosition,
			menu.GetComponent<Canvas>().worldCamera,
			out pos);*/
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        menu.transform as RectTransform,
        Mouse.current.position.ReadValue(),
        menu.GetComponent<Canvas>().worldCamera,
			out pos);
        gameObject.transform.position = menu.transform.TransformPoint(new Vector3(pos.x, pos.y + 33, -1));//new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
	}
}
