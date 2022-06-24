using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

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
	public Toggle needsTill;

    public CalculateFarmNetWorth netWorth;

    //Testing only; attempt at making a dictionary
    //The key is the position of a tile, and the mushroom is that instance of the script it's supposed to kep track of
    //public Dictionary<Vector3Int, Mushrooms> mushroomsAndTiles = new Dictionary<Vector3Int, Mushrooms>();
    public Dictionary<Vector3Int, Tile> mushroomsAndTiles = new Dictionary<Vector3Int, Tile>();
	public Dictionary<Vector3Int, bool> visitedTiles = new Dictionary<Vector3Int, bool>();

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


	int leftBound;
	int bottomBound;

	int rightBound;
	int topBound;

	//EmptyTilePrefab
	public Tile tilePrefab;

    //Creating an inventory; will probably need to be a reference later
    //public Inventory playerInventory = new Inventory(4, 9);

    // Start is called before the first frame update
    void Start()
    {
		//ResetVisited();
        if (farmField != null)
        {
            //Can be used to figure out where the (0, 0) of the tilemap is, which could be useful
            Debug.Log($"The origin is: {farmField.origin}");
            //The bottom left point of a tile is its actual coordinates
            Debug.Log($"I am the tile: {farmField.GetTile(new Vector3Int(-11, 3, 0))}");
        }

        //Making a random set of tiles
        for (int i = -5; i <= 5; i++)
        {
            for (int j = -9; j <= 1; j++)
            {
                Vector3Int cropPos = new Vector3Int(i, j, 0);
                Tile testTile = Instantiate(tilePrefab, cropPos, Quaternion.identity, transform);

                if (mushroomsAndTiles.ContainsKey(cropPos) == false)
                {
                    mushroomsAndTiles.Add(cropPos, testTile);
                }
				Debug.Log("START TILE + " + cropPos);
				if (!visitedTiles.ContainsKey(cropPos))
					visitedTiles.Add(cropPos, false);
				else
					visitedTiles[cropPos] = false;
                //farmField.SetTile(cropPos, testTile.tileSprite);
                testTile.tileSprite = testTile.sprites[0];
                if (tillableGround != null)
                {
                    tillableGround.SetTile(cropPos, testTile.tileSprite);
                }
            }
        }

        if (farmField != null)
        {
            leftBound = farmField.cellBounds.x;
            bottomBound = farmField.cellBounds.y;

            rightBound = leftBound + farmField.cellBounds.size.x;
            topBound = farmField.cellBounds.size.y;
        }

		farmingTutorial = FindObjectOfType<FarmingTutorial>();

        
    }


	private void ResetVisited()
	{
		if (visitedTiles == null) return;
		for (int i = -5; i <= 5; i++)
		{
			for (int j = -9; j <= 1; j++)
			{
				Vector3Int cropPos = new Vector3Int(i, j, 0);
				visitedTiles[cropPos] = false;
			}
		}
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
			if (shroom.Value == null && farmField != null)
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
            if (tillableGround != null)
            {
                tillableGround.SetTile(tile, mushroomsAndTiles[tile].tileSprite);
            }
            //Debug.Log($"Is the tile at {tile} tilled? : {mushroomsAndTiles[tile].isTilled}");
            if (farmingTutorial != null)
            {
                if (farmingTutorial.tutorialBools[0] == true)//(farmingTutorial.tutorialStarted == true)
                {
                    farmingTutorial.tutorialBools[2] = true;//farmingTutorial.tilledAfter = true;
                    GlobalGameSaving.Instance.tutorialBools[2] = farmingTutorial.tutorialBools[2];
                }
            }
        }
		//planting
		//If the item in question has mushroom in the name, you know you're planting. If the mushroom dictionary contains that mushroom, everything should wor
		//Might have to account for naming being similar, but not the same, which would throw things off.
		//Dictionary Keys are the IDs of the Mushrooms, which are all formatted like "Descriptor Shroom", so the item names must be the same
		//TODO change to check item.type instead of just the name
		if (tool.ToLower().Contains("shroom") && mushroomsAndTiles[tile].isTilled == true && mushroomsAndTiles[tile].hasPlant == false)
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

            //Since mushroom is determined by a non null string, if the string is nonsense
            if (mushroomPrefab != null)
            {
                GameObject newMushroom = Instantiate(mushroomPrefab, tile, Quaternion.identity, transform);
                mushroomsAndTiles.Add(tile, newMushroom.GetComponent<Tile>());

                if (farmField != null)
                {
                    farmField.SetTile(tile, newMushroom.GetComponent<Tile>().tileSprite);
                }

                //Returned nothing. Issue with tile
                Debug.Log($"Planting Debug 3: {newMushroom.GetComponent<Mushrooms>().ID}");
                mushroomsAndTiles[tile].hasPlant = true;
                mushroomsAndTiles[tile].isTilled = true;
                mushroomsAndTiles[tile].isMoist = tempMoist;
                Debug.Log($"You just planted a {mushroomPrefab.GetComponent<Mushrooms>().ID}");

                if (farmingTutorial != null)
                {
                    if (farmingTutorial.tutorialBools[2] == true)//(farmingTutorial.tilledAfter == true)
                    {
                        farmingTutorial.tutorialBools[4] = true;//farmingTutorial.plantedAfter = true;
                        GlobalGameSaving.Instance.tutorialBools[4] = farmingTutorial.tutorialBools[4];
                    }
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
                if (tillableGround != null)
                {
                    tillableGround.SetTile(tile, tilePrefab.sprites[2]);
                }

                if (farmingTutorial != null)
                {
                    if (farmingTutorial.tutorialBools[4] == true)//(farmingTutorial.plantedAfter == true)
                    {
                        farmingTutorial.tutorialBools[6] = true;//farmingTutorial.wateredAfter = true;
                        GlobalGameSaving.Instance.tutorialBools[6] = farmingTutorial.tutorialBools[6];
                    }
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
                if (tillableGround != null)
                {
                    tillableGround.SetTile(tile, mushroomsAndTiles[tile].tileSprite);
                }
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
            if (farmField != null)
            {
                farmField.SetTile(tile, null);
            }
            if (tillableGround != null)
            {
                tillableGround.SetTile(tile, tilePrefab.sprites[0]);
            }


            if (harvestShroom.GetComponent<Mushrooms>().growthStage >= harvestShroom.GetComponent<Mushrooms>().GetMaxGrowthStage())
            {

                //Instatiates the Prefab on the ground so that the player picks it up by walking over it
                for (int i = 0; i < 3; i++)
                {
                    GameObject TempItem = Instantiate(harvestShroomItem, tile, Quaternion.identity);// + new Vector3Int(0, -2, 0), Quaternion.identity);
                    netWorth.CalculateNetWorth(1);
                }

                mushroomsAndTiles[tile].isTilled = false;
            }
            else
            {

                //Instatiates the Prefab on the ground so that the player picks it up by walking over it
                GameObject TempItem = Instantiate(harvestShroomItem, tile, Quaternion.identity);// + new Vector3Int(0, -2, 0), Quaternion.identity);
                netWorth.CalculateNetWorth(1);

                mushroomsAndTiles[tile].isTilled = false;
            }

            if (farmingTutorial != null)
            {
                if (farmingTutorial.tutorialBools[8] == true)//(farmingTutorial.sleptAfter == true)
                {
                    farmingTutorial.tutorialBools[10] = true;//farmingTutorial.harvestedAfter = true;
                    GlobalGameSaving.Instance.tutorialBools[10] = farmingTutorial.tutorialBools[10];
                }
            }
        }
    }

    public void SpreadMushroom()
    {
		Debug.Log("Beginning of SpreadMushroom");
		ResetVisited();

		if (farmField != null)
		{
			leftBound = farmField.cellBounds.x;
			bottomBound = farmField.cellBounds.y;

			rightBound = leftBound + farmField.cellBounds.size.x;
			topBound = farmField.cellBounds.size.y;
		}

		//looping through the x bound and y bounds of farmfield
		for (int x = leftBound + 1; x < rightBound - 1; x++)
        {
            for (int y = bottomBound + 1; y < topBound - 1; y++)
            {
                Vector3Int tileToTest = new Vector3Int(y, x, 0);
				CheckTile(tileToTest);

			}
        }
    }

	private static Vector3Int[] adjacentVectors =
	{
		new Vector3Int(0,1,0),
		new Vector3Int(0,-1,0),
		new Vector3Int(1,0,0),
		new Vector3Int(-1,0,0)
	};

	public void CheckTile(Vector3Int tilePos)
	{
		//if this tile has already been visited, exit. 
		#region note
		//this will happen if a previous tile hybridized with this one,
		//or this tile was spread to.
		#endregion
		Debug.Log("TILEPOS + " + tilePos);
		
		if (!visitedTiles.ContainsKey(tilePos) || visitedTiles[tilePos]) return;

		//checks that the tile exists
		Tile tile = mushroomsAndTiles[tilePos];
		Mushrooms thisShroom = tile.GetComponent<Mushrooms>();

		//if this shroom exists and is an adult
		if (thisShroom != null 
			&& thisShroom.growthStage >= thisShroom.GetMaxGrowthStage()
			&& thisShroom.daysSinceFullyGrown >= 2)
		{
			//Valid spreadable directions
			List <Vector3Int> adjacents = new List<Vector3Int>();

			//checks adjacent tiles for if they are spreadable (i.e. tilled and plantless)
			for (int i = 0; i < 4; i++)
			{
				if (!mushroomsAndTiles.ContainsKey(tilePos + adjacentVectors[i]))
					continue;
				Tile adj = mushroomsAndTiles[tilePos + adjacentVectors[i]];
				//if adjacent tile is empty and tilled, add it to spreadable areas
				if (adj != null && (adj.isTilled || !needsTill.isOn)&& !adj.hasPlant)
				{
					adjacents.Add(adjacentVectors[i]);
				}
			}

			//exit if there are no spreadable tiles
			if (adjacents.Count < 1) return;

			//picks a random direction of tile and sets that as the tile to spread to
			Vector3Int dirToSpread = adjacents[Random.Range(0, adjacents.Count)];

			//the position and tile of the tile to spread
			Vector3Int spreadTilePos = dirToSpread + tilePos;
			Tile spreadTo = mushroomsAndTiles[spreadTilePos];

			//the position and tile of the next tile over - will be for hybridization
			Vector3Int parentTilePos = spreadTilePos + dirToSpread;
			
			//checks that possible parent tile exists
			if (mushroomsAndTiles.ContainsKey(parentTilePos))
			{
				Tile possibleParent = mushroomsAndTiles[parentTilePos];

				Mushrooms parent = possibleParent.GetComponent<Mushrooms>();
				
				/*	checks that parent 
				 *	- exists
				 *	- has not been visited
				 *	- can be hybridized with this one
				 *	- is an adult mushroom 
				 */
				if (parent != null
					&& !visitedTiles[parentTilePos] 
					&& ((Mushrooms)tile).hybridDictionary.ContainsKey(parent.ID) 
					&& parent.growthStage >= parent.GetMaxGrowthStage()
					&& parent.daysSinceFullyGrown >= 2)
				{
					Hybridize(thisShroom, parent, spreadTilePos);

					visitedTiles[parentTilePos] = true;
					visitedTiles[spreadTilePos] = true;
				}
				else
				{
					Spread(thisShroom, spreadTilePos);
					//sets these tiles as visited
					visitedTiles[spreadTilePos] = true;
				}
			}
			else
			{
				Spread(thisShroom, spreadTilePos);
				visitedTiles[spreadTilePos] = true;

			}

		}
		visitedTiles[tilePos] = true;
	}

	private void Hybridize(Mushrooms thisShroom, Mushrooms parent, Vector3Int spreadTo)
	{
		GameObject mushroomPrefab = thisShroom.hybridDictionary[parent.ID];

		Destroy(mushroomsAndTiles[spreadTo].gameObject);
		//creates new mushroom tile
		mushroomsAndTiles[spreadTo] = Instantiate(mushroomPrefab, spreadTo, Quaternion.identity).GetComponent<Tile>();
        if (farmField != null)
        {
            farmField.SetTile(spreadTo, mushroomsAndTiles[spreadTo].tileSprite);
        }
		mushroomsAndTiles[spreadTo].transform.parent = this.transform;
		mushroomsAndTiles[spreadTo].hasPlant = true;
		mushroomsAndTiles[spreadTo].isTilled = true;

        if (farmingTutorial != null)
        {
            if (farmingTutorial.tutorialBools[18] == true)//(farmingTutorial.spreadAfter == true)
            {
                farmingTutorial.tutorialBools[20] = true;//farmingTutorial.hybridAfter = true;
                GlobalGameSaving.Instance.tutorialBools[20] = farmingTutorial.tutorialBools[20];
            }
        }
	}
	private void Spread(Mushrooms thisShroom, Vector3Int spreadTo)
	{
		//spread
		GameObject mushroomPrefab = mushroomManager.mushroomVariants[thisShroom.ID];

		Destroy(mushroomsAndTiles[spreadTo].gameObject);
		mushroomsAndTiles[spreadTo] = Instantiate(mushroomPrefab, spreadTo, Quaternion.identity).GetComponent<Tile>();
        if (farmField != null)
        {
            farmField.SetTile(spreadTo, mushroomsAndTiles[spreadTo].tileSprite);
        }
		mushroomsAndTiles[spreadTo].transform.parent = this.transform;
		mushroomsAndTiles[spreadTo].hasPlant = true;
		mushroomsAndTiles[spreadTo].isTilled = true;

        if (farmingTutorial)
        {
            if (farmingTutorial.tutorialBools[14] == true)//(farmingTutorial.shippedAfter == true)
            {
                farmingTutorial.tutorialBools[18] = true;//farmingTutorial.spreadAfter = true;
                GlobalGameSaving.Instance.tutorialBools[18] = farmingTutorial.tutorialBools[18];
            }
        }
	}
}
