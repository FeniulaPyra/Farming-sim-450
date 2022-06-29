using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponItem : Item
{
	public int strength { get; }
	abstract public void Attack(GameObject origin, GameObject target); 
}
