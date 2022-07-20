using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomSpreadSkill : Skill
{
	public MushroomSpreadSkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		parentShroom = PrimaryMushroom.YELLOW;
		description = "Mushrooms have a ____ chance to spread an extra time per day";
		mushroomDescriptions[PrimaryMushroom.RED] =    "+20% chance to spread an extra time per day in the Fall";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "+20% chance to spread an extra time per day in the Summer";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "+20% chance to spread an extra time per day in the Spring";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "+20% chance to spread an extra time per day in the Winter";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "+5% chance to spread an extra time per day year-round.";
		comboWord = "AND";
	}

	public override Skill Copy()
	{
		MushroomSpreadSkill newMe = new MushroomSpreadSkill(mushrooms[0], mushrooms[1]);
		for (int i = 0; i < this.ChildSkills.Count; i++)
		{
			newMe.ChildSkills[i] = ChildSkills[i] != null ? ChildSkills[i].Copy() : null;
		}
		return newMe;
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

