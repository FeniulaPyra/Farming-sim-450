using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //string to tell it which conversation to play
    public string convoID;

    #region Variables

    //All other variables
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    //The tex box itself
    public Image textBoxImage;

    //The .sprite property of this is set to the npcdialaogue sprite
    public Image characterSprite;

    //list of strings that will act as keys for the dictionary
    public List<string> conversationIDs = new List<string>();
    //List of a class that just has a list of NPC dialogue
    //This nested list allows for creating dictionary values in inspector
    [SerializeField]
    public List<NPCDialogueList> dialogueLists = new List<NPCDialogueList>();


    //Dictionary of conversation IDs and a list of NPC dialogue
    public Dictionary<string, List<NPCDialogue>> conversations = new Dictionary<string, List<NPCDialogue>>();

    //Testing
    string greaterConvoID;
    #endregion

    //All variables currently public, but will be changed later as necessary
    [System.Serializable]
    public class NPCDialogue
    {
        //The character's name
        public string characterName;
        
        //The character's sprite
        public Sprite characterSprite;

        //What the character says
        public string dialogue;

        public string emotion;
    }
    //attempting something
    [System.Serializable]
    public class NPCDialogueList
    {
        //list of NPC Dialogue
        public List<NPCDialogue> convoDialogue;
    }

    // Start is called before the first frame update
    void Start()
    {
        //SetConversations(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        //Screwed up; the <= ran it twice, when it should've ran once
        for (int i = 0; i < conversationIDs.Count; i++)
        {
            Debug.Log("Loop");
            conversations[conversationIDs[i]] = dialogueLists[i].convoDialogue;
        }

        //playerScript = FindObjectOfType<PlayerMove>();

        //Setting the opacity of all of the text objects to 0
        nameText.gameObject.SetActive(false);

        dialogueText.gameObject.SetActive(false);

        textBoxImage.gameObject.SetActive(false);
        //textBoxImage.GetComponent<Image>().color = new Color(textBoxImage.GetComponent<Image>().color.r, textBoxImage.GetComponent<Image>().color.g, textBoxImage.GetComponent<Image>().color.b, 0.0f);

        characterSprite.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //As long as the key passed in is not being pressed, nothing happens. When it is, it tells the Play Dialogue Coroutine to wait a little bit before moving on
    IEnumerator WaitForInput(KeyCode key)
    {
        while (Input.GetKeyDown(key) == false)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
    }

    //method to play out a conversation, using it's ID to find it
    //Is a coroutine so the for loop for dialogue doesn't immediately blaze through the conversation list
    public IEnumerator PlayDialogue(string convoID)
    {
        //playerScript.inDialogue = true;

        //Get a dictionary to play
        List<NPCDialogue> convoToPlay = conversations[convoID];

        greaterConvoID = convoID;

        nameText.gameObject.SetActive(true);

        dialogueText.gameObject.SetActive(true);

        textBoxImage.gameObject.SetActive(true);

        characterSprite.gameObject.SetActive(true);

        for (int i = 0; i < convoToPlay.Count; i++)
        {
            //Just using Debug.Log for now
            Debug.Log($"The character's name is {convoToPlay[i].characterName}, and they have to say {convoToPlay[i].dialogue}");

            nameText.text = convoToPlay[i].characterName;
            Debug.Log($"I am nameText: {nameText.text}");
            dialogueText.text = convoToPlay[i].dialogue;
            characterSprite.sprite = convoToPlay[i].characterSprite;
            Debug.Log($"This is the sprite: {convoToPlay[i].characterSprite}");

            yield return StartCoroutine(WaitForInput(KeyCode.Return));
            //timer = timerDefault;
        }

        Debug.Log($"For conversation ID {convoID}");


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Tested with Debug.Log. The player's collider is named "PlayerCollider", so this should just work
        if (collision.name == "PlayerCollider" && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"My ID is {convoID}");
            StartCoroutine(PlayDialogue(convoID));
            //Debug.Log($"Inside the trigger; other is {collision.name}");
        }
    }
}
