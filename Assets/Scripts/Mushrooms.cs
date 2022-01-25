using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mushrooms : MonoBehaviour
{
    #region Life and Death Variables
    //constant that defines the greatest stage of growth. Once the mushroom is at this stage, it's fully grown
    const int maxGrowthStage = 3;
    //constant that defines how many days the mushroom can survive being neglected. If the number of days is ever greater than this, the mushroom has died
    const int maxDaysWithoutWater = 3;
    //represents the mushroom's current stage of growth
    float growthStage = 1.00f;
    //number of days without water
    int daysWithoutWater;
    //whether or not it's been watered
    bool isMoist = false;
    #endregion

    #region Miscellaneous Variables 
    //The id/name of the mushroom, //The base monetary value of the mushroom
    public string ID = "Super Shroom"; 
    public int baseValue = 500;
    //Solely for Testing purposes; currently a placeholder
    public Tile mushroomTile;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Temp code; just making sure Unity can call the method and make it work if the mushroom is moist
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Mushroom now moist");
            isMoist = true;
            //GrowMushroom();
        }

        //Temp code; just making sure Unity can call the method and make it work if the mushroom is dry
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Mushroom now dry");
            isMoist = false;
            GrowMushroom();
        }

        //Temp code; just making sure Unity can access the misc variables
        if (Input.GetKeyDown(KeyCode.S))
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
            }

            daysWithoutWater = 0;

            Debug.Log($"I'm at growth stage {growthStage} and it's been {daysWithoutWater} days since I've been watered.");
        }
        else
        {
            daysWithoutWater++;

            if (daysWithoutWater > maxDaysWithoutWater)
            {
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
