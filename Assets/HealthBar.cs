using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //The actual slider that controls the health
    public Slider slider;
    //The player's combatant stats script to access their health 
    public CombatantStats player;

    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
        //player = GameObject.Find("Player").GetComponent<CombatantStats>();

        //slider.maxValue = player.MaxHealth;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
