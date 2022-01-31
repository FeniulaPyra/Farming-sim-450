using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    //A set of variables all farming tiles would need to know in some capacity

    //Has this land been tilled
    public bool isTilled = false;
    //Whether or not it's been watered
    public bool isMoist = false;
    //Is there a plant here
    public bool hasPlant = false;
    //Solely for Testing purposes; currently a placeholder
    //public Tile mushroomTile;
    public TileBase tileSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
