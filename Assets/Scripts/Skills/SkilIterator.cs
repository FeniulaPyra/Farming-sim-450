using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillIterator
{
	// Start is called before the first frame update
	protected Skill root;

	protected Queue<Skill> cache;

	public SkillIterator(Skill root)
	{
		cache = new Queue<Skill>();
		this.root = root;
	}

	public Skill GetNext()
	{
		if (HasMore())
		{
			return cache.Dequeue(); ;
		}
		return default;
	}

	public bool HasMore()
	{
		return cache.Count > 0;//cache.Peek() == null;
	}
	protected abstract void GenerateCache(Skill element);
	public abstract void Reset();
}

public class DepthFirstSkillIterator : SkillIterator
{
	public DepthFirstSkillIterator(Skill root) : base(root)
	{
		GenerateCache(root);
	}

	protected override void GenerateCache(Skill element)
	{
		cache.Enqueue(element);
		foreach (Skill child in element.ChildSkills)
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
