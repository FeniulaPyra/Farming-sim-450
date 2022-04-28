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

    //The tex box itself
    public Image textBoxImage;
    public Image choiceBoxImage;

    //buttons
    public Button morningButton;
    public Button nightButton;

	public GameObject menus;
	private Menu menu;

    // Start is called before the first frame update
    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();
		menu = menus.GetComponent<Menu>(); //ah yes the menu here is made out of menus
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == playerCollider && Input.GetKeyDown(KeyCode.Space) && playerInteraction.isTalking == false)
        {
            Debug.Log("Interacting with bed");
            SetTextObjectsActive(true);
            playerInteraction.isTalking = true;
        }
    }*/

    public void SetTextObjectsActive(bool how)
    {
        dialogueText.text = "How long would you like to sleep?";

        //Making the UI elements that make this possible visible or hidden
        if (how  == true)
        {
            dialogueText.gameObject.SetActive(true);

            
            textBoxImage.gameObject.SetActive(true);
            choiceBoxImage.gameObject.SetActive(true);
        }
        else
        {
            dialogueText.gameObject.SetActive(false);


            textBoxImage.gameObject.SetActive(false);
            choiceBoxImage.gameObject.SetActive(false);
        }
    }

    public void SleepToMorning()
    {
        Debug.Log("Worked");
        timeManager.isNight = false;
        timeManager.Sleep(8);
        playerInteraction.isTalking = false;
        SetTextObjectsActive(false);
		menu.CloseBed();
    }

    public void SleepToNight()
    {
        Debug.Log("Worked 2");
        timeManager.isNight = true;
        timeManager.Sleep(8);
        playerInteraction.isTalking = false;
        SetTextObjectsActive(false);
		menu.CloseBed();
    }

    /*void SleepSelection()
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
            timeManager.isNight = false;
            timeManager.Sleep(8);
            playerInteraction.isTalking = false;
            SetTextObjectsActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && choiceArrow2Image.isActiveAndEnabled == true)
        {
            Debug.Log("Worked 2");
            timeManager.isNight = true;
            timeManager.Sleep(8);
            playerInteraction.isTalking = false;
            SetTextObjectsActive(false);
        }

        //timeManager.Sleep(8);
    }*/
}
