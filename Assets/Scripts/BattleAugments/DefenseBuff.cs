using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DefenseBuff : Buff
{
    //Constructor; actually sets the buff to augment the stat
    public DefenseBuff(TMP_Text b, int m, bool d, BuffType t)
    {
        buff = b;
        mod = m;
        isDebuff = d;
        type = t;

        if (isDebuff == false)
        {
            buff.text += "\nDefense Increased";
        }
        else
        {
            buff.text += "\nDefense Decreased";
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

    public DefenseBuff(int m, bool d, BuffType t)
    {
        mod = m;
        isDebuff = d;
        type = t;

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
