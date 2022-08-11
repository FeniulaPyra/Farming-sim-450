using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class NPCManager : MonoBehaviour
{
    //The NPC's Start Block. The rest will just naturally follow, so this is the only one it needs to know

    public Flowchart myFlowchart;
    public Flowchart myQuestFlowchart;
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
        //myFlowchart = transform.Find("Start").GetComponent<Flowchart>();
        //myQuestFlowchart = transform.Find("Quests").GetComponent<Flowchart>();
        myName = gameObject.name;
    }

    public void SaveFlowcharts(out SaveStartChart start, out SaveQuestChart quests)
    {
        start = new SaveStartChart(myFlowchart.GetIntegerVariable("seasonNum"), myFlowchart.GetIntegerVariable("dateNum"), myFlowchart.GetIntegerVariable("playerNetWorth"), myFlowchart.GetBooleanVariable("spokenToOnce"), myFlowchart.GetBooleanVariable("spokenToTwice"), myFlowchart.GetBooleanVariable("questAccepted"), myFlowchart.GetBooleanVariable("questReadyToReport"), myFlowchart.GetBooleanVariable("questComplete"));

        quests = new SaveQuestChart(myQuestFlowchart.GetBooleanVariable("questAccepted"), myQuestFlowchart.GetBooleanVariable("questReadyToReport"), myQuestFlowchart.GetBooleanVariable("questComplete"), myQuestFlowchart.GetBooleanVariable("questFailed"), myQuestFlowchart.GetIntegerVariable("QuestCounter"));
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

        myQuestFlowchart.SetBooleanVariable("questAccepted", quests.questAccepted);
        myQuestFlowchart.SetBooleanVariable("questReadyToReport", quests.questReadyToReport);
        myQuestFlowchart.SetBooleanVariable("questComplete", quests.questComplete);
        myQuestFlowchart.SetBooleanVariable("questFailed", quests.questComplete);
        myQuestFlowchart.SetIntegerVariable("questCounter", quests.questCounter);
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
    public bool questAccepted;
    public bool questReadyToReport;
    public bool questComplete;
    public bool questFailed;
    public int questCounter;

    public SaveQuestChart(bool questAccepted, bool questReadyToReport, bool questcomplete, bool questFailed, int questCounter)
    {
        this.questAccepted = questAccepted;
        this.questReadyToReport = questReadyToReport;
        this.questComplete = questcomplete;
        this.questFailed = questFailed;
        this.questCounter = questCounter;
    }
}
