using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fungus;

public class Bed : MonoBehaviour
{
    //Reference to TimeManager. Just so it can sleep
    [SerializeField]
    TimeManager timeManager;
    //player's collider
    public Collider2D playerCollider;
    PlayerInteraction playerInteraction;

	public GameObject menus;
	private Menu menu;

    Flowchart myFlowchart;

    public Flowchart MyFlowchart
    {
        get
        {
            return myFlowchart;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();
		menu = menus.GetComponent<Menu>(); //ah yes the menu here is made out of menus

        myFlowchart = transform.Find("BedFlowchart").GetComponent<Flowchart>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SleepToMorning()
    {
        Debug.Log("Worked");
        timeManager.isNight = false;
        timeManager.Sleep(8);
		FarmManager farm = GameObject.FindObjectOfType<FarmManager>();
		if (farm != null && farm.isActiveAndEnabled)
			timeManager.UpdateField();
        playerInteraction.isTalking = false;
		menu.CloseBed();
    }

    public void SleepToNight()
    {
        Debug.Log("Worked 2");
        timeManager.isNight = true;
        timeManager.Sleep(8);
        playerInteraction.isTalking = false;
		menu.CloseBed();
    }
}
