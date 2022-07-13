using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomSpreadSkill : Skill
{
	public MushroomSpreadSkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		parentShroom = PrimaryMushroom.YELLOW;
		description = "Mushrooms have a chance to spread an extra time each day";
		mushroomDescriptions[PrimaryMushroom.RED] =    "Mushrooms have a 20% chance to spread one extra time per day in the Fall";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "Mushrooms have a 20% chance to spread one extra time per day in the Summer";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "Mushrooms have a 20% chance to spread one extra time per day in the Spring";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "Mushrooms have a 20% chance to spread one extra time per day in the Winter";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "Mushrooms have a 5% chance to spread one extra time per day year-round.";
	}

	/// <summary>
	/// Returns the total boost from this skill.
	/// </summary>
	/// <param name="parameters">Should include the timemanager gameobject</param>
	/// <returns>returns the percent chance for mushrooms to spread a second time</returns>
	new public float Apply(List<GameObject> timeManager)
	{
		return base.Apply(timeManager);
	}

	protected override float Red(List<GameObject> parameters)
	{
		TimeManager time = parameters[0].GetComponent<TimeManager>();
		if (time == null)
			throw new System.Exception("No Timemanager attached.");
		if ((TimeManager.Season)time.SeasonNumber == TimeManager.Season.FALL)
			return .2f;
		return 0;

	}
	protected override float Yellow(List<GameObject> parameters)
	{
		TimeManager time = parameters[0].GetComponent<TimeManager>();
		if (time == null)
			throw new System.Exception("No Timemanager attached.");
		if ((TimeManager.Season)time.SeasonNumber == TimeManager.Season.SUMMER)
			return .2f;
		return 0;
	}
	protected override float Blue(List<GameObject> parameters)
	{
		TimeManager time = parameters[0].GetComponent<TimeManager>();
		if (time == null)
			throw new System.Exception("No Timemanager attached.");
		if ((TimeManager.Season)time.SeasonNumber == TimeManager.Season.SPRING)
			return .2f;
		return 0;
	}
	protected override float White(List<GameObject> parameters)
	{
		TimeManager time = parameters[0].GetComponent<TimeManager>();
		if (time == null)
			throw new System.Exception("No Timemanager attached.");
		if ((TimeManager.Season)time.SeasonNumber == TimeManager.Season.WINTER)
			return .2f;
		return 0;
	}
	protected override float Black(List<GameObject> parameters)
	{
		return .5f;
	}
}

