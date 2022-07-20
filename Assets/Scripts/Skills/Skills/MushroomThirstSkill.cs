using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomThirstSkill : Skill
{
	public MushroomThirstSkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		parentShroom = PrimaryMushroom.BLUE;
		description = "Mushrooms don't need as much water to survive:";
		mushroomDescriptions[PrimaryMushroom.RED] =    "Mushrooms don't need water in the Fall";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "Mushrooms don't need water in the Summer";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "Mushrooms don't need water in the Spring";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "Mushrooms don't need water in the Winter";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "Mushrooms survive without water 2 extra days year-round.";
		comboWord = "AND";
	}

	public override Skill Copy()
	{
		MushroomThirstSkill newMe = new MushroomThirstSkill(mushrooms[0], mushrooms[1]);
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
	/// <returns>returns the number of additional days that mushrooms will be able to survive</returns>
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
			return 30;
		return 0;

	}
	protected override float Yellow(List<GameObject> parameters)
	{
		TimeManager time = parameters[0].GetComponent<TimeManager>();
		if (time == null)
			throw new System.Exception("No Timemanager attached.");
		if ((TimeManager.Season)time.SeasonNumber == TimeManager.Season.SUMMER)
			return 30;
		return 0;
	}
	protected override float Blue(List<GameObject> parameters)
	{
		TimeManager time = parameters[0].GetComponent<TimeManager>();
		if (time == null)
			throw new System.Exception("No Timemanager attached.");
		if ((TimeManager.Season)time.SeasonNumber == TimeManager.Season.SPRING)
			return 30;
		return 0;
	}
	protected override float White(List<GameObject> parameters)
	{
		TimeManager time = parameters[0].GetComponent<TimeManager>();
		if (time == null)
			throw new System.Exception("No Timemanager attached.");
		if ((TimeManager.Season)time.SeasonNumber == TimeManager.Season.WINTER)
			return 30;
		return 0;
	}
	protected override float Black(List<GameObject> parameters)
	{
		return 2;
	}
}

