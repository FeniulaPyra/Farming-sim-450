using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DisplayMaxStamina : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text staminaDisplay;

// Start is called before the first frame update
void Start()
    {

    }

    //As long as the player is hovering over the stamina symbol, they can see exactly how much stamina they have
    /*public void OnPointerEnter(PointerEventData pointer)
    {
        staminaDisplay.gameObject.SetActive(true);
        Debug.Log("Pointer Enter");
    }

    public void OnPointerExit(PointerEventData pointer)
    {
        staminaDisplay.gameObject.SetActive(false);
        Debug.Log("Pointer Exit");
    }*/
}
