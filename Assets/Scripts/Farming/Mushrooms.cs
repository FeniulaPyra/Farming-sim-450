using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mushrooms : Tile
{
    #region Life and Death Variables
    //constant that defines the greatest stage of growth. Once the mushroom is at this stage, it's fully grown
    //constant cannot be public, and therefore cannot be shared within another script
    public int maxGrowthStage;// = 3;

    public int GetMaxGrowthStage()
    {
        return maxGrowthStage;
    }

    //constant that defines how many days the mushroom can survive being neglected. If the number of days is ever greater than this, the mushroom has died
    const int maxDaysWithoutWater = 0;
    //represents the mushroom's current stage of growth
    public float growthStage = 1.00f;
    //number of days without water
    int daysWithoutWater;
    #endregion

    #region Miscellaneous Variables 
    //The id/name of the mushroom, //The base monetary value of the mushroom
    public string ID = "Super Shroom";
    public int baseValue = 500;

    //For inventory interaction
    public GameObject mushroomItem;
    public Sprite spr;
    #endregion

    #region Hybridization

    //List of names of mushrooms you can hybridize with
    public List<string> mushroomsToHybridize = new List<string>();
    //List of gameobjects that are the resultant hybrids
    public List<GameObject> mushroomHybrids = new List<GameObject>();
    //Dictionary that uses the above to keep track of things
    public Dictionary<string, GameObject> hybridDictionary = new Dictionary<string, GameObject>();

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Setting item's values
        /*mushroomItem = new Item();
        mushroomItem.name = ID;
        mushroomItem.spr = spr;*/

        //populating hybrid dictionary
        for (int i = 0; i < mushroomsToHybridize.Count; i++)
        {
            hybridDictionary[mushroomsToHybridize[i]] = mushroomHybrids[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Temp code; just making sure Unity can call the method and make it work if the mushroom is moist
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Mushroom now moist");
            isMoist = true;
            //GrowMushroom();
        }

        //Just for testing things related to mushroom growth
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Mushroom has grown");
            growthStage++;
            //GrowMushroom();
        }

        //Temp code; just making sure Unity can call the method and make it work if the mushroom is dry
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Mushroom now dry");
            isMoist = false;
            GrowMushroom();
        }

        //Temp code; just making sure Unity can access the misc variables
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log($"Mushroom is named {ID} and is worth {baseValue} G.");
        }
    }

    //Method where the mushrooms check to see if they grow or not
    public void GrowMushroom()
    {
        //Nested if branch to see what the mushroom does
        if (isMoist == true)
        {
            if (growthStage < maxGrowthStage)
            {
                growthStage++;
                tileSprite = sprites[Mathf.FloorToInt(growthStage - 1)];
            }

            daysWithoutWater = 0;

            Debug.Log($"I'm at growth stage {growthStage} and it's been {daysWithoutWater} days since I've been watered.");
        }
        else
        {
            daysWithoutWater++;

            if (daysWithoutWater > maxDaysWithoutWater)
            {
				this.isTilled = false;
				this.isMoist = false;
				hasPlant = false;
				Destroy(gameObject);
            }

            Debug.Log($"I'm at growth stage {growthStage} and it's been {daysWithoutWater} days since I've been watered.");
        }
        #region Alternative mushroom growth
        //Mushrooms still grow if not moist, but more slowly
        /*else
        {
            if (growthStage < maxGrowthStage)
            {
                growthStage+= 0.5f;
            }

            daysWithoutWater++;

            if (daysWithoutWater > maxDaysWithoutWater)
            {
                Destroy(gameObject);
            }

            Debug.Log($"I'm at growth stage {growthStage} and it's been {daysWithoutWater} days since I've been watered.");
        }*/
        #endregion
    }
}
