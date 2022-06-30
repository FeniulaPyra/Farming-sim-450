using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantStats : MonoBehaviour
{
    [SerializeField]
	private int maxHealth;
	public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

    [SerializeField]
    private int health;
	public int Health { get { return health; } set { health = value; if (health < 0) { health = 0; } } }

    [SerializeField]
    private int strength;
    public int Strength { get { return strength; } set { strength = value; } }
	public int BaseStrength
	{
		get
		{
			return (int)Mathf.Round(Mathf.Pow(level, 6 / 5)) + 10;
		}
	}

    [SerializeField]
    private int defense;
	public int Defense { get { return defense; } set { defense = value; if (defense < 0) { defense = 0; } } }
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
            int strBuff = 0;
            int defBuff = 0;

            if (strength > 0)
            {
                strBuff = strength - BaseStrength;
            }

            if (defense > 0)
            {
                defBuff = defense - BaseDefense;
            }

			level = value;
			maxHealth = 5 * (int)Mathf.Floor(level) + 25;
            Health = maxHealth;
			defense = BaseDefense + defBuff;
			strength = BaseStrength + strBuff;
		}
	}

	public CombatantStats(int level)
	{
		Level = level;
		health = maxHealth;
	}
	public CombatantStats(int level, int exp, int maxhealth, int health)
	{
		Level = level;
		this.exp = exp;
		this.maxHealth = maxhealth;
		this.health = health;
	}

    private void Start()
    {
        //Will need to have conditionals for things like "Are you loading a save?"
        Level = level;
    }

    public void ResetStrength()
	{
		strength = BaseStrength;
	}
	public void ResetDefense()
	{
		defense = BaseDefense;
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
        //health -= (amt - (ignoreDefense ? 0 : defense));
        Health -= (amt - (ignoreDefense ? 0 : defense));
    }

	public void Heal(int amt, bool ignoreMax)
	{
        //health += amt;
        Health += amt;
        /*if (health > maxHealth && !ignoreMax)
			health = maxHealth;*/
        if (Health > maxHealth && !ignoreMax)
            Health = maxHealth;
    }

	//gets the amount of exp to get from the given level to the next level.
	public int ExpToLevel(int level)
	{
		return (int)Mathf.Round(20 * Mathf.Pow(2, (level-1) / 5)); //why on earth does Mathf.Round return a float
	}

}
