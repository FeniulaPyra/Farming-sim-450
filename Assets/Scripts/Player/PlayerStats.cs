using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerStats 
{
	Dictionary<Item, int> itemsCollected;
	Dictionary<Item, int> mealsCooked;
	
	#region stats
	public int maxhealth;
	public int healthLost;
	public int healthGained;
	public int healthGainedFromSleep;
	 
	public int maxStamina;
	public int staminaSpent;
	public int staminaGained;
	public int staminaGainedFromSleep;
	#endregion
	
	#region Farming Stats
	public int tilesTilled;
	public int mushroomsWatered;
	public int mushroomsSickled;
	public int mushroomsPlanted;
	public int mushroomsSpread;
	public int mushroomsHybridized;
	public int itemsDropped;
	#endregion
	
	#region World Stats
	public int furnitureBroke;
	public int talkedToNPC;
	public int attackedEnemies;
	public int enemiesKilled;
	public int changedMaps;
	public int dungeonsvisited;
	public int cropsPlantedInDungeons;
	#endregion
}
