using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBoostSkill : Skill
{
	public StaminaLostDecreaseSkill secondarySkill;

	public StaminaBoostSkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		secondarySkill = new StaminaLostDecreaseSkill(a, b);

		parentShroom = PrimaryMushroom.RED;
		description = "Stamina is increased by ___ and stamina loss from passing out is decreased by ___.";
		mushroomDescriptions[PrimaryMushroom.RED] =	   "+30% max stamina and -5% stamina loss when passing out.";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "+25% max stamina and -10% stamina loss when passing out.";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "+20% max stamina and -15% stamina loss when passing out.";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "+15% max stamina and -20% stamina loss when passing out.";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "+0% max stamina and -50% stamina loss when passing out.";
		comboWord = "AND";
	}

	public override Skill Copy()
	{
		StaminaBoostSkill newMe = new StaminaBoostSkill(mushrooms[0], mushrooms[1]);
		for (int i = 0; i < this.ChildSkills.Count; i++)
		{
			newMe.ChildSkills[i] = ChildSkills[i] != null ? ChildSkills[i].Copy() : null;
		}
		return newMe;
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

