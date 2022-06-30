using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : Item
{
	[SerializeField]
	private int defense;
	public int Defense { get { return defense; } }

	[SerializeField]
	private int health;
	public int Health { get { return health; } }

	[SerializeField]
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
