using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantEquipment : MonoBehaviour
{

	CombatantStats stats;
	private Dictionary<EquipmentItem.EquipmentItemType, EquipmentItem> equipment;
	public EquipmentItem Head { get { return equipment[EquipmentItem.EquipmentItemType.HEAD]; } }
	public EquipmentItem Chest { get { return equipment[EquipmentItem.EquipmentItemType.CHEST]; } }
	public EquipmentItem Hands { get { return equipment[EquipmentItem.EquipmentItemType.HANDS]; } }
	public EquipmentItem Legs { get { return equipment[EquipmentItem.EquipmentItemType.LEGS]; } }
	public EquipmentItem Feet { get { return equipment[EquipmentItem.EquipmentItemType.FEET]; } }

	// Start is called before the first frame update
	void Start()
    {
		stats = gameObject.GetComponent<CombatantStats>();
		equipment = new Dictionary<EquipmentItem.EquipmentItemType, EquipmentItem>();
		equipment[EquipmentItem.EquipmentItemType.HEAD] = null;
		equipment[EquipmentItem.EquipmentItemType.CHEST] = null;
		equipment[EquipmentItem.EquipmentItemType.HANDS] = null;
		equipment[EquipmentItem.EquipmentItemType.LEGS] = null;
		equipment[EquipmentItem.EquipmentItemType.FEET] = null;
	}

	public void Equip(EquipmentItem newEquipment)
	{
		//Unequips any old equipment
		Unequip(newEquipment.EquipmentType);

		//equips the new equipment in the correct slot
		equipment[newEquipment.EquipmentType] = newEquipment;

		//adjusts stats
		stats.MaxHealth += newEquipment.Health;
		stats.Defense += newEquipment.Defense;

	}
	public void Unequip(EquipmentItem.EquipmentItemType type)
	{
		if (equipment[type] != null)
		{
			//removes all the stats
			stats.MaxHealth -= equipment[type].Health;
			stats.Defense -= equipment[type].Defense;

			equipment[type] = null;
		}
	}

}
