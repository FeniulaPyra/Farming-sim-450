using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedBuff : Buff
{
    //Reference to player movement to alter their speed
    public PlayerMovement movement;

    //Reference to Pet so it can increase its speed if it is one
    public BasicPet pet;

    //Reference to Enemy, so it can increase its speed if it is one
    public BasicEnemy enemy;

    //determining if something is an enemy
    bool isEnemy;

    //Buff text so it can be edited from within the methods
    public TMP_Text buff;

    public SpeedBuff(PlayerMovement m,  BuffType t, TMP_Text b, bool d, float timer, BasicPet p = null)
    {
        movement = m;
        buff = b;
        isEnemy = false;
        isDebuff = d;
        this.timer = timer;
        pet = p;
        type = t;

        if (isDebuff == false)
        {
            IncreaseSpeed();
        }
        else
        {
            DecreaseSpeed();
        }
    }

    public SpeedBuff(BasicEnemy e, BuffType t, float timer)
    {
        enemy = e;
        isEnemy = true;
        this.timer = timer;
        type = t;
    }

    public void IncreaseSpeed()
    {
        if (isEnemy == true)
        {
            //Increase EnemySpeed
            enemy.MovementSpeed *= 2;
        }
        else
        {
            //Increase player and pet speed
            movement.MovementSpeed *= 2;
            if (pet != null)
            {
                pet.MovementSpeed *= 2;
            }

            if (isDebuff == false)
            {
                buff.text += "\nSpeed Increased";
            }
            else
            {
                buff.text = buff.text.Replace("\nSpeed Decreased", "");
            }
        }
    }

    public void DecreaseSpeed()
    {
        if (isEnemy == true)
        {
            //Increase EnemySpeed
            enemy.MovementSpeed /= 2;
        }
        else
        {
            //Increase player and pet speed

            if (isDebuff == false)
            {
                buff.text = buff.text.Replace("\nSpeed Increased", "");
            }
            else
            {
                buff.text += "\nSpeed Decreased";
            }

            movement.MovementSpeed /= 2;
            if (pet != null)
            {
                pet.MovementSpeed /= 2;
            }
        }
    }
}
