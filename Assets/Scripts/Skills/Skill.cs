using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
	public string description;
	//the word used to describe what happens when combining two effects - will be either "and" or "or"
	//for instance, for the stamina efficiency skill, if you have a red anda  white mushroom, it should say
	// "It costs 2 less stamina to use the sickle >AND< Increase stamina gained from eating mushrooms by 25%"
	//whereas for the skill boost skill it should say "Skill effects with red >OR< white mushrooms selected get a 10% bonus"
	public string comboWord;

	public Dictionary<PrimaryMushroom, string> mushroomDescriptions = new Dictionary<PrimaryMushroom, string>
	{

		{PrimaryMushroom.NONE, "" },
		{PrimaryMushroom.RED, "" },
		{PrimaryMushroom.YELLOW, "" },
		{PrimaryMushroom.BLUE, "" },
		{PrimaryMushroom.WHITE, "" },
		{PrimaryMushroom.BLACK, "" }
	};

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
	public Skill parentSkill;

	public Skill(PrimaryMushroom a, PrimaryMushroom b)
	{
		comboWord = "";
		mushrooms = new List<PrimaryMushroom> { PrimaryMushroom.NONE, PrimaryMushroom.NONE };
		ChildSkills = new List<Skill> { null, null, null, null };

		SetMushroom(a, 0);
		SetMushroom(b, 1);
	}
	public Skill(PrimaryMushroom m)
	{
		mushrooms = new List<PrimaryMushroom> { PrimaryMushroom.NONE };
		ChildSkills = new List<Skill> { null, null };

		SetMushroom(m, 0);
	}

	public abstract Skill Copy();

	public bool Equals(Skill p)
	{
		return this == p;
	}
	//https://www.geeksforgeeks.org/write-c-code-to-determine-if-two-trees-are-identical/
	public static bool TreeEquivalence(Skill left, Skill right)
	{
		//if they arent the same type, just exit
		if (left.GetType() != right.GetType()) return false;

		return Skill.TreeMushroomEquivalence(left, right);
	}

	public static bool TreeMushroomEquivalence(Skill left, Skill right)
	{
		if (left == null && right == null) return true;
		else if (left != null && right != null)
		{
			bool isEqual = (left.mushrooms.Count == right.mushrooms.Count);
			for (int i = 0; i < left.mushrooms.Count; i++)
			{
				isEqual = isEqual && left.ChildSkills[i] == right.ChildSkills[i];
			}
			return isEqual;
		}
		return false;

	}

	public static bool NodeEquivalence(Skill left, Skill right)
	{
		//if they arent the same type return false
		if (left.GetType() != right.GetType()) return false;

		return NodeMushroomEquivalence(left, right);
		
	}
	public static bool NodeMushroomEquivalence(Skill left, Skill right)
	{
		//if they dont have the same mushrooms return false;
		for (int i = 0; i < left.mushrooms.Count; i++)
		{
			if (left.mushrooms[i] != right.mushrooms[i])
				return false;
		}

		//true otherwise
		return true;
	}

	public void SetMushroom(PrimaryMushroom m, int side)
	{
		mushrooms[side] = m;

		List<Skill> newSkills = GetMushroomSkills(m);

		if(newSkills[0]	!= null)
		{
			//attaches the parent skill to the new skill
			newSkills[0].parentSkill = this;
			//attaches teh children to the new skill 
			if(ChildSkills[2 * side]!= null && ChildSkills[2 * side].ChildSkills != null)
				newSkills[0].ChildSkills = ChildSkills[2 * side].ChildSkills;
		}
		//sets the child skill
		ChildSkills[2 * side + 0] = newSkills[0];

		if(newSkills[1]	!= null)
		{
			newSkills[1].parentSkill = this;
			if(ChildSkills[2 * side + 1] != null && ChildSkills[2 * side + 1].ChildSkills != null)
				newSkills[1].ChildSkills = ChildSkills[2 * side + 1].ChildSkills;
		}
		ChildSkills[2 * side + 1] = newSkills[1];
		
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

	public static List<Skill> GetMushroomSkills(PrimaryMushroom mushroom)
	{
		List<Skill> skills = new List<Skill>();
		switch (mushroom)
		{
			case PrimaryMushroom.RED:
				skills.Add(new IncreaseHealthSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				skills.Add(new StaminaBoostSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				break;
			case PrimaryMushroom.YELLOW:
				skills.Add(new IncreaseStrengthSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				skills.Add(new MushroomSpreadSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				break;
			case PrimaryMushroom.BLUE:
				skills.Add(new IncreaseSpeedSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				skills.Add(new MushroomThirstSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				break;
			case PrimaryMushroom.WHITE:
				skills.Add(new IncreaseDefenseSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				skills.Add(new StaminaEfficiencySkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				break;
			case PrimaryMushroom.BLACK:
				skills.Add(new SkillBoostSkill(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				skills.Add(new PartySkillShare(PrimaryMushroom.NONE, PrimaryMushroom.NONE));
				break;
			default:
				skills.Add(null);
				skills.Add(null);
				break;
		}
		return skills;
	}
}
