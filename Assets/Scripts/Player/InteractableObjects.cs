using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjects : MonoBehaviour
{
    //The name of the object that can be interacted with. If it has this script, it is assumed the gameObject can be interacted with 
    public string name;
	private GameObject player;
	private const float DIST_TO_INTERACT = 1.0f;
	private SpriteRenderer bubbleSR;
	public Sprite interactBubble;
	private GameObject bubble;

	// Start is called before the first frame update
	void Start()
    {
		player = GameObject.Find("Player");
		bubble = Instantiate(new GameObject(), gameObject.transform);
		bubbleSR = bubble.AddComponent<SpriteRenderer>();
		bubbleSR.sprite = interactBubble;
		bubble.transform.position = (Vector2)bubble.transform.position + new Vector2(0, .5f);
		bubble.SetActive(false);
		bubbleSR.sortingLayerName = "Popup Bubbles";
    }

    // Update is called once per frame
    void Update()
    {
        if (ScenePersistence.Instance.gamePaused == false)
        {
            if (Vector2.Distance(gameObject.transform.position, player.transform.position) <= DIST_TO_INTERACT)
		    {
		    	bubble.SetActive(true);
		    }
		    else
		    {
		    	bubble.SetActive(false);
		    }
        }
    }
}
