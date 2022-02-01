using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmManager : MonoBehaviour
{
    //Tilemap from the thing it is one, to keep track of crops
    public Tilemap farmField;
    List<Transform> plantedMushrooms = new List<Transform>();

    //Reference to the Mushroom Prefab for creating new mushrooms
    /*public*/ GameObject mushroomPrefab;
    public MushroomManager mushroomManager;

    //Testing only; attempt at making a dictionary
    //The key is the position of a tile, and the mushroom is that instance of the script it's supposed to kep track of
    //public Dictionary<Vector3Int, Mushrooms> mushroomsAndTiles = new Dictionary<Vector3Int, Mushrooms>();
    public Dictionary<Vector3Int, Tile> mushroomsAndTiles = new Dictionary<Vector3Int, Tile>();

    //EmptyTilePrefab
    public Tile tilePrefab;

    //Creating an inventory; will probably need to be a reference later
    public Inventory playerInventory = new Inventory();

    // Start is called before the first frame update
    void Start()
    {

        //Can be used to figure out where the (0, 0) of the tilemap is, which could be useful
        Debug.Log($"The origin is: {farmField.origin}");
        //The bottom left point of a tile is its actual coordinates
        Debug.Log($"I am the tile: {farmField.GetTile(new Vector3Int(-11, 3, 0))}");

        //Making a random set of tiles
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector3Int cropPos = new Vector3Int(i, j, 0);
                Tile testTile = Instantiate(tilePrefab, cropPos, Quaternion.identity, transform);

                mushroomsAndTiles.Add(cropPos, testTile);
                farmField.SetTile(cropPos, testTile.tileSprite);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        //When mushroom destroys itself, the tile stays
        //Check the dictionary to see if the associated script still exists
        //if it doesn't, set tile to null
        foreach (KeyValuePair<Vector3Int, Tile> shroom in mushroomsAndTiles)
        {
            if(shroom.Value == null)
            {
                farmField.SetTile(shroom.Key, null);
            }
        }
    }

    public void TileInteract(Vector3Int tile, string tool, string mushID = "null")
    {
        Debug.Log("Do interaction! @" + tile);

        if (!mushroomsAndTiles.ContainsKey(tile)) return;

        // Do interaction
        //Tilling field
        if (tool == "till" && mushroomsAndTiles[tile].isTilled == false)
        {
            mushroomsAndTiles[tile].isTilled = true;
            Debug.Log($"Is the tile at {tile} tilled? : {mushroomsAndTiles[tile].isTilled}");
        }
        //planting
        if (tool == "seed" && mushroomsAndTiles[tile].isTilled == true)
        {
            //has plant is true, destroys tile and removes from dictionary, and puts mushroom in its place
            Destroy(mushroomsAndTiles[tile]);
            mushroomsAndTiles.Remove(tile);

            //setting mushroomPrefab based on the type of mushroom plants
            if(mushID != "null" && mushroomManager.mushroomVariants.ContainsKey(mushID))
            {
                mushroomPrefab = mushroomManager.mushroomVariants[mushID];
            }

            //Since mushroom is determined by a non null string, if the string is nonsense
            if (mushroomPrefab != null)
            {
                GameObject newMushroom = Instantiate(mushroomPrefab, tile, Quaternion.identity, transform);
                mushroomsAndTiles.Add(tile, newMushroom.GetComponent<Tile>());
                farmField.SetTile(tile, newMushroom.GetComponent<Tile>().tileSprite);
                mushroomsAndTiles[tile].hasPlant = true;
                Debug.Log($"You just planted a {mushroomPrefab.GetComponent<Mushrooms>().ID}");
            }
            else
            {
                Debug.Log("There was nothing to plant");
            }
        }
        //Watering
        if (tool == "watering can" && mushroomsAndTiles[tile].hasPlant == true)
        {
            //Doesn't care if the plant has already been watered; just cares that there's a plant
            mushroomsAndTiles[tile].isMoist = true;
            Debug.Log($"Is the tile at {tile} watered? : {mushroomsAndTiles[tile].isMoist}");
        }
        //Harvesting
        //Might cause problems if the hasPlant of that tile is not set back to false, even though the tile itself ceases to exist
        if (tool == "sickle" && mushroomsAndTiles[tile].hasPlant == true)
        {
            //convert tile to mushroom
            Mushrooms harvestShroom = (Mushrooms)mushroomsAndTiles[tile];
            if (harvestShroom.growthStage >= harvestShroom.GetMaxGrowthStage())
            {
                //Destroy mushroom and add to inventory
                Destroy(mushroomsAndTiles[tile]);
                mushroomsAndTiles.Remove(tile);
                farmField.SetTile(tile, null);
                ItemStack itemToAdd = new ItemStack(harvestShroom.mushroomItem, 1);
                playerInventory.AddItems(itemToAdd);
            }
        }
    }

    public void SpreadMushroom()
    {
        Debug.Log("Beginning of SpreadMushroom");

        //The bounds of the crop field
        int leftBound = farmField.cellBounds.x;
        int bottomBound = farmField.cellBounds.y;

        int rightBound = leftBound + farmField.cellBounds.size.x;
        int topBound = farmField.cellBounds.size.y;

        //Nested For Loop, going through the x bound and y bounds of farmfield
        for (int x = leftBound; x < rightBound; x++)
        {
            for (int y = bottomBound; y < topBound; y++)
            {
                Vector3Int tileToTest = new Vector3Int(x, y, 0);
                
                //Calls gettile at that point
                if (farmField.GetTile(tileToTest) != null && mushroomsAndTiles.ContainsKey(tileToTest) == true)
                {

                    //Casting all tiles to Mushrooms in order to access Mushrooms only variables
                    //Changed GetComponents to tiles
                    //Changed MushroomTile to TileBase since Unity wouldn't stpo crying about it
                    //Might need new solution if not proper

                    if (mushroomsAndTiles[tileToTest].GetComponent<Mushrooms>() != null)
                    {
                        Mushrooms compareShroom = (Mushrooms)mushroomsAndTiles[tileToTest];

                        mushroomPrefab = mushroomManager.mushroomVariants[compareShroom.ID];

                        Debug.Log($"Found a tile at {tileToTest}!");
                        Debug.Log($"compareShroom is a {compareShroom.ID}!");
                        Debug.Log($"compareShroom is at stage {compareShroom.growthStage} and it's max is {compareShroom.GetMaxGrowthStage()}");

                        //If it exists, get the mushroom script it's attached to
                        //see if it's fully grown
                        if (compareShroom.growthStage >= compareShroom.GetMaxGrowthStage())
                        {
                            Debug.Log("Inside of spreading");

                            Vector3Int above = new Vector3Int(tileToTest.x, tileToTest.y + 1, 0);
                            Vector3Int below = new Vector3Int(tileToTest.x, tileToTest.y - 1, 0);
                            Vector3Int left = new Vector3Int(tileToTest.x - 1, tileToTest.y, 0);
                            Vector3Int right = new Vector3Int(tileToTest.x + 1, tileToTest.y, 0);


                            //If it is, go back to tile, search four adjacent spaces to see if they're within bounds and gettile is null, meaning they're empty
                            //If yes, instantiate mushroom prefab, set it's tile to that empty space, and add it as child to farmfield
                            //Change is null to has plant
                            if (mushroomsAndTiles[above].hasPlant == false && above.y <= topBound)//if (farmField.GetTile(above) == null && above.y <= topBound)
                            {
                                Debug.Log("Above");
                                mushroomsAndTiles[above] = Instantiate(mushroomPrefab, above, Quaternion.identity).GetComponent<Tile>();
                                farmField.SetTile(above, mushroomsAndTiles[above].tileSprite);
                                mushroomsAndTiles[above].transform.parent = this.transform;
                                mushroomsAndTiles[above].hasPlant = true;
                            }

                            if (mushroomsAndTiles[below].hasPlant == false && below.y >= bottomBound)//if (farmField.GetTile(below) == null && below.y >= bottomBound)
                            {
                                Debug.Log("Below");
                                mushroomsAndTiles[below] = Instantiate(mushroomPrefab, below, Quaternion.identity).GetComponent<Tile>();
                                farmField.SetTile(below, mushroomsAndTiles[below].tileSprite);
                                mushroomsAndTiles[below].transform.parent = this.transform;
                                mushroomsAndTiles[below].hasPlant = true;
                            }

                            if (mushroomsAndTiles[left].hasPlant == false && left.x >= leftBound)//if (farmField.GetTile(left) == null && left.x >= leftBound)
                            {
                                Debug.Log("Left");
                                mushroomsAndTiles[left] = Instantiate(mushroomPrefab, left, Quaternion.identity).GetComponent<Tile>();
                                farmField.SetTile(left, mushroomsAndTiles[left].tileSprite);
                                mushroomsAndTiles[left].transform.parent = this.transform;
                                mushroomsAndTiles[left].hasPlant = true;
                            }

                            if (mushroomsAndTiles[right].hasPlant == false && right.x <= rightBound)//if (farmField.GetTile(right) == null && right.x <= rightBound)
                            {
                                Debug.Log("Right");
                                mushroomsAndTiles[right] = Instantiate(mushroomPrefab, right, Quaternion.identity).GetComponent<Tile>();
                                farmField.SetTile(right, mushroomsAndTiles[right].tileSprite);
                                mushroomsAndTiles[right].transform.parent = this.transform;
                                mushroomsAndTiles[right].hasPlant = true;
                            }
                        }
                    }
                }
            }
        }

        Debug.Log("End of SpreadMushroom");
    }
}
