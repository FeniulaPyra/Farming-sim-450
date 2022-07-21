using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatantStats : CombatantStats
{
	public PlayerSkills pSkills;
	public GameObject targetEnemy;
	public GameObject time;

	public override int MaxHealth
	{
		get {
			//strength multiplier
			float hpMultiplier = 1 + pSkills.SumSkillsOfType<IncreaseHealthSkill>(new List<GameObject> { time });

			return maxHealthAdjustments + (int)Mathf.Round(BaseMaxHealth * hpMultiplier);
		}
		set {
			maxHealthAdjustments = value - BaseMaxHealth;
		}
	}

	public override float Strength
	{
		get
		{
			//strength multiplier
			float strMultiplier = 1 + pSkills.SumSkillsOfType<IncreaseStrengthSkill>(new List<GameObject> { gameObject, targetEnemy });

			//if the strength debuff tries to reduce the strength lower than zero
			if (Mathf.Abs(StrengthAdjustments) > BaseStrength * strMultiplier && StrengthAdjustments < 0)
			{
				//set strength debuffs to the minimum (-basestrength)
				strengthAdjustments = Mathf.CeilToInt(-BaseStrength * strMultiplier);
				//returns essentially 0
				return strengthAdjustments + BaseStrength * strMultiplier;
			}
			return StrengthAdjustments + BaseStrength;
		}
	}

	/// <summary>
	/// SET TARGET ENEMY BEFORE CALLING THIS IF THERE IS ONE.
	/// </summary>
	public override float Defense
	{
		get
		{
			//get skill defense boosts
			float defSkillMultiplier = 1 + pSkills.SumSkillsOfType<IncreaseDefenseSkill>(new List<GameObject> { gameObject, targetEnemy });

			//if defense adjustments tries to remove more than base defense...
			if (Mathf.Abs(DefenseAdjustments) > BaseDefense * defSkillMultiplier && DefenseAdjustments < 0)
			{
				//set defenseadjustments to -basedefense (ie the minimum defenseadjustment)
				defenseAdjustments = Mathf.CeilToInt(-BaseDefense * defSkillMultiplier);
				//and return (essentially) 0
				return defenseAdjustments + BaseDefense * defSkillMultiplier;
			}
			
			return defenseAdjustments + BaseDefense * defSkillMultiplier;
		}
		set
		{
			DefenseAdjustments = (int)value - BaseDefense;
		}
	}

	// Start is called before the first frame update
	/*public override*/ void Start()
    {
        //base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
