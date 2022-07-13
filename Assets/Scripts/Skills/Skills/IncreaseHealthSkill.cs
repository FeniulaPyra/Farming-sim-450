using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseHealthSkill : Skill
{
	public IncreaseHealthSkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		parentShroom = PrimaryMushroom.RED;
		description = "Player maximum health is increased sometimes";
		mushroomDescriptions[PrimaryMushroom.RED] =    "Max Health is increased by 20% during the Fall";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "Max Health is increased by 20% during the Summer";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "Max Health is increased by 20% during the Spring";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "Max Health is increased by 20% during the Winter";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "Max Health is increased by 5% year-round";
	}

	/// <summary>
	/// Returns the total boost from this skill.
	/// </summary>
	/// <param name="parameters">Should include a TimeManager.</param>
	/// <returns>returns the percent boost to health</returns>
	new public float Apply(List<GameObject> timeManager) { return base.Apply(parameters); }

	protected override float Red(List<GameObject> parameters)
	{
		if ((TimeManager.Season)parameters[0].GetComponent<TimeManager>().SeasonNumber == TimeManager.Season.FALL) return .25f;
		return 0;
	}
	protected override float Yellow(List<GameObject> parameters)
	{
		if ((TimeManager.Season)parameters[0].GetComponent<TimeManager>().SeasonNumber == TimeManager.Season.SUMMER) return .25f;
		return 0;
	}
	protected override float Blue(List<GameObject> parameters)
	{
		if ((TimeManager.Season)parameters[0].GetComponent<TimeManager>().SeasonNumber == TimeManager.Season.SPRING) return .25f;
		return 0;
	}
	protected override float White(List<GameObject> parameters)
	{
		if ((TimeManager.Season)parameters[0].GetComponent<TimeManager>().SeasonNumber == TimeManager.Season.WINTER) return .25f;
		return 0;
	}
	protected override float Black(List<GameObject> parameters)
	{
		return .1f;
	}
}
