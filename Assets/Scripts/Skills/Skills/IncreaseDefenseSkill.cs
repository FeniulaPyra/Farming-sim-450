using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseDefenseSkill : Skill
{
	public IncreaseDefenseSkill(PrimaryMushroom a, PrimaryMushroom b) : base(a, b)
	{
		parentShroom = PrimaryMushroom.WHITE;
		description = "Player base defense is increased in certain situations.";
		mushroomDescriptions[PrimaryMushroom.RED] =    "Base Defense is increased against enemies you are facing towards.";
		mushroomDescriptions[PrimaryMushroom.YELLOW] = "Base Defense is increased while wielding a melee weapon.";
		mushroomDescriptions[PrimaryMushroom.BLUE] =   "Base Defense is increased against enemies you are facing away from.";
		mushroomDescriptions[PrimaryMushroom.WHITE] =  "Base Defense is increased while wielding a ranged weapon.";
		mushroomDescriptions[PrimaryMushroom.BLACK] =  "Base Defense is increased while not holding a weapon.";
	}

	/// <summary>
	/// Returns the total boost from this skill.
	/// </summary>
	/// <param name="parameters">Should include the player gameobject [0] and the relevant enemy gameobject [1].</param>
	/// <returns>returns the percent boost to defense</returns>
	new public float Apply(List<GameObject> playerAndEnemy)
	{
		return base.Apply(parameters);
	}

	protected override float Red(List<GameObject> parameters)
	{
		PlayerMovement pmove = parameters[0].GetComponent<PlayerMovement>();
		if (pmove == null)
			throw new System.Exception("No player movement attached.");

		BasicEnemy enemy = parameters[1].GetComponent<BasicEnemy>();
		if (enemy == null)
			throw new System.Exception("No Enemy attached.");

		Vector2 closestColliderPoint = enemy.transform.position;

		//NOTE! getcomponent<playermovement> should be changed to include enemies as well if we want enemies to have weapons.
		float angle = Vector2.Angle(pmove.Facing, pmove.transform.position - (Vector3)closestColliderPoint);

		//if reachable, damage the enemy.
		if (Mathf.Abs(angle) < 45)
			return .1f;
		return 0;
	}
	protected override float Yellow(List<GameObject> parameters)
	{
		PlayerInventoryManager pinv = parameters[0].GetComponent<PlayerInventoryManager>();
		if (pinv == null)
			throw new System.Exception("No player inventory attached.");
		if (pinv.heldItem is MeleeWeaponItem)
			return .1f;
		return 0;
	}
	protected override float Blue(List<GameObject> parameters)
	{
		PlayerMovement pmove = parameters[0].GetComponent<PlayerMovement>();
		if (pmove == null)
			throw new System.Exception("No player movement attached.");

		BasicEnemy enemy = parameters[1].GetComponent<BasicEnemy>();
		if (enemy == null)
			throw new System.Exception("No Enemy attached.");

		Vector2 closestColliderPoint = enemy.transform.position;

		//NOTE! getcomponent<playermovement> should be changed to include enemies as well if we want enemies to have weapons.
		float angle = Vector2.Angle(pmove.Facing, pmove.transform.position - (Vector3)closestColliderPoint);

		//if reachable, damage the enemy.
		if (Mathf.Abs(angle) > 45)
			return .1f;
		return 0;
	}
	protected override float White(List<GameObject> parameters)
	{
		PlayerInventoryManager pinv = parameters[0].GetComponent<PlayerInventoryManager>();
		if (pinv == null)
			throw new System.Exception("No player inventory attached.");
		if (pinv.heldItem is RangedWeaponItem)
			return .1f;
		return 0;
	}
	protected override float Black(List<GameObject> parameters)
	{
		PlayerInventoryManager pinv = parameters[0].GetComponent<PlayerInventoryManager>();
		if (pinv == null)
			throw new System.Exception("No player inventory attached.");
		if (!(pinv.heldItem is WeaponItem))
			return .1f;
		return 0;
	}
}
