using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;


public class Menu : MonoBehaviour
{
	public Inventory inv;
	public List<GameObject> gameItemPrefabs;
	private List<Item> gameItems;

	public GameObject InventoryMenu;
	public GameObject[,] InventorySlots;
	public GameObject InventorySlotPrefab;
	public GameObject selectedItem;

	// Start is called before the first frame update
	void Start()
    {
		InventorySlots = new GameObject[4,9];
		gameItems = new List<Item>();
		inv = new Inventory();
		

		foreach (GameObject g in gameItemPrefabs)
		{
			gameItems.Add(g.GetComponent<Item>());
		}

		int startingX = -333;
		int startingY = -148;
		int gap = 10;

		//creates buttons for every inventory slot
		for (int r = 0; r < 4; r++)
		{
			for (int c = 0; c < 9; c++)
			{
				//the inventory slot button
				GameObject slot = InventorySlots[r, c] = Instantiate(InventorySlotPrefab);
				slot.transform.position = new Vector2(startingX + (c+1) * 74 - 10, startingY + (r+1) * 74 - 10);

				//the text and image of the button
				Image slotIcon = slot.transform.GetChild(0).GetComponent<Image>();
				Text slotLabel = slot.transform.GetChild(1).GetComponent<Text>();

				//the item in the corresponding slot in the inventory object
				ItemStack currentSlot = inv.GetSlot(r, c);

				//labels the slot
				if (currentSlot != null)
				{
					slotLabel.text = "" + currentSlot.Amount;
					slot.name = "R" + r + " C" + currentSlot.Item.name;
					slotIcon.sprite = currentSlot.Item.spr;
				}
				else
				{
					slotLabel.text = "";
					slot.name = "R" + r + " C" + c;
					slotIcon.sprite = null;
				}
				int x = r, y = c;
				//adds click event listener to select slot.
				slot.GetComponent<Button>().onClick.AddListener(delegate {
					inv.SelectSlot(x, y);
					UpdateInventory();
				});
				slot.GetComponent<Button>().onClick.AddListener(UpdateInventory);

				//adds the slot to the inventory menu
				slot.transform.SetParent(InventoryMenu.transform, false);

			}

		}

		//debugging stuff
		inv.AddItems(new ItemStack(gameItems[0], 10));
		inv.AddItems(new ItemStack(gameItems[1], 10));
		inv.AddItems(new ItemStack(gameItems[1], 50));
		UpdateInventory();
    }

	void UpdateInventory()
	{
		//updates selected item
		Image selectedItemIcon = selectedItem.transform.GetChild(0).GetComponent<Image>();
		Text selectedItemLabel = selectedItem.transform.GetChild(1).GetComponent<Text>();

		if (inv.selectedStack != null)
		{
			Debug.Log("helo" + inv.SelectedStack.Amount);
			selectedItemLabel.text = "" + inv.SelectedStack.Amount;
			selectedItemIcon.enabled = true;
			selectedItemIcon.sprite = inv.SelectedStack.Item.spr;
		}
		else
		{
			selectedItemLabel.text = "";
			selectedItemIcon.sprite = null;
			selectedItemIcon.enabled = false;

		}

		//updates inventory slots
		for (int r = 0; r < 4; r++)
		{
			for (int c = 0; c < 9; c++)
			{

				//gets the inventory slot menu button and info about the inventory slog
				GameObject slot = InventorySlots[r, c];
				Image slotIcon = slot.transform.GetChild(0).GetComponent<Image>();
				Text slotLabel = slot.transform.GetChild(1).GetComponent<Text>();
				//corresponding inventory slot
				ItemStack currentSlot = inv.GetSlot(r, c);

				//updates slot info
				if (currentSlot != null)
				{
					slotLabel.text = "" + currentSlot.Amount;
					slotIcon.sprite = currentSlot.Item.spr;
				}
				else
				{
					slotLabel.text = "";
					slotIcon.sprite = null;
				}
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
		//https://stackoverflow.com/questions/43802207/position-ui-to-mouse-position-make-tooltip-panel-follow-cursor
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
		   InventoryMenu.transform.parent.transform as RectTransform, Input.mousePosition,
		   InventoryMenu.transform.parent.GetComponent<Canvas>().worldCamera,
		   out pos);

		//transform.position = InventoryMenu.transform.parent.transform.TransformPoint(pos);

		selectedItem.transform.position = InventoryMenu.transform.parent.transform.TransformPoint(new Vector3(pos.x, pos.y + 33, -1));//new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

	}
}
