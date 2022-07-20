using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://refactoring.guru/design-patterns/iterator
public abstract class SkillTreeNodeIterator
{
	protected SkillTreeNode root;

	protected Queue<SkillTreeNode> cache;

	public SkillTreeNodeIterator(SkillTreeNode root)
	{
		cache = new Queue<SkillTreeNode>();
		this.root = root;
	}

	public SkillTreeNode GetNext()
	{
		if (HasMore())
		{
			return cache.Dequeue(); ;
		}
		return default;
	}

	public bool HasMore()
	{
		
		return cache.Count > 0;
	}
	protected abstract void GenerateCache(SkillTreeNode element);
	public abstract void Reset();
}

public class DepthFirstSkillTreeNodeIterator : SkillTreeNodeIterator
{
	public DepthFirstSkillTreeNodeIterator(SkillTreeNode root) : base(root)
	{
		GenerateCache(root);
	}

	protected override void GenerateCache(SkillTreeNode element)
	{
		if (element == null) return;
		cache.Enqueue(element);
		foreach (SkillTreeNode child in element.childNodes)
		{
			if(child != null)	
				GenerateCache(child);
		}
	}
	public override void Reset()
	{
		GenerateCache(root);
	}
}

