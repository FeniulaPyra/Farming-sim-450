using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private PlayerMovement playerMovement;

    private Vector3Int focusTilePosition;
    private Vector3Int currentPlayerPos;

    public bool DisplayIndicator { 
        get => displayIndicator; 
        set {
            displayIndicator = value;
        } 
    }

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        var facing = playerMovement.Facing;
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

        indicator.position = Vector3.Slerp(indicator.position, indicatorPos, Time.deltaTime * 25);

        CheckInteraction();
    }

    private void CheckInteraction()
    {
        string itemName = farmManager.GetComponent<FarmManager>().playerInventory.HeldItem.Item.name;

        // Get Whatever input
        if (Input.GetKeyDown(KeyCode.F))
        {
            farmManager.TileInteract(focusTilePosition, itemName);
        }

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
}
