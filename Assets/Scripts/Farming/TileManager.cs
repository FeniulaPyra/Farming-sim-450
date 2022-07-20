using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public List<Tile> fieldObjects = new List<Tile>();
    //public FarmManager farmManager;
    public FarmManager farmManager;

    // Start is called before the first frame update
    void Awake()
    {
        farmManager = FindObjectOfType<FarmManager>();

        if (farmManager.farmField != null && farmManager.tillableGround != null)
        {
            if (GlobalGameSaving.Instance != null)
            {
                if (GlobalGameSaving.Instance.loadingSave == true)
                {
                    LoadFieldObjects(GlobalGameSaving.Instance.farmTiles, GlobalGameSaving.Instance.mushrooms);
                }
                else if (ScenePersistence.Instance != null)
                {
                    if (ScenePersistence.Instance.farmTiles.Count > 0 && ScenePersistence.Instance.mushrooms.Count > 0)
                    {
                        LoadFieldObjects(ScenePersistence.Instance.farmTiles, ScenePersistence.Instance.mushrooms);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveFarm(string what)
    {
        if (farmManager.farmField != null && farmManager.tillableGround != null)
        {
            SaveFieldObjects(out var farmland, out var mushrooms);

            if (what == "persist")
            {
                ScenePersistence.Instance.farmTiles = farmland;
                ScenePersistence.Instance.mushrooms = mushrooms;
            }
            else if (what == "save")
            {
                GlobalGameSaving.Instance.farmTiles = farmland;
                GlobalGameSaving.Instance.mushrooms = mushrooms;
            }
        }
    }

    public void SaveFieldObjects(out List<SaveTile> farmland, out List<MushroomSaveTile> mushrooms)
    {
        //Clears list so it doesn't go 16 -> 32 -> 48...
        fieldObjects.Clear();

        farmland = new List<SaveTile>();
        mushrooms = new List<MushroomSaveTile>();

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
                v.Value.position = v.Key;

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
                mushrooms.Add(v.Value.GetComponent<Mushrooms>().AsSaveTile());
                Debug.Log($"Saving mushroom position: {mushrooms[mushrooms.Count - 1].position}");

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
                v.Value.position = v.Key;

                fieldObjects.Add(v.Value);
                farmland.Add(v.Value.AsSaveTile());

                Debug.Log($"Saving tile position: {farmland[farmland.Count - 1].position}");
            }
        }
    }

    public void LoadFieldObjects(List<SaveTile> tilesToLoad, List<MushroomSaveTile> mushrooms)
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

        //setting all mushrooms
        for (int i = 0; i < mushrooms.Count; i++)
        {
            toInstantiate = farmManager.mushroomManager.mushroomVariants[mushrooms[i].ID];

            toInstantiate = Instantiate(toInstantiate, mushrooms[i].position, Quaternion.identity, transform);
            Debug.Log($"Mushroom position is: {toInstantiate.transform.position}; position is {mushrooms[i].position}");

            if (farmManager.mushroomsAndTiles.ContainsKey(mushrooms[i].position) == false)//if (farmManager.mushroomsAndTiles.ContainsKey(keys[i]) == false)
            {
                farmManager.mushroomsAndTiles.Add(mushrooms[i].position, toInstantiate.GetComponent<Tile>());//farmManager.mushroomsAndTiles.Add(keys[i], toInstantiate.GetComponent<Tile>());
            }

            Debug.Log($"Setting mushroom sprite Index {mushrooms[i].spriteIndex}");
            farmManager.mushroomsAndTiles[mushrooms[i].position].tileSprite = farmManager.mushroomsAndTiles[mushrooms[i].position].sprites[mushrooms[i].spriteIndex];
            if (farmManager.farmField != null)
            {
                farmManager.farmField.SetTile(mushrooms[i].position, farmManager.mushroomsAndTiles[mushrooms[i].position].tileSprite);
            }

            farmManager.mushroomsAndTiles[mushrooms[i].position].hasPlant = mushrooms[i].hasPlant;
            farmManager.mushroomsAndTiles[mushrooms[i].position].isTilled = mushrooms[i].isTilled;
            farmManager.mushroomsAndTiles[mushrooms[i].position].isMoist = mushrooms[i].isMoist;


            farmManager.mushroomsAndTiles[mushrooms[i].position].GetComponent<Mushrooms>().growthStage = mushrooms[i].growthStage;
            farmManager.mushroomsAndTiles[mushrooms[i].position].GetComponent<Mushrooms>().maxGrowthStage = mushrooms[i].maxGrowthStage;
            farmManager.mushroomsAndTiles[mushrooms[i].position].GetComponent<Mushrooms>().daysSinceFullyGrown = mushrooms[i].daysSinceFullyGrown;
			farmManager.mushroomsAndTiles[mushrooms[i].position].GetComponent<Mushrooms>().daysWithoutWater = mushrooms[i].daysWithoutWater;
            farmManager.mushroomsAndTiles[mushrooms[i].position].GetComponent<Mushrooms>().maxDaysWithoutWater = mushrooms[i].maxDaysWithoutWater;
            farmManager.mushroomsAndTiles[mushrooms[i].position].GetComponent<Mushrooms>().readyToDie = mushrooms[i].readyToDie;
            farmManager.mushroomsAndTiles[mushrooms[i].position].GetComponent<Mushrooms>().ID = mushrooms[i].ID;
            farmManager.mushroomsAndTiles[mushrooms[i].position].GetComponent<Mushrooms>().baseValue = mushrooms[i].baseValue;
        }

        for (int i = 0; i < tilesToLoad.Count; i++)
        {
            //-sample[keys[i]] = Instantiate(farmManager.tilePrefab, keys[i], Quaternion.identity, transform);
            
            //Destroy(farmManager.mushroomsAndTiles[keys[i]].gameObject);
            farmManager.mushroomsAndTiles[tilesToLoad[i].position] = Instantiate(farmManager.tilePrefab, tilesToLoad[i].position, Quaternion.identity, transform);//farmManager.mushroomsAndTiles[keys[i]] = Instantiate(farmManager.tilePrefab, keys[i], Quaternion.identity, transform);


            Debug.Log($"tile position is: {farmManager.mushroomsAndTiles[tilesToLoad[i].position].transform.position}; position is {tilesToLoad[i].position}");

            if (farmManager.mushroomsAndTiles.ContainsKey(tilesToLoad[i].position) == false)//if (farmManager.mushroomsAndTiles.ContainsKey(keys[i]) == false)
            {
                farmManager.mushroomsAndTiles.Add(tilesToLoad[i].position, farmManager.tilePrefab);//farmManager.mushroomsAndTiles.Add(keys[i], farmManager.tilePrefab);
            }

            /*farmManager.farmField.SetTile(keys[i], null);
            farmManager.tillableGround.SetTile(keys[i], farmManager.tilePrefab.tileSprite);
            
            farmManager.mushroomsAndTiles[keys[i]].isTilled = tilesToLoad[i].isTilled;
            farmManager.mushroomsAndTiles[keys[i]].isMoist = tilesToLoad[i].isMoist;*/

            if (farmManager.farmField != null)
            {
                farmManager.farmField.SetTile(tilesToLoad[i].position, null);
            }

            Debug.Log($"Setting Tile sprite Index {tilesToLoad[i].spriteIndex}");
            farmManager.mushroomsAndTiles[tilesToLoad[i].position].tileSprite = farmManager.mushroomsAndTiles[tilesToLoad[i].position].sprites[tilesToLoad[i].spriteIndex];
            if (farmManager.tillableGround != null)
            {
                farmManager.tillableGround.SetTile(tilesToLoad[i].position, farmManager.mushroomsAndTiles[tilesToLoad[i].position].tileSprite);
            }

            farmManager.mushroomsAndTiles[tilesToLoad[i].position].isTilled = tilesToLoad[i].isTilled;
            farmManager.mushroomsAndTiles[tilesToLoad[i].position].isMoist = tilesToLoad[i].isMoist;
			
        }

        //Setting ground accordingly
        foreach (KeyValuePair<Vector3Int, Tile> t in farmManager.mushroomsAndTiles)
        {
            if (farmManager.mushroomsAndTiles[t.Value.position].hasPlant)
            {
                Debug.Log($"T is tilled?: {farmManager.mushroomsAndTiles[t.Value.position].isTilled}");
                Debug.Log($"T is moist?: {farmManager.mushroomsAndTiles[t.Value.position].isMoist}");
            }
            //Debug.Log($"T has plant?: {farmManager.mushroomsAndTiles[t.Value.position].hasPlant}");

            if (farmManager.tillableGround != null)
            {

                //if (farmManager.mushroomsAndTiles[t.Value.position].isTilled == true && farmManager.mushroomsAndTiles[t.Value.position].isMoist == false)
                if (farmManager.mushroomsAndTiles[t.Key].isTilled == true && farmManager.mushroomsAndTiles[t.Key].isMoist == false)
                {
                    Debug.Log("Tilled");
                    //farmManager.tillableGround.SetTile(t.Value.position, farmManager.tilePrefab.sprites[1]);
                    farmManager.tillableGround.SetTile(t.Key, farmManager.tilePrefab.sprites[1]);
                }
                //else if (farmManager.mushroomsAndTiles[t.Value.position].isMoist == true)
                else if (farmManager.mushroomsAndTiles[t.Key].isMoist == true)
                {
                    Debug.Log("Watered");
                    //farmManager.tillableGround.SetTile(t.Value.position, farmManager.tilePrefab.sprites[2]);
                    farmManager.tillableGround.SetTile(t.Key, farmManager.tilePrefab.sprites[2]);
                }
                else
                {
                    Debug.Log("Plain");
                    //farmManager.tillableGround.SetTile(t.Value.position, farmManager.tilePrefab.sprites[0]);
                    farmManager.tillableGround.SetTile(t.Key, farmManager.tilePrefab.sprites[0]);
                }
            }
        }
		
    }
}
