using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfusionSkill : Skill
{
	public InfusionSkill(PrimaryMushroom a) : base(a)
	{
		parentShroom = PrimaryMushroom.NONE;
		description = "Enemies react to you like you are a ...";
		mushroomDescriptions[PrimaryMushroom.RED] =    "...Red mushroom";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "...Yellow mushroom";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "...Blue mushroom";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "...White mushroom";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "...Black mushroom";
	}
	public override Skill Copy()
	{
		InfusionSkill newMe = new InfusionSkill(mushrooms[0]);
		for(int i = 0; i < this.ChildSkills.Count; i++)
		{
			newMe.ChildSkills[i] = ChildSkills[i] != null ? ChildSkills[i].Copy() : null;
		}
		return newMe;
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
