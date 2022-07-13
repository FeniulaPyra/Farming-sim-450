using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
	/// <summary>
	/// ahahahaa i love that im going to have to serialize it ahahaa
	/// </summary>
	int skillPoints;
	Skill skills;

    // Start is called before the first frame update
    void Start()
    {
		skills = new InfusionSkill(Skill.PrimaryMushroom.NONE);
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
				str = Serialize(LeftChildSkill, str);
			}



		}
		return str;
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
		List<string> vals = new List<string>(str.Split(','));

		//create root
		skills = new InfusionSkill(Skill.PrimaryMushroom.NONE);

		di = 0; //You were expecting i = 0 to be your indexing variable, but it was me, di=0! (for [D]eserialization [I]ndex)

		//set mushrooms
		skills.SetMushroom((Skill.PrimaryMushroom)int.Parse(vals[di]), 0);
		di++;

		skills.ChildSkills[0] = DeserializeRecursive(vals, skills.ChildSkills[0]);
		skills.ChildSkills[1] = DeserializeRecursive(vals, skills.ChildSkills[1]);

	}

	public Skill DeserializeRecursive(List<string> str, Skill cur)
	{
		if (cur == null) return null;
		for(int i = 0; i < 2; i++)
		{
			cur.SetMushroom((Skill.PrimaryMushroom)int.Parse(str[di]), i);
			di++;
			cur.ChildSkills[2 * i] = DeserializeRecursive(str, cur.ChildSkills[2 * i]);
			cur.ChildSkills[2 * i + 1] = DeserializeRecursive(str, cur.ChildSkills[2 * i + 1]);
		}

		return cur;
	}
}
