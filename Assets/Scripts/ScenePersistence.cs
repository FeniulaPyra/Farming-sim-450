using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersistence : MonoBehaviour
{
    [Header("Save Info")]
    public string fileName;

    [Header("Data to Save")]
    public GameObject player;
    public TimeManager timeManager;
    public PlayerInteraction playerInteraction;
    public TileManager tileManager;
    public FarmManager farmManager;
    public FarmingTutorial farmingTutorial;

    private string constantPath;

    private bool displayLoadMenu;
    private string[] saves;

    //var path;
    string path;
    string originalPath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Saves everything that will need to be loaded in when this scene is loaded later
    /// </summary>
    /// <param name="sceneName">The exact scene name, so each scene can have its own specific file with its own specific information</param>
    /// <param name="sceneType">Type of scene, to only care about certain things; for example, only save farm dictionary when saving the farm scene</param>
    public void SaveEssentials(string sceneName,string sceneType)
    {

    }

    /// <summary>
    /// Saves everything that will need to be loaded in when this scene is loaded later
    /// </summary>
    /// <param name="sceneName">The exact scene name, so each scene can have its own specific file with its own specific information</param>
    /// <param name="sceneType">Type of scene, to only care about certain things; for example, only save farm dictionary when saving the farm scene</param>
    public void LoadEssentials(string sceneName, string sceneType)
    {
        //Definitely needs to account for there being no file with the name of the scene
    }

    /// <summary>
    /// Loads in the new scene, using SaveEssentials and Unity's build in Scene Management
    /// </summary>
    /// <param name="sceneName">Scene's name to pass into SaveEssentials</param>
    /// <param name="sceneType">Scene's type, to pass into Save Essentials</param>
    /// <param name="sceneToLoad">Exact Scene name to load into scene manager</param>
    public void LoadScene(string sceneName, string sceneType, string sceneToLoad)
    {
        //SceneManager.LoadScene
    }
}
