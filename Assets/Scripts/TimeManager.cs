using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    //Reference to the FarmManager so it can access the dictionary that has all of the mushrooms
    public FarmManager management;

    //for keeping track of player stamina, which is time
    public PlayerInteraction staminaTracker;

    //variables for displaying the date and time and their corresponding text object
    //1-7 for Sun - Sat
    int dayNum = 1;
    public TMP_Text dayDisplay;
    //1 - 30 for season length
    int dateNum = 1;
    public TMP_Text dateDisplay;
    //1 - 4 for Spr - Win
    [SerializeField]
    int seasonNum = 1;
    public TMP_Text seasonDisplay;
    int yearNum = 1;
    public TMP_Text yearDisplay;

    //For night
    public bool isNight;
    public Image nightImage;

    //Random array of DialogueManagers to handle NPC Dialogue
    DialogueManager[] NPCs = new DialogueManager[100];
    [SerializeField]
    List<DialogueManager> NPCList = new List<DialogueManager>();

    public int DayNumber => dateNum;

    public int DateNumber => dateNum;

    public int YearNumber => yearNum;

    public int SeasonNumber => seasonNum;

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

        //Gets all NPCs and saves them so their dialogue can later be updated
        NPCs = FindObjectsOfType<DialogueManager>();

        for (int i = 0; i < NPCs.Length; i++)
        {
            if (NPCs[i] != null)
            {
                NPCList.Add(NPCs[i]);
            }
        }

        //DisplayTime();
    }

    // Update is called once per frame
    void Update()
    {
        //Temp code; just making sure Unity can call the method
        if (Input.GetKeyDown(KeyCode.N))
        {
            AdvanceDay();
        }

        if (isNight == false && staminaTracker.playerStamina <= 20)
        {
            nightImage.color = new Color(nightImage.color.r, nightImage.color.g, nightImage.color.b, 0.25f);
            Debug.Log($"The color is {nightImage.color}");

            isNight = true;
        }

        if (staminaTracker.playerStamina <= 0)
        {
            Sleep(12);
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Sleep(4);
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            Sleep(6);
        }

        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            Sleep(8);
        }
    }

    /// <summary>
    /// Method to change the day and call grow Mushroom
    /// </summary>
    void AdvanceDay()
    {
        Vector3Int keyToReplace = Vector3Int.zero;

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
					if (shroom.Value.GetComponent<Mushrooms>().readyToDie == true)
					{
						Debug.Log("The mushroom is ready to die");

						keyToReplace = shroom.Key;
					}

					Mushrooms newShroom = (Mushrooms)shroom.Value;

					Debug.Log($"{newShroom} is worth {newShroom.baseValue}");

					newShroom.GrowMushroom();

					newShroom.isMoist = false;

					//Set the tile again, in case the mushroom has grown
					management.farmField.SetTile(shroom.Key, newShroom.tileSprite);
					management.tillableGround.SetTile(shroom.Key, management.tilePrefab.tilledGround);

					/*if (newShroom.daysWithoutWater > newShroom.maxDaysWithoutWater)
                    {
                        //convert tile to mushroom
                        //Mushrooms deadShroom = (Mushrooms)shroom.Value;
                        Tile deadShroom = shroom.Value;

                        //Destroy mushroom and add to inventory
                        Destroy(shroom.Value);
                        //mushroomsAndTiles.Remove(tile);
                        //resets the tile;
                        deadShroom = Instantiate(management.tilePrefab, shroom.Key, Quaternion.identity, transform);
                        management.farmField.SetTile(shroom.Key, null);
                        management.tillableGround.SetTile(shroom.Key, management.tilePrefab.tileSprite);

                        shroom.Value.GetComponent<Tile>().isTilled = false;
                    }*/
				}
				else
				{
					if (shroom.Value.isTilled)
					{
						management.tillableGround.SetTile(shroom.Key, management.tilePrefab.tilledGround);
						shroom.Value.isMoist = false;
					}
				}
            }
        }

        //After looping through the dictionary, do things to the specific mushrooms you need to destroy
        if (keyToReplace != Vector3Int.zero)
        {
            Destroy(management.mushroomsAndTiles[keyToReplace].gameObject);
            management.mushroomsAndTiles[keyToReplace] = Instantiate(management.tilePrefab, keyToReplace, Quaternion.identity, transform);
            management.farmField.SetTile(keyToReplace, null);
            management.tillableGround.SetTile(keyToReplace, management.tilePrefab.tileSprite);
        }



        //Once all of the mushrooms grow, call spread once.
        management.SpreadMushroom();

        //change year
        if (seasonNum == 4 && dateNum == 30)
        {
            yearNum++;

            for (int i = 0; i < NPCList.Count; i++)
            {
                NPCList[i].SetConversations();
            }
        }

        //change season; reset NPC dialogue at end of season, otherwise, just move on to the next day's piece of dialogue
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

            for (int i = 0; i < NPCList.Count; i++)
            {
                NPCList[i].SetConversations();
            }
        }
        else
        {
            //Change NPC Dialogue
            for (int i = 0; i < NPCList.Count; i++)
            {
                switch (seasonNum)
                {
                    case 1:
                        if (NPCList[i].convoID == NPCList[i].conversationIDs[NPCList[i].GetSpringEnd()])
                        {
                            NPCList[i].convoID = NPCList[i].conversationIDs[NPCList[i].GetSpringStart()];
                        }
                        else
                        {
                            NPCList[i].convoID = NPCList[i].conversationIDs[NPCList[i].conversationIDs.IndexOf(NPCList[i].convoID) + 1];
                        }
                        break;
                    case 2:
                        if (NPCList[i].convoID == NPCList[i].conversationIDs[NPCList[i].GetSummerEnd()])
                        {
                            NPCList[i].convoID = NPCList[i].conversationIDs[NPCList[i].GetSummerStart()];
                        }
                        else
                        {
                            NPCList[i].convoID = NPCList[i].conversationIDs[NPCList[i].conversationIDs.IndexOf(NPCList[i].convoID) + 1];
                        }
                        break;
                    case 3:
                        if (NPCList[i].convoID == NPCList[i].conversationIDs[NPCList[i].GetFallEnd()])
                        {
                            NPCList[i].convoID = NPCList[i].conversationIDs[NPCList[i].GetFallStart()];
                        }
                        else
                        {
                            NPCList[i].convoID = NPCList[i].conversationIDs[NPCList[i].conversationIDs.IndexOf(NPCList[i].convoID) + 1];
                        }
                        break;
                    case 4:
                        if (NPCList[i].convoID == NPCList[i].conversationIDs[NPCList[i].GetWinterEnd()])
                        {
                            NPCList[i].convoID = NPCList[i].conversationIDs[NPCList[i].GetWinterStart()];
                        }
                        else
                        {
                            NPCList[i].convoID = NPCList[i].conversationIDs[NPCList[i].conversationIDs.IndexOf(NPCList[i].convoID) + 1];
                        }
                        break;
                    default:
                        break;
                }
            }
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

        //change the day
        if (dayNum == 7)
        {
            dayNum = 1;
        }
        else
        {
            dayNum++;
        }

        DisplayDate();
    }

    /// <summary>
    /// Method for sleeping.
    /// Sole parameter is how long you want to sleep for
    /// </summary>
    /// <param name="duration"></param>
    public void Sleep(float duration)
    {
        //max stamina multiplied by duration/8. This is so that a full 8 hours of sleep would get you all stamina back
        //Might need to be tweaked, since a measly four hours still restores half stamina
        float staminaToAdd = staminaTracker.GetMaxPlayerStamina() * (duration/8);

        Debug.Log($"Duration is {duration}");
        Debug.Log($"Max is {staminaTracker.GetMaxPlayerStamina()} stamina");
        Debug.Log($"Multiplying by {duration/8}");
        Debug.Log($"Adding {staminaToAdd} stamina");

        staminaTracker.playerStamina += (int)staminaToAdd;

        //cap stamina if it exceeds limit
        if (staminaTracker.playerStamina > staminaTracker.GetMaxPlayerStamina())
        {
            staminaTracker.playerStamina = staminaTracker.GetMaxPlayerStamina();
        }

        //staminaTracker.staminaDisplay.text = $"Stamina: {staminaTracker.playerStamina}";

        AdvanceDay();
    }

    /// <summary>
    /// Method that displays the in game date
    /// </summary>
    public void DisplayDate()
    {
        

        yearDisplay.text = $"Year {yearNum}";

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

        dateDisplay.text = dateNum.ToString();

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
    }

    public void SetDate(int day, int date, int year, int season)
    {
        dayNum = day;
        dateNum = date;
        yearNum = year;
        seasonNum = season;

        DisplayDate();
    }
}
