using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script will be made with the intention of being used quest by quest
//The script will handle a single quest only
public class Quests : MonoBehaviour
{
    public enum QuestType
    {
        Collection,
        Fundraising,
        TimedCollection,
        TimedFundraising
    }

    //For tracking when the player has started and completed a question
    [SerializeField]
    bool questActive;
    [SerializeField]
    bool questComplete;

    [SerializeField]
    //The minimum net worth the player's farm must have to make the quest run
    int requiredNetWorth;

    public CalculateFarmNetWorth netWorth;

    //Will determine what parts of update run each frame
    public QuestType questType;

    //For collection quests
    [SerializeField]
    List<Item> requiredItemList = new List<Item>();
    List<int> requiredItemsCountList = new List<int>();
    List<int> requiredItemsAmountList = new List<int>();

    //For fundraising quests
    public int moneyRequired;
    [SerializeField]
    int moneyEarnedSinceQuestStart = 0;

    public int MoneyEarnedSinceQuestStart
    {
        set
        {
            moneyEarnedSinceQuestStart += value;
        }
    }

    //For timed Quests
    [SerializeField]
    int daysToQuestFail;

    public int DaysToQuestFail
    {
        set
        {
            daysToQuestFail = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        netWorth = FindObjectOfType<CalculateFarmNetWorth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (questActive == true)
        {
            //The problem with collection quests is that they are dependend on somehow seeing how much of an item is in the player's inventory
            if (questType == QuestType.Collection)
            {
                for (int i = 0; i < requiredItemList.Count; i++)
                {
                    //Ex. if 5> 4
                    if (requiredItemsCountList[i] >= requiredItemsAmountList[i])
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }

                    if (i == requiredItemList.Count - 1)
                    {
                        //quest complete
                    }
                }
            }
            else if (questType == QuestType.Fundraising)
            {
                if (moneyEarnedSinceQuestStart > moneyRequired)
                {
                    //quest complete
                }
            }
            else if (questType == QuestType.TimedCollection)
            {
                if (daysToQuestFail > 0)
                {
                    for (int i = 0; i < requiredItemList.Count; i++)
                    {
                        //Ex. if 5> 4
                        if (requiredItemsCountList[i] >= requiredItemsAmountList[i])
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }

                        if (i == requiredItemList.Count - 1)
                        {
                            //quest complete
                        }
                    }
                }
            }
            else if (questType == QuestType.TimedFundraising)
            {
                if (daysToQuestFail > 0)
                {
                    if (moneyEarnedSinceQuestStart > moneyRequired)
                    {
                        //quest complete
                    }
                }
            }
        }
    }
}
