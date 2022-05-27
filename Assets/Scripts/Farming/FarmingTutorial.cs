using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fungus;

public class FarmingTutorial : MonoBehaviour
{
    //softlock prevention
    public GameObject redShroom;
    public GameObject glowyShroom;

	public Toggle enableTutorial;

    //Player Guidance
    public TMP_Text objective;

    //Giant list of bools that will be used for this
    DialogueManager self;
    public bool tutorialStarted;

    public bool tilledBefore;
    public bool tilledAfter;

    public bool plantedBefore;
    public bool plantedAfter;

    public bool wateredBefore;
    public bool wateredAfter;

    public bool sleptBefore;
    public bool sleptAfter;

    public bool harvestedBefore;
    public bool harvestedAfter;

    public bool eatingBefore;
    public bool eatingAfter;

    public bool shippedBefore;
    public bool shippedAfter;

    public bool spreadBefore;
    public bool spreadAfter;

    public bool hybridBefore;
    public bool hybridAfter;

    public bool tutorialComplete;

    Flowchart myFlowchart;

    Block currentTutorial;

    [SerializeField]
    InteractableObjects shippingBin;

    //For saving
    public List<bool> tutorialBools;

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponentInParent<DialogueManager>();

        shippingBin = FindObjectOfType<ShippingBin>().GetComponent<InteractableObjects>();
        shippingBin.enabled = false;

        myFlowchart = transform.Find("TutorialFlowchart").GetComponent<Flowchart>();

        currentTutorial = myFlowchart.FindBlock("Start");

        objective.gameObject.SetActive(true);

        tutorialBools = new List<bool>();

        if (GlobalGameSaving.Instance != null)
        {
            if (GlobalGameSaving.Instance.loadingSave == true)
            {
                foreach (bool b in GlobalGameSaving.Instance.tutorialBools)
                {
                    tutorialBools.Add(b);
                }

                /*tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[0]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[1]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[2]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[3]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[4]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[5]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[6]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[7]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[8]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[9]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[10]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[11]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[12]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[13]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[14]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[15]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[16]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[17]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[18]);
                tutorialBools.Add(GlobalGameSaving.Instance.tutorialBools[19]);*/
            }
            else
            {
                tutorialBools.Add(tutorialStarted);
                tutorialBools.Add(tilledBefore);
                tutorialBools.Add(tilledAfter);
                tutorialBools.Add(plantedBefore);
                tutorialBools.Add(plantedAfter);
                tutorialBools.Add(wateredBefore);
                tutorialBools.Add(wateredAfter);
                tutorialBools.Add(sleptBefore);
                tutorialBools.Add(sleptAfter);
                tutorialBools.Add(harvestedBefore);
                tutorialBools.Add(harvestedAfter);
                tutorialBools.Add(eatingBefore);
                tutorialBools.Add(eatingAfter);
                tutorialBools.Add(shippedBefore);
                tutorialBools.Add(shippedAfter);
                tutorialBools.Add(spreadBefore);
                tutorialBools.Add(spreadAfter);
                tutorialBools.Add(hybridBefore);
                tutorialBools.Add(hybridAfter);
                tutorialBools.Add(tutorialComplete);

                GlobalGameSaving.Instance.tutorialBools = tutorialBools;
            }
        }

        
        

        //check if there's a save to load. If yes, set here
    }

    // Update is called once per frame
    void Update()
    {
		//skips if tutorial is not enabled
		if (!enableTutorial.isOn)
		{
			shippingBin.enabled = true;
			return;
		}
        else if (!(tutorialBools[12] == true))//else if(!(eatingAfter == true))
        {
			shippingBin.enabled = false;
			
		}
		
        //Rwplay Text
        if (Input.GetKeyDown(KeyCode.T))
        {
            //StartCoroutine(self.PlayDialogue(self.convoID));
            myFlowchart.ExecuteBlock(currentTutorial);
        }

        //First step
        //Hoe
        //Start Block
        if (tutorialBools[0] == false)//(tutorialStarted == false)
        {
            tutorialBools[0] = true;//tutorialStarted = true;
            //self.convoID = self.conversationIDs[0];

            objective.text = $"Current Objective: Till the field using the hoe";

            GlobalGameSaving.Instance.tutorialBools[0] = tutorialBools[0];
            GlobalGameSaving.Instance.tutorialObjective = objective.text;

            myFlowchart.ExecuteBlock("Start");

            //StartCoroutine(self.PlayDialogue(self.convoID));
        }

        //Ground hoe'd
        //Plant
        //After Tilling Block
        if (tutorialBools[1] == false && tutorialBools[2] == true)//(tilledBefore == false && tilledAfter == true)
        {
            tutorialBools[1] = true;//tilledBefore = true;
            //self.convoID = self.conversationIDs[1];

            objective.text = $"Current Objective: Plant a Mushroom";

            GlobalGameSaving.Instance.tutorialBools[1] = tutorialBools[1];
            GlobalGameSaving.Instance.tutorialObjective = objective.text;

            //StartCoroutine(self.PlayDialogue(self.convoID));
            myFlowchart.ExecuteBlock("After Tilling");
            currentTutorial = myFlowchart.FindBlock("After Tilling");
        }

        //Planted
        //Water
        //After Planting Block
        if (tutorialBools[3] == false && tutorialBools[4] == true)//(plantedBefore == false && plantedAfter == true)
        {
            tutorialBools[3] = true;//plantedBefore = true;
            //self.convoID = self.conversationIDs[2];

            objective.text = $"Current Objective: Water the- mushroom";

            GlobalGameSaving.Instance.tutorialBools[3] = tutorialBools[3];
            GlobalGameSaving.Instance.tutorialObjective = objective.text;

            //StartCoroutine(self.PlayDialogue(self.convoID));
            myFlowchart.ExecuteBlock("After Planting");
            currentTutorial = myFlowchart.FindBlock("After Planting");
        }

        //Watered
        //Now sleep
        //After Watering Block
        if (tutorialBools[5] == false && tutorialBools[6] == true)//(wateredBefore == false && wateredAfter == true)
        {
            tutorialBools[5] = true;//wateredBefore = true;
            //self.convoID = self.conversationIDs[3];

            objective.text = $"Current Objective: Sleep so mushroom grows";

            GlobalGameSaving.Instance.tutorialBools[5] = tutorialBools[5];
            GlobalGameSaving.Instance.tutorialObjective = objective.text;

            //Unnecessary at the moment:  It's completely up to you when you wake up.

            //StartCoroutine(self.PlayDialogue(self.convoID));

            myFlowchart.ExecuteBlock("After Watering");
            currentTutorial = myFlowchart.FindBlock("After Watering");
        }

        //New Day
        //Now Harvest
        //After Sleeping Block 7, 8, 6
        if (tutorialBools[7] == false && tutorialBools[8] == true && tutorialBools[6] == true)//(sleptBefore == false && sleptAfter == true && wateredAfter == true)
        {
            tutorialBools[7] = true;//sleptBefore = true;
            //self.convoID = self.conversationIDs[4];

            objective.text = $"Current Objective: Harvest grown mushroom";

            GlobalGameSaving.Instance.tutorialBools[7] = tutorialBools[7];
            GlobalGameSaving.Instance.tutorialObjective = objective.text;

            //StartCoroutine(self.PlayDialogue(self.convoID));

            myFlowchart.ExecuteBlock("After Sleeping");
            currentTutorial = myFlowchart.FindBlock("After Sleeping");
        }

        //Harvested
        //Snack time
        //After Harvesting Block 9, 10
        if (tutorialBools[9] == false && tutorialBools[10] == true)//(harvestedBefore == false && harvestedAfter == true)
        {
            tutorialBools[9] = true;//harvestedBefore = true;
            //self.convoID = self.conversationIDs[5];

            objective.text = $"Current Objective: Eat Mushroom to recover stamina";

            GlobalGameSaving.Instance.tutorialBools[9] = tutorialBools[9];
            GlobalGameSaving.Instance.tutorialObjective = objective.text;

            //StartCoroutine(self.PlayDialogue(self.convoID));
            myFlowchart.ExecuteBlock("After Harvesting");
            currentTutorial = myFlowchart.FindBlock("After Harvesting");
        }

        //Full
        //Shipping Time + Spreading Prep
        //After Eating Block 11, 12
        if (tutorialBools[11] == false && tutorialBools[12] == true)//(eatingBefore == false && eatingAfter == true)
        {
            tutorialBools[11] = true;//eatingBefore = true;
            //self.convoID = self.conversationIDs[6];

            objective.text = $"Current Objective: Ship mushroom and go to sleep";

            GlobalGameSaving.Instance.tutorialBools[11] = tutorialBools[11];
            GlobalGameSaving.Instance.tutorialObjective = objective.text;

            shippingBin.enabled = true;

            //StartCoroutine(self.PlayDialogue(self.convoID));

            myFlowchart.ExecuteBlock("After Eating");
            currentTutorial = myFlowchart.FindBlock("After Eating");
        }

        //Shipping done
        //Sleep again and then prepare to spread
        //After Shipping Block 13, 14
        if (tutorialBools[13] == false && tutorialBools[14] == true)//(shippedBefore == false && shippedAfter == true)
        {
            tutorialBools[13] = true;//shippedBefore = true;
            //self.convoID = self.conversationIDs[7];

            objective.text = $"Current Objective: Plant and water mushroom, then sleep for two days";

            GlobalGameSaving.Instance.tutorialBools[13] = tutorialBools[13];
            GlobalGameSaving.Instance.tutorialObjective = objective.text;

            //StartCoroutine(self.PlayDialogue(self.convoID));

            myFlowchart.ExecuteBlock("After Shipping");
            currentTutorial = myFlowchart.FindBlock("After Shipping");
        }

        //Spread
        //Now hybridize
        //After Spreading Block 15, 16
        if (tutorialBools[15] == false && tutorialBools[16] == true)//(spreadBefore == false && spreadAfter == true)
        {
            Instantiate(redShroom, FindObjectOfType<PlayerInteraction>().gameObject.transform.position, Quaternion.identity);
            Instantiate(glowyShroom, FindObjectOfType<PlayerInteraction>().gameObject.transform.position, Quaternion.identity);

            tutorialBools[15] = true;//spreadBefore = true;
            //self.convoID = self.conversationIDs[8];

            objective.text = $"Current Objective: Plant glowy and red shrooms with the space between tilled and sleep until they spread";

            GlobalGameSaving.Instance.tutorialBools[15] = tutorialBools[15];
            GlobalGameSaving.Instance.tutorialObjective = objective.text;

            //StartCoroutine(self.PlayDialogue(self.convoID));

            myFlowchart.ExecuteBlock("After Spreading");
            currentTutorial = myFlowchart.FindBlock("After Spreading");
        }

        //Hybridization
        //Saving and tutorial done
        //After Hybridization Block 17, 18
        if (tutorialBools[17] == false && tutorialBools[18] == true)//(hybridBefore == false && hybridAfter == true)
        {
            tutorialBools[17] = true;//hybridBefore = true;
            //self.convoID = self.conversationIDs[9];

            objective.gameObject.SetActive(false);

            //StartCoroutine(self.PlayDialogue(self.convoID));

            myFlowchart.ExecuteBlock("After Hybridization");
            currentTutorial = myFlowchart.FindBlock("After Hybridization");

            GlobalGameSaving.Instance.tutorialBools[17] = tutorialBools[17];
            GlobalGameSaving.Instance.tutorialBools[19] = tutorialBools[19];
            GlobalGameSaving.Instance.tutorialObjective = objective.text;

            //19
            tutorialBools[19] = true;//tutorialComplete = true;
        }
    }
}
