using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatantStats : MonoBehaviour
{
    //List of buffs
    public List<Buff> buffs = new List<Buff>();
    //For the player
    bool isPlayer;
    //Invincibility Frames
    [SerializeField]
    float iTimer;
    float baseITimer = 2.5f;
    bool invincible;

    //For buffs and debuffs
    [SerializeField]
    TMP_Text buffNotification;

    //player healthBar
    [SerializeField]
    public HealthBar healthBar;

    //Specifically for augmenting stats to make bosses more difficult then enemies of their same level
    public bool isBoss;

    [SerializeField]
	protected int maxHealthAdjustments;
	public virtual int MaxHealth {
		get
		{
			return maxHealthAdjustments + BaseMaxHealth;
		}
		set
		{
			maxHealthAdjustments = value - BaseMaxHealth;
		}
	}
	public int BaseMaxHealth
	{
		get
		{
			return 5 * (int)Mathf.Floor(level) + 25;
		}
	}

    [SerializeField]
	protected int health;
	public int Health {
		get {
			return health;
		}
		set {
			health = value;
			if (health < 0)
			{
				health = 0;
			}
		}
	}

    [SerializeField]
	protected int strengthAdjustments;
    public int StrengthAdjustments
    {
        get
        {
            int strMod = 0;

            foreach (Buff b in buffs)
            {
                if (b.type == Buff.BuffType.offense)
                {
                    strMod += b.Mod;
                }
            }

            strengthAdjustments = strMod;
            return strengthAdjustments;
        }
    }
    //public int Strength { get { return strengthAdjustments + BaseStrength; } set { strengthAdjustments = value - BaseStrength; } }
    public virtual float Strength
    {
        get
        {
            if (Mathf.Abs(StrengthAdjustments) > BaseStrength && StrengthAdjustments < 0)
            {
                strengthAdjustments = -BaseStrength;
                return strengthAdjustments + BaseStrength;
            }
            return StrengthAdjustments + BaseStrength;
        }
    }
    public int BaseStrength
	{
		get
		{
			return (int)Mathf.Round(Mathf.Pow(level, 6 / 5)) + 10;
		}
	}

    [SerializeField]
	protected int defenseAdjustments;
    public int DefenseAdjustments
    {
        get
        {
            int buffMod = 0;
            int equipMod = defenseAdjustments;

            foreach (Buff b in buffs)
            {
                if (b.type == Buff.BuffType.defense)
                {
                    buffMod += b.Mod;
                    if (b.added == true) { equipMod -= b.Mod; }
                    b.added = true;
                }
            }

            defenseAdjustments = buffMod + equipMod;
            return defenseAdjustments;
        }
        set
        {
            defenseAdjustments = value;
        }
    }
    //public int Defense { get { return defenseAdjustments + BaseDefense; } set { defenseAdjustments = value - BaseDefense; if (defenseAdjustments < 0) { defenseAdjustments = 0; } } }
    public virtual float Defense
    {
        get
        {
            if (Mathf.Abs(DefenseAdjustments) > BaseDefense && DefenseAdjustments < 0)
            {
                defenseAdjustments = -BaseDefense;
                return defenseAdjustments + BaseDefense;
            }
            return defenseAdjustments + BaseDefense;
        }
        set
        {
            DefenseAdjustments = (int)value - BaseDefense;
        }
    }
    public int BaseDefense
	{
		get
		{
			return (int)Mathf.Floor(level / 2) + 5;
		}
	}

    [SerializeField]
	protected int exp;
	public int Experience
	{
		get
        {
            exp = level * 3;

            if (isBoss == true)
            {
                exp *= 5;
            }
            return exp;
        }
	}

    [SerializeField]
	protected int level;
	public int Level {
		get
		{
			return level;
		}
		set
		{
			level = value;
			Health = MaxHealth;
            if (healthBar != null)
            {
                healthBar.SetMaxHealth(Health, true);
            }
		}
	}

    /*public virtual*/ void Start()
    {
        //Will need to have conditionals for things like "Are you loading a save?"
        Level = level;

        //Only the player will ever have this, so this should be safe
        if (gameObject.GetComponent<PlayerInteraction>() != null)
        {
            healthBar = GameObject.Find("Health Bar").GetComponent<HealthBar>();
            if (healthBar != null)
            {
                healthBar.SetMaxHealth(Health, true);
            }

            isPlayer = true;
            iTimer = baseITimer;

            buffNotification = GameObject.Find("TutorialObjective").GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        if (invincible == true)
        {
            iTimer -= Time.deltaTime;
        }

        if (iTimer <= 0.0f)
        {
            invincible = false;
            iTimer = baseITimer;
        }

        //Looping through all buffs, doing regen/poison
        //Decrement time.deltatime, if they've run out, remove them and call inverse if necessary.
        for (int i = 0; i < buffs.Count; i++)
        {
            //regen/poison
            if (buffs[i] is RegenBuff)
            {
                //cast buff
                RegenBuff buff = buffs[i] as RegenBuff;

                //They are identical, up until their actual functions
                if (buffs[i].iterations < buffs[i].maxIterations)
                {
                    buffs[i].effectTimer-= Time.deltaTime;
                    if (buffs[i].effectTimer <= 0.0f)
                    {
                        if (buff.type == Buff.BuffType.regen)
                        {
                            Heal(buff.factor, false);
                        }
                        else if (buff.type == Buff.BuffType.poison)
                        {
                            //This triggers invincibility frames
                            TakeDamage(buff.factor, true);
                        }

                        buffs[i].iterations++;
                        buffs[i].effectTimer = buff.baseTimer;
                    }
                }
                //After final iteration, remove it
                else
                {
                    buffs.RemoveAt(i);

                    if (buff.type == Buff.BuffType.regen)
                    {
                        buffNotification.text = buffNotification.text.Replace("\nRegenerating Health", "");
                    }
                    else if (buff.type == Buff.BuffType.poison)
                    {
                        //This triggers invincibility frames
                        buffNotification.text = buffNotification.text.Replace("\nPoisoned", "");
                    }

                    continue;
                }
            }
            else
            {
                buffs[i].timer -= Time.deltaTime;

                if (buffs[i].timer <= 0.0f)
                {
                    //Remove/invert effects of buffs
                    if (buffs[i] is StrengthBuff)
                    {
                        StrengthBuff buff = buffs[i] as StrengthBuff;
                        if (buff.IsDebuff == true)
                        {
                            buffNotification.text = buffNotification.text.Replace("\nStrength Decreased", "");
                        }
                        else
                        {
                            buffNotification.text = buffNotification.text.Replace("\nStrength Increased", "");
                        }
                    }
                    else if (buffs[i] is DefenseBuff)
                    {
                        DefenseBuff buff = buffs[i] as DefenseBuff;

                        Defense -= DefenseAdjustments;

                        if (buff.IsDebuff == true)
                        {
                            buffNotification.text = buffNotification.text.Replace("\nDefense Decreased", "");
                        }
                        else
                        {
                            buffNotification.text = buffNotification.text.Replace("\nDefense Increased", "");
                        }
                    }
                    else if (buffs[i] is SpeedBuff)
                    {
                        SpeedBuff buff = buffs[i] as SpeedBuff;
                        if (buff.IsDebuff == true)
                        {
                            buff.IncreaseSpeed();
                        }
                        else
                        {
                            buff.DecreaseSpeed();
                        }
                    }
                    //Remove buff from list
                    buffs.RemoveAt(i);
                    continue;
                }
            }
        }
    }

    public void ResetStrength()
	{
		strengthAdjustments = BaseStrength;
	}
	public void ResetDefense()
	{
		defenseAdjustments = BaseDefense;
	}

	public void IncreaseExp(int amt, bool ignoreLevelCheck = false)
	{
        //Adds on amount to player's existing experience
        exp += amt;
        //If that sum is greater than what they need to level up, continue
        if (exp >= ExpToLevel(Level + 1))
        {
            //Find out how much experience is left over
            int surplus = exp - ExpToLevel(Level + 1);
            //reset experience
            exp = 0;
            //increase level
            Level++;
            IncreaseExp(surplus); //recursive
        }
    }

	public void TakeDamage(int amt, bool ignoreDefense = false)
	{
        if (invincible == false)
        {
            //Always make sure you're taking damage
            amt = amt - (ignoreDefense ? 0 : defenseAdjustments);
            //subtracting negative damage would heal, so always do at least 1
            if (amt < 0)
            {
                amt = 1;
            }
            Health -= amt;

            if (healthBar != null)
            {
                healthBar.SetHealth(Health);
            }

            if (isPlayer == true)
            {
                invincible = true;
                iTimer = baseITimer;
            }
        }
    }

	public void Heal(int amt, bool ignoreMax)
	{
        Health += amt;

        if (Health > MaxHealth && !ignoreMax)
            Health = MaxHealth;

        if (healthBar != null)
        {
            healthBar.SetHealth(Health);
        }
    }

	//gets the amount of exp to get from the given level to the next level.
	public int ExpToLevel(int level)
	{
		return (int)Mathf.Round(100 * Mathf.Pow(2, (level-1) / 5)); //why on earth does Mathf.Round return a float
	}

}
