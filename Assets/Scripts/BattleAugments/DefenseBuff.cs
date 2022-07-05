using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DefenseBuff
{
    //Reference to player or enemy to alter their defense
    public CombatantStats stats;

    //Buff text so it can be edited from within the methods
    public TMP_Text buff;

    public DefenseBuff(CombatantStats s, TMP_Text b)
    {
        stats = s;
        buff = b;
    }
    public DefenseBuff(CombatantStats s)
    {
        stats = s;
    }

    public void IncreaseDefense()
    {
        stats.Defense += 10;
        if (buff != null)
        {
            buff.text += "\nDefense Increased";
        }
        Debug.Log($"Defense Mod: {stats.Defense}");
    }

    public void DecreaseDefense()
    {
        if (buff != null)
        {
            buff.text = buff.text.Replace("\nStrength Increased", "");
        }
        stats.ResetStrength();
    }
}
