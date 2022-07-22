using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyDebuffs : MonoBehaviour
{
    //Reference to player movement to alter their speed
    [SerializeField]
    PlayerMovement movement;
    //The player's stats
    [SerializeField]
    CombatantStats stats;

    //chance of debuff activating
    public int activateChance;

    public bool decreaseSpeed;

    public bool decreaseDefense;

    public bool decreaseStrength;

    public bool poison;

    [SerializeField]
    public bool debuffApplied;
    [SerializeField]
    public float debuffTimer;
    public float baseDebuffTimer;

    [SerializeField]
    TMP_Text debuffNotification;

    SpeedBuff speed;
    public int strMod;//To be filled out in inspector for buffs to differ pet by pet //StrengthBuff strength;
    public int defMod;//To be filled out in inspector for buffs to differ pet by pet //DefenseBuff defense;
    RegenBuff poisonDebuff;
    StrengthBuff str; //exists solely so that buffs can be removed with remove(str/def)
    DefenseBuff def;


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
        stats = movement.gameObject.GetComponent<CombatantStats>();
        debuffNotification = GameObject.Find("TutorialObjective").GetComponent<TextMeshProUGUI>();
        poisonDebuff = new RegenBuff(stats, Buff.BuffType.poison, 5, 5f, 2);
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
    }

    public void ApplyDebuff()
    {
        if (decreaseSpeed == true && debuffApplied == false)
        {
            speed = new SpeedBuff(movement, Buff.BuffType.speed, debuffNotification, true, 30.0f);
            stats.buffs.Add(speed);

        }

        if (decreaseStrength == true && debuffApplied == false)
        {
            str = new StrengthBuff(debuffNotification, strMod, true, Buff.BuffType.offense, 30.0f);
            stats.buffs.Add(str);
            Debug.Log($"Strength Mod: {stats.StrengthAdjustments}");
        }

        if (decreaseDefense == true && debuffApplied == false)
        {
            def = new DefenseBuff(debuffNotification, defMod, true, Buff.BuffType.defense, 30.0f);
            stats.buffs.Add(def);
            Debug.Log($"Defense Mod: {stats.DefenseAdjustments}");
        }

        if (poison == true && debuffApplied == false)
        {
            stats.buffs.Add(poisonDebuff);
            debuffNotification.text += "\nPoisoned";
        }

        debuffApplied = true;
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
