using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantStats : MonoBehaviour
{
    //List of buffs
    public List<Buff> buffs = new List<Buff>();

    [SerializeField]
	private int maxHealthAdjustments;
	public int MaxHealth { get { return maxHealthAdjustments + BaseMaxHealth; } set { maxHealthAdjustments = value - BaseMaxHealth; } }
	public int BaseMaxHealth
	{
		get
		{
			return 5 * (int)Mathf.Floor(level) + 25;
		}
	}


    [SerializeField]
    private int health;
	public int Health { get { return health; } set { health = value; if (health < 0) { health = 0; } } }

    [SerializeField]
    private int strengthAdjustments;
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
    public int Strength { get { if (Mathf.Abs(StrengthAdjustments) > BaseStrength && StrengthAdjustments < 0) { strengthAdjustments = -BaseStrength; return strengthAdjustments + BaseStrength; } return StrengthAdjustments + BaseStrength; }}
    public int BaseStrength
	{
		get
		{
			return (int)Mathf.Round(Mathf.Pow(level, 6 / 5)) + 10;
		}
	}

    [SerializeField]
    private int defenseAdjustments;
    public int DefenseAdjustments
    {
        get
        {
            int defMod = 0;

            foreach (Buff b in buffs)
            {
                if (b.type == Buff.BuffType.defense)
                {
                    defMod += b.Mod;
                }
            }

            defenseAdjustments = defMod;
            return defenseAdjustments;
        }
    }
    //public int Defense { get { return defenseAdjustments + BaseDefense; } set { defenseAdjustments = value - BaseDefense; if (defenseAdjustments < 0) { defenseAdjustments = 0; } } }
    public int Defense { get { if (Mathf.Abs(DefenseAdjustments) > BaseDefense && DefenseAdjustments < 0) { defenseAdjustments = -BaseDefense; return defenseAdjustments + BaseDefense; } return DefenseAdjustments + BaseDefense; } set { Defense += value; } }
    public int BaseDefense
	{
		get
		{
			return (int)Mathf.Floor(level / 2) + 5;
		}
	}

    [SerializeField]
    private int exp;
	public int Experience
	{
		get { return exp; }
	}

    [SerializeField]
    private int level;
	public int Level {
		get
		{
			return level;
		}
		set
		{
			level = value;
			Health = MaxHealth;
		}
	}

	public CombatantStats(int level)
	{
		Level = level;
		health = maxHealthAdjustments;
	}
	public CombatantStats(int level, int exp, int maxhealth, int health)
	{
		Level = level;
		this.exp = exp;
		this.maxHealthAdjustments = maxhealth;
		this.health = health;
	}

    private void Start()
    {
        //Will need to have conditionals for things like "Are you loading a save?"
        Level = level;
    }

    public void ResetStrength()
	{
		strengthAdjustments = BaseStrength;
	}
	public void ResetDefense()
	{
		defenseAdjustments = BaseDefense;
	}

	public void IncreaseExp(int amt, bool ignoreLevelCheck)
	{
		if (amt > ExpToLevel(Level + 1) && !ignoreLevelCheck)
		{
			exp = 0;
			IncreaseExp(amt - ExpToLevel(Level + 1), false); //recursive
			Level++;
		}
		exp += amt;
	}

	public void TakeDamage(int amt, bool ignoreDefense = false)
	{
        //Always make sure you're taking damage
        amt = amt - (ignoreDefense ? 0 : defenseAdjustments);
        //subtracting negative damage would heal, so always do at least 1
        if (amt < 0)
        {
            amt = 1;
        }
        //health -= (amt - (ignoreDefense ? 0 : defense));
        Health -= amt;
    }

	public void Heal(int amt, bool ignoreMax)
	{
        //health += amt;
        Health += amt;
        /*if (health > maxHealth && !ignoreMax)
			health = maxHealth;*/
        if (Health > maxHealthAdjustments && !ignoreMax)
            Health = maxHealthAdjustments;
    }

	//gets the amount of exp to get from the given level to the next level.
	public int ExpToLevel(int level)
	{
		return (int)Mathf.Round(20 * Mathf.Pow(2, (level-1) / 5)); //why on earth does Mathf.Round return a float
	}

}
