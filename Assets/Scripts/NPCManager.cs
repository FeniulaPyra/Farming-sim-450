using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class NPCManager : MonoBehaviour
{
    //The NPC's Start Block. The rest will just naturally follow, so this is the only one it needs to know

    Flowchart myFlowchart;
    Flowchart myQuestFlowchart;
    [SerializeField]
    string myName;

    public Flowchart MyFlowchart
    {
        get
        {
            return myFlowchart;
        }
        set
        {
            myFlowchart = value;
        }
    }

    public Flowchart MyQuestFlowchart
    {
        get
        {
            return myQuestFlowchart;
        }
        set
        {
            myQuestFlowchart = value;
        }
    }

    public string MyName
    {
        get
        {
            return myName;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myFlowchart = transform.Find("Start").GetComponent<Flowchart>();
        myQuestFlowchart = transform.Find("Quests").GetComponent<Flowchart>();
        myName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveFlowcharts(out SaveStartChart start, out SaveQuestChart quests)
    {
        start = new SaveStartChart(myFlowchart.GetIntegerVariable("seasonNum"), myFlowchart.GetIntegerVariable("dateNum"), myFlowchart.GetIntegerVariable("playerNetWorth"), myFlowchart.GetBooleanVariable("spokenToOnce"), myFlowchart.GetBooleanVariable("spokenToTwice"), myFlowchart.GetBooleanVariable("questAccepted"), myFlowchart.GetBooleanVariable("questReadyToReport"), myFlowchart.GetBooleanVariable("questComplete"));

        quests = new SaveQuestChart(myQuestFlowchart.GetBooleanVariable("Quest1Accepted"), myQuestFlowchart.GetBooleanVariable("Quest1ReadyToReport"), myQuestFlowchart.GetBooleanVariable("Quest1Complete"), myQuestFlowchart.GetBooleanVariable("Quest1Failed"), myQuestFlowchart.GetIntegerVariable("QuestCounter"));
    }

    public void LoadFlowcharts(SaveStartChart start, SaveQuestChart quests)
    {
        myFlowchart.SetIntegerVariable("seasonNum", start.seasonNum);
        myFlowchart.SetIntegerVariable("dateNum", start.dateNum);
        myFlowchart.SetIntegerVariable("playerNetWorth", start.playerNetWorth);
        myFlowchart.SetBooleanVariable("spokenToOnce", start.spokenToOnce);
        myFlowchart.SetBooleanVariable("spokenToTwice", start.spokenToTwice);
        myFlowchart.SetBooleanVariable("questAccepted", start.questAccepted);
        myFlowchart.SetBooleanVariable("questReadyToReport", start.questReadyToReport);
        myFlowchart.SetBooleanVariable("questComplete", start.questComplete);

        myQuestFlowchart.SetBooleanVariable("Quest1Accepted", quests.quest1Accepted);
        myQuestFlowchart.SetBooleanVariable("Quest1ReadyToReport", quests.quest1ReadyToReport);
        myQuestFlowchart.SetBooleanVariable("Quest1Complete", quests.quest1Complete);
        myQuestFlowchart.SetBooleanVariable("Quest1Failed", quests.quest1Complete);
        myQuestFlowchart.SetIntegerVariable("QuestCounter", quests.questCounter);
    }
}

[System.Serializable]
public class SaveStartChart
{
    public int seasonNum;
    public int dateNum;
    public int playerNetWorth;
    public bool spokenToOnce;
    public bool spokenToTwice;
    public bool questAccepted;
    public bool questReadyToReport;
    public bool questComplete;

    public SaveStartChart(int seasonNum, int dateNum, int playerNetWorth, bool spokenToOnce, bool spokenToTwice, bool questAccepted, bool questReadyToReport, bool questComplete)
    {
        this.seasonNum = seasonNum;
        this.dateNum = dateNum;
        this.playerNetWorth = playerNetWorth;
        this.spokenToOnce = spokenToOnce;
        this.spokenToTwice = spokenToTwice;
        this.questAccepted = questAccepted;
        this.questReadyToReport = questReadyToReport;
        this.questComplete = questComplete;
    }
}

[System.Serializable]
public class SaveQuestChart
{
    public bool quest1Accepted;
    public bool quest1ReadyToReport;
    public bool quest1Complete;
    public bool quest1Failed;
    public int questCounter;

    public SaveQuestChart(bool quest1Accepted, bool quest1ReadyToReport, bool quest1complete, bool quest1Failed, int questCounter)
    {
        this.quest1Accepted = quest1Accepted;
        this.quest1ReadyToReport = quest1ReadyToReport;
        this.quest1Complete = quest1complete;
        this.quest1Failed = quest1Failed;
        this.questCounter = questCounter;
    }
}
