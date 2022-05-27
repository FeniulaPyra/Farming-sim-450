using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class SceneTransitionManager : MonoBehaviour
{
    public GameObject player;//The player themselves
    public TimeManager timeManager;//The date displayed in the top right
    public PlayerInteraction playerInteraction;//The player's stamina and how much money they have
    public TileManager tileManager;//Theplayer's field
    public FarmManager farmManager;//Mushrooms
    public FarmingTutorial farmingTutorial;//The state of the tutorial. Shouldn't be an issue
    public EntityManager entityManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        timeManager = FindObjectOfType<TimeManager>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();
        tileManager = FindObjectOfType<TileManager>();
        farmManager = FindObjectOfType<FarmManager>();
        farmingTutorial = FindObjectOfType<FarmingTutorial>();
        entityManager = FindObjectOfType<EntityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //LoadScene("FungusTestScene");
            LoadScene("GroundScene 1");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            LoadScene("GroundScene");
        }
    }

    public void LoadScene(string sceneToLoad)
    {
        //Disables this boolean so the scene to load doesn't load the save data
        GlobalGameSaving.Instance.loadingSave = false;

        timeManager.SaveDate("persist");
        timeManager.SaveNPCs("persist");
        
        entityManager.SaveEntities("persist");

        farmManager.SaveInventory("persist");

        playerInteraction.SavePlayer("persist");

        tileManager.SaveFarm("persist");

        SceneManager.LoadScene(sceneToLoad);
    }

    public void SavePlayer()
    {

        /*ScenePersistence.Instance.date = new Vector4(timeManager.DayNumber, timeManager.DateNumber, timeManager.YearNumber, timeManager.SeasonNumber);
        ScenePersistence.Instance.stamina = playerInteraction.playerStamina;
        ScenePersistence.Instance.gold = playerInteraction.playerGold;
        ScenePersistence.Instance.inventory = farmManager.playerInventory.GetSaveableInventory();*/

        //persist.date = new Vector4(timeManager.DayNumber, timeManager.DateNumber, timeManager.YearNumber, timeManager.SeasonNumber);
        //persist.stamina = playerInteraction.playerStamina;
        //persist.gold = playerInteraction.playerGold;
        //persist.inventory = farmManager.playerInventory.GetSaveableInventory();
    }

    /*[Header("Save Info")]
    public string fileName;

    [Header("Data to Save")]
    public GameObject player;//The player themselves
    public TimeManager timeManager;//The date displayed in the top right
    public PlayerInteraction playerInteraction;//The player's stamina and how much money they have
    public TileManager tileManager;//Theplayer's field
    public FarmManager farmManager;//Mushrooms
    public FarmingTutorial farmingTutorial;//The state of the tutorial. Shouldn't be an issue

    private string constantPath;//The path to where the files are

    //var path;
    string path; //Changes per file
    string originalPath; //The constant part of the path that all files share

    // Start is called before the first frame update
    void Start()
    {
        constantPath = Application.persistentDataPath;
        originalPath = constantPath + "/scenesaves/";
        path = originalPath;

        string sceneName = SceneManager.GetActiveScene().name;
        string filePath = path + sceneName;

        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);

            var json = sr.ReadLine();
            var persist = JsonUtility.FromJson<PersistentScene>(json);

            LoadEssentials(sceneName, persist.sceneType);

            sr.Close();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            LoadScene(SceneManager.GetActiveScene().name, "farm", "FungusTestScene");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            LoadScene(SceneManager.GetActiveScene().name, "farm", "GroundScene");
        }
    }

    /// <summary>
    /// Saves everything that will need to be loaded in when this scene is loaded later
    /// </summary>
    /// <param name="sceneName">The exact scene name, so each scene can have its own specific file with its own specific information</param>
    /// <param name="sceneType">Type of scene, to only care about certain things; for example, only save farm dictionary when saving the farm scene</param>
    public void SaveEssentials(string sceneName,string sceneType)
    {
        if (Directory.Exists(originalPath) == false)
        {
            Directory.CreateDirectory(originalPath);
        }

        //string name = SceneManager.GetActiveScene().name;

        string name = sceneName;

        path += name;

        Debug.Log("Saving scene to " + path);

        PersistentScene persist = new PersistentScene();

        persist.sceneType = sceneType;
        persist.date = new Vector4(timeManager.DayNumber, timeManager.DateNumber, timeManager.YearNumber, timeManager.SeasonNumber);
        persist.stamina = playerInteraction.playerStamina;
        persist.gold = playerInteraction.playerGold;
        persist.inventory = farmManager.playerInventory.GetSaveableInventory();


        //The very end of the method
        var json = JsonUtility.ToJson(persist);

        StreamWriter sw = new StreamWriter(path);
        sw.WriteLine(json);
        sw.Close();
    }

    /// <summary>
    /// Saves everything that will need to be loaded in when this scene is loaded later
    /// </summary>
    /// <param name="sceneName">The exact scene name, so each scene can have its own specific file with its own specific information</param>
    /// <param name="sceneType">Type of scene, to only care about certain things; for example, only save farm dictionary when saving the farm scene</param>
    /// <param name="previousScene">Name of the previous scene, so permenantly consistent thing, like amount of gold, cna be properly set</param>
    public void LoadEssentials(string sceneName, string sceneType, string previousScene)
    {
        //Definitely needs to account for there being no file with the name of the scene

        string filePath = path + sceneName;
        StreamReader sr = new StreamReader(filePath);

        var json = sr.ReadLine();
        var persist = JsonUtility.FromJson<PersistentScene>(json);

        //Code goes here
        timeManager.SetDate((int)persist.date.x, (int)persist.date.y, (int)persist.date.z, (int)persist.date.w);
        playerInteraction.SetStamina((int)persist.stamina);
        playerInteraction.playerGold = persist.gold;
        GameObject.Find("GoldDisplay").GetComponent<TMP_Text>().text = $"{playerInteraction.playerGold} G";
        farmManager.playerInventory.SetSaveableInventory(persist.inventory);

        sr.Close();
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
        SaveEssentials(sceneName, sceneType);
        SceneManager.LoadScene(sceneToLoad);
    }*/
}

public class PersistentScene
{
    //Always needs to be saved
    public string sceneType; //Type of scene to be saved, will be used for loading purposes
    public Vector4 date; //The present in game date
    public int stamina; //How much stamina the player has
    public int gold; //How much gold the player has
    public List<int> inventory; //The player's inventory

    //Only relevant when saving/loading farm
    public List<SaveTile> farmTiles; //The tiles of the player's field
    public List<MushroomSaveTile> mushrooms; //The mushrooms in the player's field
    public int farmNetWorth; //Net worth of the player's farm

    //Other
    public List<SaveStartChart> NPCStartflowcharts = new List<SaveStartChart>();//Fungus Flowcharts
    public List<SaveQuestChart> NPCQuestflowcharts = new List<SaveQuestChart>();//Fungus Quest Flowcharts
    public List<SaveQuest> NPCQuests = new List<SaveQuest>();//Quest Scripts
    public List<SaveEntity> allEntities = new List<SaveEntity>(); //all entities in scene
    public List<SaveEntity> entities = new List<SaveEntity>(); //all non pet entities that are in the list of all entities
    public List<string> entityNames = new List<string>();
    public List<SavePet> pets = new List<SavePet>(); //all pets that are in the list of all entities
    public List<string> petNames = new List<string>();

    //Currently unused
    public bool isNight; //Whether or not it's night; currently unused

    //Probably Unnecessary
    public List<bool> tutorialBools = new List<bool>(); //The state of the tutorial; probably unneeded
    public string tutorialObjective; //What the player currently needs to do in the tutorial; probably unneeded
    public Vector3 position;//Player's position; probably unneeded
    //public List<NPCManager> NPCs = new List<NPCManager>();
}
