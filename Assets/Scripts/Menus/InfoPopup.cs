using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPopup : MonoBehaviour
{
	public GameObject toShow; 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	/*private void OnMouseOver()
	{
		Debug.Log("HOVERING OVER INFO");
		toShow.SetActive(true);
	}*/
	public void HidePanel()
	{
		toShow.SetActive(false);
	}
	public void ShowPanel()
	{
		toShow.SetActive(true);
	}
}
