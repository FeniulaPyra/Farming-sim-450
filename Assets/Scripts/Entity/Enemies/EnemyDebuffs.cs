using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyDebuffs : MonoBehaviour
{
    //Reference to player movement to alter their speed
    PlayerMovement movement;

    //chance of debuff activating
    public int activateChance;

    public bool decreaseSpeed;

    public bool poison;
    public int poisonIterations = 0; // How many times you take poison damage before the buff wears off
    public int poisonFactor; //How much you are hurt by poison.
    float poisonTimer = 5.0f;
    public int testHealth = 100;

    [SerializeField]
    public bool debuffApplied;
    [SerializeField]
    public float debuffTimer;
    public float baseDebuffTimer;

    [SerializeField]
    TMP_Text debuffNotification;


    // Start is called before the first frame update
    void Start()
    {
        if (GlobalGameSaving.Instance != null && ScenePersistence.Instance != null)
        {
            if (GlobalGameSaving.Instance.loadingSave == false && ScenePersistence.Instance.changingScene == false)
            {
                //timer = baseTimer;
                debuffTimer = baseDebuffTimer;
            }
        }

        movement = FindObjectOfType<PlayerMovement>();
        debuffNotification = GameObject.Find("TutorialObjective").GetComponent<TextMeshProUGUI>();
        //debuffNotification.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        /*if (GlobalGameSaving.Instance != null && ScenePersistence.Instance != null)
        {
            if (GlobalGameSaving.Instance.loadingSave == true || ScenePersistence.Instance.changingScene == true)
            {
                if (debuffApplied == true)
                {
                    ApplyBuff();
                    ScenePersistence.Instance.changingScene = false;
                    GlobalGameSaving.Instance.loadingSave = false;
                }
                else
                {
                    CancelBuff();
                }
            }
        }*/

        if (debuffApplied == true)
        {
            if (poison == true && poisonIterations < 5)
            {
                if (poisonIterations < 5)
                {
                    poisonTimer -= Time.deltaTime;
                    if (poisonTimer <= 0.0f)
                    {
                        testHealth -= poisonFactor;
                        poisonIterations++;
                        poisonTimer = 5.0f;
                    }
                }
                else
                {
                    poisonIterations = 0;
                    CancelDebuff();
                }
            }
            

            debuffTimer -= Time.deltaTime;

            if (debuffTimer <= 0.0f)
            {
                CancelDebuff();
            }
        }
    }

    public void ApplyDebuff()
    {
        if (decreaseSpeed == true && debuffApplied == false)
        {
            movement.MovementSpeed /= 2;
            debuffNotification.text += "\nSpeed Decreased";
        }

        if (poison == true && debuffApplied == false)
        {
            poisonTimer = 5.0f;
            debuffNotification.text += "\nPoisoned";
        }

        debuffApplied = true;
    }

    void CancelDebuff()
    {
        //debuffNotification.text = "";
        if (decreaseSpeed == true)
        {
            movement.MovementSpeed *= 2;
            debuffNotification.text = debuffNotification.text.Replace("\nSpeed Decreased", "");
        }

        if (poison == true)
        {
            debuffNotification.text = debuffNotification.text.Replace("\nPoisoned", "");
        }

        debuffApplied = false;
        debuffTimer = baseDebuffTimer;
    }

    /*public void SaveBuffPet(out SaveBuffPet buffPet)
    {
        buffPet = new SaveBuffPet(movementSpeed, menu, size, speedCurve, maxSeekDistance, minSeekDistance, sr, player, rb, facing, gameObject, gameObject.transform.position, normalImage, pettingImage, petItem, manager, gameObject, timer, baseTimer, activateChance, increaseSpeed, buffApplied, buffTimer, baseBuffTimer);
    }*/
}

/*[System.Serializable]
public class SaveBuffPet : SavePet
{
    //Timer on which the item spawns
    public float timer;
    public float baseTimer;
    //chance of buff activating
    public int activateChance;

    public bool increaseSpeed;
    public bool buffApplied;
    public float buffTimer;
    public float baseBuffTimer;

    public SaveBuffPet(float mS, GameObject m, Slider s, AnimationCurve sC, float maxD, float minD, SpriteRenderer sR, Transform p, Rigidbody2D rB, Vector2 f, GameObject self, Vector3 pos, Sprite n, Sprite petting, Item pet, FarmManager manager, GameObject gameObject, float t, float bT, int chance, bool iS, bool bA, float buffT, float bBuffT) : base(mS, m, s, sC, maxD, minD, sR, p, rB, f, self, pos, n, petting, pet, manager, gameObject)
    {
        timer = t;
        baseTimer = bT;
        activateChance = chance;
        increaseSpeed = iS;
        buffApplied = bA;
        buffTimer = buffT;
        baseBuffTimer = bBuffT;
    }
}*/
