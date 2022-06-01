using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    //Reference to the FarmManager so it can access the dictionary that has all of the mushrooms
    public FarmManager management;

    public ShippingBin shippingBin;

    //for keeping track of player stamina, which is time
    public PlayerInteraction staminaTracker;

    //Net worth goes up slightly each day
    public CalculateFarmNetWorth netWorth;

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
    //DialogueManager[] NPCs = new DialogueManager[100];
    NPCManager[] NPCs = new NPCManager[100];
    [SerializeField]
    //List<DialogueManager> NPCList = new List<DialogueManager>();
    List<NPCManager> NPCList = new List<NPCManager>();

    public int DayNumber => dateNum;

    public int DateNumber => dateNum;

    public int YearNumber => yearNum;

    public int SeasonNumber => seasonNum;

    public FarmingTutorial farmingTutorial;

    Inventory inventory;

    MushroomManager mushroomManager;

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
        //NPCs = FindObjectsOfType<DialogueManager>();
        NPCs = FindObjectsOfType<NPCManager>();

        for (int i = 0; i < NPCs.Length; i++)
        {
            if (NPCs[i] != null && NPCs[i].gameObject.name != "TutorialManager")
            {
                NPCList.Add(NPCs[i]);
            }
        }

        //DisplayTime();

        farmingTutorial = FindObjectOfType<FarmingTutorial>();

		inventory = staminaTracker.gameObject.GetComponent<PlayerInventoryManager>().inv;//FindObjectOfType<FarmManager>().GetComponent<FarmManager>().playerInventory;

        mushroomManager = FindObjectOfType<MushroomManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (staminaTracker.playerStamina <= 20)
        {
            isNight = true;
        }

        if (isNight == true)
        {
            nightImage.color = new Color(nightImage.color.r, nightImage.color.g, nightImage.color.b, 0.25f);
        }
        else
        {
            nightImage.color = new Color(nightImage.color.r, nightImage.color.g, nightImage.color.b, 0.0f);
        }

        if (staminaTracker.playerStamina <= 0)
        {
            Sleep(0);
            Sleep(5);
            isNight = false;
            staminaTracker.gameObject.transform.position = FindObjectOfType<Bed>().transform.position;
        }
    }

    /// <summary>
    /// Method to change the day and call grow Mushroom
    /// </summary>
    void AdvanceDay()
    {
        Debug.Log("Advancing with Day");

        List<Vector3Int> keysToReplace = new List<Vector3Int>();

        //Uses the Farm Managers dictionary of mushrooms to grow each mushroom and then dry them out for the next day
        foreach (KeyValuePair<Vector3Int, Tile> shroom in management.mushroomsAndTiles)
        {
            if(shroom.Value == null)
            {
                Debug.Log($"null at {shroom.Key}");

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
                    //shroom.Value.tileSprite = shroom.Value.sprites[1];
                    management.tillableGround.SetTile(shroom.Key, management.tilePrefab.sprites[1]);

                    if (shroom.Value.GetComponent<Mushrooms>().readyToDie == true)
                    {
                        Debug.Log("The mushroom is ready to die");

                        keysToReplace.Add(shroom.Key);
                    }
                    //management.tillableGround.SetTile(shroom.Key, shroom.Value.tileSprite);

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
                        //management.tilePrefab.tileSprite = management.tilePrefab.sprites[1];
                        shroom.Value.tileSprite = shroom.Value.sprites[1];

                        //management.tillableGround.SetTile(shroom.Key, management.tilePrefab.tileSprite);
                        management.tillableGround.SetTile(shroom.Key, shroom.Value.tileSprite);
                        shroom.Value.isMoist = false;
					}
				}
            }
        }

        //After looping through the dictionary, do things to the specific mushrooms you need to destroy
        for (int i = 0; i < keysToReplace.Count; i++)
        {
            Destroy(management.mushroomsAndTiles[keysToReplace[i]].gameObject);
            management.mushroomsAndTiles[keysToReplace[i]] = Instantiate(management.tilePrefab, keysToReplace[i], Quaternion.identity, transform);
            management.farmField.SetTile(keysToReplace[i], null);
            management.tillableGround.SetTile(keysToReplace[i], management.tilePrefab.sprites[0]);
        }



        //Once all of the mushrooms grow, call spread once.
        management.SpreadMushroom();

        //change year
        if (seasonNum == 4 && dateNum == 30)
        {
            yearNum++;

            /*for (int i = 0; i < NPCList.Count; i++)
            {
                NPCList[i].SetConversations();
            }*/
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

            /*for (int i = 0; i < NPCList.Count; i++)
            {
                NPCList[i].SetConversations();
            }*/
        }
        /*else
        {
            //Change NPC Dialogue
            for (int i = 0; i < NPCList.Count; i++)
            {
                if (NPCList[i].oldConvoID != "")
                {
                    NPCList[i].convoID = NPCList[i].oldConvoID;
                } 

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
        }*/

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

        netWorth.CalculateNetWorth(5);

        shippingBin.PayPlayer();

        //Final thing in a day that determines net worth. If it goes over a threshold, activate a quest
        foreach (NPCManager npc in NPCList)
        {
            //change dialogue
            npc.MyFlowchart.SetIntegerVariable("seasonNum", seasonNum);
            npc.MyFlowchart.SetIntegerVariable("dateNum", dateNum);
            npc.MyFlowchart.SetIntegerVariable("playerNetWorth", netWorth.FarmNetWorth);
            npc.MyFlowchart.SetBooleanVariable("spokenToOnce", false);
            npc.MyFlowchart.SetBooleanVariable("spokenToTwice", false);

            //change net worth
        }
        /*foreach (DialogueManager npc in NPCList)
        {
            if (netWorth.FarmNetWorth >= npc.myQuests.activeQuest.requiredNetWorth && npc.myQuests.activeQuest.questActive == false)
            {
                npc.oldConvoID = npc.convoID;
                npc.convoID = npc.myQuests.activeQuest.beginID;
                Debug.Log("Quest id is now active");
            }

            /*if (npc.myQuests.activeQuest.questActive == true && npc.myQuests.activeQuest.readyToReport == true)
            {
                npc.oldConvoID = npc.convoID;
                npc.convoID = npc.myQuests.activeQuest.endID;
            }

            if (npc.myQuests.activeQuest.questType == Quests.QuestType.TimedCollection || npc.myQuests.activeQuest.questType == Quests.QuestType.TimedFundraising)
            {
                if (npc.myQuests.activeQuest.daysToQuestFail > 0)
                {
                    npc.myQuests.activeQuest.daysToQuestFail--;
                }
            }
        }*/

        //Tutorial Softlock Prevention
        //Check all mushroom names. If none of them are there, instantiate one.
        if (farmingTutorial.spreadAfter == false)
        {
            for (int i = 0; i < mushroomManager.mushroomList.Count; i++)
            {
                if (inventory.CountItem(mushroomManager.mushroomList[i].GetComponent<Mushrooms>().ID) <= 0)
                {
                    Debug.Log($"MushroomList[{i}] is a {mushroomManager.mushroomList[i].GetComponent<Mushrooms>().ID}");

                    if (i == mushroomManager.mushroomList.Count - 1)
                    {
                        Debug.Log($"No softlock; i is {i}");
                        Instantiate(farmingTutorial.redShroom, staminaTracker.gameObject.transform.position, Quaternion.identity);
                    }

                    continue;
                }
                else
                {
                    break;
                }
            }
        }
        else if (farmingTutorial.spreadAfter == true && farmingTutorial.hybridAfter == false)
        {
            if (inventory.CountItem("Red Shroom") <= 0)
            {
                Instantiate(farmingTutorial.redShroom, FindObjectOfType<PlayerInteraction>().gameObject.transform.position, Quaternion.identity);
            }
            if (inventory.CountItem("Glowy Shroom") <= 0)
            {
                Instantiate(farmingTutorial.glowyShroom, FindObjectOfType<PlayerInteraction>().gameObject.transform.position, Quaternion.identity);
            }
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
        //float staminaToAdd = staminaTracker.GetMaxPlayerStamina() * (duration/8);

	if (farmingTutorial.wateredAfter == true)
        {
            farmingTutorial.sleptAfter = true;
        }

        //Do nothing is above 100
        if (staminaTracker.playerStamina > 100)
        {
            AdvanceDay();
	    //return;
        }
	else
	{
	    float staminaToAdd = 100 * (duration / 8);

            Debug.Log($"Duration is {duration}");
            //Debug.Log($"Max is {staminaTracker.GetMaxPlayerStamina()} stamina");
            Debug.Log($"Multiplying by {duration/8}");
            Debug.Log($"Adding {staminaToAdd} stamina");

            staminaTracker.playerStamina += (int)staminaToAdd;

            //cap stamina if it exceeds limit
            if (staminaTracker.playerStamina > 100)
            {
                staminaTracker.playerStamina = 100;
            }
            /*if (staminaTracker.playerStamina > staminaTracker.GetMaxPlayerStamina())
            {
                staminaTracker.playerStamina = staminaTracker.GetMaxPlayerStamina();
            }*/

            //staminaTracker.staminaDisplay.text = $"Stamina: {staminaTracker.playerStamina}";

            

            AdvanceDay();
	}

        
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
