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
    //List of tiles to easily handle multiple mushroom growth stages
    public List<TileBase> sprites = new List<TileBase>();

    public Vector3Int position;

    private void Start()
    {
        tileSprite = sprites[0];
    }

    public SaveTile AsSaveTile()
    {
        return new SaveTile(isTilled, isMoist, hasPlant, position, sprites.IndexOf(tileSprite));
    }
}

[System.Serializable]
public class SaveTile
{
    public bool isTilled;
    public bool isMoist;
    public bool hasPlant;
    public Vector3Int position;
    public int spriteIndex;


    public SaveTile(bool isTilled, bool isMoist, bool hasPlant, Vector3Int position, int spriteIndex)
    {
        this.isTilled = isTilled;
        this.isMoist = isMoist;
        this.hasPlant = hasPlant;
        this.position = position;
        this.spriteIndex = spriteIndex;

        Debug.Log($"Sprite index is {this.spriteIndex}");
    }
}
