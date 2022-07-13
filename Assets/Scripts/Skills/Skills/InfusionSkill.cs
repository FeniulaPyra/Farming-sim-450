using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfusionSkill : Skill
{
	public InfusionSkill(PrimaryMushroom a) : base(a)
	{
		parentShroom = PrimaryMushroom.NONE;
		description = "Mushrooms don't need as much water to survive";
		mushroomDescriptions[PrimaryMushroom.RED] = "Mushrooms don't need water in the Fall";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "Mushrooms don't need water in the Summer";
		mushroomDescriptions[PrimaryMushroom.BLUE] = "Mushrooms don't need water in the Spring";
		mushroomDescriptions[PrimaryMushroom.WHITE] = "Mushrooms don't need water in the Winter";
		mushroomDescriptions[PrimaryMushroom.BLACK] = "Mushrooms survive without water 2 extra days year-round.";
	}

	protected override float Black(List<GameObject> parameters)
	{
		throw new System.NotImplementedException();
	}

	protected override float Blue(List<GameObject> parameters)
	{
		throw new System.NotImplementedException();
	}

	protected override float Red(List<GameObject> parameters)
	{
		throw new System.NotImplementedException();
	}

	protected override float White(List<GameObject> parameters)
	{
		throw new System.NotImplementedException();
	}

	protected override float Yellow(List<GameObject> parameters)
	{
		throw new System.NotImplementedException();
	}
}
