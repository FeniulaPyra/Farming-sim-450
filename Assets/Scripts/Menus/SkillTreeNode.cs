using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeNode : MonoBehaviour
{

	//public static float WIDTH = 750;
	//public static float HEIGHT = 500;

	public float Width
	{
		get
		{
			RectTransform rect = gameObject.GetComponent<RectTransform>();
			return rect.rect.width * rect.localScale.x;
		}
	}
	public float Height
	{
		get
		{
			RectTransform rect = gameObject.GetComponent<RectTransform>();
			return rect.rect.height * rect.localScale.y;
		}
	}

	[SerializeField]
	float padding;

	//why
	public SkillTreeMenu menu;

	//my skill
	[SerializeField]
	private Skill mySkill;
	public Skill MySkill
	{
		set
		{
			//saves info about parent
			Skill parent = mySkill.parentSkill;
			//saves position in siblings
			int indexInParent = parent.ChildSkills.IndexOf(mySkill);

			//saves info about children and mushrooms
			List<Skill> children = mySkill.ChildSkills;
			List<Skill.PrimaryMushroom> mushrooms = mySkill.mushrooms;

			if (value == null)
			{
				parent.ChildSkills[indexInParent] = null;
			}
			else {
				mySkill = value;
				//reattaches to parent...
				mySkill.parentSkill = parent;
				//...in the correct place among siblings
				parent.ChildSkills[indexInParent] = mySkill;

				//reattaches children and mushrooms
				mySkill.ChildSkills = children;
				mySkill.mushrooms = mushrooms;
			}
		}
		get
		{
			return mySkill;
		}
	}

	//family relations
	public SkillTreeNode ParentNode;
	public List<SkillTreeNode> childNodes;
	public List<LineRenderer> childBranchLines;

	///Descriptions
	public Text MainDescription;

	public Dropdown LeftMushroom;
	public Text LeftDescription;

	public Dropdown RightMushroom;
	public Text RightDescription;

	public Text ComboText;

	//warnings
	public Image UnappliedChanges;
	public Text DeletingBranchWarning;

	//skill backgrounds 
	[SerializeField]
	List<Sprite> SkillCrystals;
	public Image SkillCrystal;
	

    // Start is called before the first frame update
    void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {
		if ((ParentNode == null || !ParentNode.childNodes.Contains(this)) &&  !(mySkill is InfusionSkill))
			Destroy(gameObject);
	}

	public void SetPlayerSkillReference(Skill playerSkillReference)
	{
		mySkill = playerSkillReference;
	}

	public void HideMe()
	{
		foreach(SkillTreeNode stn in childNodes)
		{
			stn.HideMe();
		}
		gameObject.SetActive(false);
	}

	public void ShowMe()
	{
		foreach (SkillTreeNode stn in childNodes)
		{
			stn.ShowMe();
		}
		gameObject.SetActive(true);
	}

	public void Initialize(Skill k, SkillTreeNode parent, SkillTreeMenu menuArea)
	{
		mySkill = k;
		k.parentSkill = k is InfusionSkill ? null : parent.mySkill;
		ParentNode = parent;
		menu = menuArea;

		
		if (childNodes == null || childNodes.Count < 1)
		{
			childNodes = new List<SkillTreeNode> { null, null };
			if(!(k is InfusionSkill))
			{
				childNodes.Add(null);
				childNodes.Add(null);
			}
		}
		LeftMushroom.SetValueWithoutNotify((int)k.mushrooms[0]);
		if (k.mushrooms.Count > 1)
			RightMushroom.SetValueWithoutNotify((int)k.mushrooms[1]);
		/*LeftMushroom.value = (int)k.mushrooms[0];
		if (k.mushrooms.Count > 1)
			RightMushroom.value = (int)k.mushrooms[1];*/
			UpdateMyDisplay();	
	}
	//will be run on dropdown change.
	public void UpdateMyDisplay()
	{
		//destroys me if i no longer have a skill :(
		if (mySkill == null)
		{
			return;
		}

		//sets description
		MainDescription.text = mySkill.description;

		//sets left mushroom stuff
		LeftDescription.text = mySkill.mushroomDescriptions[mySkill.mushrooms[0]];
		

		//sets right mushroom stuff if it exists
		if (!(mySkill is InfusionSkill))
		{
			RightDescription.text = mySkill.mushroomDescriptions[mySkill.mushrooms[1]];
		}
		//hides right mushroom stuff if it is not there
		else
		{
			RightMushroom.gameObject.SetActive(false);
			RightDescription.gameObject.SetActive(false);
		}

		if(mySkill.parentShroom != Skill.PrimaryMushroom.NONE)
		{
			SkillCrystal.sprite = SkillCrystals[(int)mySkill.parentShroom - 1];
			SkillCrystal.color = Color.white;
		}
		else
		{
			SkillCrystal.color = Color.clear;
		}

		ComboText.text = mySkill.comboWord;

		//sets branches to go to child nodes
		for (int i = 0; i < childNodes.Count; i++)
		{
			LineRenderer lr = childBranchLines[i];
			SkillTreeNode stn = childNodes[i];

			if(stn != null)
				lr.SetPosition(1, stn.gameObject.GetComponent<RectTransform>().localPosition);
		}

		
	}

	public void SetLeftMushroom()
	{
		SetMushroom((Skill.PrimaryMushroom)LeftMushroom.value, 0);
		UpdateMyDisplay();
		menu.RearangeNodes();
		menu.UpdateDisplay();
	}
	public void SetRightMushroom()
	{
		SetMushroom((Skill.PrimaryMushroom)RightMushroom.value, 1);
		UpdateMyDisplay();
		menu.RearangeNodes();
		menu.UpdateDisplay();
	}


	public void SetMushroom(Skill.PrimaryMushroom newShroom, int side)
	{
		//List<Skill> toAdd = Skill.GetMushroomSkills(newShroom);

		mySkill.SetMushroom(newShroom, side);//mushrooms[side] = newShroom;

		if(newShroom == Skill.PrimaryMushroom.NONE)
		{
			if(2 * side + 1 > childNodes.Count)
			{
				childNodes.Add(null);
				childNodes.Add(null);
			}
			else
			{
				//childNodes[2 * side].MySkill = null;
				childNodes[2 * side].DestroyRecursive();
				childNodes[2 * side] = null;


				//childNodes[2 * side + 1].MySkill = null;
				childNodes[2 * side + 1].DestroyRecursive();
				childNodes[2 * side + 1] = null;
			}
		}
		//if the childnode doesnt exist yet
		else if(2 * side + 1 > childNodes.Count)
		{
			GameObject newLeftNodeObject = (GameObject)Instantiate(menu.SkillTreeNodePrefab, gameObject.transform.parent);
			SkillTreeNode newLeftNode = newLeftNodeObject.GetComponent<SkillTreeNode>();
			newLeftNode.Initialize(mySkill.ChildSkills[2 * side]/*toAdd[0]*/, this, menu);
			childNodes.Add(newLeftNode);

			GameObject newRightNodeObject = (GameObject)Instantiate(menu.SkillTreeNodePrefab, gameObject.transform.parent);
			SkillTreeNode newRightNode = newRightNodeObject.GetComponent<SkillTreeNode>();
			newRightNode.Initialize(mySkill.ChildSkills[2 * side + 1], this, menu);
			childNodes.Add(newRightNode);

		}
		else
		{
			if (childNodes[2 * side] == null)
			{
				GameObject newLeftNodeObject = (GameObject)Instantiate(menu.SkillTreeNodePrefab, gameObject.transform.parent);
				SkillTreeNode newLeftNode = newLeftNodeObject.GetComponent<SkillTreeNode>();
				newLeftNode.Initialize(mySkill.ChildSkills[2 * side], this, menu);
				childNodes[2 * side] = newLeftNode;
			}
			else
			{
				List<Skill.PrimaryMushroom> shrooms = childNodes[2 * side].mySkill.mushrooms;
				childNodes[2 * side].mySkill = mySkill.ChildSkills[2 * side];
				childNodes[2 * side].mySkill.mushrooms = shrooms;


			}
			if (childNodes[2 * side + 1] == null)
			{
				GameObject newRightNodeObject = (GameObject)Instantiate(menu.SkillTreeNodePrefab, gameObject.transform.parent);
				SkillTreeNode newRightNode = newRightNodeObject.GetComponent<SkillTreeNode>();
				newRightNode.Initialize(mySkill.ChildSkills[2 * side + 1], this, menu);
				childNodes[2 * side + 1] = newRightNode;
			}
			else
			{
				List<Skill.PrimaryMushroom> shrooms = childNodes[2 * side + 1].mySkill.mushrooms;
				childNodes[2 * side + 1].mySkill = mySkill.ChildSkills[2 * side + 1];
				childNodes[2 * side + 1].mySkill.mushrooms = shrooms;
			}
		}
	}

	public float GetPadding()
	{
		float myBranchWidth = Width;
		foreach(SkillTreeNode child in childNodes)
		{
			if(child != null)
				myBranchWidth += child.GetPadding();
		}
		/*
		if (myBranchWidth == 0)
			return Width * 2;//SkillTreeNode.WIDTH;
			*/
		padding = myBranchWidth;
		return myBranchWidth;
	}

	public void DestroyRecursive()
	{
		foreach(SkillTreeNode child in childNodes)
		{
			if (child != null)
				child.DestroyRecursive();
		}
		Destroy(gameObject);
	}
}
