using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentItem : Item
{
	private int defense;
	public int Defense { get { return defense; } }

	private int health;
	public int Health { get { return health; } }

	private EquipmentItemType equipmentType;
	public EquipmentItemType EquipmentType { get { return equipmentType; } }

	public enum EquipmentItemType
	{
		HEAD,
		CHEST,
		HANDS,
		LEGS,
		FEET
	}
}
