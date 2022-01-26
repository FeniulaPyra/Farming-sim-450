using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    //Length of the day in minutes
    //static int daysInMinutes = 24;
    static float daysInMinutes = 0.33f;
    
    //Length of day in seconds; actually used for timekeeping
    //static int daysInSeconds = daysInMinutes * 60;
    static float daysInSeconds = daysInMinutes * 60f;

    //Timer to keep track of time passed
    float dayTimer = daysInSeconds;

    //Reference to the FarmManager so it can access the dictionary that has all of the mushrooms
    public FarmManager management;// = new FarmManager();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"There are {daysInMinutes} minutes in the day and {daysInSeconds} seconds in the day");   
    }

    // Update is called once per frame
    void Update()
    {
        //Temp code; just making sure Unity can call the method
        if (Input.GetKeyDown(KeyCode.N))
        {
            AdvanceDay();
        }

        //Also temp code
        //Counts down the day, then when it's over, calls the method
        dayTimer -= Time.deltaTime;
        Debug.Log($"There are {dayTimer} seconds remaining");

        if(dayTimer <= 0.00f)
        {
            AdvanceDay();
        }
    }

    //Method to change the day and call grow Mushroom
    void AdvanceDay()
    {
        /*Mushrooms TestShroom = FindObjectOfType<Mushrooms>();

        Debug.Log($"{TestShroom} is worth {TestShroom.baseValue}");

        TestShroom.GrowMushroom();*/

        //Uses the Farm Managers dictionary of mushrooms to grow each mushroom
        foreach (KeyValuePair<Vector3Int, Mushrooms> shroom in management.mushroomsAndTiles)
        {
            Debug.Log($"{shroom.Value} is worth {shroom.Value.baseValue}");

            shroom.Value.GrowMushroom();
        }

        //Once all of the mushrooms grow, call spread once.
        management.SpreadMushroom();

        dayTimer = daysInSeconds;
    }
}
