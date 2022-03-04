using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public List<Tile> fieldObjects = new List<Tile>();
    public FarmManager farmManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Tile> SaveFieldObjects()
    {
        //Clears list so it doesn't go 16 -> 32 -> 48...
        fieldObjects.Clear();

        //MAking tiles and mushrooms to actually save information
        //Tile newTile = new Tile();
        //Mushrooms newMushroom = new Mushrooms();

        
        foreach (KeyValuePair<Vector3Int, Tile> v in farmManager.mushroomsAndTiles)
        {
            //Saving mushroom and tile information and adding them to the list for later use
            if (v.Value.GetComponent<Mushrooms>() != null)
            {
                Mushrooms newMushroom = new Mushrooms();

                newMushroom.isTilled = v.Value.isTilled;
                newMushroom.isMoist = v.Value.isMoist;
                newMushroom.hasPlant = v.Value.hasPlant;
                newMushroom.tileSprite = v.Value.tileSprite;
                newMushroom.sprites = v.Value.sprites;
                newMushroom.position = v.Key;

                newMushroom.growthStage = v.Value.GetComponent<Mushrooms>().growthStage;
                newMushroom.maxGrowthStage = v.Value.GetComponent<Mushrooms>().maxGrowthStage;
                newMushroom.daysWithoutWater = v.Value.GetComponent<Mushrooms>().daysWithoutWater;
                newMushroom.maxDaysWithoutWater = v.Value.GetComponent<Mushrooms>().maxDaysWithoutWater;
                newMushroom.readyToDie = v.Value.GetComponent<Mushrooms>().readyToDie;
                newMushroom.ID = v.Value.GetComponent<Mushrooms>().ID;
                newMushroom.baseValue = v.Value.GetComponent<Mushrooms>().baseValue;
                newMushroom.mushroomItem = v.Value.GetComponent<Mushrooms>().mushroomItem;
                newMushroom.spr = v.Value.GetComponent<Mushrooms>().spr;

                fieldObjects.Add(newMushroom);

                Debug.Log($"Testing, Testing, 1, 2, 3: {newMushroom.ID}");
            }
            else
            {
                Tile newTile = new Tile();

                newTile.isTilled = v.Value.isTilled;
                newTile.isMoist = v.Value.isMoist;
                newTile.hasPlant = v.Value.hasPlant;
                newTile.tileSprite = v.Value.tileSprite;
                newTile.sprites = v.Value.sprites;

                newTile.position = v.Key;

                fieldObjects.Add(newTile);
            }
        }

        return fieldObjects;
    }

    public void LoadFieldObjects(List<Tile> tilesToLoad)
    {
        List<Vector3Int> keys = new List<Vector3Int>();
        GameObject toInstantiate;

        //Actually getting the keys, since Unity doesn't like trying to change a dictionary mid-foreach
        foreach (KeyValuePair<Vector3Int, Tile> v in farmManager.mushroomsAndTiles)
        {
            keys.Add(v.Key);
            Destroy(v.Value.gameObject);
        }

        farmManager.mushroomsAndTiles.Clear();

        Debug.Log($"There are this many keys: {keys.Count}");

        for (int i = 0; i < keys.Count; i++)
        {
            //Destroy and remove whatever is there; make a clean slate
            //Destroy(farmManager.mushroomsAndTiles[keys[i]].gameObject);
            //farmManager.mushroomsAndTiles.Remove(keys[i]);
            //farmManager.mushroomsAndTiles.Clear();

            //Instantiating a mushroom
            if (tilesToLoad[i] is Mushrooms)
            {
                //Mushrooms mushroom = (Mushrooms)fieldObjects[i];

                Mushrooms mushroom = tilesToLoad[i] as Mushrooms;

                Debug.Log($"My name is {mushroom.ID}");

                toInstantiate = farmManager.mushroomManager.mushroomVariants[mushroom.ID];

                //toInstantiate = Instantiate(toInstantiate, keys[i], Quaternion.identity, transform);
                toInstantiate = Instantiate(toInstantiate, mushroom.position, Quaternion.identity, transform);


                if (farmManager.mushroomsAndTiles.ContainsKey(mushroom.position) == false)//if (farmManager.mushroomsAndTiles.ContainsKey(keys[i]) == false)
                {
                    farmManager.mushroomsAndTiles.Add(mushroom.position, toInstantiate.GetComponent<Tile>());//farmManager.mushroomsAndTiles.Add(keys[i], toInstantiate.GetComponent<Tile>());
                }

                /*farmManager.farmField.SetTile(keys[i], tilesToLoad[i].tileSprite);

                farmManager.mushroomsAndTiles[keys[i]].hasPlant = true;
                farmManager.mushroomsAndTiles[keys[i]].isTilled = true;
                farmManager.mushroomsAndTiles[keys[i]].isMoist = tilesToLoad[i].isMoist;*/

                farmManager.farmField.SetTile(mushroom.position, tilesToLoad[i].tileSprite);

                farmManager.mushroomsAndTiles[mushroom.position].hasPlant = true;
                farmManager.mushroomsAndTiles[mushroom.position].isTilled = true;
                farmManager.mushroomsAndTiles[mushroom.position].isMoist = tilesToLoad[i].isMoist;
            }
            else
            {
                //-sample[keys[i]] = Instantiate(farmManager.tilePrefab, keys[i], Quaternion.identity, transform);

                //Destroy(farmManager.mushroomsAndTiles[keys[i]].gameObject);
                farmManager.mushroomsAndTiles[tilesToLoad[i].position] = Instantiate(farmManager.tilePrefab, tilesToLoad[i].position, Quaternion.identity, transform);//farmManager.mushroomsAndTiles[keys[i]] = Instantiate(farmManager.tilePrefab, keys[i], Quaternion.identity, transform);


                if (farmManager.mushroomsAndTiles.ContainsKey(tilesToLoad[i].position) == false)//if (farmManager.mushroomsAndTiles.ContainsKey(keys[i]) == false)
                {
                    farmManager.mushroomsAndTiles.Add(tilesToLoad[i].position, farmManager.tilePrefab);//farmManager.mushroomsAndTiles.Add(keys[i], farmManager.tilePrefab);
                }

                /*farmManager.farmField.SetTile(keys[i], null);
                farmManager.tillableGround.SetTile(keys[i], farmManager.tilePrefab.tileSprite);

                farmManager.mushroomsAndTiles[keys[i]].isTilled = tilesToLoad[i].isTilled;
                farmManager.mushroomsAndTiles[keys[i]].isMoist = tilesToLoad[i].isMoist;*/

                farmManager.farmField.SetTile(tilesToLoad[i].position, null);
                farmManager.tillableGround.SetTile(tilesToLoad[i].position, farmManager.tilePrefab.tileSprite);

                farmManager.mushroomsAndTiles[tilesToLoad[i].position].isTilled = tilesToLoad[i].isTilled;
                farmManager.mushroomsAndTiles[tilesToLoad[i].position].isMoist = tilesToLoad[i].isMoist;
            }

            //Setting ground accordingly
            if (farmManager.mushroomsAndTiles[tilesToLoad[i].position].isTilled == true && farmManager.mushroomsAndTiles[tilesToLoad[i].position].isMoist == false)
            {
                Debug.Log("Tilled");
                farmManager.tillableGround.SetTile(tilesToLoad[i].position, farmManager.tilePrefab.tilledGround);
            }
            else if (farmManager.mushroomsAndTiles[tilesToLoad[i].position].isMoist == true)
            {
                Debug.Log("Watered");
                farmManager.tillableGround.SetTile(tilesToLoad[i].position, farmManager.tilePrefab.wateredGround);
            }
            else
            {
                Debug.Log("Plain");
                farmManager.tillableGround.SetTile(tilesToLoad[i].position, farmManager.tilePrefab.unTilledGround);
            }

            /*if (farmManager.mushroomsAndTiles[keys[i]].isTilled == true && farmManager.mushroomsAndTiles[keys[i]].isMoist == false)
            {
                Debug.Log("Tilled");
                farmManager.tillableGround.SetTile(keys[i], farmManager.tilePrefab.tilledGround);
            }
            else if (farmManager.mushroomsAndTiles[keys[i]].isMoist == true)
            {
                Debug.Log("Watered");
                farmManager.tillableGround.SetTile(keys[i], farmManager.tilePrefab.wateredGround);
            }
            else
            {
                Debug.Log("Plain");
                farmManager.tillableGround.SetTile(keys[i], farmManager.tilePrefab.unTilledGround);
            }*/
        }
    }
}
