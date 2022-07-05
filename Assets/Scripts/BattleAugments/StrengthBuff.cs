using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StrengthBuff
{
    //Reference to player or enemy to alter their strength
    public CombatantStats stats;

    //Buff text so it can be edited from within the methods
    public TMP_Text buff;

    //For Player
    public StrengthBuff(CombatantStats s, TMP_Text b)
    {
        stats = s;
        buff = b;
    }
    //For enemy
    public StrengthBuff(CombatantStats s)
    {
        stats = s;
    }

    public void IncreaseStrength()
    {
        stats.Strength += 10;
        if (buff != null)
        {
            buff.text += "\nStrength Increased";
        }
        Debug.Log($"Strength Mod: {stats.Strength}");
    }

    public void DecreaseStrength()
    {
        if (buff != null)
        {
            buff.text = buff.text.Replace("\nDefense Increased", "");
        }
        stats.ResetDefense();
    }
}
