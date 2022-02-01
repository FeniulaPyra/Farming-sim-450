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

    private PlayerMovement playerMovement;

    private Vector3Int focusTilePosition;

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
        var pos = transform.position;
        focusTilePosition = new Vector3Int(Mathf.RoundToInt(pos.x + facing.x), Mathf.RoundToInt(pos.y + facing.y), 0);
        Debug.DrawRay(transform.position, facing, Color.red);


        // Get Whatever input
        if (Input.GetKeyDown(KeyCode.F))
        {
            farmManager.TileInteract(focusTilePosition, "till");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            farmManager.TileInteract(focusTilePosition, "seed", "Shiitake");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            farmManager.TileInteract(focusTilePosition, "watering can");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            farmManager.TileInteract(focusTilePosition, "sickle");
        }

        var indicatorPos = focusTilePosition;
        if (displayIndicator)
            indicatorPos.z = 0;
        else
            indicatorPos.z = 11;

        indicator.position = Vector3.Slerp(indicator.position, indicatorPos, Time.deltaTime * 25);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(focusTilePosition, Vector3.one);

        var inTile = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(inTile, Vector3.one);
    }
}
