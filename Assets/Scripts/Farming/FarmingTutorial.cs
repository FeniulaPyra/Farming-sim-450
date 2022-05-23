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
    public bool tutorialComplete;

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

    Flowchart myFlowchart;

    Block currentTutorial;

    [SerializeField]
    InteractableObjects shippingBin;

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponentInParent<DialogueManager>();

        shippingBin = FindObjectOfType<ShippingBin>().GetComponent<InteractableObjects>();
        shippingBin.enabled = false;

        myFlowchart = transform.Find("TutorialFlowchart").GetComponent<Flowchart>();

        currentTutorial = myFlowchart.FindBlock("Start");
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
		else if(!(eatingAfter == true))
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
        if (tutorialStarted == false)
        {
            tutorialStarted = true;
            //self.convoID = self.conversationIDs[0];

            objective.text = $"Current Objective: Till the field using the hoe";

            myFlowchart.ExecuteBlock("Start");

            //StartCoroutine(self.PlayDialogue(self.convoID));
        }

        //Ground hoe'd
        //Plant
        //After Tilling Block
        if (tilledBefore == false && tilledAfter == true)
        {
            tilledBefore = true;
            //self.convoID = self.conversationIDs[1];

            objective.text = $"Current Objective: Plant a Mushroom";

            //StartCoroutine(self.PlayDialogue(self.convoID));
            myFlowchart.ExecuteBlock("After Tilling");
            currentTutorial = myFlowchart.FindBlock("After Tilling");
        }

        //Planted
        //Water
        //After Planting Block
        if (plantedBefore == false && plantedAfter == true)
        {
            plantedBefore = true;
            //self.convoID = self.conversationIDs[2];

            objective.text = $"Current Objective: Water the mushroom";

            //StartCoroutine(self.PlayDialogue(self.convoID));
            myFlowchart.ExecuteBlock("After Planting");
            currentTutorial = myFlowchart.FindBlock("After Planting");
        }

        //Watered
        //Now sleep
        //After Watering Block
        if (wateredBefore == false && wateredAfter == true)
        {
            wateredBefore = true;
            //self.convoID = self.conversationIDs[3];

            objective.text = $"Current Objective: Sleep so mushroom grows";

            //Unnecessary at the moment:  It's completely up to you when you wake up.

            //StartCoroutine(self.PlayDialogue(self.convoID));

            myFlowchart.ExecuteBlock("After Watering");
            currentTutorial = myFlowchart.FindBlock("After Watering");
        }

        //New Day
        //Now Harvest
        //After Sleeping Block
        if (sleptBefore == false && sleptAfter == true && wateredAfter == true)
        {
            sleptBefore = true;
            //self.convoID = self.conversationIDs[4];

            objective.text = $"Current Objective: Harvest grown mushroom";

            //StartCoroutine(self.PlayDialogue(self.convoID));

            myFlowchart.ExecuteBlock("After Sleeping");
            currentTutorial = myFlowchart.FindBlock("After Sleeping");
        }

        //Harvested
        //Snack time
        //After Harvesting Block
        if (harvestedBefore == false && harvestedAfter == true)
        {
            harvestedBefore = true;
            //self.convoID = self.conversationIDs[5];

            objective.text = $"Current Objective: Eat Mushroom to recover stamina";

            //StartCoroutine(self.PlayDialogue(self.convoID));
            myFlowchart.ExecuteBlock("After Harvesting");
            currentTutorial = myFlowchart.FindBlock("After Harvesting");
        }

        //Full
        //Shipping Time + Spreading Prep
        //After Eating Block
        if (eatingBefore == false && eatingAfter == true)
        {
            eatingBefore = true;
            //self.convoID = self.conversationIDs[6];

            objective.text = $"Current Objective: Ship mushroom and go to sleep";

            shippingBin.enabled = true;

            //StartCoroutine(self.PlayDialogue(self.convoID));

            myFlowchart.ExecuteBlock("After Eating");
            currentTutorial = myFlowchart.FindBlock("After Eating");
        }

        //Shipping done
        //Sleep again and then prepare to spread
        //After Shipping Block
        if (shippedBefore == false && shippedAfter == true)
        {
            shippedBefore = true;
            //self.convoID = self.conversationIDs[7];

            objective.text = $"Current Objective: Plant and water mushroom, then sleep for two days";

            //StartCoroutine(self.PlayDialogue(self.convoID));

            myFlowchart.ExecuteBlock("After Shipping");
            currentTutorial = myFlowchart.FindBlock("After Shipping");
        }

        //Spread
        //Now hybridize
        //After Spreading Block
        if (spreadBefore == false && spreadAfter == true)
        {
            Instantiate(redShroom, FindObjectOfType<PlayerInteraction>().gameObject.transform.position, Quaternion.identity);
            Instantiate(glowyShroom, FindObjectOfType<PlayerInteraction>().gameObject.transform.position, Quaternion.identity);

            spreadBefore = true;
            //self.convoID = self.conversationIDs[8];

            objective.text = $"Current Objective: Plant glowy and red shrooms with the space between tilled and sleep until they spread";

            //StartCoroutine(self.PlayDialogue(self.convoID));

            myFlowchart.ExecuteBlock("After Spreading");
            currentTutorial = myFlowchart.FindBlock("After Spreading");
        }

        //Hybridization
        //Saving and tutorial done
        //After Hybridization Block
        if (hybridBefore == false && hybridAfter == true)
        {
            hybridBefore = true;
            //self.convoID = self.conversationIDs[9];

            objective.gameObject.SetActive(false);

            //StartCoroutine(self.PlayDialogue(self.convoID));

            myFlowchart.ExecuteBlock("After Hybridization");
            currentTutorial = myFlowchart.FindBlock("After Hybridization");

            tutorialComplete = true;
        }
    }
}
