using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this video is sponsored by
public class PartySkillShare : Skill
{
	public PartySkillShare(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		parentShroom = PrimaryMushroom.BLACK;
		description = "Apply ___ skills to party members at 75% efficiency";
		mushroomDescriptions[PrimaryMushroom.RED] =    "Red skills";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "Yellow skills";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "Blue skills";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "White skills";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "Black skills";
		comboWord = "AND";

	}

	public override Skill Copy()
	{
		PartySkillShare newMe = new PartySkillShare(mushrooms[0], mushrooms[1]);
		for(int i = 0; i < this.ChildSkills.Count; i++)
		{
			newMe.ChildSkills[i] = ChildSkills[i] != null ? ChildSkills[i].Copy() : null;
		}
		return newMe;
	}


	/// <summary>
	/// Returns the total boost from this skill.
	/// </summary>
	/// <param name="parameters">Should include nothing because im insane and going to implement this skill
	/// differently because i dont know how to freaking do it</param>
	/// <returns>returns nothin</returns>
	new public float Apply(List<GameObject> player)
	{
		return base.Apply(parameters);
	}

	protected override float Red(List<GameObject> parameters) { return .1f; }
	protected override float Yellow(List<GameObject> parameters) { return .1f; }
	protected override float Blue(List<GameObject> parameters) { return .1f; }
	protected override float White(List<GameObject> parameters) { return .1f; }
	protected override float Black(List<GameObject> parameters) { return .1f; }

}
