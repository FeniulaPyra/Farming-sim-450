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

    public Collider2D playerCollider;

    //All other variables
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    //The tex box itself
    public Image textBoxImage;

    //The .sprite property of this is set to the npcdialaogue sprite
    public Image characterSprite;
    public Image tutorialImage;

    //list of strings that will act as keys for the dictionary
    public List<string> conversationIDs = new List<string>();
    //List of a class that just has a list of NPC dialogue
    //This nested list allows for creating dictionary values in inspector
    [SerializeField]
    public List<NPCDialogueList> dialogueLists = new List<NPCDialogueList>();


    //Dictionary of conversation IDs and a list of NPC dialogue
    public Dictionary<string, List<NPCDialogue>> conversations = new Dictionary<string, List<NPCDialogue>>();

    //For quests
    [SerializeField]
    public Quests myQuests;

    public List<string> questIDs = new List<string>();
    [SerializeField]
    public List<NPCDialogueList> questLists = new List<NPCDialogueList>();
    public Dictionary<string, List<NPCDialogue>> quests = new Dictionary<string, List<NPCDialogue>>();
    [SerializeField] int questNumber;

    public string oldConvoID;

    //Testing
    string greaterConvoID;

    //reference to time manage
    [SerializeField]
    TimeManager timeManager;
    PlayerInteraction playerInteraction;

    //for disabling the hotbar during dialogue
    Menu menu;

    //ints to track seasonal dialogue
    public int springConvoStart = 0;
    public int springConvoEnd = 1;
    public int summerConvoStart = 2;
    public int summerConvoEnd = 3;
    public int fallConvoStart;
    public int fallConvoEnd;
    public int winterConvoStart;
    public int winterConvoEnd;
    #endregion

    public int GetSpringStart()
    {
        return springConvoStart;
    }
    public int GetSpringEnd()
    {
        return springConvoEnd;
    }

    public int GetSummerStart()
    {
        return summerConvoStart;
    }
    public int GetSummerEnd()
    {
        return summerConvoEnd;
    }

    public int GetFallStart()
    {
        return fallConvoStart;
    }
    public int GetFallEnd()
    {
        return fallConvoEnd;
    }

    public int GetWinterStart()
    {
        return winterConvoStart;
    }
    public int GetWinterEnd()
    {
        return winterConvoEnd;
    }
    //All variables currently public, but will be changed later as necessary
    [System.Serializable]
    public class NPCDialogue
    {
        //The character's name
        public string characterName;
        
        //The character's sprite
        public Sprite characterSprite;
        public Sprite tutorialImage;

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
        timeManager = FindObjectOfType<TimeManager>();

        playerInteraction = FindObjectOfType<PlayerInteraction>();

        //SetConversations(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        SetConversations();
        LoadQuests();

        //playerScript = FindObjectOfType<PlayerMove>();

        //Setting the opacity of all of the text objects to 0
        nameText.gameObject.SetActive(false);

        dialogueText.gameObject.SetActive(false);

        textBoxImage.gameObject.SetActive(false);
        //textBoxImage.GetComponent<Image>().color = new Color(textBoxImage.GetComponent<Image>().color.r, textBoxImage.GetComponent<Image>().color.g, textBoxImage.GetComponent<Image>().color.b, 0.0f);

        characterSprite.gameObject.SetActive(false);

        myQuests = gameObject.GetComponent<Quests>();

        menu = FindObjectOfType<Menu>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checking if the player can do a quest
    }

    public void LoadQuests()
    {
        for (int i = 0; i < questIDs.Count; i++)
        {
            quests[questIDs[i]] = questLists[i].convoDialogue;
        }
    }

    public void SetConversations()
    {
        //Clear dictionary
        conversations.Clear();

        switch (timeManager.SeasonNumber)
        {
            case 1:
                //Screwed up; the <= ran it twice, when it should've ran once
                for (int i = springConvoStart; i <= springConvoEnd; i++)
                {
                    Debug.Log("Loop");
                    conversations[conversationIDs[i]] = dialogueLists[i].convoDialogue;
                }

                convoID = conversationIDs[springConvoStart];
                break;
            case 2:
                for (int i = summerConvoStart; i <= summerConvoEnd; i++)
                {
                    Debug.Log("Loop");
                    conversations[conversationIDs[i]] = dialogueLists[i].convoDialogue;
                }

                convoID = conversationIDs[summerConvoStart];
                break;
            case 3:
                for (int i = fallConvoStart; i <= fallConvoEnd; i++)
                {
                    Debug.Log("Loop");
                    conversations[conversationIDs[i]] = dialogueLists[i].convoDialogue;
                }

                convoID = conversationIDs[fallConvoStart];
                break;
            case 4:
                for (int i = winterConvoStart; i <= winterConvoEnd; i++)
                {
                    Debug.Log("Loop");
                    conversations[conversationIDs[i]] = dialogueLists[i].convoDialogue;
                }

                convoID = conversationIDs[winterConvoStart];
                break;
            default:
                break;
        }
    }

    //As long as the key passed in is not being pressed, nothing happens. When it is, it tells the Play Dialogue Coroutine to wait a little bit before moving on
    IEnumerator WaitForInput(KeyCode key, KeyCode secondKey)
    {
        Debug.Log("Waiting");

        while (Input.GetKeyDown(key) == false && Input.GetKeyDown(secondKey) == false)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);
    }

    //method to play out a conversation, using it's ID to find it
    //Is a coroutine so the for loop for dialogue doesn't immediately blaze through the conversation list
    /*public IEnumerator PlayDialogue(string convoID)
    {
        menu.OpenDialog();

        playerInteraction.isTalking = true;
        playerInteraction.CanInteract = false;

        List<NPCDialogue> convoToPlay = conversations[conversationIDs[0]];

        //Get a dictionary to play
        if (conversations.ContainsKey(convoID))
        {
            convoToPlay = conversations[convoID];
        }
        else if (quests.ContainsKey(convoID))
        {
            convoToPlay = quests[convoID];
        }
        else
        {
            yield return null;
        }

        greaterConvoID = convoID;

        nameText.gameObject.SetActive(true);

        dialogueText.gameObject.SetActive(true);

        textBoxImage.gameObject.SetActive(true);

        menu.HotbarUIObject.SetActive(false);

        for (int i = 0; i < convoToPlay.Count; i++)
        {
            //Just using Debug.Log for now
            Debug.Log($"Iteration {i}");

            nameText.text = convoToPlay[i].characterName;
            dialogueText.text = convoToPlay[i].dialogue;
            characterSprite.sprite = convoToPlay[i].characterSprite;
            tutorialImage.sprite = convoToPlay[i].tutorialImage;

            if (convoToPlay[i].characterSprite == null)
            {
                characterSprite.gameObject.SetActive(false);
            }
            else
            {
                characterSprite.gameObject.SetActive(true);
            }

            if (convoToPlay[i].tutorialImage == null)
            {
                tutorialImage.gameObject.SetActive(false);
            }
            else
            {
                tutorialImage.gameObject.SetActive(true);
            }

            //yield return StartCoroutine(WaitForInput(KeyCode.Space));
            yield return StartCoroutine(WaitForInput(KeyCode.Space, KeyCode.Mouse0));
            //timer = timerDefault;
        }

        nameText.gameObject.SetActive(false);

        dialogueText.gameObject.SetActive(false);

        textBoxImage.gameObject.SetActive(false);

        characterSprite.gameObject.SetActive(false);

        tutorialImage.gameObject.SetActive(false);

        Debug.Log($"For conversation ID {convoID}");

        playerInteraction.isTalking = false;
        playerInteraction.CanInteract = true;

        menu.HotbarUIObject.SetActive(true);

        if (quests.ContainsKey(convoID))
        {

            if (myQuests.activeQuest.questActive != true)
            {
                myQuests.activeQuest.questActive = true;
            }

            if (myQuests.activeQuest.questComplete == false && myQuests.activeQuest.readyToReport == true)
            {
                myQuests.activeQuest.questComplete = true;
            }

            Debug.Log("Quest now active");
            Debug.Log($"Quest oldConvoID is {oldConvoID}");
            this.convoID = oldConvoID;
            //Debug.Log("Quest id replaced");
        }

        menu.CloseDialog();
    }*/

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        //Tested with Debug.Log. The player's collider is named "PlayerCollider", so this should just work
        if (collision == playerCollider && Input.GetKeyDown(KeyCode.Space) && playerInteraction.isTalking == false)
        {
            Debug.Log($"My ID is {convoID}");
            StartCoroutine(PlayDialogue(convoID));
            //Debug.Log($"Inside the trigger; other is {collision.name}");
        }
    }*/
}
