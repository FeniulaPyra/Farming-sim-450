using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantStats
{
	private int maxHealth;
	public int MaxHealth { get; }

	private int health;
	public int Health { get; }

	private int strength;
	public int Strength { get; set; }
	public int BaseStrength
	{
		get
		{
			return (int)Mathf.Round(Mathf.Pow(level, 6 / 5)) + 10;
		}
	}

	private int defense;
	public int Defense { get; set; }
	public int BaseDefense
	{
		get
		{
			return (int)Mathf.Floor(level / 2) + 5;
		}
	}

	private int exp;
	public int Experience
	{
		get { return exp; }
	}

	private int level;
	public int Level {
		get
		{
			return level;
		}
		set
		{
			level = value;
			maxHealth = 5 * (int)Mathf.Floor(level) + 25;
			defense = BaseDefense;
			strength = BaseStrength;
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
		exp = amt;
	}

	public void TakeDamage(int amt, bool ignoreDefense)
	{
		health -= (amt - (ignoreDefense ? 0 : defense));
	}

	public void Heal(int amt, bool ignoreMax)
	{
		health += amt;
		if (health > maxHealth && !ignoreMax)
			health = maxHealth;
	}

	//gets the amount of exp to get from the given level to the next level.
	public int ExpToLevel(int level)
	{
		return (int)Mathf.Round(20 * Mathf.Pow(2, (level-1) / 5)); //why on earth does Mathf.Round return a float
	}

}
