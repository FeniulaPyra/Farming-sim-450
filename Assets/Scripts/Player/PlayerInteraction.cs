using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{

    [SerializeField]
    private FarmManager farmManager;

    [SerializeField]
    private SpriteRenderer indicatorImage;

    [SerializeField]
    private Transform indicator;

    [SerializeField]
    private bool displayIndicator;

    [SerializeField]
    private Vector3 interactionOffset;

    [SerializeField]
    private float maxInteractionDistance = 1.25f;

    [SerializeField]
    private Color activeColor;

    [SerializeField]
    private Color inactiveColor;

    private PlayerMovement playerMovement;

    private Vector3Int focusTilePosition;
    private Vector3 playerPosition;
    private bool interactInRange;
    [SerializeField]
    private bool canInteract;

	public bool isShown;
    public bool ableToPlacePet;
    public int oldPetCount;
    public int petCount; //The number of pets the player currently has on the screen.
    public const int PET_LIMIT = 3;

    public bool DisplayIndicator { 
        get => displayIndicator; 
        set {
            displayIndicator = value;
        } 
    }

    public bool CanInteract
    {
        get => canInteract;
        set
        {
            canInteract = value;
        }
    }

    public bool GetCanInteract()
    {
        return canInteract;
    }

    //Stamina, which serves the same purpose as time
    public TMP_Text staminaDisplay;
    public int playerStamina = 0;
    //int maxPlayerStamina = 100;

    public Image timeRadial;
    public Image timeRadial2;
    public Image timeRadial3;

    /*public int GetMaxPlayerStamina()
    {
        return maxPlayerStamina;
    }*/

    //boolean for whether or not the player is talking
    public bool isTalking;

    public int playerGold;

	public GameObject player;
	public PlayerInventoryManager playerInventoryManager;

    Inventory playerInventory;

    //List of interactable objects
    [SerializeField]
    List<InteractableObjects> objects = new List<InteractableObjects>();

    //Random array of DialogueManagers to handle NPC Dialogue
    InteractableObjects[] objectsArray = new InteractableObjects[100];

    public FarmingTutorial farmingTutorial;

    [SerializeField]
    Bed bed;

	public Menu menu;


	private void Start()
    {
		menu = GameObject.Find("Menus").GetComponent<Menu>();

        playerMovement = GetComponent<PlayerMovement>();

        //playerStamina = maxPlayerStamina;
        playerStamina = 100;

        timeRadial = GameObject.Find("TimeRadial").GetComponent<Image>();
        timeRadial2 = GameObject.Find("TimeRadial2").GetComponent<Image>();
        timeRadial3 = GameObject.Find("TimeRadial3").GetComponent<Image>();

        timeRadial2.fillAmount = 0;
        timeRadial3.fillAmount = 0;

        //playerInventoryManager = player.GetComponent<PlayerInventoryManager>();
        playerInventoryManager = gameObject.GetComponent<PlayerInventoryManager>();
        playerInventory = playerInventoryManager.inv;

        //playerInventory = farmManager.GetComponent<FarmManager>().playerInventory;
        //staminaDisplay.text = $"Stamina: {playerStamina}";

        //Gets all interactable objectss and saves them
        objectsArray = FindObjectsOfType<InteractableObjects>();

        for (int i = 0; i < objectsArray.Length; i++)
        {
            if (objectsArray[i] != null)
            {
                objects.Add(objectsArray[i]);

                if (objects[objects.Count - 1].GetComponent<Bed>() != null)
                {
                    bed = objects[objects.Count - 1].GetComponent<Bed>();
                }
            }
        }

        farmingTutorial = FindObjectOfType<FarmingTutorial>();

        if (farmingTutorial == null)
        {
            StartPlayer();
        }

        if (GlobalGameSaving.Instance != null)
        {
            if (GlobalGameSaving.Instance.loadingSave == true)
            {
                playerGold = GlobalGameSaving.Instance.gold;
                SetStamina(GlobalGameSaving.Instance.stamina);
            }
            else
            {
                if (ScenePersistence.Instance != null)
                {
                    playerGold = ScenePersistence.Instance.gold;
                    SetStamina(ScenePersistence.Instance.stamina);
                }
            }
        }
    }

    public void SavePlayer(string what)
    {
        if (what == "persist")
        {
            ScenePersistence.Instance.stamina = playerStamina;
            ScenePersistence.Instance.gold = playerGold;
        }
        else if (what == "save")
        {
            GlobalGameSaving.Instance.stamina = playerStamina;
            GlobalGameSaving.Instance.gold = playerGold;
        }
    }

    void OnEatFood()
    {
        Item heldItem = playerInventoryManager.GetHeldItem();

        if (canInteract && playerInventory.isShown == false && heldItem != null)
        {
            if (heldItem.isEdible == true)
            {
                SetStamina(playerStamina + heldItem.staminaToRestore);
                playerInventoryManager.RemoveHeldItems(1);

                if (farmingTutorial != null)
                {
                    if (farmingTutorial.tutorialBools[10] == true)//(farmingTutorial.harvestedAfter == true)
                    {
                        farmingTutorial.tutorialBools[12] = true;//farmingTutorial.eatingAfter = true;
                        GlobalGameSaving.Instance.tutorialBools[12] = farmingTutorial.tutorialBools[12];
                    }
                }
            }
        }
    }

    private void Update()
    {
        playerPosition = transform.position;
        //var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        focusTilePosition = new Vector3Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y), 0);

        var indicatorPos = focusTilePosition;
        if (displayIndicator && canInteract)
            indicatorPos.z = 0;
        else
            indicatorPos.z = 11;

        //indicator.position = Vector3.Slerp(indicator.position, indicatorPos, Time.deltaTime * 25);
        indicator.position = indicatorPos;

        Debug.DrawLine(playerPosition + interactionOffset, focusTilePosition);

        if(Vector2.Distance(playerPosition + interactionOffset, (Vector2Int)focusTilePosition) < maxInteractionDistance)
        {
            indicatorImage.color = activeColor;
            interactInRange = true;
        } else
        {
            indicatorImage.color = inactiveColor;
            interactInRange = false;
        }

		Item heldItem = playerInventoryManager.GetHeldItem();
		int heldItemAmount = playerInventoryManager.GetHeldItemAmount();

        timeRadial.fillAmount = Mathf.Lerp(timeRadial.fillAmount, (float)playerStamina / 100, 10 * Time.deltaTime);
        if (playerStamina > 100 && playerStamina <= 200)
        {
            timeRadial2.fillAmount = Mathf.Lerp(timeRadial2.fillAmount, (float)(playerStamina % 100) / 100, 10 * Time.deltaTime);
        }
        else if (playerStamina > 200)
        {
            timeRadial3.fillAmount = Mathf.Lerp(timeRadial3.fillAmount, (float)(playerStamina % 100) / 100, 10 * Time.deltaTime);
        }

        if (playerStamina < 101)
        {
            timeRadial2.fillAmount = Mathf.Lerp(timeRadial2.fillAmount, 0, 10 * Time.deltaTime);
        }
        else if (playerStamina < 201)
        {
            timeRadial3.fillAmount = Mathf.Lerp(timeRadial3.fillAmount, 0, 10 * Time.deltaTime);
        }

        staminaDisplay.text = $"{playerStamina}";

        petCount = FindObjectsOfType<BasicPet>().ToList().Count;

        if (petCount <= PET_LIMIT - 1)
        {
            ableToPlacePet = true;
            //petCount = oldPetCount;
        }
        else
        {
            ableToPlacePet = false;
        }

        //StartCoroutine(InteractionChecker());
        //InteractionChecker();
    }

    /// <summary>
    /// Small method for stopping the player during dialogue, which apparently breaks Fungus
    /// </summary>
    public void StopPlayer()
    {
        isTalking = true;
        CanInteract = false;
        playerMovement.Frozen = true;
        canInteract = false;
    }

    public void StartPlayer()
    {
        isTalking = false;
        CanInteract = true;
        playerMovement.Frozen = false;
        canInteract = true;
    }

    void OnInteraction()//private void CheckInteraction()
    {
        //goes through all interactable objects
        for (int i = 0; i < objects.Count; i++)
        {
            Collider2D col = objects[i].gameObject.GetComponent<Collider2D>();
            Vector2 closestColliderPoint = col.ClosestPoint(gameObject.transform.position);
            //Sees how close the player is to them
            float distance = Vector2.Distance(gameObject.transform.position, closestColliderPoint);//objects[i].gameObject.transform.position);


            //1 seems like a fine number
            if (distance <= 0.5f && objects[i].enabled == true)
            {
                //switch on name to see what it is
                switch (objects[i].name)
                {
                    case "npc":
                        Debug.Log("Interact update");
                        objects[i].gameObject.GetComponent<NPCManager>().MyFlowchart.ExecuteBlock("Start");
                        break;
                    case "shipping bin":
                        menu.OpenShippingBin();
                        break;
                    case "bed":
                        menu.OpenBed();
                        break;
                    case "Shop":
                        menu.OpenShop();
                        break;
                    case "chest":
						//if the player is holding a hoe
						if(playerInventoryManager.heldItem != null && playerInventoryManager.heldItem.name.ToLower().Contains("sickle"))
						{
							//Destroy the chest.
							objects[i].gameObject.GetComponent<InventoryEntity>().DestroyMe();
							objects.Remove(objects[i]);
							i--;
						}
						else
						{
							menu.OpenExternalInventory(objects[i].gameObject);
						}
                        break;
                    default:
                        break;
                }
            }
        }

        if (interactInRange == true)
        {
            string itemName = "";

            Dictionary<Vector3Int, Tile> mushroomsAndTiles = farmManager.GetComponent<FarmManager>().mushroomsAndTiles;

            Item heldItem = playerInventoryManager.GetHeldItem();
            int heldItemAmount = playerInventoryManager.GetHeldItemAmount();
			//if (playerInventory.HeldItem != null)
			itemName = heldItem == null ? "" : heldItem.name;

            //gets rid of the item if the stack is empty
            if (heldItemAmount <= 0)
            {
                playerInventoryManager.RemoveHeldItems(heldItemAmount);//DeleteHeldItemStack();
            }

            // Get Whatever input
            if (itemName != "" && isTalking == false)//if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0) && itemName != "" && isTalking == false)//if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0) && itemName != "" && isTalking == false)
            {
                heldItem = playerInventoryManager.GetHeldItem();
                heldItemAmount = playerInventoryManager.GetHeldItemAmount();

                //Debug.Log("LEP2738 HELD: " + heldItem.name);
                if (mushroomsAndTiles.ContainsKey(focusTilePosition))
                {
                    if (heldItemAmount > 0 && itemName.Contains("Shroom") && mushroomsAndTiles[focusTilePosition].isTilled == true && mushroomsAndTiles[focusTilePosition].hasPlant == false)//if(farmManager.GetComponent<FarmManager>().playerInventory.HeldItem.Amount > 0 && itemName.Contains("Shroom"))
                    {
                        Debug.Log("Plant One");
                        playerInventoryManager.RemoveHeldItems(1);
                    }


                    //before actually doing interaction, deduct player stamina accordingly
                    //switch on the four main item types, then some default value for everything else
                    if (itemName.Contains("Shroom") && mushroomsAndTiles[focusTilePosition].isTilled == true && mushroomsAndTiles[focusTilePosition].hasPlant == false)
                    {
                        ReduceStamina(heldItem.staminaUsed);
                    }
                    else if (itemName == "Sickle" && mushroomsAndTiles[focusTilePosition].hasPlant == true)
                    {
                        ReduceStamina(heldItem.staminaUsed);
                    }
                    else if (itemName == "Watering Can" && mushroomsAndTiles[focusTilePosition].isTilled == true && mushroomsAndTiles[focusTilePosition].isMoist == false)
                    {
                        ReduceStamina(heldItem.staminaUsed);
                    }
                    else if (itemName == "Hoe" && mushroomsAndTiles[focusTilePosition].isTilled == false)
                    {
                        ReduceStamina(heldItem.staminaUsed);
                    }
                }
                else if (heldItem != null && itemName.Contains("Shroom") == false && itemName != "Hoe" && itemName != "Watering Can" && itemName != "Sickle")
                {
                    ReduceStamina(heldItem.staminaUsed);
                }

                if (itemName.Contains("Pet") && !itemName.Contains("Petrified") && ableToPlacePet == true)
                {
                    Vector3 position = this.gameObject.transform.position;
                    position.x -= 1.5f;
                    heldItem.itemObj.transform.localPosition = position;

                    Vector3 pos = new Vector3(focusTilePosition.x, focusTilePosition.y, focusTilePosition.z);
                    Instantiate(heldItem.itemObj, pos, Quaternion.identity);
                    playerInventoryManager.RemoveHeldItems(1);

                    petCount++;
                }
				else if(heldItem != null && itemName.ToLower().Contains("chest"))
				{
					Vector3 position = focusTilePosition;
					Debug.Log(heldItem == null);
					Debug.Log(position == null);
					GameObject newChest = Instantiate(heldItem.itemObj, position, Quaternion.identity);
					objects.Add(newChest.GetComponent<InteractableObjects>());
					playerInventoryManager.RemoveHeldItems(1);
				}
				

                farmManager.TileInteract(focusTilePosition, itemName);
            }
        }

    }

    public int PlayerStamina => playerStamina;

    public void SetStamina(int stamina)
    {
        playerStamina = stamina;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(focusTilePosition, Vector3.one);
		
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(playerPosition, Vector3.one);
        Gizmos.DrawWireSphere(playerPosition, 0.5f);
    }

    //Simple function for reducing stamina. IF anything needs to be changed, change it here and only here
    public void ReduceStamina(int amount)
    {
        playerStamina -= amount;
    }
}
