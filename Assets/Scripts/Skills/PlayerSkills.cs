using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
	/// <summary>
	/// ahahahaa i love that im going to have to serialize it ahahaa
	/// </summary>
	public int skillPoints;

	public int usedPoints;

	public Skill skillTree;

	public TimeManager time;

	public InfusionSkill Root
	{
		get
		{
			return (InfusionSkill)skillTree;
		}
	}
	
    // Start is called before the first frame update
    void Start()
    {
		skillTree = new InfusionSkill(Skill.PrimaryMushroom.NONE);
		if (GlobalGameSaving.Instance != null)
		{
			if (GlobalGameSaving.Instance.loadingSave == true)
			{
				//inv.SetSaveableInventory(GlobalGameSaving.Instance.inventory);
				Deserialize(GlobalGameSaving.Instance.skills);
			}
			else if (ScenePersistence.Instance != null)
			{
				Deserialize(ScenePersistence.Instance.skills);
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public string Serialize(Skill parent, string str)
	{
		for (int i = 0; i < parent.mushrooms.Count; i++)
		{
			str += (int)parent.mushrooms[i];

			if ((int)parent.mushrooms[i] != 0)
			{
				//left skill
				Skill LeftChildSkill = parent.ChildSkills[2 * i];
				str = Serialize(LeftChildSkill, str);

				//right skill
				Skill RightChildSkill = parent.ChildSkills[2 * i + 1];
				str = Serialize(RightChildSkill, str);
			}



		}
		return str;
	}

	public void Save(string what)
	{
		if (what == "persist")
		{
			ScenePersistence.Instance.skills = Serialize(skillTree, "");//inv.GetSaveableInventory();
		}
		else if (what == "save")
		{
			GlobalGameSaving.Instance.skills = Serialize(skillTree, "");//inv.GetSaveableInventory();
		}
	}
	private int di;
	/// <summary>
	/// modified from
	/// https://stackoverflow.com/questions/4611555/how-to-serialize-binary-tree
	/// https://www.geeksforgeeks.org/serialize-deserialize-binary-tree/
	/// </summary>
	/// <param name="str">the saved string of the skill</param>
	public void Deserialize(string str)
	{
		if (str.Length < 1) return;
		//List<string> vals = new List<string>(str.Split());
		string vals = str;
		//create root
		skillTree = new InfusionSkill(Skill.PrimaryMushroom.NONE);

		di = 0; //You were expecting i = 0 to be your indexing variable, but it was me, di=0! (for [D]eserialization [I]ndex)

		//set mushrooms
		skillTree.SetMushroom((Skill.PrimaryMushroom)int.Parse("" + vals[di]), 0);
		di++;

		skillTree.ChildSkills[0] = DeserializeRecursive(vals, skillTree.ChildSkills[0]);
		skillTree.ChildSkills[1] = DeserializeRecursive(vals, skillTree.ChildSkills[1]);

	}

	public Skill DeserializeRecursive(string str, Skill cur)
	{
		if (cur == null) return null;
		for(int i = 0; i < 2; i++)
		{
			cur.SetMushroom((Skill.PrimaryMushroom)int.Parse("" + str[di]), i);
			di++;
			cur.ChildSkills[2 * i] = DeserializeRecursive(str, cur.ChildSkills[2 * i]);
			cur.ChildSkills[2 * i + 1] = DeserializeRecursive(str, cur.ChildSkills[2 * i + 1]);
		}

		return cur;
	}

	public float SumSkillsOfType<T>(List<GameObject> parameters)
	{
		Dictionary<Skill.PrimaryMushroom, float> skillBoosters = GetSkillBoost();
		float boost = 0;
		SkillIterator iterate = new DepthFirstSkillIterator(skillTree);
		
		//since staminalostdecreaseskills are all nested withing staminaboost skills, this is here
		//to check if the player is summing them, and to instead search for staminaboost skills and 
		//then call the secondary skill of those skills (i.e. staminalostdecreaseskill) - same goes for foodefficiency skill
		StaminaLostDecreaseSkill defaultStaminaLoss = new StaminaLostDecreaseSkill(Skill.PrimaryMushroom.NONE, Skill.PrimaryMushroom.NONE);
		FoodEfficiencySkill defaultFoodEfficiency = new FoodEfficiencySkill(Skill.PrimaryMushroom.NONE, Skill.PrimaryMushroom.NONE);

		while (iterate.HasMore())
		{
			Skill k = iterate.GetNext();
			if(k is T)
			{
				float skillBoost = 0;
				//adds up the skill boost to this skill
				foreach(Skill.PrimaryMushroom m in k.mushrooms)
				{
					if(m != Skill.PrimaryMushroom.NONE)
						skillBoost += skillBoosters[m];
				}
				//applies the skill boost to this skill and adds the effect from this skill to the total effect.
				boost += k.Apply(parameters) * (1 + skillBoost);
			}
			else if (defaultStaminaLoss is T && k is StaminaBoostSkill)
			{
				float skillBoost = 0;
				//adds up the skill boost to this skill
				foreach (Skill.PrimaryMushroom m in k.mushrooms)
				{
					if (m != Skill.PrimaryMushroom.NONE)
						skillBoost += skillBoosters[m];
				}
				//applies the skill boost to this skill and adds the effect from this skill to the total effect.
				boost += ((StaminaBoostSkill)k).secondarySkill.Apply(parameters) * (1 + skillBoost);
			}
			else if (defaultFoodEfficiency is T && k is StaminaEfficiencySkill)
			{
				float skillBoost = 0;
				//adds up the skill boost to this skill
				foreach (Skill.PrimaryMushroom m in k.mushrooms)
				{
					if (m != Skill.PrimaryMushroom.NONE)
						skillBoost += skillBoosters[m];
				}
				//applies the skill boost to this skill and adds the effect from this skill to the total effect.
				boost += ((StaminaEfficiencySkill)k).secondarySkill.Apply(parameters) * (1 + skillBoost);
			}
		}

		return boost;

	}

	public Dictionary<Skill.PrimaryMushroom, float> GetSkillBoost()
	{
		Dictionary<Skill.PrimaryMushroom, float> boosts = new Dictionary<Skill.PrimaryMushroom, float>
		{
			{Skill.PrimaryMushroom.RED, 0 },
			{Skill.PrimaryMushroom.YELLOW, 0 },
			{Skill.PrimaryMushroom.BLUE, 0 },
			{Skill.PrimaryMushroom.WHITE, 0 },
			{Skill.PrimaryMushroom.BLACK, 0 }
		};
		SkillIterator iterate = new DepthFirstSkillIterator(skillTree);
		while(iterate.HasMore())
		{
			Skill k = iterate.GetNext();
			if(k is SkillBoostSkill)
			{
				if(k.mushrooms[0] != Skill.PrimaryMushroom.NONE)
					boosts[k.mushrooms[0]] += k.Apply(null);
				if(k.mushrooms[1] != Skill.PrimaryMushroom.NONE)
					boosts[k.mushrooms[1]] += k.Apply(null);
			}
		}
		return boosts;
	}

	public void UpdateSkill(Skill skillInTree, Skill newSkill)
	{

	}
}
