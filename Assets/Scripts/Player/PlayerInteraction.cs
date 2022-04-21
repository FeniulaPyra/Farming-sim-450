using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        timeRadial2.fillAmount = 0;
        timeRadial3.fillAmount = 0;

        playerInventory = farmManager.GetComponent<FarmManager>().playerInventory;
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
    }

    private void Update()
    {
        playerPosition = transform.position;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

        if (canInteract && playerInventory.isShown == false && playerInventory.HeldItem != null)// && isTalking == false)
        {
            //If you change item and it isn't edible, or if you stop holding down the key, reset eating
            if (playerInventory.HeldItem.Item.isEdible == true && Input.GetKeyDown(KeyCode.F))
            {

                SetStamina(playerStamina + playerInventory.HeldItem.Item.staminaToRestore);
                playerInventory.RemoveHeldItems(1);

                if (farmingTutorial.harvestedAfter == true)
                {
                    farmingTutorial.eatingAfter = true;
                }
            }

            if (interactInRange)
                CheckInteraction();
        }

        //Stop all player movement when in dialogue
        if (isTalking == true)
        {
            CanInteract = false;
            playerMovement.Frozen = true;
            canInteract = false;
        }
        else
        {
            CanInteract = true;
            playerMovement.Frozen = false;
            canInteract = true;
        }

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

        StartCoroutine(InteractionChecker());
    }

    private void CheckInteraction()
    {
        string itemName = "";
        
        Dictionary<Vector3Int, Tile> mushroomsAndTiles = farmManager.GetComponent<FarmManager>().mushroomsAndTiles;

        //if (playerInventory.HeldItem != null)
		itemName = playerInventory.HeldItem.Item.name;

        //gets rid of the item if the stack is empty
        if (playerInventory.HeldItem.Amount <= 0)
        {
            playerInventory.DeleteHeldItemStack();
        }
        /*if (playerInventory.HeldItem.Item != null)
        {
            if (playerInventory.HeldItem.Amount <= 0)
            {
                playerInventory.DeleteHeldItemStack();
            }
        }*/

        // Get Whatever input
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0) && itemName != "" && isTalking == false)
        {
            Item heldItem = playerInventory.HeldItem.Item;

            if (mushroomsAndTiles.ContainsKey(focusTilePosition))
            {
                if (playerInventory.HeldItem.Amount > 0 && itemName.Contains("Shroom") && mushroomsAndTiles[focusTilePosition].isTilled == true && mushroomsAndTiles[focusTilePosition].hasPlant == false)//if(farmManager.GetComponent<FarmManager>().playerInventory.HeldItem.Amount > 0 && itemName.Contains("Shroom"))
                {
                    Debug.Log("Plant One");
					//ItemStack minusOne = new ItemStack(playerInventory.HeldItem.Item, -1);
					//playerInventory.HeldItem.CombineStacks(minusOne, playerInventory.STACK_SIZE);
					playerInventory.RemoveHeldItems(1);
                }
                /*else if (playerInventory.HeldItem.Item.isEdible == true)
                {
                    Debug.Log("Restore One");
                    ItemStack minusOne = new ItemStack(playerInventory.HeldItem.Item, -1);
                    playerInventory.HeldItem.CombineStacks(minusOne, playerInventory.STACK_SIZE);
                    SetStamina(playerStamina + playerInventory.HeldItem.Item.staminaToRestore);
                }*/

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
            /*else if (playerInventory.HeldItem.Item.isEdible == true)
            {
                ItemStack minusOne = new ItemStack(playerInventory.HeldItem.Item, -1);
                playerInventory.HeldItem.CombineStacks(minusOne, playerInventory.STACK_SIZE);
                SetStamina(playerStamina + playerInventory.HeldItem.Item.staminaToRestore);
            }*/
            else if(itemName.Contains("Shroom") == false && itemName != "Hoe" && itemName != "Watering Can" && itemName != "Sickle")
            {
                ReduceStamina(playerInventory.HeldItem.Item.staminaUsed);
            }

            if (itemName.Contains("Pet"))
            {
                Vector3 position = this.gameObject.transform.position;
                position.x -= 1.5f;
                heldItem.itemObj.transform.localPosition = position;
                Instantiate(heldItem.itemObj);
                playerInventory.RemoveHeldItems(1);
            }


            //staminaDisplay.text = $"Stamina: {playerStamina}";
            //timeRadial.fillAmount = (float)playerStamina/100;

            farmManager.TileInteract(focusTilePosition, itemName);
        }

    }

    /// <summary>
    /// IEnumerator appears to be necessary
    /// The spacebar needed to check for interaction in the first place, if the player is interacting with an NPC, also triggers to WaitForInput in Dialogue Manager
    /// That means it waits half a second before moving onto the next piece of dialogue without warning
    /// This pauses for a very brief amount of time so that first spacebar press doesn't register for the WaitForInput
    /// </summary>
    /// <returns></returns>
    public IEnumerator InteractionChecker()
    {
        //checks if the player hits the key for interaction
        //Debug.Log($"Interact talking: {playerInteraction.isTalking}");
        if (Input.GetKeyDown(KeyCode.Space) && isTalking == false || Input.GetKeyDown(KeyCode.Mouse0) && isTalking == false)
        {
            KeyCode dialoguePress = KeyCode.Space;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dialoguePress = KeyCode.Space;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                dialoguePress = KeyCode.Mouse0;
            }

            //goes through all interactable objects
            for (int i = 0; i < objects.Count; i++)
            {
                //Sees how close the player is to them
                float distance = Vector2.Distance(gameObject.transform.position, objects[i].gameObject.transform.position);

                //1 seems like a fine number
                if (distance <= 1.0f)
                {
                    //switch on name to see what it is
                    switch (objects[i].name)
                    {
                        case "npc":
                            Debug.Log("Interact update");
                            yield return new WaitForSeconds(0.025f);
                            //StartCoroutine(objects[i].gameObject.GetComponent<DialogueManager>().PlayDialogue(objects[i].gameObject.GetComponent<DialogueManager>().convoID));
                            StartCoroutine(objects[i].gameObject.GetComponent<DialogueManager>().PlayDialogue(objects[i].gameObject.GetComponent<DialogueManager>().convoID, dialoguePress));
                            break;
                        case "shipping bin":
							menu.OpenShippingBin();
							//objects[i].gameObject.GetComponent<ShippingBin>().PutItemInBin();
                            break;
                        case "bed":
                            objects[i].gameObject.GetComponent<Bed>().SetTextObjectsActive(true);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //Escape is mapped to the pause menu
        //Allows the player to back out of the bed menu, if they accidentally interacted with it
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            bed.SetTextObjectsActive(false);
        }

        //Debug.Log($"Distance to player and {objects[0].gameObject.name} is {Vector2.Distance(gameObject.transform.position, objects[0].gameObject.transform.position)}");
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

        /*
        var inTile = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(inTile, Vector3.one);
        */

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
