using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
	public string description;

	public Dictionary<PrimaryMushroom, string> mushroomDescriptions;

	//the mushroom selection that leads to this skill. Also the flavortext "type" of this skill.
	public PrimaryMushroom parentShroom;

	//model like buff, except have the different skills extend instead of using an enum.
	public enum PrimaryMushroom
	{
		NONE,
		RED,
		YELLOW,
		BLUE,
		WHITE,
		BLACK
	}

	public List<PrimaryMushroom> mushrooms;

	public List<GameObject> parameters;

	public List<Skill> ChildSkills;

	public Skill(PrimaryMushroom a, PrimaryMushroom b)
	{
		mushrooms = new List<PrimaryMushroom>();
		ChildSkills = new List<Skill>();

		SetMushroom(a, 0);
		SetMushroom(b, 1);
	}
	public Skill(PrimaryMushroom m)
	{
		mushrooms = new List<PrimaryMushroom>();
		ChildSkills = new List<Skill>();

		SetMushroom(m, 0);
	}

	public void SetMushroom(PrimaryMushroom m, int side)
	{
		mushrooms[side] = m;
		switch(m)
		{
			case PrimaryMushroom.RED:
				ChildSkills[side + 0] = (new IncreaseHealthSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				ChildSkills[side + 1] = (new StaminaBoostSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				break;
			case PrimaryMushroom.YELLOW:
				ChildSkills[side + 0] = (new IncreaseStrengthSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				ChildSkills[side + 1] = (new MushroomSpreadSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				break;
			case PrimaryMushroom.BLUE:
				ChildSkills[side + 0] = (new IncreaseSpeedSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				ChildSkills[side + 1] = (new MushroomThirstSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				break;
			case PrimaryMushroom.WHITE:
				ChildSkills[side + 0] = (new IncreaseDefenseSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				ChildSkills[side + 1] = (new StaminaEfficiencySkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				break;
			case PrimaryMushroom.BLACK:
				ChildSkills[side + 0] = (new SkillBoostSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				ChildSkills[side + 1] = (new PartySkillShare(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				break;
			default:
				ChildSkills[side + 0] = (null);
				ChildSkills[side + 1] = (null);
				break;
		}
	}

	public float Apply(List<GameObject> parameters) {
		float boost = 0;

		foreach (PrimaryMushroom shroom in mushrooms)
		{
			switch (shroom)
			{
				case PrimaryMushroom.RED:
					boost += Red(parameters);
					break;
				case PrimaryMushroom.YELLOW:
					boost += Yellow(parameters);
					break;
				case PrimaryMushroom.BLUE:
					boost += Blue(parameters);
					break;
				case PrimaryMushroom.WHITE:
					boost += White(parameters);
					break;
				case PrimaryMushroom.BLACK:
					boost += Black(parameters);
					break;
				default:
					break;
			}
		}

		return boost;
	}

	protected abstract float Red(List<GameObject> parameters);
	protected abstract float Yellow(List<GameObject> parameters);
	protected abstract float Blue(List<GameObject> parameters);
	protected abstract float White(List<GameObject> parameters);
	protected abstract float Black(List<GameObject> parameters);
}
