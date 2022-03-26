using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithObjects : MonoBehaviour
{
    //List of interactable objects
    [SerializeField]
    List<InteractableObjects> objects = new List<InteractableObjects>();

    //Random array of DialogueManagers to handle NPC Dialogue
    InteractableObjects[] objectsArray = new InteractableObjects[100];

    PlayerInteraction playerInteraction;

    [SerializeField]
    Bed bed;

    // Start is called before the first frame update
    void Start()
    {
        //Gets all interactable objectss and saves them
        objectsArray = FindObjectsOfType<InteractableObjects>();

        for (int i = 0; i < objectsArray.Length; i++)
        {
            if (objectsArray[i] != null)
            {
                objects.Add(objectsArray[i]);

                if (objects[objects.Count - 1].GetComponent<Bed>() != null)
                {
                    bed = objects[objects.Count - 1].GetComponent<Bed>();
                }
            }
        }

        playerInteraction = FindObjectOfType<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(InteractionChecker());
    }

    /// <summary>
    /// IEnumerator appears to be necessary
    /// The spacebar needed to check for interaction in the first place, if the player is interacting with an NPC, also triggers to WaitForInput in Dialogue Manager
    /// That means it waits half a second before moving onto the next piece of dialogue without warning
    /// This pauses for a very brief amount of time so that first spacebar press doesn't register for the WaitForInput
    /// </summary>
    /// <returns></returns>
    public IEnumerator InteractionChecker()
    {
        //checks if the player hits the key for interaction
        //Debug.Log($"Interact talking: {playerInteraction.isTalking}");
        if (Input.GetKeyDown(KeyCode.Space) && playerInteraction.isTalking == false || Input.GetKeyDown(KeyCode.Mouse0) && playerInteraction.isTalking == false)
        {
            //goes through all interactable objects
            for (int i = 0; i < objects.Count; i++)
            {
                //Sees how close the player is to them
                float distance = Vector2.Distance(gameObject.transform.position, objects[i].gameObject.transform.position);

                //1 seems like a fine number
                if (distance <= 1.0f)
                {
                    //switch on name to see what it is
                    switch (objects[i].name)
                    {
                        case "npc":
                            Debug.Log("Interact update");
                            yield return new WaitForSeconds(0.025f);
                            //StartCoroutine(objects[i].gameObject.GetComponent<DialogueManager>().PlayDialogue(objects[i].gameObject.GetComponent<DialogueManager>().convoID));
                            break;
                        case "shipping bin":
                            objects[i].gameObject.GetComponent<ShippingBin>().PutItemInBin();
                            break;
                        case "bed":
                            objects[i].gameObject.GetComponent<Bed>().SetTextObjectsActive(true);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            bed.SetTextObjectsActive(false);
        }

        //Debug.Log($"Distance to player and {objects[0].gameObject.name} is {Vector2.Distance(gameObject.transform.position, objects[0].gameObject.transform.position)}");
    }
}
