using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBoostSkill : Skill
{
	StaminaLostDecreaseSkill secondarySkill;

	public StaminaBoostSkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		secondarySkill = new StaminaLostDecreaseSkill(a, b);

		parentShroom = PrimaryMushroom.RED;
		description = "\"Max\" Stamina is increased and stamina loss from passing out is decreased.";
		mushroomDescriptions[PrimaryMushroom.RED] =	   "Stamina is increased by 30% and loss is decreased by 5%.";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "Stamina is increased by 25% and loss is decreased by 10%.";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "Stamina is increased by 20% and loss is decreased by 15%.";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "Stamina is increased by 15% and loss is decreased by 20%.";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "Stamina is increased by 0% and loss is decreased by 50%.";
	}

	/// <summary>
	/// Returns the total boost from this skill.
	/// </summary>
	/// <param name="parameters">Should include no parameters for this skill.</param>
	/// <returns>returns the percent boost to max stamina</returns>
	new public float Apply(List<GameObject> putNullHere) { return base.Apply(parameters); }

	protected override float Red(List<GameObject> parameters)
	{
		return .3f;
	}
	protected override float Yellow(List<GameObject> parameters)
	{
		return .25f;
	}
	protected override float Blue(List<GameObject> parameters)
	{
		return .2f;
	}
	protected override float White(List<GameObject> parameters)
	{
		return .15f;
	}
	protected override float Black(List<GameObject> parameters)
	{
		return 0;
	}
}

