using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaLostDecreaseSkill : Skill
{
	public StaminaLostDecreaseSkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b) 
	{
		//since its part of the stamina efficiency skill, doesnt need descriptions.
	}

	/// <summary>
	/// Returns the total boost from this skill.
	/// </summary>
	/// <param name="parameters">Should include no parameters for this skill.</param>
	/// <returns>returns the percent boost to max stamina</returns>
	new public float Apply(List<GameObject> putNullHere) { return base.Apply(parameters); }

	protected override float Red(List<GameObject> parameters)
	{
		return -.05f;
	}
	protected override float Yellow(List<GameObject> parameters)
	{
		return -.1f;
	}
	protected override float Blue(List<GameObject> parameters)
	{
		return -.15f;
	}
	protected override float White(List<GameObject> parameters)
	{
		return -.2f;
	}
	protected override float Black(List<GameObject> parameters)
	{
		return -.5f;
	}
}
