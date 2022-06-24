using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
    [SerializeField]
    int dayNum = 1;
    public TMP_Text dayDisplay;
    //1 - 30 for season length
    [SerializeField]
    int dateNum = 1;
    public TMP_Text dateDisplay;
    //1 - 4 for Spr - Win
    [SerializeField]
    int seasonNum = 1;
    public TMP_Text seasonDisplay;
    [SerializeField]
    int yearNum = 1;
    public TMP_Text yearDisplay;
    public TMP_Text holidayDisplay;

    //For night
    public bool isNight;
    public Image nightImage;

    //Random array of DialogueManagers to handle NPC Dialogue
    //DialogueManager[] NPCs = new DialogueManager[100];
    NPCManager[] NPCs = new NPCManager[100];
    [SerializeField]
    //List<DialogueManager> NPCList = new List<DialogueManager>();
    public List<NPCManager> NPCList = new List<NPCManager>();

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

        dayDisplay.text = "Mon";
        dateDisplay.text = "1";
        seasonDisplay.text = "Spring";
        yearDisplay.text = "Year 1";

        holidayDisplay = GameObject.Find("HolidayDisplay").GetComponent<TMP_Text>();

        //Gets all NPCs and saves them so their dialogue can later be updated
        //NPCs = FindObjectsOfType<DialogueManager>();
        NPCs = FindObjectsOfType<NPCManager>();

        for (int i = 0; i < NPCs.Length; i++)
        {
            if (NPCs[i] != null /*&& NPCs[i].gameObject.name != "TutorialManager"*/)
            {
                NPCList.Add(NPCs[i]);
            }
        }

        //DisplayTime();

        farmingTutorial = FindObjectOfType<FarmingTutorial>();

		inventory = staminaTracker.gameObject.GetComponent<PlayerInventoryManager>().inv;//FindObjectOfType<FarmManager>().GetComponent<FarmManager>().playerInventory;

        mushroomManager = FindObjectOfType<MushroomManager>();

        if (GlobalGameSaving.Instance != null)
        {
            if (GlobalGameSaving.Instance.loadingSave == true)
            {
                SetDate((int)GlobalGameSaving.Instance.date.x, (int)GlobalGameSaving.Instance.date.y, (int)GlobalGameSaving.Instance.date.z, (int)GlobalGameSaving.Instance.date.w);

                netWorth.FarmNetWorth = GlobalGameSaving.Instance.farmNetWorth;

                //VERY DANGEROUS
                //Clearing scene persistence NPC Lists, to get rid of any people that might've been added since last save, since they shouldn't exist
                ScenePersistence.Instance.NPCStartflowcharts.Clear();
                ScenePersistence.Instance.NPCQuestflowcharts.Clear();
                ScenePersistence.Instance.NPCQuests.Clear();
                ScenePersistence.Instance.NPCNames.Clear();

                //Adding to scene persistence lists based on save instance lists
                for (int i = 0; i < GlobalGameSaving.Instance.NPCNames.Count; i++)
                {
                    ScenePersistence.Instance.NPCStartflowcharts.Add(GlobalGameSaving.Instance.NPCStartflowcharts[i]);
                    ScenePersistence.Instance.NPCQuestflowcharts.Add(GlobalGameSaving.Instance.NPCQuestflowcharts[i]);
                    ScenePersistence.Instance.NPCQuests.Add(GlobalGameSaving.Instance.NPCQuests[i]);
                    ScenePersistence.Instance.NPCNames.Add(GlobalGameSaving.Instance.NPCNames[i]);
                }

                //Setting current NPCs based on save instance
                for (int i = 0; i < NPCList.Count; i++)
                {
                    if (GlobalGameSaving.Instance.NPCNames.Contains(NPCList[i].MyName) == true)
                    {
                        int index = GlobalGameSaving.Instance.NPCNames.IndexOf(NPCList[i].MyName);

                        NPCList[i].LoadFlowcharts(GlobalGameSaving.Instance.NPCStartflowcharts[index], GlobalGameSaving.Instance.NPCQuestflowcharts[index]);
                        NPCList[i].gameObject.GetComponent<Quests>().LoadQuest(GlobalGameSaving.Instance.NPCQuests[index]);

                    }
                }

                //Setting scene instance variables to save instance variables
                /*for (int i = 0; i < GlobalGameSaving.Instance.NPCNames.Count; i++)
                {
                    //These need to happen regardless
                    SaveStartChart startChart = GlobalGameSaving.Instance.NPCStartflowcharts[i];
                    SaveQuestChart questChart = GlobalGameSaving.Instance.NPCQuestflowcharts[i];
                    SaveQuest saveQuest = GlobalGameSaving.Instance.NPCQuests[i];
                    saveQuest.inventory = GlobalGameSaving.Instance.inventory;

                    //update NPCs it has
                    if (GlobalGameSaving.Instance.NPCNames.Contains(ScenePersistence.Instance.NPCNames[i]) == true)
                    {
                        //The index of the NPC to be overwritten, to avoid putting that in constantly
                        int index = GlobalGameSaving.Instance.NPCNames.IndexOf(ScenePersistence.Instance.NPCNames[i]);

                        GlobalGameSaving.Instance.NPCStartflowcharts[index] = startChart;
                        GlobalGameSaving.Instance.NPCQuestflowcharts[index] = questChart;
                        GlobalGameSaving.Instance.NPCQuests[index] = saveQuest;
                    }
                    //Add NPCs it doesn't have
                    else
                    {
                        GlobalGameSaving.Instance.NPCStartflowcharts.Add(startChart);
                        GlobalGameSaving.Instance.NPCQuestflowcharts.Add(questChart);
                        GlobalGameSaving.Instance.NPCQuests.Add(saveQuest);
                        GlobalGameSaving.Instance.NPCNames.Add(ScenePersistence.Instance.NPCNames[i]);
                    }
                }*/
            }
            else
            {
                if (ScenePersistence.Instance != null)
                {
                    SetDate((int)ScenePersistence.Instance.date.x, (int)ScenePersistence.Instance.date.y, (int)ScenePersistence.Instance.date.z, (int)ScenePersistence.Instance.date.w);

                    netWorth.FarmNetWorth = ScenePersistence.Instance.farmNetWorth;

                    for (int i = 0; i < NPCList.Count; i++)
                    {
                        if (ScenePersistence.Instance.NPCNames.Contains(NPCList[i].MyName) == true)
                        {
                            int index = ScenePersistence.Instance.NPCNames.IndexOf(NPCList[i].MyName);

                            NPCList[i].LoadFlowcharts(ScenePersistence.Instance.NPCStartflowcharts[index], ScenePersistence.Instance.NPCQuestflowcharts[index]);
                            NPCList[i].gameObject.GetComponent<Quests>().LoadQuest(ScenePersistence.Instance.NPCQuests[index]);

                        }
                    }
                }
            }
        }
    }

    public void SaveNPCs(string what)
    {
        if (what == "persist")
        {
            for (int i = 0; i < NPCList.Count; i++)
            {
                //These need to happen regardless
                NPCList[i].SaveFlowcharts(out var startChart, out var questChart);
                NPCList[i].gameObject.GetComponent<Quests>().SaveQuest(out var saveQuest);
                saveQuest.inventory = ScenePersistence.Instance.inventory;

                //If it does contain it, just overwrite them
                if (ScenePersistence.Instance.NPCNames.Contains(NPCList[i].MyName) == true)
                {
                    //The index of the NPC to be overwritten, to avoid putting that in constantly
                    int index = ScenePersistence.Instance.NPCNames.IndexOf(NPCList[i].MyName);

                    ScenePersistence.Instance.NPCStartflowcharts[index] = startChart;
                    ScenePersistence.Instance.NPCQuestflowcharts[index] = questChart;
                    ScenePersistence.Instance.NPCQuests[index] = saveQuest;
                }
                //If it doesn't, add them
                else
                {
                    ScenePersistence.Instance.NPCStartflowcharts.Add(startChart);
                    ScenePersistence.Instance.NPCQuestflowcharts.Add(questChart);
                    ScenePersistence.Instance.NPCQuests.Add(saveQuest);
                    ScenePersistence.Instance.NPCNames.Add(NPCList[i].MyName);

                }
            }
        }
        //Set the global NPCs to the persistence NPCs
        //All NPCs in the game will be saved in the state they were when you saved
        else if (what == "save")
        {
            //Get all other NPCs in game, skipping over those in this scene, since persistence wouldn't have the most up to date information
            for (int i = 0; i < ScenePersistence.Instance.NPCNames.Count; i++)
            {
                //These need to happen regardless
                SaveStartChart startChart = ScenePersistence.Instance.NPCStartflowcharts[i];
                SaveQuestChart questChart= ScenePersistence.Instance.NPCQuestflowcharts[i];
                SaveQuest saveQuest = ScenePersistence.Instance.NPCQuests[i];
                saveQuest.inventory = ScenePersistence.Instance.inventory;

                //update NPCs it has
                if (GlobalGameSaving.Instance.NPCNames.Contains(ScenePersistence.Instance.NPCNames[i]) == true)
                {
                    //The index of the NPC to be overwritten, to avoid putting that in constantly
                    int index = GlobalGameSaving.Instance.NPCNames.IndexOf(ScenePersistence.Instance.NPCNames[i]);

                    GlobalGameSaving.Instance.NPCStartflowcharts[index] = startChart;
                    GlobalGameSaving.Instance.NPCQuestflowcharts[index] = questChart;
                    GlobalGameSaving.Instance.NPCQuests[index] = saveQuest;
                }
                //Add NPCs it doesn't have
                else
                {
                    GlobalGameSaving.Instance.NPCStartflowcharts.Add(startChart);
                    GlobalGameSaving.Instance.NPCQuestflowcharts.Add(questChart);
                    GlobalGameSaving.Instance.NPCQuests.Add(saveQuest);
                    GlobalGameSaving.Instance.NPCNames.Add(ScenePersistence.Instance.NPCNames[i]);
                }
            }

            //Get the NPCs currently in the scene to overwrite potentially outdated information
            for (int i = 0; i < NPCList.Count; i++)
            {
                //These need to happen regardless
                NPCList[i].SaveFlowcharts(out var startChart, out var questChart);
                NPCList[i].gameObject.GetComponent<Quests>().SaveQuest(out var saveQuest);
                saveQuest.inventory = GlobalGameSaving.Instance.inventory;

                //If it does contain it, just overwrite them
                if (GlobalGameSaving.Instance.NPCNames.Contains(NPCList[i].MyName) == true)
                {
                    //The index of the NPC to be overwritten, to avoid putting that in constantly
                    int index = GlobalGameSaving.Instance.NPCNames.IndexOf(NPCList[i].MyName);

                    GlobalGameSaving.Instance.NPCStartflowcharts[index] = startChart;
                    GlobalGameSaving.Instance.NPCQuestflowcharts[index] = questChart;
                    GlobalGameSaving.Instance.NPCQuests[index] = saveQuest;
                }
                //If it doesn't, add them
                else
                {
                    GlobalGameSaving.Instance.NPCStartflowcharts.Add(startChart);
                    GlobalGameSaving.Instance.NPCQuestflowcharts.Add(questChart);
                    GlobalGameSaving.Instance.NPCQuests.Add(saveQuest);
                    GlobalGameSaving.Instance.NPCNames.Add(NPCList[i].MyName);
                }
            }
        }

        //check if the scene persistence script contains the name of an NPC
        /*for (int i = 0; i < NPCList.Count; i++)
        {
            if (what == "persist")
            {
                //These need to happen regardless
                NPCList[i].SaveFlowcharts(out var startChart, out var questChart);
                NPCList[i].gameObject.GetComponent<Quests>().SaveQuest(out var saveQuest);
                saveQuest.inventory = ScenePersistence.Instance.inventory;

                //If it does contain it, just overwrite them
                if (ScenePersistence.Instance.NPCNames.Contains(NPCList[i].MyName) == true)
                {
                    //The index of the NPC to be overwritten, to avoid putting that in constantly
                    int index = ScenePersistence.Instance.NPCNames.IndexOf(NPCList[i].MyName);

                    ScenePersistence.Instance.NPCStartflowcharts[index] = startChart;
                    ScenePersistence.Instance.NPCQuestflowcharts[index] = questChart;
                    ScenePersistence.Instance.NPCQuests[index] = saveQuest;
                }
                //If it doesn't, add them
                else
                {
                    ScenePersistence.Instance.NPCStartflowcharts.Add(startChart);
                    ScenePersistence.Instance.NPCQuestflowcharts.Add(questChart);
                    ScenePersistence.Instance.NPCQuests.Add(saveQuest);
                    ScenePersistence.Instance.NPCNames.Add(NPCList[i].MyName);

                }
            }
            else if (what == "save")
            {
                //These need to happen regardless
                NPCList[i].SaveFlowcharts(out var startChart, out var questChart);
                NPCList[i].gameObject.GetComponent<Quests>().SaveQuest(out var saveQuest);
                saveQuest.inventory = GlobalGameSaving.Instance.inventory;

                //If it does contain it, just overwrite them
                if (GlobalGameSaving.Instance.NPCNames.Contains(NPCList[i].MyName) == true)
                {
                    //The index of the NPC to be overwritten, to avoid putting that in constantly
                    int index = GlobalGameSaving.Instance.NPCNames.IndexOf(NPCList[i].MyName);

                    GlobalGameSaving.Instance.NPCStartflowcharts[index] = startChart;
                    GlobalGameSaving.Instance.NPCQuestflowcharts[index] = questChart;
                    GlobalGameSaving.Instance.NPCQuests[index] = saveQuest;
                }
                //If it doesn't, add them
                else
                {
                    GlobalGameSaving.Instance.NPCStartflowcharts.Add(startChart);
                    GlobalGameSaving.Instance.NPCQuestflowcharts.Add(questChart);
                    GlobalGameSaving.Instance.NPCQuests.Add(saveQuest);
                    GlobalGameSaving.Instance.NPCNames.Add(NPCList[i].MyName);

                }
            }
        }*/
    }

    public void SaveDate(string what)
    {
        if (what == "persist")
        {
            ScenePersistence.Instance.date = new Vector4(DayNumber, DateNumber, YearNumber, SeasonNumber);
        }
        else if (what == "save")
        {
            GlobalGameSaving.Instance.date = new Vector4(DayNumber, DateNumber, YearNumber, SeasonNumber);
        }
    }

    public void SaveWorth()
    {
        netWorth.SaveWorth(out var savedWorth);
        ScenePersistence.Instance.farmNetWorth = savedWorth;
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
    public void AdvanceDay()
    {
        Debug.Log("Advancing with Day");
        
        //change year
        if (seasonNum == 4 && dateNum == 30)
        {
            yearNum++;

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

        netWorth.CalculateNetWorth(5);


        //Final thing in a day that determines net worth. If it goes over a threshold, activate a quest
        foreach (NPCManager npc in NPCList)
        {
            //change dialogue and quest variables
            npc.MyFlowchart.SetIntegerVariable("seasonNum", seasonNum);
            npc.MyFlowchart.SetIntegerVariable("dateNum", dateNum);
            npc.MyFlowchart.SetIntegerVariable("playerNetWorth", netWorth.FarmNetWorth);
            npc.MyFlowchart.SetBooleanVariable("spokenToOnce", false);
            npc.MyFlowchart.SetBooleanVariable("spokenToTwice", false);

            if (npc.gameObject.GetComponent<Quests>().GetQuestAccepted() == true)
            {
                if (npc.gameObject.GetComponent<Quests>().questType == Quests.QuestType.TimedCollection || npc.gameObject.GetComponent<Quests>().questType == Quests.QuestType.TimedFundraising)
                {
                    if (npc.gameObject.GetComponent<Quests>().GetDaysToFail() > 0)
                    {
                        npc.gameObject.GetComponent<Quests>().SetDaysToFail(npc.gameObject.GetComponent<Quests>().GetDaysToFail() - 1);

                        Debug.Log($"Days remaining: {npc.gameObject.GetComponent<Quests>().GetDaysToFail()}");
                    }
                    else
                    {
                        npc.gameObject.GetComponent<Quests>().SetQuestFailed(true);
                        npc.gameObject.GetComponent<Quests>().MyFlowchart.SetBooleanVariable("questFailed", npc.gameObject.GetComponent<Quests>().GetQuestFailed());

                        Debug.Log($"Days remaining: {npc.gameObject.GetComponent<Quests>().GetDaysToFail()}; Failed Quest?: {npc.gameObject.GetComponent<Quests>().GetQuestFailed()}");
                    }
                }
            }

        }

        DisplayDate();
    }

	public void UpdateField()
	{
		List<Vector3Int> keysToReplace = new List<Vector3Int>();

		//Uses the Farm Managers dictionary of mushrooms to grow each mushroom and then dry them out for the next day
		foreach (KeyValuePair<Vector3Int, Tile> shroom in management.mushroomsAndTiles)
		{
			if (shroom.Value == null)
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

		//Tutorial Softlock Prevention
		//Check all mushroom names. If none of them are there, instantiate one.
		if (farmingTutorial != null)
		{
			if (farmingTutorial.tutorialBools[16] == false)//(farmingTutorial.spreadAfter == false)
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
			else if (farmingTutorial.tutorialBools[16] == true && farmingTutorial.tutorialBools[18] == false)//(farmingTutorial.spreadAfter == true && farmingTutorial.hybridAfter == false)
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
		}

		shippingBin.PayPlayer();
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

        if (farmingTutorial != null)
        {
            if (farmingTutorial.tutorialBools[6] == true)//(farmingTutorial.wateredAfter == true)
            {
                farmingTutorial.tutorialBools[8] = true;//farmingTutorial.sleptAfter = true;
                GlobalGameSaving.Instance.tutorialBools[8] = farmingTutorial.tutorialBools[8];
            }
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

            
                AdvanceDay();
	    }
		GameObject.Find("Player").GetComponent<PlayerMovement>().visited = new List<string>();
        
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
                dayDisplay.text = "Mon";
                break;
            case 2:
                dayDisplay.text = "Tues";
                break;
            case 3:
                dayDisplay.text = "Wed";
                break;
            case 4:
                dayDisplay.text = "Thurs";
                break;
            case 5:
                dayDisplay.text = "Fri";
                break;
            case 6:
                dayDisplay.text = "Sat";
                break;
            case 7:
                dayDisplay.text = "Sun";
                break;
            default:
                dayDisplay.text = "";
                break;
        }

        switch (seasonNum)
        {
            case 1:
                switch (dateNum)
                {
                    case 1:
                        holidayDisplay.text = "New Year's Day";
                        break;
                    default:
						if(holidayDisplay == null)
							holidayDisplay = GameObject.Find("HolidayDisplay").GetComponent<TMP_Text>();
						holidayDisplay.text = "";
                        break;
                }
                break;
            case 2:
                switch (dateNum)
                {
                    case 1:
                        break;
                    default:
                        holidayDisplay.text = "";
                        break;
                }
                break;
            case 3:
                switch (dateNum)
                {
                    case 1:
                        break;
                    default:
                        holidayDisplay.text = "";
                        break;
                }
                break;
            case 4:
                switch (dateNum)
                {
                    case 1:
                        break;
                    default:
                        holidayDisplay.text = "";
                        break;
                }
                break;
            default:
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
