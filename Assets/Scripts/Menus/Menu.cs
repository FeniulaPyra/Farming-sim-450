using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//using UnityEngine.UIElements;


public class Menu : MonoBehaviour
{

	public static int TOP = -148;
	public static int LEFT = -360;
	public static int SLOT_GAP = 10;

	public Inventory inv;
	public Inventory shippingInventory;
	public GameObject[] ShippingBinSlots;

	public GameObject PauseMenu;
	public GameObject SettingsMenu;
	public GameObject HelpMenu;
	public GameObject SaveMenu;
	public GameObject LoadMenu;
	public GameObject ShopMenu;
	public GameObject gameInfo;
	public GameObject controls;

	public GameObject ShippingMenuObject;
	public ShippingMenu shippingMenu;

	public GameObject InventoryMenuObject;
	public InventoryMenu inventoryMenu;

	public GameObject HotbarUIObject;
	public HotbarMenu hotbarMenu;


	public GameObject ItemGrabberObject;
	InventoryItemGrabber ItemGrabber;

	public GameObject BubbleToggle;

	private int startingX = -360;
	private int startingY = -148;
	private int gap = 10;
	
	public GameObject ShippingBin;
	public GameObject player;
	private PlayerInteraction pi;
	private PlayerInventoryManager pim;

	public double MouseScrollDeadzone;
	public double currentMouseScroll;

	public Camera cam;

    //Player's bed
    Bed bed;

	public enum MenuControls
	{
		OPEN_INVENTORY = KeyCode.E,
		NEXT_HOTBAR = KeyCode.I,
		PREV_HOTBAR = KeyCode.K,
		NEXT_HOTBAR_SLOT = KeyCode.L,
		PREV_HOTBAR_SLOT = KeyCode.J,
		DROP = KeyCode.Q
	}

	public enum MenuState
	{
		NO_MENU,
		INVENTORY,
		PAUSE,
		SETTINGS,
		HELP,
		SHOP,
		SHIPPING_BIN,
		BED,
		DIALOG
	}

    [SerializeField]
	MenuState state = MenuState.NO_MENU;

	// Start is called before the first frame update
	void Start()
    {
		//TODO have this be grabbed from the player once that is be do be done-ificated
		pi = player.GetComponent<PlayerInteraction>();
		inventoryMenu = InventoryMenuObject.GetComponent<InventoryMenu>();
		shippingMenu = ShippingMenuObject.GetComponent<ShippingMenu>();
		hotbarMenu = HotbarUIObject.GetComponent<HotbarMenu>();
		pi = player.GetComponent<PlayerInteraction>();
		pim = player.GetComponent<PlayerInventoryManager>();

		ItemGrabber = ItemGrabberObject.GetComponent<InventoryItemGrabber>();

		inventoryMenu.SetInventoryToDisplay(pim.inv);

		//hides inventory menu at start.
		InventoryMenuObject.SetActive(false);

        bed = FindObjectOfType<Bed>().GetComponent<Bed>();
    }

	// Update is called once per frame
	void Update()
	{

		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (InventoryMenuObject.activeSelf)
		{
		}
		Debug.Log("menustate: " + state);

        Debug.Log("Before switch");
		switch(state)
		{
			case MenuState.INVENTORY:
                #region MenuState.INVENTORY
                Debug.Log("Inventory swtichcase");
				inventoryMenu.UpdateDisplay();

				//close inventory
				if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
				{
					//resets grabbed item
					if (ItemGrabber.item != null && ItemGrabber.amount > 0)
					{
						inv.AddItems(ItemGrabber.item, ItemGrabber.amount);
						ItemGrabber.item = null;
						ItemGrabber.amount = 0;
					}
					InventoryMenuObject.SetActive(false);
					ItemGrabber.Hide();
					state = MenuState.NO_MENU;
					pi.CanInteract = true;
				}
				break;
			#endregion
			case MenuState.PAUSE:
                #region MenuState.PAUSE
                Debug.Log("Pause swtichcase");
                if (Input.GetKeyDown(KeyCode.Escape))
				{
					PauseMenu.SetActive(false);
					state = MenuState.NO_MENU;
					pi.CanInteract = true;
				}
				break;
			#endregion
			case MenuState.SETTINGS:
                #region MenuState.SETTINGS
                Debug.Log("Settings swtichcase");
                if (Input.GetKeyDown(KeyCode.Escape))
				{
					SettingsMenu.SetActive(false);
					PauseMenu.SetActive(true);
					state = MenuState.PAUSE;
					pi.CanInteract = true;
				}
				break;
				#endregion
			case MenuState.HELP:
                #region MenuState.HELP
                Debug.Log("Help swtichcase");
                if (Input.GetKeyDown(KeyCode.Escape))
				{
					HelpMenu.SetActive(false);
					PauseMenu.SetActive(true);
					state = MenuState.PAUSE;
					pi.CanInteract = true;
				}
				break;
				#endregion
			case MenuState.SHOP:
                #region MenuState.SHOP
                Debug.Log("Shop swtichcase");
                if (Input.GetKeyDown(KeyCode.Escape))
				{
					ShopMenu.SetActive(false);
					//PauseMenu.SetActive(true);
					state = MenuState.NO_MENU;//PAUSE;
					pi.CanInteract = true;
				}
				break;
				#endregion
			case MenuState.SHIPPING_BIN:
                #region MenuState.SHIPPING_BIN
                Debug.Log("Bin swtichcase");
				shippingMenu.UpdateDisplay();
				//close shipping bin
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					if (ItemGrabber.item != null && ItemGrabber.amount > 0)
					{
						inv.AddItems(ItemGrabber.item, ItemGrabber.amount);
						ItemGrabber.item = null;
						ItemGrabber.amount = 0;
					}
					shippingMenu.Hide();
					inventoryMenu.Hide();
					ItemGrabber.Hide();
					state = MenuState.NO_MENU;
					pi.CanInteract = true;
				}
				break;
				#endregion
			case MenuState.NO_MENU:
                #region MenuState.NO_MENU
                Debug.Log("Bin swtichcase");

				//open/close inventory
				if (Input.GetKeyDown(KeyCode.E))
				{
					InventoryMenuObject.SetActive(true);//!InventoryMenu.activeSelf);
					ItemGrabber.Show();
					state = MenuState.INVENTORY;
					pi.CanInteract = false;
				}

				//drop held item
				if(Input.GetKeyDown(KeyCode.Q))
				{
					pim.DropHeldItem();
					hotbarMenu.UpdateDisplay();
					if(Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift))
					{
						pim.DropHeldItems();
					}
					else
					{
						pim.DropHeldItem();
					}
				}
				
				if(Input.GetKeyDown(KeyCode.Escape))
				{
					pi.CanInteract = false;
					PauseMenu.SetActive(true);
					state = MenuState.PAUSE;
				}

				break;
            #endregion
            case MenuState.DIALOG:
                Debug.Log("Dialog default");
                break;
			default:
                Debug.Log("default switch");
                break;
		}
        Debug.Log("After switch");
		inventoryMenu.UpdateDisplay();
		hotbarMenu.UpdateDisplay();
		player.GetComponent<PlayerInventoryManager>().inv.isShown = InventoryMenuObject.activeSelf;
	}
	
	//hides the pause menu - for button use
	public void HidePauseMenu()
	{
		PauseMenu.SetActive(false);
		state = MenuState.NO_MENU;
		pi.CanInteract = true;
	}

	public void OpenSettings()
	{
		PauseMenu.SetActive(false);
		SettingsMenu.SetActive(true);
		state = MenuState.SETTINGS;
		pi.CanInteract = false;
	}

	//quits game - for button use;
	public void QuitGame()
	{
		Application.Quit();
	}

	//switches to main menu scene - for button use
	public void ReturnToMenu()
	{
		SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
	}

	public void OpenHelp()
	{
		PauseMenu.SetActive(false);
		HelpMenu.SetActive(true);
		state = MenuState.HELP;
		pi.CanInteract = false;

	}

	public void CloseHelp()
	{
		HelpMenu.SetActive(false);
		PauseMenu.SetActive(true);
		state = MenuState.PAUSE;
	}
	public void OpenShop()
	{
		PauseMenu.SetActive(false);
		ShopMenu.SetActive(true);
		state = MenuState.SHOP;
		pi.CanInteract = false;

	}

	public void OpenControls()
	{
		if(state == MenuState.HELP)
		{
			gameInfo.SetActive(false);
			controls.SetActive(true);
		}
	}

	public void OpenGameInfo()
	{
		if (state == MenuState.HELP)
		{
			gameInfo.SetActive(true);
			controls.SetActive(false);
		}
	}

	public void OpenShippingBin()
	{
		if(state == MenuState.NO_MENU)
		{
			inventoryMenu.Show();
			shippingMenu.Show();
			ItemGrabber.Show();
			state = MenuState.SHIPPING_BIN;
			pi.CanInteract = false;

		}
	}

	public void OpenBed()
    {
		if(state == MenuState.NO_MENU)
        {
			state = MenuState.BED;
            //bed.SetTextObjectsActive(true);
            if (bed.MyFlowchart.isActiveAndEnabled == true)
            {
                bed.MyFlowchart.ExecuteBlock("Start");
                Debug.Log("fungus");
            }
			pi.CanInteract = false;
        }
    }

	public void CloseBed()
    {
		if(state == MenuState.BED)
        {
			state = MenuState.NO_MENU;
            if (bed.MyFlowchart.isActiveAndEnabled == true)
            {
                bed.SetTextObjectsActive(false);
            }
            pi.CanInteract = true;
        }
    }

	public void OpenDialog()
	{
		state = MenuState.DIALOG;
		pi.CanInteract = false;
	}

	public void CloseDialog()
	{
		if(state == MenuState.DIALOG)
		{
			state = MenuState.NO_MENU;
			pi.CanInteract = true;
		}
	}

}
