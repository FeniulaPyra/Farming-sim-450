using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEnemy : MonoBehaviour
{
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
    //TMP_Text buffNotification;

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
        //base.Start();

        if (GlobalGameSaving.Instance != null && ScenePersistence.Instance != null)
        {
            if (GlobalGameSaving.Instance.loadingSave == false && ScenePersistence.Instance.changingScene == false)
            {
                timer = baseTimer;
                buffTimer = baseBuffTimer;
            }
        }
        stats = gameObject.GetComponent<CombatantStats>();

        speed = new SpeedBuff(gameObject.GetComponent<BasicEnemy>(), Buff.BuffType.speed, 30.0f);
        regen = new RegenBuff(stats, Buff.BuffType.speed, 5, 5.0f, 10);
    }

    // Update is called once per frame
    void Update()
    {
        //base.Update();

        if (ScenePersistence.Instance.gamePaused == false)
        {
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
                        //CancelBuff();
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

    }

    void ApplyBuff()
    {
        if (increaseSpeed == true)
        {
            stats.buffs.Add(speed);
            speed.IncreaseSpeed();
        }

        if (increaseStrength == true)
        {
            str = new StrengthBuff(strMod, false, Buff.BuffType.offense, 30.0f);
            stats.buffs.Add(str);
            Debug.Log($"Strength Mod: {stats.Strength}");
        }

        if (increaseDefense == true)
        {
            def = new DefenseBuff(defMod, false, Buff.BuffType.defense, 30.0f);
            stats.buffs.Add(def);
            Debug.Log($"Defense Mod: {stats.Defense}");
        }

        if (regenHealth == true)
        {
            stats.buffs.Add(regen);
        }

        buffApplied = true;
        timer = baseTimer;
    }
}
