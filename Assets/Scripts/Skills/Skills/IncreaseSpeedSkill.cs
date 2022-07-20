using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IncreaseSpeedSkill : Skill
{
	public IncreaseSpeedSkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		parentShroom = PrimaryMushroom.BLUE;
		description = "Increase player speed by...";
		mushroomDescriptions[PrimaryMushroom.RED] =    "...30% while in dungeons";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "...30% while in the wilderness";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "...30% while in the town/farm";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "...30% while you have less than 50% health left";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "...15% everywhere";
		comboWord = "AND";
	}

	public override Skill Copy()
	{
		IncreaseSpeedSkill newMe = new IncreaseSpeedSkill(mushrooms[0], mushrooms[1]);
		for (int i = 0; i < this.ChildSkills.Count; i++)
		{
			newMe.ChildSkills[i] = ChildSkills[i] != null ? ChildSkills[i].Copy() : null;
		}
		return newMe;
	}


	/// <summary>
	/// Returns the total boost from this skill.
	/// </summary>
	/// <param name="parameters">Should include nothing.</param>
	/// <returns>returns the percent boost to speed</returns>
	new public float Apply(List<GameObject> putNullHere) { return base.Apply(parameters); }

	protected override float Red(List<GameObject> parameters)
	{
		if (SceneManager.GetActiveScene().name.Contains("Dungeon"))
			return .3f;
		return 0;
	}
	protected override float Yellow(List<GameObject> parameters)
	{
		if (SceneManager.GetActiveScene().name.Contains("Wilderness"))
			return .3f;
		return 0;
	}
	protected override float Blue(List<GameObject> parameters)
	{
		if (SceneManager.GetActiveScene().name.Contains("GroundScene") || SceneManager.GetActiveScene().name.Contains("TownScene"))
			return .3f;
		return 0;
	}
	protected override float White(List<GameObject> parameters)
	{
		if (SceneManager.GetActiveScene().name.Contains("Dungeon"))
			return .5f;
		return 0;
	}
	protected override float Black(List<GameObject> parameters)
	{
		return .15f;
	}
}

