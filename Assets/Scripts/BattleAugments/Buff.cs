using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Buff
{
    //Buff text so it can be edited from within the methods
    protected TMP_Text buff;

    //For debuffs
    [SerializeField]
    protected bool isDebuff;
    public bool IsDebuff
    {
        get
        {
            return isDebuff;
        }
    }

    //Checks to see if it's already be added to strength or defense
    public bool added;

    //the actual stat modifier
    [SerializeField]
    protected int mod;
    public int Mod
    {
        get
        {
            return mod;
        }
    }

    public enum BuffType
    {
        offense,
        defense,
        speed,
        regen,
        poison
    }

    public BuffType type;

    //How long a buff lasts
    public float timer;

    //For regen and poison, so that they aren't reset with each cast
    //How long until the next iteration of poison or regen happens
    public float effectTimer;
    public int iterations = 0;
    public int maxIterations;
}
