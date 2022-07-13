using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaEfficiencySkill : Skill
{
	public StaminaEfficiencySkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		parentShroom = PrimaryMushroom.WHITE;
		description = "Stamina is reduced for certain tools or stamina gain from certain foods is increased";
		mushroomDescriptions[PrimaryMushroom.RED] =    "It costs 2 less stamina to use the sickle";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "It costs 2 less stamina to use the hoe";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "It costs 2 less stamina to use the watering can";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "Increase stamina from eating mushrooms";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "Increase stamina from eating cooked meals";
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
		PlayerInventoryManager pinv = parameters[0].GetComponent<PlayerInventoryManager>();
		if (pinv == null)
			throw new System.Exception("No player inventory attached.");
		if (pinv.heldItem.type.ToLower().Contains("mushroom"))
			return .25f;
		return 0;
	}
	protected override float Black(List<GameObject> parameters)
	{
		PlayerInventoryManager pinv = parameters[0].GetComponent<PlayerInventoryManager>();
		if (pinv == null)
			throw new System.Exception("No player inventory attached.");
		if (pinv.heldItem.type.ToLower().Contains("meal"))
			return .25f;
		return 0;
	}
}
