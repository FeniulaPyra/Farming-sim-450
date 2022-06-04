using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

	public GameObject ExternalInventory;
	public InventoryMenu externalInventoryMenu;

	public double MouseScrollDeadzone;
	public double currentMouseScroll;

	public Camera cam;

    //Player's bed
    Bed bed;

    public GameObject save;

    PlayerInput playerInput;

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
		DIALOG,
		EXTERNAL_INVENTORY
	}

    [SerializeField]
	MenuState state = MenuState.NO_MENU;

    public void SetState(MenuState value)
    {
        state = value;
    }

	// Start is called before the first frame update
	void Start()
    {

        PauseMenu = gameObject.transform.Find("Pause Menu").gameObject;
        
        //Sets the save function of the onclick in code, so it works just by being dragged into a scene
        PauseMenu.transform.Find("Save").gameObject.GetComponent<Button>().onClick.AddListener(GlobalGameSaving.Instance.SaveGame);

        player = GameObject.Find("Player");

		//TODO have this be grabbed from the player once that is be do be done-ificated
		pi = player.GetComponent<PlayerInteraction>();
		inventoryMenu = InventoryMenuObject.GetComponent<InventoryMenu>();
		shippingMenu = ShippingMenuObject.GetComponent<ShippingMenu>();
		hotbarMenu = HotbarUIObject.GetComponent<HotbarMenu>();
		pi = player.GetComponent<PlayerInteraction>();
		pim = player.GetComponent<PlayerInventoryManager>();
        //playerInput = player.GetComponent<PlayerInput>();
        playerInput = FindObjectOfType<PlayerInput>();
		externalInventoryMenu = ExternalInventory.GetComponent<InventoryMenu>();

		ItemGrabber = ItemGrabberObject.GetComponent<InventoryItemGrabber>();

		inventoryMenu.SetInventoryToDisplay(pim.inv);


		//hides inventory menu at start.
		InventoryMenuObject.SetActive(false);


        bed = FindObjectOfType<Bed>().GetComponent<Bed>();
    }

	// Update is called once per frame
	void Update()
	{

		//float scroll = Input.GetAxis("Mouse ScrollWheel");
		/*switch(state)
		{
			case MenuState.INVENTORY:
                #region MenuState.INVENTORY
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
			case MenuState.EXTERNAL_INVENTORY:

				externalInventoryMenu.UpdateDisplay();
				
				//close external inv
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					if (ItemGrabber.item != null && ItemGrabber.amount > 0)
					{
						inv.AddItems(ItemGrabber.item, ItemGrabber.amount);
						ItemGrabber.item = null;
						ItemGrabber.amount = 0;
					}
					externalInventoryMenu.Hide();
					inventoryMenu.Hide();
					ItemGrabber.Hide();
					state = MenuState.NO_MENU;
					pi.CanInteract = true;
				}
				break;
			case MenuState.PAUSE:
                #region MenuState.PAUSE

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
					//pim.DropHeldItem();
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
                break;
			default:
                break;
		}*/
		inventoryMenu.UpdateDisplay();
		hotbarMenu.UpdateDisplay();
        shippingMenu.UpdateDisplay();
        externalInventoryMenu.UpdateDisplay();
        player.GetComponent<PlayerInventoryManager>().inv.isShown = InventoryMenuObject.activeSelf;
	}
	
	//hides the pause menu - for button use
	public void HidePauseMenu()
	{
		PauseMenu.SetActive(false);
		state = MenuState.NO_MENU;
		pi.CanInteract = true;
	}

    void OnDropItem()
    {
        //pim.DropHeldItem();
        
        if (Keyboard.current.shiftKey.wasPressedThisFrame == true)// || Input.GetKeyDown(KeyCode.LeftShift))//if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            pim.DropHeldItems();
        }
        else
        {
            pim.DropHeldItem();
        }
    }

    void OnOpenCloseInventory()
    {
        //closes inventory
        if (state == MenuState.INVENTORY)
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

            playerInput.SwitchCurrentActionMap("Gameplay");
        }
        //opens inventory
        else if (state == MenuState.NO_MENU)
        {
            InventoryMenuObject.SetActive(true);//!InventoryMenu.activeSelf);
            ItemGrabber.Show();
            state = MenuState.INVENTORY;
            pi.CanInteract = false;

            playerInput.SwitchCurrentActionMap("Menu");
        }
    }

    void OnOpenExitMenu()
    {
        switch (state)
        {
            case MenuState.NO_MENU:
                pi.CanInteract = false;
                PauseMenu.SetActive(true);
                state = MenuState.PAUSE;
                break;
            case MenuState.INVENTORY:
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
                break;
            case MenuState.PAUSE:
                PauseMenu.SetActive(false);
                state = MenuState.NO_MENU;
                pi.CanInteract = true;
                break;
            case MenuState.SETTINGS:
                SettingsMenu.SetActive(false);
                PauseMenu.SetActive(true);
                state = MenuState.PAUSE;
                pi.CanInteract = true;
                break;
            case MenuState.HELP:
                HelpMenu.SetActive(false);
                PauseMenu.SetActive(true);
                state = MenuState.PAUSE;
                pi.CanInteract = true;
                break;
            case MenuState.SHOP:
                ShopMenu.SetActive(false);
                //PauseMenu.SetActive(true);
                state = MenuState.NO_MENU;//PAUSE;
                pi.CanInteract = true;
                break;
            case MenuState.SHIPPING_BIN:
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
                break;
            case MenuState.BED:
                break;
            case MenuState.DIALOG:
                break;
            case MenuState.EXTERNAL_INVENTORY:
                if (ItemGrabber.item != null && ItemGrabber.amount > 0)
                {
                    inv.AddItems(ItemGrabber.item, ItemGrabber.amount);
                    ItemGrabber.item = null;
                    ItemGrabber.amount = 0;
                }
                externalInventoryMenu.Hide();
                inventoryMenu.Hide();
                ItemGrabber.Hide();
                state = MenuState.NO_MENU;
                pi.CanInteract = true;
                break;
            default:
                break;
        }
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
            }
			pi.CanInteract = false;
        }
    }

	public void CloseBed()
    {
		if(state == MenuState.BED)
        {
			state = MenuState.NO_MENU;
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

	public void OpenExternalInventory(GameObject ExternalInventoryObject)
	{
		if(state == MenuState.NO_MENU)
		{
			inventoryMenu.Show();
			externalInventoryMenu.ClearMenu();
			externalInventoryMenu.SetInventoryToDisplay(ExternalInventoryObject.GetComponent<InventoryEntity>().inv);
			externalInventoryMenu.Show();
			ItemGrabber.Show();
			state = MenuState.EXTERNAL_INVENTORY;
			pi.CanInteract = false;
		}
	}

}
