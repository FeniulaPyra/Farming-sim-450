using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeMenu : TogglableMenu
{

	public PlayerSkills pSkills;
	InfusionSkill root;
	//the tree that is actually displayed in the menu. will be copied over to player skills when apply changes is called.
	public InfusionSkill workingRoot;
	
	public SkillTreeNode rootNode;
	public SkillTreeNodeIterator treeIterator;

	//for spacing things out without overlapping stuff
	public List<List<SkillTreeNode>> layers;
	private int currentLayer;

	public GameObject SkillTreeNodePrefab;
	public GameObject SkillTreeContentAreaObject;

	public Text CurrentSP;
	public Text SPCostLabel;

	public Button ApplyButton;
	public Button ResetButton;

	public int totalSkillPointCost;

    // Start is called before the first frame update
    void Awake()
    {
		//TODO traverse pskills and create nodes for everything
		root = pSkills.Root;
		workingRoot = (InfusionSkill)root.Copy();

		Generate();

    }

	public override void Show()
	{
		gameObject.SetActive(true);

		workingRoot = (InfusionSkill)root.Copy();

		//UpdateDisplay();

	}
	// Update is called once per frame
	void Update()
    {
		
    }

	public void Generate()
	{
		CurrentSP.text = "" + pSkills.skillPoints;

		currentLayer = 0;

		layers = new List<List<SkillTreeNode>>();
		rootNode = GenerateRecursive(workingRoot, null);

		RearangeNodes();
	}

	//todofixme
	public SkillTreeNode GenerateRecursive(Skill current, SkillTreeNode parent)
	{
		//creates the node for this skill
		GameObject node = Instantiate(SkillTreeNodePrefab, SkillTreeContentAreaObject.transform);

		SkillTreeNode nodeScript = node.GetComponent<SkillTreeNode>();
		nodeScript.Initialize(current, parent, this);

		//increments the current layer
		currentLayer++;

		//adds a new layer if we are on a new layer
		if (currentLayer > layers.Count)
			layers.Add(new List<SkillTreeNode>());
		
		//recursively generates node objects for children
		for(int i = 0; i < current.ChildSkills.Count; i++)
		{
			Skill child = current.ChildSkills[i];
			if (child != null)
			{ 
				//generates node
				nodeScript.childNodes[i] = GenerateRecursive(child, nodeScript);
			}

		}
		//updates the display of the node
		nodeScript.UpdateMyDisplay();
		currentLayer--;

		layers[currentLayer].Add(nodeScript);

		return nodeScript;
	}

	public void GenerateLayers(SkillTreeNode current)
	{
		//increments the current layer
		currentLayer++;

		//adds a new layer if we are on a new layer
		if (currentLayer > layers.Count)
			layers.Add(new List<SkillTreeNode>());

		if (current == null) return;
		//recursively generates node objects for children
		for (int i = 0; i < current.childNodes.Count; i++)
		{
			SkillTreeNode child = current.childNodes[i];
			if (child != null)
			{
				GenerateLayers(child);
			}

		}
		//updates the display of the node
		//nodeScript.UpdateMyDisplay();
		currentLayer--;

		layers[currentLayer].Add(current);

	}

	public void RearangeNodes()
	{
		/* 
		 * set my position to: 
		 * my parent's position
		 * - my parent's padding/2
		 * + the padding of my left-er siblings
		 * 
		 */


		GenerateLayers(rootNode);

		for (int i = 0; i < layers.Count; i++)
		{
			List<SkillTreeNode> layer = layers[i];
			for(int j = 0; j < layer.Count; j++)
			{
				SkillTreeNode node = layer[j];
				SkillTreeNode parent = node.ParentNode;

				float parentX = 0;
				float parentPadding = 0;
				float leftSiblingPadding = 0;
				int indexInSiblings = 1;
				int totalSiblings = 1;
				if (parent != null)
				{
					//parent x and parent padding - used to find left bounds of this branch
					parentX = parent.GetComponent<RectTransform>().localPosition.x;
					parentPadding = parent.GetPadding();

					indexInSiblings = parent.childNodes.IndexOf(node);
					totalSiblings = parent.childNodes.Count;
					if(indexInSiblings > 0)
					{

						List<SkillTreeNode> leftSiblings = parent.childNodes.GetRange(0, indexInSiblings);
						foreach(SkillTreeNode sibling in leftSiblings)
						{
							if(sibling != null)
								leftSiblingPadding += sibling.GetPadding();
						}
					}
				}

				node.GetComponent<RectTransform>().localPosition = new Vector2(
					parentX //-400
					- parentPadding/2 
					+ parentPadding * ((float)indexInSiblings/totalSiblings),//- leftSiblingPadding,
					((i - layers.Count/2) * 10) - i * SkillTreeNodePrefab.GetComponent<SkillTreeNode>().Height * 2);
			}
		}
	}
	
	public void UpdateDisplay()
	{
		//count up number of skill points needed to apply current changes
		//if they are not enough, disable "Apply" button


		SkillTreeNodeIterator treeIterator = new DepthFirstSkillTreeNodeIterator(rootNode);
		while(treeIterator.HasMore())
		{
			SkillTreeNode node = treeIterator.GetNext();

			node.UpdateMyDisplay();
		}

		totalSkillPointCost = 0;
		CheckChanges(root, rootNode);

		ApplyButton.interactable = totalSkillPointCost < pSkills.skillPoints;
		SPCostLabel.text = "" + totalSkillPointCost;

	} 
	
	/// <summary>
	/// checks for any differences between the actual tree and the unapplied tree and displayes
	/// the necessary notifications for skills with changes.
	/// </summary>
	/// <param name="main"></param>
	/// <param name="nodeDisplay"></param>
	public void CheckChanges(Skill main, SkillTreeNode nodeDisplay)
	{
		//neither exists
		if (main == null && (nodeDisplay == null || nodeDisplay.MySkill == null))
		{
			return;
			//nodeDisplay.UnappliedChanges.gameObject.SetActive(false);
		}
		//both exist
		else if (main!= null && nodeDisplay != null && nodeDisplay.MySkill != null)
		{
			bool isEqual = (main.mushrooms.Count == nodeDisplay.MySkill.mushrooms.Count);
			for (int i = 0; i < main.mushrooms.Count; i++)
			{
				bool hasSameMushroomInSlot = main.mushrooms[i] == nodeDisplay.MySkill.mushrooms[i];

				if(main.ChildSkills.Count == nodeDisplay.childNodes.Count)
				{
					CheckChanges(main.ChildSkills[2 * i], nodeDisplay.childNodes[2 * i]);
					CheckChanges(main.ChildSkills[2 * i + 1], nodeDisplay.childNodes[2 * i + 1]);
				}

				if (!hasSameMushroomInSlot)
					totalSkillPointCost++;
				isEqual = isEqual && hasSameMushroomInSlot;
			}
			nodeDisplay.UnappliedChanges.gameObject.SetActive(!isEqual);
		}
		//only one of them exist
		else
		{
			//the display one is the one that exists. and it has one mushroom
			if (nodeDisplay != null && nodeDisplay.MySkill != null && (nodeDisplay.MySkill.mushrooms[0] != Skill.PrimaryMushroom.NONE || nodeDisplay.MySkill.mushrooms[1] != Skill.PrimaryMushroom.NONE))
			{
				totalSkillPointCost++;
				nodeDisplay.UnappliedChanges.gameObject.SetActive(false);
				for (int i = 0; i < nodeDisplay.childNodes.Count; i++)
				{
					CheckChanges(null, nodeDisplay.childNodes[i]);
				}
			}
		}
	}

	public void ResetSkills()
	{
		totalSkillPointCost = 0;
		//resets to main root
		ClearMenu();
		workingRoot = (InfusionSkill)root.Copy();
		Generate();
		UpdateDisplay();
	}

	public void ClearMenu()
	{

		rootNode.DestroyRecursive();
		rootNode = null;

	}

	public void ApplyChanges()
	{
		//counts up the amount of skills
		//count if player has enough skillpoints
		if (totalSkillPointCost > pSkills.skillPoints)
			return;
		pSkills.skillTree = rootNode.MySkill.Copy() ;

		root = (InfusionSkill)pSkills.skillTree;
		workingRoot = (InfusionSkill)root.Copy();

		pSkills.skillPoints -= totalSkillPointCost;
		totalSkillPointCost = 0;
		ResetSkills();
		//UpdateDisplay();
	}
}
