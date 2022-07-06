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
        defense
    }

    public BuffType type;
}
