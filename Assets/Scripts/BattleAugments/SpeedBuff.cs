using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedBuff
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

    public SpeedBuff(PlayerMovement m, TMP_Text b, BasicPet p)
    {
        movement = m;
        buff = b;
        pet = p;
        isEnemy = false;
    }

    public SpeedBuff(BasicEnemy e)
    {
        enemy = e;
        isEnemy = true;
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
            pet.MovementSpeed *= 2;
            if (buff != null)
            {
                buff.text += "\nSpeed Increased";
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
            if (buff != null)
            {
                buff.text = buff.text.Replace("\nSpeed Increased", "");
            }
            movement.MovementSpeed /= 2;
            pet.MovementSpeed /= 2;
        }
    }
}
