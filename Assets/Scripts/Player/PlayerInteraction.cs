using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    [SerializeField]
    private FarmManager farmManager;

    private PlayerMovement playerMovement;

    private Vector3Int focusTile;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        var facing = playerMovement.Facing;
        var pos = transform.position;
        focusTile = new Vector3Int((int)(pos.x + facing.x), (int)(pos.y + facing.y), 0);
        Debug.DrawRay(transform.position, facing, Color.red);


        // Get Whatever input
        if (Input.GetKeyDown(KeyCode.F))
        {
            farmManager.TileInteract(focusTile, "till");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(focusTile, Vector3.one);
    }
}
