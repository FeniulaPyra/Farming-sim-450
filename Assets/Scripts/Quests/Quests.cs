using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script will be made with the intention of being used quest by quest
//The script will handle a single quest only
public class Quests : MonoBehaviour
{
    public CalculateFarmNetWorth netWorth;

    public Inventory inventory;

    FarmManager farmManager;

    public miniQuest activeQuest;

    DialogueManager myNPC;

    [SerializeField]
    int questIndex;

    public enum QuestType
    {
        Collection,
        Fundraising,
        TimedCollection,
        TimedFundraising
    }

    [System.Serializable]
    public class miniQuest
    {   
        //For tracking when the player has started and completed a question
        [SerializeField]
        public bool questActive;
        [SerializeField]
        public bool readyToReport;
        [SerializeField]
        public bool questComplete;

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
        public List<int> requiredItemsCountList = new List<int>();
        public List<int> requiredItemsAmountList = new List<int>();

        //For fundraising quests
        public int moneyRequired;
        public int moneyEarnedSinceQuestStart = 0;

        

        public int MoneyEarnedSinceQuestStart
        {
            set
            {
                moneyEarnedSinceQuestStart += value;
            }
        }

        //For timed Quests
        [SerializeField]
        public int daysToQuestFail;

        public int DaysToQuestFail
        {
            set
            {
                daysToQuestFail = value;
            }
        }
    }

    public List<miniQuest> miniQuests = new List<miniQuest>();
    

    // Start is called before the first frame update
    void Start()
    {
        netWorth = FindObjectOfType<CalculateFarmNetWorth>();

        farmManager = FindObjectOfType<FarmManager>();

		inventory = GameObject.Find("Player").GetComponent<PlayerInventoryManager>().inv;//farmManager.GetComponent<FarmManager>().playerInventory;

        //populating list with something at start
        foreach (miniQuest m in miniQuests)
        {
            foreach (Item i in m.requiredItemList)
            {
                m.requiredItemsCountList.Add(0);
            }
        }

        questIndex = 0;

        activeQuest = miniQuests[questIndex];

        myNPC = gameObject.GetComponent<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
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

        /*if (activeQuest.readyToReport == true && activeQuest.questComplete == false)
        {
            myNPC.oldConvoID = myNPC.convoID;
            myNPC.convoID = myNPC.myQuests.activeQuest.endID;
        }*/

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
        }
    }
}
