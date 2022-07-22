using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StrengthBuff : Buff
{
    //Constructor; actually sets the buff to augment the stat
    public StrengthBuff(TMP_Text b, int m, bool d, BuffType t, float timer)
    {
        buff = b;
        mod = m;
        isDebuff = d;
        type = t;
        this.timer = timer;

        if (isDebuff == false)
        {
            buff.text += "\nStrength Increased";
        }
        else
        {
            buff.text += "\nStrength Decreased";
        }

        //If a debuff somehow gets a positive number, or if a buff somehow gets a negative number
        if (isDebuff == true && mod > 0)
        {
            mod *= -1;
        }
        else if (isDebuff == false && mod < 0)
        {
            mod *= -1;
        }
    }

    public StrengthBuff(int m, bool d, BuffType t, float timer)
    {
        mod = m;
        isDebuff = d;
        type = t;
        this.timer = timer;

        //If a debuff somehow gets a positive number, or if a buff somehow gets a negative number
        if (isDebuff == true && mod > 0)
        {
            mod *= -1;
        }
        else if (isDebuff == false && mod < 0)
        {
            mod *= -1;
        }
    }
}
