using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitioner : MonoBehaviour
{
    //The gameobject it's on
    [SerializeField]
    GameObject self;
    //The player
    [SerializeField]
    GameObject player;
    //A reference to the scene transition manager so it can call its functions
    [SerializeField]
    SceneTransitionManager manager;

	[SerializeField]
	int transitionRange = 1;

    //The level it loads
    public string levelToLoad;

	public Vector2 playerPositionInScene;

    // Start is called before the first frame update
    void Start()
    {
        self = gameObject;
        player = GameObject.Find("Player");
        manager = GameObject.FindObjectOfType<SceneTransitionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(self.transform.position, player.transform.position);
        Debug.Log($"Distance beween object and player is: {distance}");
        if (distance <= transitionRange)
        {
			GameObject.FindObjectOfType<Menu>().ShowLoadingScreen();
            manager.LoadScene(levelToLoad, playerPositionInScene);
        }
    }
}
