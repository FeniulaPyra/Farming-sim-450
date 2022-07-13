using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBoostSkill : Skill
{
	public SkillBoostSkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		parentShroom = PrimaryMushroom.BLACK;
		description = "Skill effects with the certain mushrooms selected get a 10% bonus";
		mushroomDescriptions[PrimaryMushroom.RED] =    "Skills with Red mushrooms attached to them get a 10% bonus.";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "Skills with Yellow mushrooms attached to them get a 10% bonus.";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "Skills with Blue mushrooms attached to them get a 10% bonus.";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "Skills with White mushrooms attached to them get a 10% bonus.";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "Skills with Black mushrooms attached to them get a 10% bonus.";
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
