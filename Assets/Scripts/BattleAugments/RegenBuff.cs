using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegenBuff : Buff
{
    //Reference to player or enemy to alter their health
    public CombatantStats stats;

    public float baseTimer;
    public int factor;

    public RegenBuff(CombatantStats s, BuffType t, int i, float b, int f)
    {
        stats = s;
        maxIterations = i;
        baseTimer = b;
        effectTimer = baseTimer;
        factor = f;
        type = t;
    }
}
