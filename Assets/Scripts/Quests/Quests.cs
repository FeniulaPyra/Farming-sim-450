using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Fungus;
using TMPro;

//Script will be made with the intention of being used quest by quest
//The script will handle a single quest only
public class Quests : MonoBehaviour
{
    public CalculateFarmNetWorth netWorth;

    public Inventory inventory;

    public PlayerInteraction interaction;

    FarmManager farmManager;

    //public miniQuest activeQuest;

    //DialogueManager myNPC;
    NPCManager myNPC;

    //Flowchart of NPC that has the quest
    Flowchart myFlowchart;

    public Flowchart MyFlowchart
    {
        get
        {
            return myFlowchart;
        }
    }

    [SerializeField]
    int questIndex;

    public enum QuestType
    {
        Collection,
        Fundraising,
        TimedCollection,
        TimedFundraising
    }

    //Will determine what parts of update run each frame
    public QuestType questType;

    [SerializeField]
    bool questAccepted;
    public bool GetQuestAccepted()
    {
        return questAccepted; 
    }
    public void SetQuestAccepted(bool value)
    {
        questAccepted = value;
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

    [SerializeField]
    bool questFailed;
    public bool GetQuestFailed()
    {
        return questFailed;
    }
    public void SetQuestFailed(bool value)
    {
        questFailed = value;
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
    public void SetQuestParams(QuestType type, Item item1, Item item2, Item item3, int numItems, int item1Count, int item2Count, int item3Count, int goldRequired, int daysToFail)
    {
        questType = type;

        if (questType == QuestType.Collection || questType == QuestType.TimedCollection)
        {
            switch (numItems)
            {
                case 1:
                    requiredItemList.Add(item1);
                    requiredItemsAmountList.Add(item1Count);
                    break;
                case 2:
                    requiredItemList.Add(item1);
                    requiredItemList.Add(item2);
                    requiredItemsAmountList.Add(item1Count);
                    requiredItemsAmountList.Add(item2Count);
                    break;
                case 3:
                    requiredItemList.Add(item1);
                    requiredItemList.Add(item2);
                    requiredItemList.Add(item3);
                    requiredItemsAmountList.Add(item1Count);
                    requiredItemsAmountList.Add(item2Count);
                    requiredItemsAmountList.Add(item3Count);
                    break;
                default:
                    requiredItemList.Add(item1);
                    requiredItemList.Add(item2);
                    requiredItemList.Add(item3);
                    requiredItemsAmountList.Add(item1Count);
                    requiredItemsAmountList.Add(item2Count);
                    requiredItemsAmountList.Add(item3Count);
                    break;
            }

            foreach (Item item in requiredItemList)
            {
                requiredItemsCountList.Add(inventory.CountItem(item.name));
            }
            
        }
        else if (questType == QuestType.Fundraising || questType == QuestType.TimedFundraising)
        {
            moneyRequired = goldRequired;

            moneyEarnedSinceQuestStart = interaction.playerGold;
        }
        if (questType == QuestType.TimedCollection || questType == QuestType.TimedFundraising)
        {
            daysToQuestFail = daysToFail;
        }
    }

    /// <summary>
    /// Based on the quest's type, does certain things to the player
    /// </summary>
    /// <param name="type"></param>
    public void TurnInQuest(QuestType type)
    {
        if (type == QuestType.Collection || type == QuestType.TimedCollection)
        {
            for (int i = 0; i < requiredItemList.Count; i++)
            {
                inventory.RemoveItems(requiredItemList[i], requiredItemsAmountList[i]);
                //Debug.Log("Quest Turned In");//
            }
        }
        else if (type == QuestType.Fundraising || type == QuestType.TimedFundraising)
        {
            interaction.playerGold -= moneyRequired;

            GameObject.Find("GoldDisplay").GetComponent<TMP_Text>().text = $"{interaction.playerGold} G";
        }
    }

    /// <summary>
    /// Rewards the player after turning in the quest
    /// </summary>
    /// <param name="quest">item for money, money for items</param>
    /// <param name="reward">the item that player will be given</param>
    /// <param name="payout">how much money the player will be given</param>
    public void RewardPlayer(QuestType type, Item reward, int amount, int payout)
    {
        if (type == QuestType.Collection || type == QuestType.TimedCollection)
        {
            interaction.playerGold += payout;

            GameObject.Find("GoldDisplay").GetComponent<TMP_Text>().text = $"{interaction.playerGold} G";
        }
        else if (type == QuestType.Fundraising || type == QuestType.TimedFundraising)
        {
            inventory.AddItems(new ItemStack(reward, amount));
        }
    }

    /// <summary>
    /// Resetting the NPC's quest upon completion by changing all variables to default
    /// </summary>
    public void ResetQuest()
    {
        questAccepted = false;
        readyToReport = false;
        questComplete = false;
        questFailed = false;
        requiredItemList.Clear();
        requiredItemsAmountList.Clear();
        requiredItemsCountList.Clear();
        moneyRequired = 0;
        moneyEarnedSinceQuestStart = 0;
        daysToQuestFail = 0;
    }

    public void SaveQuest(out SaveQuest saved)
    {
        saved = new SaveQuest(netWorth, interaction, farmManager, myNPC, myFlowchart, questType, questAccepted, readyToReport, questComplete, questFailed, moneyRequired, moneyEarnedSinceQuestStart, daysToQuestFail, requiredNetWorth, requiredItemList, requiredItemsCountList, requiredItemsAmountList);
    }

    public void LoadQuest(SaveQuest QuestToLoad)
    {
        netWorth = QuestToLoad.netWorth;

        inventory.SetSaveableInventory(QuestToLoad.inventory);

        interaction = QuestToLoad.interaction;
        farmManager = QuestToLoad.farmManager;
        myNPC = QuestToLoad.myNPC;
        myFlowchart = QuestToLoad.myFlowchart;
        questType = QuestToLoad.questType;
        questAccepted = QuestToLoad.questAccepted;
        readyToReport = QuestToLoad.readyToReport;
        questComplete = QuestToLoad.questComplete;
        questFailed = QuestToLoad.questFailed;
        moneyRequired = QuestToLoad.moneyRequired;
        moneyEarnedSinceQuestStart = QuestToLoad.moneyEarnedSinceQuestStart;
        daysToQuestFail = QuestToLoad.daysToQuestFail;
        requiredItemList = QuestToLoad.requiredItemList;
        requiredItemsCountList = QuestToLoad.requiredItemsCountList;
        requiredItemsAmountList = QuestToLoad.requiredItemsAmountList;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        netWorth = FindObjectOfType<CalculateFarmNetWorth>();

        farmManager = FindObjectOfType<FarmManager>();

        interaction = FindObjectOfType<PlayerInteraction>();

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
        if (questAccepted == true && readyToReport == false)
        {
            if (questType == QuestType.Collection || questType == QuestType.TimedCollection)
            {
                for (int i = 0; i < requiredItemList.Count; i++)
                {
                    requiredItemsCountList[i] = inventory.CountItem(requiredItemList[i].name);
                    //Debug.Log($"PRINT ME; You have {requiredItemsCountList[i]} of {requiredItemList[i].name}");

                    //Ex. if 5> 4
                    if (requiredItemsCountList[i] >= requiredItemsAmountList[i])
                    {
                        if (i == requiredItemList.Count - 1)
                        {
                            //quest complete
                            //Account for Fungus, somehow
                            Debug.Log("Quest complete item");
                            readyToReport = true;
                            myFlowchart.SetBooleanVariable("Quest1ReadyToReport", readyToReport);
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
            else if (questType == QuestType.Fundraising || questType == QuestType.TimedFundraising)
            {
                if (moneyEarnedSinceQuestStart > moneyRequired)
                {
                    //quest complete
                    readyToReport = true;
                    myFlowchart.SetBooleanVariable("Quest1ReadyToReport", readyToReport);
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

[System.Serializable]
public class SaveQuest
{
    public CalculateFarmNetWorth netWorth;
    //public Inventory inventory;
    public List<int> inventory;
    public PlayerInteraction interaction;
    public FarmManager farmManager;

    public NPCManager myNPC;
    public Flowchart myFlowchart;

    public Quests.QuestType questType;

    public bool questAccepted;
    public bool readyToReport;
    public bool questComplete;
    public bool questFailed;
    public int moneyRequired;
    public int moneyEarnedSinceQuestStart = 0;
    public int daysToQuestFail;

    public int requiredNetWorth;

    public List<Item> requiredItemList = new List<Item>();
    public List<int> requiredItemsCountList = new List<int>();
    public List<int> requiredItemsAmountList = new List<int>();

    //public SaveQuest(CalculateFarmNetWorth nW, Inventory i, PlayerInteraction pI, FarmManager fM, NPCManager npc, Flowchart mF, Quests.QuestType qT, bool qA, bool rTR, bool qC, bool qF, int mR, int mE, int dTQF, int rNW, List<Item> rIL, List<int> rICL, List<int> rIAL)
    public SaveQuest(CalculateFarmNetWorth nW, PlayerInteraction pI, FarmManager fM, NPCManager npc, Flowchart mF, Quests.QuestType qT, bool qA, bool rTR, bool qC, bool qF, int mR, int mE, int dTQF, int rNW, List<Item> rIL, List<int> rICL, List<int> rIAL)
    {
        netWorth = nW;
        interaction = pI;
        farmManager = fM;

        myNPC = npc;
        myFlowchart = mF;

        questType = qT;

        questAccepted = qA;
        readyToReport = rTR;
        questComplete = qC;
        questFailed = qF;
        moneyRequired = mR;
        moneyEarnedSinceQuestStart = mE;
        daysToQuestFail = dTQF;

        requiredNetWorth = rNW;

        requiredItemList = rIL;
        requiredItemsCountList = rICL;
        requiredItemsAmountList = rIAL;
    }
}
