using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegenBuff
{
    //Reference to player or enemy to alter their health
    public CombatantStats stats;

    public int healIterations;
    public float healTimer;
    public float baseHealTimer;
    //public int testHealth;
    public int healFactor;

    public RegenBuff(CombatantStats s, int i, float t, float b, int f)
    {
        stats = s;
        healIterations = i;
        baseHealTimer = b;
        healTimer = t;
        healFactor = f;
    }
}
