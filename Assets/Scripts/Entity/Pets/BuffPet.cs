using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

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
    //chance of item buff activating
    public int activateChance;

    public bool increaseSpeed;

    public bool increaseDefense;

    public bool increaseStrength;

    public bool regenHealth;

    [SerializeField]
    public bool buffApplied;
    [SerializeField]
    public float buffTimer;
    public float baseBuffTimer;

    [SerializeField]
    TMP_Text buffNotification;

    //The four buff types, but instances of them.
    SpeedBuff speed;
    public int strMod;//To be filled out in inspector for buffs to differ pet by pet //StrengthBuff strength;
    public int defMod;//To be filled out in inspector for buffs to differ pet by pet //DefenseBuff defense;
    RegenBuff regen;
    StrengthBuff str; //exists solely so that buffs can be removed with remove(str/def)
    DefenseBuff def;

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

        regen = new RegenBuff(stats, Buff.BuffType.regen, 5, 5.0f, 5);
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
    }

    void ApplyBuff()
    {
        if (increaseSpeed == true)
        {
            speed = new SpeedBuff(movement, Buff.BuffType.speed, buffNotification, false, 30.0f, this);
            stats.buffs.Add(speed);
        }

        if (increaseStrength == true)
        {
            str = new StrengthBuff(buffNotification, strMod, false, Buff.BuffType.offense, 30.0f);
            stats.buffs.Add(str);
            Debug.Log($"Strength Mod: {stats.StrengthAdjustments}");
        }

        if (increaseDefense == true)
        {
            def = new DefenseBuff(buffNotification, defMod, false, Buff.BuffType.defense, 30.0f);
            stats.buffs.Add(def);
            Debug.Log($"Defense Mod: {stats.DefenseAdjustments}");
        }

        if (regenHealth == true)
        {
            stats.buffs.Add(regen);
            buffNotification.text += "\nRegenerating Health";
        }

        buffApplied = true;
        timer = baseTimer;
    }

    void CancelBuff()
    {
        if (increaseSpeed == true)
        {
            speed.DecreaseSpeed();
        }

        if (increaseDefense == true)
        {
            buffNotification.text = buffNotification.text.Replace("\nDefense Increased", "");
            stats.Defense -= stats.DefenseAdjustments; //After removing the buff from the mod, set Defense using the newly adjusted defense modifier
            stats.buffs.Remove(def);
            Debug.Log($"Defense Mod: {stats.DefenseAdjustments}");
        }

        if (increaseStrength == true)
        {
            buffNotification.text = buffNotification.text.Replace("\nStrength Increased", "");
            stats.buffs.Remove(str);
            Debug.Log($"Strength Mod: {stats.StrengthAdjustments}");
        }

        if (regenHealth == true)
        {
            buffNotification.text = buffNotification.text.Replace("\nRegenerating Health", "");
        }

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
