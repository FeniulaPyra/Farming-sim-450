using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class StatsPanel : MonoBehaviour
{
	public GameObject Combatant;
	Sprite CombatantSprite;
	CombatantStats stats;
	CombatantEquipment equipment;

	public InventoryItemGrabber ItemGrabber;

	public Image CombatantDisplay;

	public Text LevelValueText;
	public Text HPValueText;
	public Text MaxHPValueText;
	public Text StrengthValueText;
	public Text DefenseValueText;


	public Image HelmetButton;
	public Image ChestButton;
	public Image LeftHandButton;
	public Image RightHandButton;
	public Image LegsButton;
	public Image FeetButton;


    // Start is called before the first frame update
    void Start()
    {
		if(Combatant != null)
			SetCombatantToDisplay(Combatant);
    }

	public void SetCombatantToDisplay(GameObject newCombatant)
	{
		Combatant = newCombatant;
		if (newCombatant.name.ToLower().Contains("player")) //why on earth was the player set up like this T.T
			CombatantSprite = Combatant.GetComponentsInChildren<SpriteRenderer>()[1].sprite;
		else
			CombatantSprite = Combatant.GetComponent<SpriteRenderer>().sprite;
		equipment = Combatant.GetComponent<CombatantEquipment>();
		stats = Combatant.GetComponent<CombatantStats>();
	}
	

    // Update is called once per frame
    void Update()
    {
        if (Combatant == null) return;
        CombatantDisplay.sprite = CombatantSprite;

        HelmetButton.sprite = equipment.Head != null ? equipment.Head.spr : null;
        ChestButton.sprite = equipment.Chest != null ? equipment.Chest.spr : null;
        LeftHandButton.sprite = equipment.Hands != null ? equipment.Hands.spr : null;
        RightHandButton.sprite = equipment.Hands != null ? equipment.Hands.spr : null;
        LegsButton.sprite = equipment.Legs != null ? equipment.Legs.spr : null;
        FeetButton.sprite = equipment.Feet != null ? equipment.Feet.spr : null;

        HelmetButton.color = equipment.Head != null ? Color.white : Color.clear;
        ChestButton.color = equipment.Chest != null ? Color.white : Color.clear;
        LeftHandButton.color = equipment.Hands != null ? Color.white : Color.clear;
        RightHandButton.color = equipment.Hands != null ? Color.white : Color.clear;
        LegsButton.color = equipment.Legs != null ? Color.white : Color.clear;
        FeetButton.color = equipment.Feet != null ? Color.white : Color.clear;

        LevelValueText.text = "" + stats.Level;
        HPValueText.text = "" + stats.Health;
        MaxHPValueText.text = "" + stats.MaxHealth;
        StrengthValueText.text = "" + stats.Strength;
        DefenseValueText.text = "" + stats.Defense;

        /*if (Keyboard.current.mKey.wasPressedThisFrame == true)
        {
            DefenseValueText.text = "" + stats.Defense;
        }*/
    }

	public void EquipItem(EquipmentItem.EquipmentItemType equipmentType)
	{
		if (ItemGrabber.item != null && ItemGrabber.item is EquipmentItem && equipment[((EquipmentItem)ItemGrabber.item).EquipmentType] == null)
		{
			equipment.Equip((EquipmentItem)ItemGrabber.item);
			ItemGrabber.amount -= 1;
			if(ItemGrabber.amount < 1)
				ItemGrabber.item = null;
		}
		else if (ItemGrabber.item == null && equipment[equipmentType] != null)
		{
			ItemGrabber.item = equipment[equipmentType];
			ItemGrabber.amount = 1;
			equipment.Unequip(equipmentType);
		}
	}

	public void EquipHead() { EquipItem(EquipmentItem.EquipmentItemType.HEAD); }
	public void EquipChest() { EquipItem(EquipmentItem.EquipmentItemType.CHEST); }
	public void EquipHands() { EquipItem(EquipmentItem.EquipmentItemType.HANDS); }
	public void EquipLegs() { EquipItem(EquipmentItem.EquipmentItemType.LEGS); }
	public void EquipFeet() { EquipItem(EquipmentItem.EquipmentItemType.FEET); }
}
