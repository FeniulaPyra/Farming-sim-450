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

	public EquipmentItem this[EquipmentItem.EquipmentItemType type]
	{
		get
		{
			return equipment[type];
		}
	}

	// Start is called before the first frame update
	void Awake()
    {
		//When loading a game/changing scenes, loading of equipment should be done on the object 
		// that the equipment is attached to, the same way inventories are. 
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

	public List<string> GetSaveableEquipment()
	{
		List<string> saveableEquipment = new List<string>();
		foreach(KeyValuePair<EquipmentItem.EquipmentItemType, EquipmentItem> item in equipment)
		{
			EquipmentItem equipmentItem = item.Value;
			if(equipmentItem != null)
				saveableEquipment.Add(item.Value.name);
		}
		return saveableEquipment;
	}
	public void SetSaveableEquipment(List<string> savedEquipment, ItemManager im)
	{
		equipment[EquipmentItem.EquipmentItemType.HEAD] = null;
		equipment[EquipmentItem.EquipmentItemType.CHEST] = null;
		equipment[EquipmentItem.EquipmentItemType.HANDS] = null;
		equipment[EquipmentItem.EquipmentItemType.LEGS] = null;
		equipment[EquipmentItem.EquipmentItemType.FEET] = null;
		foreach (string equipmentName in savedEquipment)
		{
			EquipmentItem item = (EquipmentItem)im.GetItemByName(equipmentName);
			Equip(item);
		}
	}

}
