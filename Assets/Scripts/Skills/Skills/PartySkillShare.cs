using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this video is sponsored by
public class PartySkillShare : Skill
{
	public PartySkillShare(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		parentShroom = PrimaryMushroom.BLACK;
		description = "Apply some of your skills to party members too.";
		mushroomDescriptions[PrimaryMushroom.RED] = "Apply Red skills to party members at 75% efficiency";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "Apply Yellow skills to party memberes at 75% efficiency";
		mushroomDescriptions[PrimaryMushroom.BLUE] = " Apply Blue skills to party members at 75% efficiency";
		mushroomDescriptions[PrimaryMushroom.WHITE] = "Apply White skills to party members at 75% efficiency";
		mushroomDescriptions[PrimaryMushroom.BLACK] = "Apply Black skills to party members at 75% efficiency";
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
