using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuffPet : BasicPet
{
    //Reference to player movement to alter their speed
    PlayerMovement movement;

    //Timer on which the item spawns
    [SerializeField]
    float timer;
    public float baseTimer;
    //chance of item spawning
    public int activateChance;

    public bool increaseSpeed;
    [SerializeField]
    bool buffApplied;
    [SerializeField]
    float buffTimer;
    public float baseBuffTimer;

    [SerializeField]
    TMP_Text buffNotification;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        timer = baseTimer;
        buffTimer = baseBuffTimer;

        movement = FindObjectOfType<PlayerMovement>();
        buffNotification = GameObject.Find("TutorialObjective").GetComponent<TextMeshProUGUI>();
        buffNotification.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (buffApplied == false)
        {
            timer -= Time.deltaTime;

            if (timer <= 0.0f)
            {
                int chance = Random.Range(0, 101);

                if (chance <= activateChance)
                {
                    ApplyBuff();
                }
            }
        }
        else
        {
            buffTimer -= Time.deltaTime;

            if (buffTimer <= 0.0f)
            {
                CancelBuff();
            }
        }
    }

    void ApplyBuff()
    {
        if (increaseSpeed == true)
        {
            movement.MovementSpeed *= 2;
            movementSpeed *= 2;
            buffNotification.text += "\nSpeed Increased";
        }

        buffApplied = true;
        timer = baseTimer;
    }

    void CancelBuff()
    {
        movement.MovementSpeed /= 2;
        movementSpeed /= 2;

        buffNotification.text = "";

        buffApplied = false;
        buffTimer = baseBuffTimer;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnMouseEnter()//private void OnMouseEnter()
    {
        base.OnMouseEnter();
    }

    private void OnMouseExit()//private void OnMouseExit()
    {
        base.OnMouseExit();
    }

    private void OnMouseOver()//private void OnMouseOver()
    {
        base.OnMouseOver();
    }
}
