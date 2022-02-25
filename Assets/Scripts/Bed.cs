using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bed : MonoBehaviour
{
    //Reference to TimeManager. Just so it can sleep
    [SerializeField]
    TimeManager timeManager;
    //player's collider
    public Collider2D playerCollider;
    PlayerInteraction playerInteraction;

    //Sleep Selection
    public TMP_Text dialogueText;
    public TMP_Text choice1Text;
    public TMP_Text choice2Text;

    //The tex box itself
    public Image textBoxImage;
    public Image choiceBoxImage;
    public Image choiceArrow1Image;
    public Image choiceArrow2Image;

    // Start is called before the first frame update
    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInteraction.isTalking == true)
        {
            

            SleepSelection();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == playerCollider && Input.GetKeyDown(KeyCode.Space) && playerInteraction.isTalking == false)
        {
            SetTextObjectsActive(true);
            playerInteraction.isTalking = true;
        }
    }

    void SetTextObjectsActive(bool how)
    {
        dialogueText.text = "How long would you like to sleep?";
        choice1Text.text = "Until morning";
        choice2Text.text = "Until night";

        //Making the UI elements that make this possible visible or hidden
        if (how  == true)
        {
            dialogueText.gameObject.SetActive(true);
            choice1Text.gameObject.SetActive(true);
            choice2Text.gameObject.SetActive(true);

            
            textBoxImage.gameObject.SetActive(true);
            choiceBoxImage.gameObject.SetActive(true);
            choiceArrow1Image.gameObject.SetActive(false);
            choiceArrow2Image.gameObject.SetActive(false);
        }
        else
        {
            dialogueText.gameObject.SetActive(false);
            choice1Text.gameObject.SetActive(false);
            choice2Text.gameObject.SetActive(false);


            textBoxImage.gameObject.SetActive(false);
            choiceBoxImage.gameObject.SetActive(false);
            choiceArrow1Image.gameObject.SetActive(false);
            choiceArrow2Image.gameObject.SetActive(false);
        }
    }

    void SleepSelection()
    {
        //Movement
        if (Input.GetKeyDown(KeyCode.DownArrow) && choiceArrow2Image.isActiveAndEnabled == false)
        {
            choiceArrow1Image.gameObject.SetActive(false);
            choiceArrow2Image.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && choiceArrow2Image.isActiveAndEnabled == true)
        {
            choiceArrow1Image.gameObject.SetActive(true);
            choiceArrow2Image.gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && choiceArrow2Image.isActiveAndEnabled == false)
        {
            choiceArrow1Image.gameObject.SetActive(false);
            choiceArrow2Image.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && choiceArrow2Image.isActiveAndEnabled == true)
        {
            choiceArrow1Image.gameObject.SetActive(true);
            choiceArrow2Image.gameObject.SetActive(false);
        }

        //Selection
        //Morning
        if (Input.GetKeyDown(KeyCode.Space) && choiceArrow1Image.isActiveAndEnabled == true)
        {
            Debug.Log("Worked");
            timeManager.nightImage.color = new Color(timeManager.nightImage.color.r, timeManager.nightImage.color.g, timeManager.nightImage.color.b, 0f);
            timeManager.isNight = false;
            timeManager.Sleep(8);
            playerInteraction.isTalking = false;
            SetTextObjectsActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && choiceArrow2Image.isActiveAndEnabled == true)
        {
            Debug.Log("Worked 2");
            timeManager.nightImage.color = new Color(timeManager.nightImage.color.r, timeManager.nightImage.color.g, timeManager.nightImage.color.b, 0.25f);
            timeManager.isNight = true;
            timeManager.Sleep(8);
            playerInteraction.isTalking = false;
            SetTextObjectsActive(false);
        }

        //timeManager.Sleep(8);
    }
}
