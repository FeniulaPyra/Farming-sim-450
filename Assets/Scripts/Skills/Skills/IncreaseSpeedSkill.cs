using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IncreaseSpeedSkill : Skill
{
	public IncreaseSpeedSkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		parentShroom = PrimaryMushroom.BLUE;
		description = "Increase player speed";
		mushroomDescriptions[PrimaryMushroom.RED] =    "Base Speed is increased by 30% while in dungeons";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "Base Speed is increased by 30% while in the wilderness";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "Base Speed is increased by 30% while in the town/farm";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "Base Speed is increased by 30% while you have less than 50% health left";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "Base Speed is increased by 15%";
	}

	/// <summary>
	/// Returns the total boost from this skill.
	/// </summary>
	/// <param name="parameters">Should include the player gameobject.</param>
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
		CombatantStats stats = parameters[0].GetComponent<CombatantStats>();
		if (stats == null)
			throw new System.Exception("No Player stats attached.");
		if (SceneManager.GetActiveScene().name.Contains("Dungeon"))
			return .5f;
		return 0;
	}
	protected override float Black(List<GameObject> parameters)
	{
		return .15f;
	}
}

