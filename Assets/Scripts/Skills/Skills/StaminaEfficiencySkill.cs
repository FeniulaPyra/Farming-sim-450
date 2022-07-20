using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaEfficiencySkill : Skill
{
	public FoodEfficiencySkill secondarySkill;

	public StaminaEfficiencySkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		secondarySkill = new FoodEfficiencySkill(a, b);

		parentShroom = PrimaryMushroom.WHITE;
		description = "Stamina is more efficient";
		mushroomDescriptions[PrimaryMushroom.RED] =    "It costs 2 less stamina to use the sickle";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "It costs 2 less stamina to use the hoe";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "It costs 2 less stamina to use the watering can";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "Increase stamina gained from eating mushrooms by 25%";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "Increase stamina gained from eating cooked meals by 25%";
		comboWord = "AND";
	}

	public override Skill Copy()
	{
		StaminaEfficiencySkill newMe = new StaminaEfficiencySkill(mushrooms[0], mushrooms[1]);
		for (int i = 0; i < this.ChildSkills.Count; i++)
		{
			newMe.ChildSkills[i] = ChildSkills[i] != null ? ChildSkills[i].Copy() : null;
		}
		return newMe;
	}

	/// <summary>
	/// Returns the total boost from this skill.
	/// </summary>
	/// <param name="parameters">Should include the player gameobject</param>
	/// <returns>returns the boost to stamina</returns>
	new public float Apply(List<GameObject> player)
	{
		return base.Apply(parameters);
	}

	protected override float Red(List<GameObject> parameters)
	{
		PlayerInventoryManager pinv = parameters[0].GetComponent<PlayerInventoryManager>();
		if (pinv == null)
			throw new System.Exception("No player inventory attached.");
		if (pinv.heldItem.name.ToLower().Contains("sickle"))
			return 2;
		return 0;
	}
	protected override float Yellow(List<GameObject> parameters)
	{
		PlayerInventoryManager pinv = parameters[0].GetComponent<PlayerInventoryManager>();
		if (pinv == null)
			throw new System.Exception("No player inventory attached.");
		if (pinv.heldItem.name.ToLower().Contains("hoe"))
			return 2;
		return 0;
	}
	protected override float Blue(List<GameObject> parameters)
	{
		PlayerInventoryManager pinv = parameters[0].GetComponent<PlayerInventoryManager>();
		if (pinv == null)
			throw new System.Exception("No player inventory attached.");
		if (pinv.heldItem.name.ToLower().Contains("watering can"))
			return 2;
		return 0;
	}
	protected override float White(List<GameObject> parameters)
	{
		return 0;
	}
	protected override float Black(List<GameObject> parameters)
	{
		return 0;
	}
}
