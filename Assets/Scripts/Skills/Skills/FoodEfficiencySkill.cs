using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodEfficiencySkill : Skill
{

	public FoodEfficiencySkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		//part of stamina efficiency so does not need display info.
	}

	public override Skill Copy()
	{
		FoodEfficiencySkill newMe = new FoodEfficiencySkill(mushrooms[0], mushrooms[1]);
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
		return 0;
	}
	protected override float Yellow(List<GameObject> parameters)
	{
		return 0;
	}
	protected override float Blue(List<GameObject> parameters)
	{
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
