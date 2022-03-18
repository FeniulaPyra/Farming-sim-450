using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingTutorial : MonoBehaviour
{
    //softlock prevention
    public GameObject redShroom;
    public GameObject glowyShroom;

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
        if (tutorialStarted == false)
        {
            tutorialStarted = true;
            self.convoID = self.conversationIDs[0];

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        if (tilledBefore == false && tilledAfter == true)
        {
            tilledBefore = true;
            self.convoID = self.conversationIDs[1];

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        if (plantedBefore == false && plantedAfter == true)
        {
            plantedBefore = true;
            self.convoID = self.conversationIDs[2];

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        if (wateredBefore == false && wateredAfter == true)
        {
            wateredBefore = true;
            self.convoID = self.conversationIDs[3];

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        if (sleptBefore == false && sleptAfter == true && wateredAfter == true)
        {
            sleptBefore = true;
            self.convoID = self.conversationIDs[4];

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        if (harvestedBefore == false && harvestedAfter == true)
        {
            harvestedBefore = true;
            self.convoID = self.conversationIDs[5];

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        if (eatingBefore == false && eatingAfter == true)
        {
            eatingBefore = true;
            self.convoID = self.conversationIDs[6];

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        if (shippedBefore == false && shippedAfter == true)
        {
            shippedBefore = true;
            self.convoID = self.conversationIDs[7];

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        if (spreadBefore == false && spreadAfter == true)
        {
            Instantiate(redShroom, FindObjectOfType<PlayerInteraction>().gameObject.transform.position, Quaternion.identity);
            Instantiate(glowyShroom, FindObjectOfType<PlayerInteraction>().gameObject.transform.position, Quaternion.identity);

            spreadBefore = true;
            self.convoID = self.conversationIDs[8];

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));
        }

        if (hybridBefore == false && hybridAfter == true)
        {
            hybridBefore = true;
            self.convoID = self.conversationIDs[9];

            StartCoroutine(self.PlayDialogue(self.convoID, KeyCode.Space));

            tutorialComplete = true;
        }
    }
}
