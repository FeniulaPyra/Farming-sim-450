using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GlobalGameSaving : MonoBehaviour
{
    public static GlobalGameSaving Instance;

    #region All variables from the top of the original game save manager
    [Header("Save Info")]
    public string saveName; //used for saving file

    [Header("Data to Save")]
    public GameObject player;//Will manually save movement speed (buffs) and positions (placement after loading)
    public TimeManager timeManager;//Timemanager already has save functions
    public PlayerInteraction playerInteraction;//Playerinteraction already has save functions
    public PlayerSkills skillManager;//Playerinteraction already has save functions\
	public PlayerMovement playerMovement;
    public TileManager tileManager;//Tilemanager already has save functions
    public FarmManager farmManager;//FarmManager already has save functions
    public EntityManager entityManager;
	public PlayerInventoryManager invManager;
    //public FarmingTutorial farmingTutorial;//Will be manually done in farming tutorial

    private string constantPath;

    private bool displayLoadMenu;
    private string[] saves;

    //var path;
    string path;
    string originalPath;

    [SerializeField]
    Flowchart flowchart;
    #endregion

    #region All variables from the original save manager's GameSave class
    public string sceneName;
    public Vector3 position;
    public bool isNight;
    public Vector4 date;
    public int stamina;
    public int gold;
    public List<SaveTile> farmTiles;
    public List<MushroomSaveTile> mushrooms;
    public List<int> inventory;
    public List<bool> tutorialBools;// = new List<bool>();
    public string tutorialObjective;
    //public List<NPCManager> NPCs = new List<NPCManager>();
    public int farmNetWorth;
    public List<SaveStartChart> NPCStartflowcharts = new List<SaveStartChart>();//Fungus Flowcharts
    public List<SaveQuestChart> NPCQuestflowcharts = new List<SaveQuestChart>();//Fungus Quest Flowcharts
    public List<SaveQuest> NPCQuests = new List<SaveQuest>();//Quest Scripts
    public List<string> NPCNames = new List<string>();//The names of all NPCs that this has

    public List<SaveEntity> allEntities = new List<SaveEntity>(); //all entities in scene
    public List<SaveEntity> entities = new List<SaveEntity>(); //all non pet entities that are in the list of all entities
    public List<string> entityNames = new List<string>();
    public List<SavePet> pets = new List<SavePet>(); //all pets that are in the list of all entities
    public List<string> petNames = new List<string>();
    public List<SaveLivestockPet> livestockPets = new List<SaveLivestockPet>();
    public List<string> livestockPetNames = new List<string>();
    public List<SaveBuffPet> buffPets = new List<SaveBuffPet>();
    public List<string> buffPetNames = new List<string>();
	public List<SaveInventoryEntity> inventoryEntities = new List<SaveInventoryEntity>();
	public List<string> inventoryEntityNames = new List<string>();

	public List<string> playerEquipment = new List<string>();

	public List<string> mapsVisited = new List<string>();

	public string skills;

    //public Inventory shippingInv;
    public List<int> shippingInv;
    public ShippingBin bin;
    #endregion

    //boolean to let other scripts know to load a save
    public bool loadingSave;

    private void Awake()
    {
        //Makes sure this is the only instance in existence by destroying any others that aren't it
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        tutorialBools = new List<bool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        constantPath = Application.persistentDataPath;

        flowchart = GameObject.Find("SaveFlowchart").GetComponent<Flowchart>();

        originalPath = constantPath + "/saves/";
        path = originalPath;

        player = GameObject.Find("Player");
		invManager = player.GetComponent<PlayerInventoryManager>();
		skillManager = player.GetComponent<PlayerSkills>();
		playerMovement = player.GetComponent<PlayerMovement>();
        timeManager = FindObjectOfType<TimeManager>();
        tileManager = FindObjectOfType<TileManager>();
        farmManager = FindObjectOfType<FarmManager>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();
        entityManager = FindObjectOfType<EntityManager>();

        bin = FindObjectOfType<ShippingBin>();

        if (loadingSave == true)
        {
            player.transform.position = position;
            timeManager.isNight = isNight;
            //timeManager.netWorth.FarmNetWorth = farmNetWorth;
        }
    }

    void OnManualSave()
    {
        if (playerInteraction.CanInteract == true)
        {
            string name = $"{saveName} -on- {timeManager.SeasonNumber}.{timeManager.DateNumber}.{timeManager.YearNumber} -with- {playerInteraction.PlayerStamina} stamina";

            path += name;

            Debug.Log($"Saved game to {path}");

            if (File.Exists(path) == true)
            {
                Debug.Log("Save does exist");
                flowchart.SetStringVariable("SaveName", name);
                flowchart.ExecuteBlock("Start");
            }
            else
            {
                Debug.Log("Save does not exist");
                //SaveGame(saveName + "-at-" + timeManager.SeasonNumber + "." + timeManager.DateNumber + "." + timeManager.YearNumber);
                SaveGame(name);
            }

            path = originalPath;
        }
    }

    void OnManualLoad()
    {
        /*if (playerInteraction.CanInteract == true)
        {
            displayLoadMenu = true;
            saves = FindAllSaves();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInteraction != null)
        {
            if (playerInteraction.CanInteract == true)
            {
                if (Keyboard.current.pKey.wasPressedThisFrame)
                {
                    displayLoadMenu = true;
                    saves = FindAllSaves();
                }
                if (Keyboard.current.oKey.wasPressedThisFrame)
                {
                    OnManualSave();
                }
            }
        }
    }

    public void SaveGame()
    {
        string name = $"{saveName} -on- {timeManager.SeasonNumber}.{timeManager.DateNumber}.{timeManager.YearNumber} -with- {playerInteraction.PlayerStamina} stamina";

        path += name;

        Debug.Log($"Saved game to {path}");

        if (File.Exists(path) == true)
        {
            Debug.Log("Save does exist");
            flowchart.SetStringVariable("SaveName", name);
            flowchart.ExecuteBlock("Start");
            //SaveGame(flowchart.GetStringVariable("SaveName"));
        }
        else
        {
            Debug.Log("Save does not exist");
            //SaveGame(saveName + "-at-" + timeManager.SeasonNumber + "." + timeManager.DateNumber + "." + timeManager.YearNumber);
            SaveGame(name);
        }

        path = originalPath;
    }

    public void SaveGame(string saveName)
    {
        // Here is where saving happens...
        var folderPath = constantPath + "/saves/";

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        //var path = constantPath + "/saves/" + saveName;
        var path = originalPath + saveName;
        Debug.Log("Saving game to " + path);

        //Everything after this comment will be different
        //Manually save scene name
        //Manually save is night from timemanager
        //Manually save player speed from the gameobject's movement component
        //save tutorial bools and objective inside of the tutorial script
        //manually save net worth from timemanager's networth's saveworth method
        //use timemanager's save NPCs for the NPC lists
        //use entity manager's save entities for all three types

        //Must be saved manually
        //sceneName
        //Posiion
        //Isnight
        //networth
        sceneName = SceneManager.GetActiveScene().name;
        position = player.transform.position;//
        isNight = timeManager.isNight;//
        farmNetWorth = timeManager.netWorth.FarmNetWorth;//

        timeManager.SaveDate("save");//
        timeManager.SaveNPCs("save");//
        
        playerInteraction.SavePlayer("save");//
		playerMovement.SaveVisited("save");
        tileManager.SaveFarm("save");//

        invManager.SaveInventory("save");//
        skillManager.Save("save");//

        if (bin != null)
        {
            bin.SaveInventory("save");
        }

        entityManager.SaveEntities("save");//

        GameSave save = new GameSave(sceneName, position, isNight, date, stamina, gold, farmTiles, mushrooms,inventory, tutorialBools, tutorialObjective, farmNetWorth, NPCStartflowcharts, NPCQuestflowcharts, NPCQuests, NPCNames, entities, entityNames, pets, petNames, livestockPets, livestockPetNames, inventoryEntities, inventoryEntityNames, playerEquipment, skills);


        var json = JsonUtility.ToJson(save);

        StreamWriter sw = new StreamWriter(path);
        sw.WriteLine(json);
        loadingSave = false;
        sw.Close();

        flowchart.ExecuteBlock("SaveConfirm");

        saves = FindAllSaves();
    }

    void LoadGame(string saveName)
    {
        // Here is where loading happens...
        var path = constantPath + "/saves/" + saveName;
        Debug.Log("Loading game from " + path);

        StreamReader sr = new StreamReader(path);

        var json = sr.ReadLine();
        //Instance = JsonUtility.FromJson<GlobalGameSaving>(json);
        GameSave save = JsonUtility.FromJson<GameSave>(json);


        sceneName = save.sceneName;
        position = save.position;
        isNight = save.isNight;
        date = save.date;
        stamina = save.stamina;
        gold = save.gold;
        farmTiles = save.farmTiles;
        mushrooms = save.mushrooms;
        inventory = save.inventory;
        tutorialBools = save.tutorialBools;
        tutorialObjective = save.tutorialObjective;
        farmNetWorth = save.farmNetWorth;
        NPCStartflowcharts = save.NPCStartflowcharts;
        NPCQuestflowcharts = save.NPCQuestflowcharts;
        NPCQuests = save.NPCQuests;
        NPCNames = save.NPCNames;
        entities = save.entities;
        entityNames = save.entityNames;
        pets = save.pets;
        petNames = save.petNames;
        livestockPets = save.livestockPets;
        livestockPetNames = save.livestockPetNames;
		inventoryEntities = save.inventoryEntities;
		inventoryEntityNames = save.inventoryEntityNames;
		playerEquipment = save.Equipment;

        sr.Close();

        //Tells scripts loading that they are loading whatever is in the save
        loadingSave = true;

        SceneManager.LoadScene(sceneName);

       
    }

    void DeleteSave(string saveName)
    {
        var folderPath = constantPath + "/saves/";

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var path = constantPath + "/saves/" + saveName;
        Debug.Log("Deleting game at " + path);

        File.Delete(path);
    }

    string[] FindAllSaves()
    {
        // Load in all the possible save files...
        var folderPath = constantPath + "/saves/";

        if (!Directory.Exists(folderPath))
            return null;

        var saves = Directory.GetFiles(folderPath);
        for (int i = 0; i < saves.Length; i++)
        {
            var splitString = saves[i].Split('/');
            saves[i] = splitString[splitString.Length - 1];
        }

        return saves;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        GUILayout.Box("Press O to save...");
        if (!displayLoadMenu)
        {
            GUILayout.Box("Press P to load saves...");
        }
        else
        {
            if (saves.Length > 0)
            {
                foreach (var save in saves)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(save))
                    {
                        LoadGame(save);
                        displayLoadMenu = false;
                    }
                    if (GUILayout.Button("X"))
                    {
                        DeleteSave(save);
                        saves = FindAllSaves();
                    }
                    GUILayout.EndHorizontal();
                }
            }
            else
            {
                GUILayout.Box("No saves to load!");
            }

            if (GUILayout.Button("Close Saves"))
                displayLoadMenu = false;
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
}

public class GameSave
{
    public string sceneName;
    public Vector3 position;
    public bool isNight;
    public Vector4 date;
    public int stamina;
    public int gold;
    public List<SaveTile> farmTiles;
    public List<MushroomSaveTile> mushrooms;
    public List<int> inventory;
    public List<bool> tutorialBools = new List<bool>();
    public string tutorialObjective;
    //public List<NPCManager> NPCs = new List<NPCManager>();
    public int farmNetWorth;
    public List<SaveStartChart> NPCStartflowcharts = new List<SaveStartChart>();//Fungus Flowcharts
    public List<SaveQuestChart> NPCQuestflowcharts = new List<SaveQuestChart>();//Fungus Quest Flowcharts
    public List<SaveQuest> NPCQuests = new List<SaveQuest>();//Quest Scripts
    public List<string> NPCNames = new List<string>();//The names of all NPCs that this has

    //public List<SaveEntity> allEntities = new List<SaveEntity>(); //all entities in scene
    public List<SaveEntity> entities = new List<SaveEntity>(); //all non pet entities that are in the list of all entities
    public List<string> entityNames = new List<string>();
    public List<SavePet> pets = new List<SavePet>(); //all pets that are in the list of all entities
    public List<string> petNames = new List<string>();
    public List<SaveLivestockPet> livestockPets = new List<SaveLivestockPet>();
    public List<string> livestockPetNames = new List<string>();
	public List<SaveInventoryEntity> inventoryEntities = new List<SaveInventoryEntity>();
	public List<string> inventoryEntityNames = new List<string>();
	public List<string> Equipment = new List<string>();
	public string skills;

    public GameSave(string s, Vector3 p, bool n, Vector4 d, int st, int g, List<SaveTile> fT, List<MushroomSaveTile> m, List<int> i, List<bool> tB, string t, int f, List<SaveStartChart> sC, List<SaveQuestChart> qC, List<SaveQuest> q, List<string> names, List<SaveEntity> ents, List<string> entNames, List<SavePet> pets, List<string> petNames, List<SaveLivestockPet> live, List<string> liveNames, List<SaveInventoryEntity> chests, List<string> chestNames, List<string> equipment, string skills)
    {
        sceneName = s;
        position = p;
        isNight = n;
        date = d;
        stamina = st;
        gold = g;
        farmTiles = fT;
        mushrooms = m;
        inventory = i;
        tutorialBools = tB;
        tutorialObjective = t;
        farmNetWorth = f;
        NPCStartflowcharts = sC;
        NPCQuestflowcharts = qC;
        NPCQuests = q;
        NPCNames = names;
        entities = ents;
        entityNames = entNames;
        this.pets = pets;
        this.petNames = petNames;
        livestockPets = live;
        livestockPetNames = liveNames;
		inventoryEntities = chests;
		inventoryEntityNames = chestNames;
		Equipment = equipment;
		this.skills = skills;
    }
}
