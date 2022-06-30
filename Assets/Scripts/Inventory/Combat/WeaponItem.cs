using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponItem : Item
{
	[SerializeField]
	public int strength;
	public int Strength { get { return strength; } }
	abstract public void Attack(GameObject origin, GameObject target); 
}
