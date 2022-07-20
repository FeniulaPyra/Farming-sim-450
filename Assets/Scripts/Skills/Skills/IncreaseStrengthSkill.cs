using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseStrengthSkill : Skill
{
	public IncreaseStrengthSkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		parentShroom = PrimaryMushroom.YELLOW;
		description = "Increase player base strength by...";
		mushroomDescriptions[PrimaryMushroom.RED] =    "...10% when using a melee weapon";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "...10% against ranged enemies";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "...10% against melee enemies";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "...10% when using a ranged weapon";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "...10% when you have lower than 50% of your health left";
		comboWord = "AND";
	}

	public override Skill Copy()
	{
		IncreaseStrengthSkill newMe = new IncreaseStrengthSkill(mushrooms[0], mushrooms[1]);
		for (int i = 0; i < this.ChildSkills.Count; i++)
		{
			newMe.ChildSkills[i] = ChildSkills[i] != null ? ChildSkills[i].Copy() : null;
		}
		return newMe;
	}


	/// <summary>
	/// Returns the total boost from this skill.
	/// </summary>
	/// <param name="parameters">Should include the player gameobject [0] and the relevant enemy gameobject [1].</param>
	/// <returns>returns the percent boost to max stamina</returns>
	new public float Apply(List<GameObject> playerAndEnemy)
	{
		return base.Apply(parameters);
	}

	protected override float Red(List<GameObject> parameters)
	{
		PlayerInventoryManager pinv = parameters[0].GetComponent<PlayerInventoryManager>();
		if (pinv == null)
			throw new System.Exception("No player inventory attached.");
		if (pinv.heldItem is MeleeWeaponItem)
			return .1f;
		return 0;
	}
	protected override float Yellow(List<GameObject> parameters)
	{
		BasicEnemy enemy = parameters[1].GetComponent<BasicEnemy>();
		if (enemy == null)
			throw new System.Exception("No Enemy attached.");
		if (enemy is RangedEnemy)
			return .1f;
		return 0;
	}
	protected override float Blue(List<GameObject> parameters)
	{
		BasicEnemy enemy = parameters[1].GetComponent<BasicEnemy>();
		if (enemy == null)
			throw new System.Exception("No Enemy attached.");
		if (!(enemy is RangedEnemy))
			return .1f;
		return 0;
	}
	protected override float White(List<GameObject> parameters)
	{
		PlayerInventoryManager pinv = parameters[0].GetComponent<PlayerInventoryManager>();
		if (pinv == null)
			throw new System.Exception("No player inventory attached.");
		if (pinv.heldItem is RangedWeaponItem)
			return .1f;
		return 0;
	}
	protected override float Black(List<GameObject> parameters)
	{
		CombatantStats playerStats = parameters[0].GetComponent<CombatantStats>();
		if (playerStats == null)
			throw new System.Exception("No player inventory attached.");
		if (playerStats.Health < playerStats.MaxHealth / 2)
			return .1f;
		return 0;
	}
}

