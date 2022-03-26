using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmManager : MonoBehaviour
{
    //Tilemap from the thing it is one, to keep track of crops
    public Tilemap farmField;
    public Tilemap tillableGround;
    List<Transform> plantedMushrooms = new List<Transform>();

    //Reference to the Mushroom Prefab for creating new mushrooms
    /*public*/
    GameObject mushroomPrefab;
    GameObject originalMushroom;
    public MushroomManager mushroomManager;
    public TileManager tileManager;

    public FarmingTutorial farmingTutorial;

    //Testing only; attempt at making a dictionary
    //The key is the position of a tile, and the mushroom is that instance of the script it's supposed to kep track of
    //public Dictionary<Vector3Int, Mushrooms> mushroomsAndTiles = new Dictionary<Vector3Int, Mushrooms>();
    public Dictionary<Vector3Int, Tile> mushroomsAndTiles = new Dictionary<Vector3Int, Tile>();

    public Tile this[Vector3Int key]
    {
        get
        {
            return mushroomsAndTiles[key];
        }
        set
        {
            mushroomsAndTiles[key] = value;
        }
    }


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
        for (int i = -5; i <= 5; i++)
        {
            for (int j = -9; j <= 1; j++)
            {
                Vector3Int cropPos = new Vector3Int(i, j, 0);
                Tile testTile = Instantiate(tilePrefab, cropPos, Quaternion.identity, transform);

                mushroomsAndTiles.Add(cropPos, testTile);
                //farmField.SetTile(cropPos, testTile.tileSprite);
                testTile.tileSprite = testTile.sprites[0];
                tillableGround.SetTile(cropPos, testTile.tileSprite);
            }
        }

        farmingTutorial = FindObjectOfType<FarmingTutorial>();

    }

    // Update is called once per frame
    void Update()
    {
		//When mushroom destroys itself, the tile stays
		//Check the dictionary to see if the associated script still exists
		//if it doesn't, set tile to null
		List<Vector3Int> deadPlants = new List<Vector3Int>();
		foreach (KeyValuePair<Vector3Int, Tile> shroom in mushroomsAndTiles)
		{
			if (shroom.Value == null)
            {
				farmField.SetTile(shroom.Key, null);
            }
        }

        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    tileManager.SaveFieldObjects();
        //
        //    Debug.Log("Saved");
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    tileManager.LoadFieldObjects(tileManager.fieldObjects);
        //
        //    Debug.Log("Loaded");
        //}

        //clears dead plants
        foreach (Vector3Int plantPosition in deadPlants)
		{
			mushroomsAndTiles[plantPosition] = Instantiate(tilePrefab, plantPosition, Quaternion.identity, transform);
		}
    }

    public void TileInteract(Vector3Int tile, string tool)
    {
        Debug.Log("Do interaction! @" + tile);
        //Debug.Log($"I am tilled: {mushroomsAndTiles[tile].isTilled}");

        if (!mushroomsAndTiles.ContainsKey(tile)) return;

        //force to all lowercase, just in case
        //tool = tool.ToLower();
        //Debug.Log($"I am the tool: {tool}");
        

        // Do interaction
        //Tilling field
        if (tool == "Hoe" && mushroomsAndTiles[tile].isTilled == false)
        {
            mushroomsAndTiles[tile].isTilled = true;
            //tilePrefab.tileSprite = tilePrefab.sprites[1];
            mushroomsAndTiles[tile].tileSprite = mushroomsAndTiles[tile].sprites[1];
            Debug.Log($"Hoe'd Ground; index is now {tilePrefab.sprites.IndexOf(tilePrefab.tileSprite)}");
            //tillableGround.SetTile(tile, tilePrefab.tileSprite);
            tillableGround.SetTile(tile, mushroomsAndTiles[tile].tileSprite);
            //Debug.Log($"Is the tile at {tile} tilled? : {mushroomsAndTiles[tile].isTilled}");
            if (farmingTutorial.tutorialStarted == true)
            {
                farmingTutorial.tilledAfter = true;
            }
        }
        //planting
        //If the item in question has mushroom in the name, you know you're planting. If the mushroom dictionary contains that mushroom, everything should wor
        //Might have to account for naming being similar, but not the same, which would throw things off.
        //Dictionary Keys are the IDs of the Mushrooms, which are all formatted like "Descriptor Shroom", so the item names must be the same
        if (tool.Contains("Shroom") && mushroomsAndTiles[tile].isTilled == true && mushroomsAndTiles[tile].hasPlant == false)
        //if (tool == "seed" && mushroomsAndTiles[tile].isTilled == true)
        {
            //see if the tile was already moist
            bool tempMoist = mushroomsAndTiles[tile].isMoist;

            //has plant is true, destroys tile and removes from dictionary, and puts mushroom in its place
            Destroy(mushroomsAndTiles[tile].gameObject);
            mushroomsAndTiles.Remove(tile);

            if (mushroomManager.mushroomVariants.ContainsKey(tool))
            {
                mushroomPrefab = mushroomManager.mushroomVariants[tool];
            }
            //setting mushroomPrefab based on the type of mushroom plants
            /*if(mushID != "null" && mushroomManager.mushroomVariants.ContainsKey(mushID))
            {
                mushroomPrefab = mushroomManager.mushroomVariants[tool];
            }*/

            //Since mushroom is determined by a non null string, if the string is nonsense
            if (mushroomPrefab != null)
            {
                GameObject newMushroom = Instantiate(mushroomPrefab, tile, Quaternion.identity, transform);
                mushroomsAndTiles.Add(tile, newMushroom.GetComponent<Tile>());

                farmField.SetTile(tile, newMushroom.GetComponent<Tile>().tileSprite);

                //Returned nothing. Issue with tile
                Debug.Log($"Planting Debug 3: {newMushroom.GetComponent<Mushrooms>().ID}");
                mushroomsAndTiles[tile].hasPlant = true;
                mushroomsAndTiles[tile].isTilled = true;
                mushroomsAndTiles[tile].isMoist = tempMoist;
                Debug.Log($"You just planted a {mushroomPrefab.GetComponent<Mushrooms>().ID}");

                if (farmingTutorial.tilledAfter == true)
                {
                    farmingTutorial.plantedAfter = true;
                }
            }
            else
            {
                Debug.Log("There was nothing to plant");
            }
        }
        //Watering
        if (tool == "Watering Can" && mushroomsAndTiles[tile].isTilled== true)
        {
            if (mushroomsAndTiles[tile].hasPlant == true)
            {
                //Doesn't care if the plant has already been watered; just cares that there's a plant
                mushroomsAndTiles[tile].isMoist = true;
                Debug.Log($"Is the tile at {tile} watered? : {mushroomsAndTiles[tile].isMoist}");
                tillableGround.SetTile(tile, tilePrefab.sprites[2]);

                if (farmingTutorial.plantedAfter == true)
                {
                    farmingTutorial.wateredAfter = true;
                }
            }
            else
            {
                //Doesn't care if the plant has already been watered; just cares that there's a plant
                mushroomsAndTiles[tile].isMoist = true;
                Debug.Log($"Is the tile at {tile} watered? : {mushroomsAndTiles[tile].isMoist}");
                //tilePrefab.tileSprite = tilePrefab.sprites[2];
                mushroomsAndTiles[tile].tileSprite = mushroomsAndTiles[tile].sprites[2];
                //tillableGround.SetTile(tile, tilePrefab.tileSprite);
                tillableGround.SetTile(tile, mushroomsAndTiles[tile].tileSprite);
            }
        }
        //Harvesting
        //Might cause problems if the hasPlant of that tile is not set back to false, even though the tile itself ceases to exist
        if (tool == "Sickle" && mushroomsAndTiles[tile].hasPlant == true)
        {
            //convert tile to mushroom
            Mushrooms harvestShroom = (Mushrooms)mushroomsAndTiles[tile];
            GameObject harvestShroomItem = harvestShroom.mushroomItem;

            //Destroy mushroom and add to inventory
            Destroy(mushroomsAndTiles[tile].gameObject);
			//mushroomsAndTiles.Remove(tile);
			//resets the tile;
			mushroomsAndTiles[tile] = Instantiate(tilePrefab, tile, Quaternion.identity, transform);
			farmField.SetTile(tile, null);
            tillableGround.SetTile(tile, tilePrefab.sprites[0]);


            if (harvestShroom.GetComponent<Mushrooms>().growthStage >= harvestShroom.GetComponent<Mushrooms>().GetMaxGrowthStage())
            {
                /*ItemStack itemToAdd = new ItemStack(harvestShroomItem, 1);
                Debug.Log("Added Item");
                playerInventory.AddItems(itemToAdd);*/

                //Instatiates the Prefab on the ground so that the player picks it up by walking over it
                for (int i = 0; i < 3; i++)
                {
                    GameObject TempItem = Instantiate(harvestShroomItem, tile, Quaternion.identity);// + new Vector3Int(0, -2, 0), Quaternion.identity);
                }

                mushroomsAndTiles[tile].isTilled = false;
            }
            else
            {
                /*ItemStack itemToAdd = new ItemStack(harvestShroomItem, 1);
                Debug.Log("Added Item");
                playerInventory.AddItems(itemToAdd);*/

                //Instatiates the Prefab on the ground so that the player picks it up by walking over it
                GameObject TempItem = Instantiate(harvestShroomItem, tile, Quaternion.identity);// + new Vector3Int(0, -2, 0), Quaternion.identity);

                mushroomsAndTiles[tile].isTilled = false;
            }

            if (farmingTutorial.sleptAfter == true)
            {
                farmingTutorial.harvestedAfter = true;
            }
        }
    }

    public void SpreadMushroom()
    {
		// = mushroomManager.mushroomVariants["Red Shroom"];

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

                        originalMushroom = mushroomManager.mushroomVariants[compareShroom.ID];
						mushroomPrefab = originalMushroom;

                        Debug.Log($"Found a tile at {tileToTest}!");
                        Debug.Log($"compareShroom is a {compareShroom.ID}!");
                        Debug.Log($"compareShroom is at stage {compareShroom.growthStage} and it's max is {compareShroom.GetMaxGrowthStage()}");

                        //If it exists, get the mushroom script it's attached to
                        //see if it's fully grown
                        if (compareShroom.growthStage >= compareShroom.GetMaxGrowthStage() && compareShroom.daysSinceFullyGrown >= 2)
                        {
                            Debug.Log("Inside of spreading");

                            Vector3Int above = new Vector3Int(tileToTest.x, tileToTest.y + 1, 0);
                            Vector3Int below = new Vector3Int(tileToTest.x, tileToTest.y - 1, 0);
                            Vector3Int left = new Vector3Int(tileToTest.x - 1, tileToTest.y, 0);
                            Vector3Int right = new Vector3Int(tileToTest.x + 1, tileToTest.y, 0);

                            int randomSpread = Random.Range(1, 100 + 1);
                            Debug.Log($"RandomSpread is: {randomSpread}");

                            //For Hybridization, change the below if statements
                            //The if stays, for regular spreading
                            //Add elses, which *should* call if there is something there
                            //Then inside the else, check if the other is part of a specific hybrid dictionary and if a hybrid can be made
                            //If a hybrid cannot be made, continue the loop and do nothing

                            //If the tile is in the dictionary, doesn't have a plant, and isn't outside of the bounds of the field
                            if (randomSpread >= 1 && randomSpread <= 25)
                            {
                                if (mushroomsAndTiles.ContainsKey(above) && mushroomsAndTiles[above].isTilled == true)// && above.y <= topBound)
                                {
                                    if (mushroomsAndTiles[above].hasPlant == false)
                                    {
                                        Debug.Log("Above");
                                        mushroomPrefab = originalMushroom;
                                        Destroy(mushroomsAndTiles[above].gameObject);
                                        mushroomsAndTiles[above] = Instantiate(mushroomPrefab, above, Quaternion.identity).GetComponent<Tile>();
                                        farmField.SetTile(above, mushroomsAndTiles[above].tileSprite);
                                        mushroomsAndTiles[above].transform.parent = this.transform;
                                        mushroomsAndTiles[above].hasPlant = true;
                                        mushroomsAndTiles[above].isTilled = true;

                                        if (farmingTutorial.shippedAfter == true)
                                        {
                                            farmingTutorial.spreadAfter = true;
                                        }
                                    }
                                    else
                                    {
                                        Debug.Log("Make Hybrid");

                                        Mushrooms aboveShroom = (Mushrooms)mushroomsAndTiles[above];

                                        if (compareShroom.hybridDictionary.ContainsKey(aboveShroom.ID))
                                        {
                                            mushroomPrefab = compareShroom.hybridDictionary[aboveShroom.ID];

                                            Destroy(mushroomsAndTiles[above].gameObject);

                                            mushroomsAndTiles[above] = Instantiate(mushroomPrefab, above, Quaternion.identity).GetComponent<Tile>();
                                            farmField.SetTile(above, mushroomsAndTiles[above].tileSprite);
                                            mushroomsAndTiles[above].transform.parent = this.transform;
                                            mushroomsAndTiles[above].hasPlant = true;
                                            mushroomsAndTiles[above].isTilled = true;

                                            if (farmingTutorial.spreadAfter == true)
                                            {
                                                farmingTutorial.hybridAfter = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else if (randomSpread >= 26 && randomSpread <= 50)
                            {
                                if (mushroomsAndTiles.ContainsKey(below) == true && mushroomsAndTiles[below].isTilled == true)// && below.y >= bottomBound)
                                {
                                    if (mushroomsAndTiles[below].hasPlant == false)
                                    {
                                        Debug.Log("Below");
                                        mushroomPrefab = originalMushroom;
                                        Destroy(mushroomsAndTiles[below].gameObject);
                                        mushroomsAndTiles[below] = Instantiate(mushroomPrefab, below, Quaternion.identity).GetComponent<Tile>();
                                        farmField.SetTile(below, mushroomsAndTiles[below].tileSprite);
                                        mushroomsAndTiles[below].transform.parent = this.transform;
                                        mushroomsAndTiles[below].hasPlant = true;
                                        mushroomsAndTiles[below].isTilled = true;

                                        if (farmingTutorial.shippedAfter == true)
                                        {
                                            farmingTutorial.spreadAfter = true;
                                        }
                                    }
                                    else
                                    {
                                        Debug.Log("Make Hybrid");

                                        Mushrooms belowShroom = (Mushrooms)mushroomsAndTiles[below];

                                        if (compareShroom.hybridDictionary.ContainsKey(belowShroom.ID))
                                        {
                                            mushroomPrefab = compareShroom.hybridDictionary[belowShroom.ID];

                                            Destroy(mushroomsAndTiles[below].gameObject);

                                            mushroomsAndTiles[below] = Instantiate(mushroomPrefab, below, Quaternion.identity).GetComponent<Tile>();
                                            farmField.SetTile(below, mushroomsAndTiles[below].tileSprite);
                                            mushroomsAndTiles[below].transform.parent = this.transform;
                                            mushroomsAndTiles[below].hasPlant = true;
                                            mushroomsAndTiles[below].isTilled = true;

                                            if (farmingTutorial.spreadAfter == true)
                                            {
                                                farmingTutorial.hybridAfter = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else if (randomSpread >= 51 && randomSpread <= 75)
                            {
                                if (mushroomsAndTiles.ContainsKey(left) && mushroomsAndTiles[left].isTilled == true)// && left.x >= leftBound)
                                {
                                    if (mushroomsAndTiles[left].hasPlant == false)
                                    {
                                        Debug.Log("Left");
                                        mushroomPrefab = originalMushroom;
                                        Destroy(mushroomsAndTiles[left].gameObject);
                                        mushroomsAndTiles[left] = Instantiate(mushroomPrefab, left, Quaternion.identity).GetComponent<Tile>();
                                        farmField.SetTile(left, mushroomsAndTiles[left].tileSprite);
                                        mushroomsAndTiles[left].transform.parent = this.transform;
                                        mushroomsAndTiles[left].hasPlant = true;
                                        mushroomsAndTiles[left].isTilled = true;

                                        if (farmingTutorial.shippedAfter == true)
                                        {
                                            farmingTutorial.spreadAfter = true;
                                        }
                                    }
                                    else
                                    {
                                        Debug.Log("Make Hybrid");

                                        Mushrooms leftShroom = (Mushrooms)mushroomsAndTiles[left];

                                        if (compareShroom.hybridDictionary.ContainsKey(leftShroom.ID))
                                        {
                                            mushroomPrefab = compareShroom.hybridDictionary[leftShroom.ID];

                                            Destroy(mushroomsAndTiles[left].gameObject);

                                            mushroomsAndTiles[left] = Instantiate(mushroomPrefab, left, Quaternion.identity).GetComponent<Tile>();
                                            farmField.SetTile(left, mushroomsAndTiles[left].tileSprite);
                                            mushroomsAndTiles[left].transform.parent = this.transform;
                                            mushroomsAndTiles[left].hasPlant = true;
                                            mushroomsAndTiles[left].isTilled = true;

                                            if (farmingTutorial.spreadAfter == true)
                                            {
                                                farmingTutorial.hybridAfter = true;
                                            }
                                        }
                                    }

                                }
                            }
                            else if (randomSpread >= 76 && randomSpread <= 100)
                            {
                                if (mushroomsAndTiles.ContainsKey(right) && mushroomsAndTiles[right].isTilled == true)// && right.x <= rightBound)
                                {
                                    if (mushroomsAndTiles[right].hasPlant == false)
                                    {
                                        Debug.Log("Right");
                                        mushroomPrefab = originalMushroom;
                                        Destroy(mushroomsAndTiles[right].gameObject);
                                        mushroomsAndTiles[right] = Instantiate(mushroomPrefab, right, Quaternion.identity).GetComponent<Tile>();
                                        farmField.SetTile(right, mushroomsAndTiles[right].tileSprite);
                                        mushroomsAndTiles[right].transform.parent = this.transform;
                                        mushroomsAndTiles[right].hasPlant = true;
                                        mushroomsAndTiles[right].isTilled = true;

                                        if (farmingTutorial.shippedAfter == true)
                                        {
                                            farmingTutorial.spreadAfter = true;
                                        }
                                    }
                                    else
                                    {
                                        Debug.Log("Make Hybrid");

                                        Mushrooms rightShroom = (Mushrooms)mushroomsAndTiles[right];

                                        if (compareShroom.hybridDictionary.ContainsKey(rightShroom.ID))
                                        {
                                            mushroomPrefab = compareShroom.hybridDictionary[rightShroom.ID];

                                            Destroy(mushroomsAndTiles[right].gameObject);

                                            mushroomsAndTiles[right] = Instantiate(mushroomPrefab, right, Quaternion.identity).GetComponent<Tile>();
                                            farmField.SetTile(right, mushroomsAndTiles[right].tileSprite);
                                            mushroomsAndTiles[right].transform.parent = this.transform;
                                            mushroomsAndTiles[right].hasPlant = true;
                                            mushroomsAndTiles[right].isTilled = true;

                                            if (farmingTutorial.spreadAfter == true)
                                            {
                                                farmingTutorial.hybridAfter = true;
                                            }
                                        }
                                    }

                                }
                            }

                            Debug.Log("End of SpreadMushroom");
                        }
                    }
                }
            }
        }
    }
}
