using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponentInParent<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
		//skips if tutorial is not enabled
		if (!enableTutorial.isOn) return;

        //Rwplay Text
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        //First step
        //Hoe
        if (tutorialStarted == false)
        {
            tutorialStarted = true;
            self.convoID = self.conversationIDs[0];

            objective.text = $"Current Objective: Till the field using the hoe";

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        //Ground hoe'd
        //Plant
        if (tilledBefore == false && tilledAfter == true)
        {
            tilledBefore = true;
            self.convoID = self.conversationIDs[1];

            objective.text = $"Current Objective: Plant a Mushroom";

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        //Planted
        //Water
        if (plantedBefore == false && plantedAfter == true)
        {
            plantedBefore = true;
            self.convoID = self.conversationIDs[2];

            objective.text = $"Current Objective: Water the mushroom";

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        //Watered
        //Now sleep
        if (wateredBefore == false && wateredAfter == true)
        {
            wateredBefore = true;
            self.convoID = self.conversationIDs[3];

            objective.text = $"Current Objective: Sleep so mushroom grows";

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        //New Day
        //Now Harvest
        if (sleptBefore == false && sleptAfter == true && wateredAfter == true)
        {
            sleptBefore = true;
            self.convoID = self.conversationIDs[4];

            objective.text = $"Current Objective: Harvest grown mushroom";

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        //Harvested
        //Snack time
        if (harvestedBefore == false && harvestedAfter == true)
        {
            harvestedBefore = true;
            self.convoID = self.conversationIDs[5];

            objective.text = $"Current Objective: Eat Mushroom to recover stamina";

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        //Full
        //Shipping Time + Spreading Prep
        if (eatingBefore == false && eatingAfter == true)
        {
            eatingBefore = true;
            self.convoID = self.conversationIDs[6];

            objective.text = $"Current Objective: Ship mushroom and go to sleep";

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        //Shipping done
        //Sleep again to Spread
        if (shippedBefore == false && shippedAfter == true)
        {
            shippedBefore = true;
            self.convoID = self.conversationIDs[7];

            objective.text = $"Current Objective: Plant and water mushroom, then sleep for two days";

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        //Spread
        //Now hybridize
        if (spreadBefore == false && spreadAfter == true)
        {
            Instantiate(redShroom, FindObjectOfType<PlayerInteraction>().gameObject.transform.position, Quaternion.identity);
            Instantiate(glowyShroom, FindObjectOfType<PlayerInteraction>().gameObject.transform.position, Quaternion.identity);

            spreadBefore = true;
            self.convoID = self.conversationIDs[8];

            objective.text = $"Current Objective: Plant glowy and red shrooms with the space between tilled and sleep until they spread";

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        //Hybridization
        //Saving and tutorial done
        if (hybridBefore == false && hybridAfter == true)
        {
            hybridBefore = true;
            self.convoID = self.conversationIDs[9];

            objective.gameObject.SetActive(false);

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));

            tutorialComplete = true;
        }
    }
}
