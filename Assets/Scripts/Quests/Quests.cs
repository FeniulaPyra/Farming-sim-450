using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Fungus;

//Script will be made with the intention of being used quest by quest
//The script will handle a single quest only
public class Quests : MonoBehaviour
{
    public CalculateFarmNetWorth netWorth;

    public Inventory inventory;

    FarmManager farmManager;

    //public miniQuest activeQuest;

    //DialogueManager myNPC;
    NPCManager myNPC;

    //Flowchart of NPC that has the quest
    Flowchart myFlowchart;


    [SerializeField]
    int questIndex;

    public enum QuestType
    {
        Collection,
        Fundraising,
        TimedCollection,
        TimedFundraising
    }

    [SerializeField]
    bool questActive;
    /*public bool GetQuestActive()
    {
        return questActive; 
    }*/
    public void SetQuestActive(bool value)
    {
        questActive = value;
    }

    [SerializeField]
    bool readyToReport;
    public bool GetQuestReadyToReport()
    {
        return readyToReport;
    }
    public void SetQuestReadyToReport(bool value)
    {
        readyToReport = value;
    }

    [SerializeField]
    bool questComplete;
    /*public bool GetQuestComplete()
    {
        return questComplete;
    }*/
    public void SetQuestComplete(bool value)
    {
        questComplete = value;
    }

    //For fundraising quests
    [SerializeField]
    int moneyRequired;
    public int GetMoneyRequired()
    {
        return moneyRequired;
    }

    [SerializeField]
    int moneyEarnedSinceQuestStart = 0;
    public int GetMoneySinceStart()
    {
        return moneyEarnedSinceQuestStart;
    }
    public void SetMoneySinceStart(int value)
    {
        moneyEarnedSinceQuestStart = value;
    }

    //For timed Quests
    [SerializeField]
    int daysToQuestFail;
    public int GetDaysToFail()
    {
        return daysToQuestFail;
    }
    public void SetDaysToFail(int value)
    {
        daysToQuestFail = value;
    }



    //Tied to dialogue manager
    public string beginID;
    public string endID;

    [SerializeField]
    //The minimum net worth the player's farm must have to make the quest run
    public int requiredNetWorth;


    //Will determine what parts of update run each frame
    public QuestType questType;

    //For collection quests
    public List<Item> requiredItemList = new List<Item>();
    //How many of them you have
    public List<int> requiredItemsCountList = new List<int>();
    //How many of them you need
    public List<int> requiredItemsAmountList = new List<int>();

    //public List<miniQuest> miniQuests = new List<miniQuest>();

    /// <summary>
    /// Big old function for setting quest parameters
    /// </summary>
    /// <param name="type">The type of quest; will determine how other things are set</param>
    /// <param name="item1">The first of three potential items</param>
    /// <param name="item2">The second of three potential items</param>
    /// <param name="item3">The second of three potential items</param>
    /// <param name="item1Count">How many of the first item the player needs</param>
    /// <param name="Item2Count">How many of the second item the player needs</param>
    /// <param name="Item3Count">How many of the third item the player needs</param>
    /// <param name="goldRequired">How much gold the player needs for the quest</param>
    /// <param name="daysToFail">How many days the player has until they fail the quest</param>
    public void SetQuestParams(QuestType type, Item item1, Item item2, Item item3, int item1Count, int item2Count, int item3Count, int goldRequired, int daysToFail)
    {
        questType = type;

        if (questType == QuestType.Collection || questType == QuestType.TimedCollection)
        {
            requiredItemList.Add(item1);
            requiredItemList.Add(item2);
            requiredItemList.Add(item3);
            requiredItemsAmountList.Add(item1Count);
            requiredItemsAmountList.Add(item2Count);
            requiredItemsAmountList.Add(item3Count);
        }
        else if (questType == QuestType.Fundraising || questType == QuestType.TimedFundraising)
        {
            moneyRequired = goldRequired;
        }
        if (questType == QuestType.TimedCollection || questType == QuestType.TimedFundraising)
        {
            daysToQuestFail = daysToFail;
        }
    }

    /// <summary>
    /// Resetting the NPC's quest upon completion by changing all variables to default
    /// </summary>
    public void ResetQuest()
    {
        questActive = false;
        readyToReport = false;
        questComplete = false;
        requiredItemList.Clear();
        requiredItemsAmountList.Clear();
        requiredItemsCountList.Clear();
        moneyRequired = 0;
        moneyEarnedSinceQuestStart = 0;
        daysToQuestFail = 0;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        netWorth = FindObjectOfType<CalculateFarmNetWorth>();

        farmManager = FindObjectOfType<FarmManager>();

        inventory = farmManager.GetComponent<FarmManager>().playerInventory;

        //populating list with something at start
        /*foreach (miniQuest m in miniQuests)
        {
            foreach (Item i in m.requiredItemList)
            {
                m.requiredItemsCountList.Add(0);
            }
        }

        questIndex = 0;

        activeQuest = miniQuests[questIndex];

        

        miniQuest testQuest = new miniQuest(50);*/

        //myNPC = gameObject.GetComponent<DialogueManager>();
        myNPC = gameObject.GetComponent<NPCManager>();

        myFlowchart = gameObject.transform.Find("Quests").GetComponent<Flowchart>();
    }

    // Update is called once per frame
    void Update()
    {
        //Quest has been started
        if (questActive == true && readyToReport == false)
        {
            if (questType == QuestType.Collection)
            {
                for (int i = 0; i < requiredItemList.Count; i++)
                {
                    requiredItemsCountList[i] = inventory.CountItem(requiredItemList[i].name);

                    //Ex. if 5> 4
                    if (requiredItemsCountList[i] >= requiredItemsAmountList[i])
                    {
                        if (i == requiredItemList.Count - 1)
                        {
                            //quest complete
                            //Account for Fungus, somehow
                            Debug.Log("Quest complete item");
                            readyToReport = true;
                            //myNPC.oldConvoID = myNPC.convoID;
                            //myNPC.convoID = myNPC.myQuests.activeQuest.endID;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (questType == QuestType.Fundraising)
            {
                if (moneyEarnedSinceQuestStart > moneyRequired)
                {
                    //quest complete
                    readyToReport = true;
                }
            }
        }
        /*if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log($"There are {inventory.CountItem(miniQuests[0].requiredItemList[0].name)} {miniQuests[0].requiredItemList[0].name}s");
        }

        if (activeQuest.questComplete == true)
        {
            //moves on to next quest
            if (questIndex < miniQuests.Count - 1)
            {
                questIndex++;
                activeQuest = miniQuests[questIndex];
                Debug.Log("New Quest");
            }
        }

        if (activeQuest.questActive == true && activeQuest.readyToReport == false)
        {
            //The problem with collection quests is that they are dependend on somehow seeing how much of an item is in the player's inventory
            if (activeQuest.questType == QuestType.Collection)
            {
                for (int i = 0; i < activeQuest.requiredItemList.Count; i++)
                {
                    activeQuest.requiredItemsCountList[i] = inventory.CountItem(activeQuest.requiredItemList[i].name);

                    //Ex. if 5> 4
                    if (activeQuest.requiredItemsCountList[i] >= activeQuest.requiredItemsAmountList[i])
                    {
                        if (i == activeQuest.requiredItemList.Count - 1)
                        {
                            //quest complete
                            Debug.Log("Quest complete item");
                            activeQuest.readyToReport = true;
                            myNPC.oldConvoID = myNPC.convoID;
                            myNPC.convoID = myNPC.myQuests.activeQuest.endID;
                        }
                        else
                        { 
                            continue;
                        }
                    }
                    else
                    {
                        break;
                    }

                    
                }
            }
            else if (activeQuest.questType == QuestType.Fundraising)
            {
                if (activeQuest.moneyEarnedSinceQuestStart > activeQuest.moneyRequired)
                {
                    //quest complete
                    activeQuest.readyToReport = true;
                }
            }
            else if (activeQuest.questType == QuestType.TimedCollection)
            {
                if (activeQuest.daysToQuestFail > 0)
                {
                    for (int i = 0; i < activeQuest.requiredItemList.Count; i++)
                    {
                        //Ex. if 5> 4
                        if (activeQuest.requiredItemsCountList[i] >= activeQuest.requiredItemsAmountList[i])
                        {
                            if (i == activeQuest.requiredItemList.Count - 1)
                            {
                                //quest complete
                                Debug.Log("Quest complete item");
                                activeQuest.readyToReport = true;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else if (activeQuest.questType == QuestType.TimedFundraising)
            {
                if (activeQuest.daysToQuestFail > 0)
                {
                    if (activeQuest.moneyEarnedSinceQuestStart > activeQuest.moneyRequired)
                    {
                        //quest complete
                        activeQuest.readyToReport = true;
                    }
                }
            }
        }*/
    }
}
