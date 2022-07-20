using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegenBuff
{
    //Reference to player or enemy to alter their health
    public CombatantStats stats;

    public int iterations = 0;
    public int maxIterations;
    public float timer;
    public float baseTimer;
    //public int testHealth;
    public int factor;

    public RegenBuff(CombatantStats s, int i, float t, float b, int f)
    {
        stats = s;
        maxIterations = i;
        baseTimer = b;
        timer = t;
        factor = f;
    }
}
