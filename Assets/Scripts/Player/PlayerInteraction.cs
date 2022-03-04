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
    private Transform indicator;

    [SerializeField]
    private bool displayIndicator;

    [SerializeField]
    private Vector3 interactionOffset;

    [SerializeField]
    private float maxDistanceToInteraction = 1.25f;

    private PlayerMovement playerMovement;

    private Vector3Int focusTilePosition;
    private Vector3Int currentPlayerPos;

    public bool DisplayIndicator { 
        get => displayIndicator; 
        set {
            displayIndicator = value;
        } 
    }

    //Stamina, which serves the same purpose as time
    //public TMP_Text staminaDisplay;
    public int playerStamina = 0;
    int maxPlayerStamina = 100;

    public Image TimeRadial;

    public int GetMaxPlayerStamina()
    {
        return maxPlayerStamina;
    }

    //boolean for whether or not the player is talking
    public bool isTalking;

    public int playerGold;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        playerStamina = maxPlayerStamina;

        //staminaDisplay.text = $"Stamina: {playerStamina}";
    }

    private void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var facing = mousePos - transform.position;

        if (Mathf.Abs(facing.x) > Mathf.Abs(facing.y))
        {
            facing.y = 0;
            facing.x = Mathf.Abs(facing.x) < 1 ? facing.x : 1 * Mathf.Sign(facing.x);
        } else
        {
            facing.x = 0;
            facing.y = Mathf.Abs(facing.y) < 1 ? facing.y : 1 * Mathf.Sign(facing.y);
        }

        //Debug.DrawLine(transform.position, mousePos);
        //Debug.DrawRay(transform.position, facing, Color.red);

        var pos = transform.position + interactionOffset;
        var rPos = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));

        if (Vector2.Distance(pos, rPos) < 0.5f && (currentPlayerPos.x != rPos.x || currentPlayerPos.y != rPos.y))
        {
            currentPlayerPos = new Vector3Int((int)rPos.x, (int)rPos.y, 0);
        }

        focusTilePosition = new Vector3Int(Mathf.RoundToInt(currentPlayerPos.x + facing.x), Mathf.RoundToInt(currentPlayerPos.y + facing.y), 0);
        var indicatorPos = focusTilePosition;
        if (displayIndicator)
            indicatorPos.z = 0;
        else
            indicatorPos.z = 11;

        var targetPos = focusTilePosition;
        if (Vector2.Distance(new Vector2(targetPos.x, targetPos.y), pos) > maxDistanceToInteraction)
            targetPos = new Vector3Int((int)rPos.x, (int)rPos.y, targetPos.z);

        indicator.position = Vector3.Lerp(indicator.position, targetPos, Time.deltaTime * 25);
        focusTilePosition = targetPos;

        CheckInteraction();

        TimeRadial.fillAmount = Mathf.Lerp(TimeRadial.fillAmount, (float)playerStamina / 100, 10 * Time.deltaTime);
    }

    private void CheckInteraction()
    {
		string itemName = "";
        Inventory playerInventory = farmManager.GetComponent<FarmManager>().playerInventory;
        Dictionary<Vector3Int, Tile> mushroomsAndTiles = farmManager.GetComponent<FarmManager>().mushroomsAndTiles;

        if (playerInventory.HeldItem != null)
			itemName = playerInventory.HeldItem.Item.name;

        //gets rid of the item if the stack is empty
        if (playerInventory.HeldItem != null)
        {
            if (playerInventory.HeldItem.Amount <= 0)
            {
                playerInventory.DeleteHeldItemStack();
            }
        }

        // Get Whatever input
        if (Input.GetKeyDown(KeyCode.Space) && itemName != "" && isTalking == false)
        {
            if (mushroomsAndTiles.ContainsKey(focusTilePosition))
            {
                if (playerInventory.HeldItem.Amount > 0 && itemName.Contains("Shroom") && mushroomsAndTiles[focusTilePosition].isTilled == true && mushroomsAndTiles[focusTilePosition].hasPlant == false)//if(farmManager.GetComponent<FarmManager>().playerInventory.HeldItem.Amount > 0 && itemName.Contains("Shroom"))
                {
                    ItemStack minusOne = new ItemStack(playerInventory.HeldItem.Item, -1);
                    playerInventory.HeldItem.CombineStacks(minusOne, playerInventory.STACK_SIZE);
                }

                //before actually doing interaction, deduct player stamina accordingly
                //switch on the four main item types, then some default value for everything else
                if (itemName.Contains("Shroom") && mushroomsAndTiles[focusTilePosition].isTilled == true && mushroomsAndTiles[focusTilePosition].hasPlant == false)
                {
                    ReduceStamina(2);
                }
                else if(itemName == "Sickle" && mushroomsAndTiles[focusTilePosition].hasPlant == true)
                {
                    ReduceStamina(6);
                }
                else if(itemName == "Watering Can" && mushroomsAndTiles[focusTilePosition].isTilled == true && mushroomsAndTiles[focusTilePosition].isMoist == false)
                {
                    ReduceStamina(3);
                }
                else if(itemName == "Hoe" && mushroomsAndTiles[focusTilePosition].isTilled == false)
                {
                    ReduceStamina(10);
                }
            }
            else if(itemName.Contains("Shroom") == false && itemName != "Hoe" && itemName != "Watering Can" && itemName != "Sickle")
            {
                ReduceStamina(5);
            }
            else if (mushroomsAndTiles.ContainsKey(focusTilePosition) == false && itemName == "Hoe")
            {
                Vector3Int cropPos = focusTilePosition;
                Tile testTile = Instantiate(farmManager.tilePrefab, cropPos, Quaternion.identity, transform);

                mushroomsAndTiles.Add(cropPos, testTile);
                //farmField.SetTile(cropPos, testTile.tileSprite);
                testTile.tileSprite = testTile.sprites[0];
                farmManager.tillableGround.SetTile(cropPos, testTile.tileSprite);
            }

            //staminaDisplay.text = $"Stamina: {playerStamina}";
            TimeRadial.fillAmount = (float)playerStamina/100;

            farmManager.TileInteract(focusTilePosition, itemName);
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

        /*
        var inTile = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(inTile, Vector3.one);
        */

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(currentPlayerPos, Vector3.one);
        Gizmos.DrawWireSphere(currentPlayerPos, 0.5f);
    }

    //Simple function for reducing stamina. IF anything needs to be changed, change it here and only here
    public void ReduceStamina(int amount)
    {
        playerStamina -= amount;
    }
}
