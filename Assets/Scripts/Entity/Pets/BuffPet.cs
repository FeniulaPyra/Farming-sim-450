using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffPet : BasicPet
{
    //Reference to player movement to alter their speed
    [SerializeField]
    PlayerMovement movement;
    //The player's stats
    [SerializeField]
    CombatantStats stats;

    //Timer on which the item spawns
    [SerializeField]
    public float timer;
    public float baseTimer;
    //chance of item spawning
    public int activateChance;

    public bool increaseSpeed;

    public bool increaseDefense;

    public bool increaseStrength;

    public bool regenHealth;

    /*public int healIterations;
    public float healTimer;
    public int testHealth;
    public int healFactor;*/

    [SerializeField]
    public bool buffApplied;
    [SerializeField]
    public float buffTimer;
    public float baseBuffTimer;

    [SerializeField]
    TMP_Text buffNotification;

    //The four buff types, but instances of them.
    SpeedBuff speed;
    StrengthBuff strength;
    DefenseBuff defense;
    RegenBuff regen;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        if (GlobalGameSaving.Instance != null && ScenePersistence.Instance != null)
        {
            if (GlobalGameSaving.Instance.loadingSave == false && ScenePersistence.Instance.changingScene == false)
            {
                timer = baseTimer;
                buffTimer = baseBuffTimer;
            }
        }

        movement = FindObjectOfType<PlayerMovement>();
        stats = movement.gameObject.GetComponent<CombatantStats>();
        buffNotification = GameObject.Find("TutorialObjective").GetComponent<TextMeshProUGUI>();
        buffNotification.text = "";

        speed = new SpeedBuff(movement, buffNotification, this);
        strength = new StrengthBuff(stats, buffNotification);
        defense = new DefenseBuff(stats, buffNotification);
        regen = new RegenBuff(stats, 5, 5.0f, 5.0f, 10);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (GlobalGameSaving.Instance != null && ScenePersistence.Instance != null)
        {
            if (GlobalGameSaving.Instance.loadingSave == true || ScenePersistence.Instance.changingScene == true)
            {
                if (buffApplied == true)
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
        }

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
            if (regenHealth == true)
            {
                /*if (healIterations < 5)
                {
                    healTimer -= Time.deltaTime;
                    if (healTimer <= 0.0f)
                    {
                        stats.Heal(healFactor, false);
                        healIterations++;
                        healTimer = 5.0f;
                    }
                }
                else
                {
                    healIterations = 0;
                    CancelBuff();
                }*/

                if (regen.healIterations < 5)
                {
                    regen.healTimer -= Time.deltaTime;
                    if (regen.healTimer <= 0.0f)
                    {
                        stats.Heal(regen.healFactor, false);
                        regen.healIterations++;
                        regen.healTimer = regen.baseHealTimer;
                    }
                }
                else
                {
                    regen.healIterations = 0;
                    CancelBuff();
                }
            }
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
            /*movement.MovementSpeed *= 2;
            movementSpeed *= 2;
            buffNotification.text += "\nSpeed Increased";*/
            speed.IncreaseSpeed();
        }

        if (increaseStrength == true)
        {
            /*stats.Strength += 10;
            buffNotification.text += "\nStrength Increased";
            Debug.Log($"Strength Mod: {stats.Strength}");*/
            strength.IncreaseStrength();
        }

        if (increaseDefense == true)
        {
            /*stats.Defense += 10;
            buffNotification.text += "\nDefense Increased";
            Debug.Log($"Defense Mod: {stats.Defense}");*/
            defense.IncreaseDefense();
        }

        if (regenHealth == true)
        {
            //healTimer = 5.0f;
            regen.healTimer = regen.baseHealTimer;
            buffNotification.text += "\nRegenerating Health";
        }

        buffApplied = true;
        timer = baseTimer;
    }

    void CancelBuff()
    {
        if (increaseSpeed == true)
        {
            /*buffNotification.text = buffNotification.text.Replace("\nSpeed Increased", "");
            movement.MovementSpeed /= 2;
            movementSpeed /= 2;*/
            speed.DecreaseSpeed();
        }

        if (increaseDefense == true)
        {
            /*buffNotification.text = buffNotification.text.Replace("\nDefense Increased", "");
            stats.ResetDefense();*/
            strength.DecreaseStrength();
        }

        if (increaseStrength == true)
        {
            /*buffNotification.text = buffNotification.text.Replace("\nStrength Increased", "");
            stats.ResetStrength();*/
            defense.DecreaseDefense();
        }

        if (regenHealth == true)
        {
            buffNotification.text = buffNotification.text.Replace("\nRegenerating Health", "");
        }

        //buffNotification.text = "";

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

    public void SaveBuffPet(out SaveBuffPet buffPet)
    {
        buffPet = new SaveBuffPet(movementSpeed, menu, size, speedCurve, maxSeekDistance, minSeekDistance, sr, player, rb, facing, gameObject, gameObject.transform.position, normalImage, pettingImage, petItem, manager, gameObject, timer, baseTimer, activateChance, increaseSpeed, buffApplied, buffTimer, baseBuffTimer);
    }
}

[System.Serializable]
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

    public SaveBuffPet(float mS, GameObject m, Slider s, AnimationCurve sC, float maxD, float minD, SpriteRenderer sR, Transform p, Rigidbody2D rB, Vector2 f, GameObject self, Vector3 pos, Sprite n, Sprite petting, Item pet, FarmManager manager, GameObject gameObject, float t, float bT, int chance, bool iS, bool bA, float buffT, float bBuffT ) : base(mS, m, s, sC, maxD, minD, sR, p, rB, f, self, pos, n, petting, pet, manager, gameObject)
    {
        timer = t;
        baseTimer = bT;
        activateChance = chance;
        increaseSpeed = iS;
        buffApplied = bA;
        buffTimer = buffT;
        baseBuffTimer = bBuffT;
    }
}
