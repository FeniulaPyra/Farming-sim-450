using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponItem : WeaponItem
{
	//degree angle from player facing direction. Will determine what enemies
	//get hit and which don't. Angle is NOT centered around player facing vector,
	//but is instead applied to both sides of the player facing direction
	[SerializeField]
	private int angle;
	public int Angle { get { return angle; } }

	[SerializeField]
	private int reach;
	public int Reach { get { return reach; } }

	//for instance, if the weapon has a hit angle of 15 degrees, 
	//it would look like this:              NOT this:
	//         \ <15deg								
	//       ---)                            ___\ <7.5deg
	//         / <15deg                         / <7.5deg


	public override void Attack(GameObject origin, GameObject target)
	{

		//Collider2D col = target.gameObject.GetComponent<Collider2D>();
		Vector2 closestColliderPoint = target.transform.position;//col.ClosestPoint(origin.transform.position);
		//Sees how close the player is to them
		float distance = Vector2.Distance(origin.transform.position, closestColliderPoint);
		//NOTE! getcomponent<playermovement> should be changed to include enemies as well if we want enemies to have weapons.
		float angle = Vector2.Angle(origin.GetComponent<PlayerMovement>().Facing, origin.transform.position - (Vector3)closestColliderPoint);

		//if reachable, damage the enemy.
		if (distance < reach && Mathf.Abs(angle) < this.angle)
			target.GetComponent<CombatantStats>().TakeDamage(Mathf.CeilToInt(this.Strength + target.GetComponent<CombatantStats>().Strength));
	}
}
