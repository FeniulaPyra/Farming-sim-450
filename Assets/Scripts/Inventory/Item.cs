using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	public string name;
	public Sprite spr;

	public GameObject itemObj;
	public FarmManager manager;
	public GameObject player;

    public bool isSellable;
    public int sellValue;

    public bool isEdible;
    public int staminaToRestore;

    public int staminaUsed;

	public bool rare;

	public void Use(Tile i)
	{

	}
	public void Use(BasicEntity e)
	{

	}

	public void Use(Player p)
	{

	}

	public void Consume(Player p)
	{

	}

}
