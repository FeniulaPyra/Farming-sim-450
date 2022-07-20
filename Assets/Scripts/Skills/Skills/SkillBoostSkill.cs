using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBoostSkill : Skill
{
	public SkillBoostSkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		parentShroom = PrimaryMushroom.BLACK;
		description = "Skill effects with ______ mushrooms selected get a 10% bonus";
		mushroomDescriptions[PrimaryMushroom.RED] =    "Red";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "Yellow";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "Blue";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "White";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "Black";
		comboWord = "OR";
	}

	public override Skill Copy()
	{
		SkillBoostSkill newMe = new SkillBoostSkill(mushrooms[0], mushrooms[1]);
		for (int i = 0; i < this.ChildSkills.Count; i++)
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
	/// <returns>returns ur mom</returns>
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
