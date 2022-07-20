using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldDisplay : MonoBehaviour
{
    //The TMP Text for displaying Gold
    [SerializeField]
    TMP_Text goldDisplay;

    //PlayerInteraction, which holds the gold amount
    [SerializeField]
    PlayerInteraction interaction;

    private void Awake()
    {
        interaction = FindObjectOfType<PlayerInteraction>();

        goldDisplay = gameObject.GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        goldDisplay.text = $"{interaction.playerGold} G";
    }
}
