using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    //Length of the day in minutes
    //static int daysInMinutes = 24;
    static float daysInMinutes = 0.25f;
    
    //Length of day in seconds; actually used for timekeeping
    //static int daysInSeconds = daysInMinutes * 60;
    static float daysInSeconds = daysInMinutes * 60f;

    //Timer to keep track of time passed
    float dayTimer = daysInSeconds;

    //Reference to the FarmManager so it can access the dictionary that has all of the mushrooms
    public FarmManager management;

    //variables for displaying the date and time and their corresponding text object
    //1-7 for Sun - Sat
    int dayNum = 1;
    public TMP_Text dayDisplay;
    //1 - 30 for season length
    int dateNum = 1;
    public TMP_Text dateDisplay;
    //1 - 4 for Spr - Win
    int seasonNum = 1;
    public TMP_Text seasonDisplay;
    int yearNum = 1;
    public TMP_Text yearDisplay;
    int hourCount = 6;
    public TMP_Text hourDisplay;
    int minuteCount = 0;
    public TMP_Text minuteDisplay;


    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log($"There are {daysInMinutes} minutes in the day and {daysInSeconds} seconds in the day");

        //Getting Time Manager
        //management = FindObjectOfType<FarmManager>();

        dayDisplay.text = "Sun";
        dateDisplay.text = "1";
        seasonDisplay.text = "Spring";
        yearDisplay.text = "Year 1";

        DisplayTime();
    }

    // Update is called once per frame
    void Update()
    {
        //Temp code; just making sure Unity can call the method
        if (Input.GetKeyDown(KeyCode.N))
        {
            AdvanceDay();
        }

        DisplayTime();

        //Also temp code
        //Counts down the day, then when it's over, calls the method
        //dayTimer -= Time.deltaTime;
         Debug.Log($"There are {dayTimer} seconds remaining");

        if(dayTimer <= 0.00f)
        {
            AdvanceDay();
        }
    }

    /// <summary>
    /// Method to change the day and call grow Mushroom
    /// </summary>
    void AdvanceDay()
    {

        //Uses the Farm Managers dictionary of mushrooms to grow each mushroom and then dry them out for the next day
        foreach (KeyValuePair<Vector3Int, Tile> shroom in management.mushroomsAndTiles)
        {
            if(shroom.Value == null)
            {
				continue;
                //management.mushroomsAndTiles.Remove(shroom.Key);
                //shroom.Value.isTilled = false;
            }
            else
            {
                if (shroom.Value.GetComponent<Mushrooms>() != null)
                {
                    Mushrooms newShroom = (Mushrooms)shroom.Value;

                    Debug.Log($"{newShroom} is worth {newShroom.baseValue}");

                    newShroom.GrowMushroom();

                    newShroom.isMoist = false;

                    //Set the tile again, in case the mushroom has grown
                    management.farmField.SetTile(shroom.Key, newShroom.tileSprite);
                    management.tillableGround.SetTile(shroom.Key, management.tilePrefab.tilledGround);
                }
            }
        }

        //Once all of the mushrooms grow, call spread once.
        management.SpreadMushroom();

        DisplayDate();
    }

    public void DisplayTime()
    {
        //display hour
        if(minuteCount == 59)
        {
            if (hourCount < 24)
            {
                hourCount++;
            }
            else
            {
                hourCount = 0;

                //Advances the day if the clock passes midnight, which just makes sense
                AdvanceDay();
            }

        }

        if(hourCount < 10)
        {
            hourDisplay.text = $"0{hourCount}:";
        }
        else
        {
            hourDisplay.text = $"{hourCount}:";
        }

        //display minute
        if (minuteCount == 59)
        {
            minuteCount = 0;
        }
        else
        {
            minuteCount += 1;
        }

        if (minuteCount < 10)
        {
            minuteDisplay.text = $"0{minuteCount}";
        }
        else
        {
            minuteDisplay.text = minuteCount.ToString(); ;
        }
    }

    /// <summary>
    /// Method that displays the in game date
    /// </summary>
    public void DisplayDate()
    {
        //change year
        if (seasonNum == 4 && dateNum == 30)
        {
            yearNum++;
        }

        yearDisplay.text = $"Year {yearNum}";

        //change season
        if (dateNum == 30)
        {
            if (seasonNum == 4)
            {
                seasonNum = 1;
            }
            else
            {
                seasonNum++;
            }
        }

        switch (seasonNum)
        {
            case 1:
                seasonDisplay.text = "Spring";
                break;
            case 2:
                seasonDisplay.text = "Summer";
                break;
            case 3:
                seasonDisplay.text = "Fall";
                break;
            case 4:
                seasonDisplay.text = "Winter";
                break;
            default:
                seasonDisplay.text = $"";
                break;
        }

        //change the date
        if (dateNum == 30)
        {
            dateNum = 1;
        }
        else
        {
            dateNum++;
        }

        dateDisplay.text = dateNum.ToString();

        //change the day
        if (dayNum == 7)
        {
            dayNum = 1;
        }
        else
        {
            dayNum++;
        }

        switch (dayNum)
        {
            case 1:
                dayDisplay.text = "Sun";
                break;
            case 2:
                dayDisplay.text = "Mon";
                break;
            case 3:
                dayDisplay.text = "Tues";
                break;
            case 4:
                dayDisplay.text = "Wed";
                break;
            case 5:
                dayDisplay.text = "Thurs";
                break;
            case 6:
                dayDisplay.text = "Fri";
                break;
            case 7:
                dayDisplay.text = "Sat";
                break;
            default:
                dayDisplay.text = "";
                break;
        }

        dayTimer = daysInSeconds;
    }
}
