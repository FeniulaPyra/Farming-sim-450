using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using System.IO;
using UnityEngine.SceneManagement;

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
        timeManager = FindObjectOfType<TimeManager>();
        tileManager = FindObjectOfType<TileManager>();
        farmManager = FindObjectOfType<FarmManager>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();
        entityManager = FindObjectOfType<EntityManager>();

        if (loadingSave == true)
        {
            player.transform.position = position;
            timeManager.isNight = isNight;
            //timeManager.netWorth.FarmNetWorth = farmNetWorth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInteraction.CanInteract == true)
        {
            if (Input.GetKeyDown(KeyCode.O))
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

            if (Input.GetKeyDown(KeyCode.P))
            {
                displayLoadMenu = true;
                saves = FindAllSaves();
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

        tileManager.SaveFarm("save");//

        invManager.SaveInventory("save");//

        entityManager.SaveEntities("save");//

        //var save = new GameSave();
        //save.sceneName = SceneManager.GetActiveScene().name;
        //save.position = player.transform.position;
        //save.isNight = timeManager.isNight;
        /*save.date = new Vector4(
            timeManager.DayNumber,
            timeManager.DateNumber,
            timeManager.YearNumber,
            timeManager.SeasonNumber
            );*/
        //save.stamina = playerInteraction.PlayerStamina;
        //save.gold = playerInteraction.playerGold;

        /*tileManager.SaveFieldObjects(out var farmland, out var mushrooms);

        if (tileManager.farmManager.farmField != null && tileManager.farmManager.tillableGround != null)
        {
            save.farmTiles = farmland;
            save.mushrooms = mushrooms;
        }
        else
        {
            save.farmTiles = ScenePersistence.Instance.farmTiles;
            save.mushrooms = ScenePersistence.Instance.mushrooms;
        }*/

        //save.inventory = farmManager.playerInventory.GetSaveableInventory();

        /*if (farmingTutorial != null)
        {
            foreach (bool b in farmingTutorial.tutorialBools)
            {
                save.tutorialBools.Add(b);
            }
            save.tutorialObjective = farmingTutorial.objective.text;
        }*/

        //timeManager.netWorth.SaveWorth(out var savedWorth);
        //save.farmNetWorth = savedWorth;


        /*for (int i = 0; i < timeManager.NPCList.Count; i++)
        {
            timeManager.NPCList[i].SaveFlowcharts(out var startChart, out var questChart);
            save.NPCStartflowcharts.Add(startChart);
            save.NPCQuestflowcharts.Add(questChart);
            //Going back up to access quests and then save them
            Debug.Log($"Inventory before saving: {timeManager.NPCList[i].gameObject.GetComponent<Quests>().inventory.isShown}");
            timeManager.NPCList[i].gameObject.GetComponent<Quests>().SaveQuest(out var saveQuest);
            saveQuest.inventory = save.inventory;
            save.NPCQuests.Add(saveQuest);
            Debug.Log($"Date?: {save.NPCStartflowcharts[0].dateNum}");
        }*/

        //saving all entities and pets
        /*List<BasicEntity> entities = FindObjectsOfType<BasicEntity>().ToList();

        foreach (BasicEntity e in entities)
        {
            if (e is BasicPet)
            {
                BasicPet b = (BasicPet)e;

                if (b is LivestockPet)
                {
                    LivestockPet l = (LivestockPet)b;
                    l.SaveLivestockPet(out SaveLivestockPet livestockPet);
                    save.livestockPets.Add(livestockPet);
                    if (save.livestockPets[save.livestockPets.Count - 1].self.name.Contains('('))
                    {
                        string[] name = save.livestockPets[save.livestockPets.Count - 1].self.name.Split('(');
                        save.livestockPetNames.Add(name[0]);
                    }
                    else
                    {
                        save.livestockPetNames.Add(save.livestockPets[save.livestockPets.Count - 1].self.name);
                    }

                    Debug.Log($"Livestock Pet name at [{save.livestockPetNames.Count - 1}] is {save.livestockPetNames[save.livestockPetNames.Count - 1]}");
                }
                else
                {
                    b.SavePet(out SavePet pet);
                    save.pets.Add(pet);
                    if (save.pets[save.pets.Count - 1].self.name.Contains('('))
                    {
                        string[] name = save.pets[save.pets.Count - 1].self.name.Split('(');
                        save.petNames.Add(name[0]);
                    }
                    else
                    {
                        save.petNames.Add(save.pets[save.pets.Count - 1].self.name);
                    }

                    Debug.Log($"Pet name at [{save.petNames.Count - 1}] is {save.petNames[save.petNames.Count - 1]}");
                }
            }
            else
            {
                e.SaveEntity(out SaveEntity entity);
                entity.type = entity.gameObject.GetComponent<Item>().type;
                save.entities.Add(entity);
                if (save.entities[save.entities.Count - 1].self.name.Contains('('))
                {
                    string[] name = save.entities[save.entities.Count - 1].self.name.Split('(');
                    save.entityNames.Add(name[0]);
                }
                else
                {
                    save.entityNames.Add(save.entities[save.entities.Count - 1].self.name);
                }

                Debug.Log($"Entity name at [{save.entityNames.Count - 1}] is {save.entityNames[save.entityNames.Count - 1]}");
            }
        }*/

        GameSave save = new GameSave(sceneName, position, isNight, date, stamina, gold, farmTiles, mushrooms,inventory, tutorialBools, tutorialObjective, farmNetWorth, NPCStartflowcharts, NPCQuestflowcharts, NPCQuests, NPCNames, entities, entityNames, pets, petNames, livestockPets, livestockPetNames);


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

        sr.Close();

        //Tells scripts loading that they are loading whatever is in the save
        loadingSave = true;

        SceneManager.LoadScene(sceneName);

        /*SceneManager.LoadScene(save.sceneName);
        player.transform.position = save.position;
        timeManager.isNight = save.isNight;
        timeManager.SetDate(
            (int)save.date.x,
            (int)save.date.y,
            (int)save.date.z,
            (int)save.date.w
            );
        playerInteraction.SetStamina((int)save.stamina);
        playerInteraction.playerGold = save.gold;
        GameObject.Find("GoldDisplay").GetComponent<TMP_Text>().text = $"{playerInteraction.playerGold} G";

        if (tileManager.farmManager.farmField == null)
        {
            Debug.Log("Farmfield empty");
        }
        else
        {
            Debug.Log("Farmfield not empty");
        }
        tileManager.LoadFieldObjects(save.farmTiles, save.mushrooms);

        farmManager.playerInventory.SetSaveableInventory(save.inventory);
        if (farmingTutorial != null)
        {
            for (int i = 0; i < save.tutorialBools.Count; i++)
            {
                farmingTutorial.tutorialBools[i] = save.tutorialBools[i];
            }
            farmingTutorial.objective.text = save.tutorialObjective;
        }

        timeManager.netWorth.FarmNetWorth = save.farmNetWorth;

        for (int i = 0; i < timeManager.NPCList.Count; i++)
        {
            timeManager.NPCList[i].LoadFlowcharts(save.NPCStartflowcharts[i], save.NPCQuestflowcharts[i]);
            timeManager.NPCList[i].gameObject.GetComponent<Quests>().LoadQuest(save.NPCQuests[i]);
            Debug.Log($"Inventory after Loading: {timeManager.NPCList[i].gameObject.GetComponent<Quests>().inventory.isShown}");
            Debug.Log($"Date?: {timeManager.NPCList[0].transform.Find("Start").GetComponent<Flowchart>().GetIntegerVariable("dateNum")}");
        }

        //Destroying entities then replacing them with their saved counterparts
        List<BasicEntity> entities = FindObjectsOfType<BasicEntity>().ToList();

        for (int i = 0; i < save.entities.Count; i++)
        {
            switch (save.entities[i].type)
            {
                case "tool":
                    Instantiate(Resources.Load($"Prefabs/Tools/{save.entityNames[i]}"), save.entities[i].pos, Quaternion.identity);
                    break;
                case "mushroom":
                    Instantiate(Resources.Load($"Prefabs/MushroomPrefabs/{save.entityNames[i]}"), save.entities[i].pos, Quaternion.identity);
                    break;
                default:
                    break;
            }
        }

        for (int i = 0; i < save.pets.Count; i++)
        {
            Instantiate(Resources.Load($"Prefabs/Pets/{save.petNames[i]}"), save.pets[i].pos, Quaternion.identity);
        }

        for (int i = 0; i < save.livestockPets.Count; i++)
        {
            Instantiate(Resources.Load($"Prefabs/Pets/{save.livestockPetNames[i]}"), save.livestockPets[i].pos, Quaternion.identity);
        }

        for (int i = 0; i < entities.Count; i++)
        {
            if (entities[i] != null)
            {
                Destroy(entities[i].gameObject);
            }
        }


        sr.Close();*/
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

    public GameSave(string s, Vector3 p, bool n, Vector4 d, int st, int g, List<SaveTile> fT, List<MushroomSaveTile> m, List<int> i, List<bool> tB, string t, int f, List<SaveStartChart> sC, List<SaveQuestChart> qC, List<SaveQuest> q, List<string> names, List<SaveEntity> ents, List<string> entNames, List<SavePet> pets, List<string> petNames, List<SaveLivestockPet> live, List<string> liveNames)
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
    }
}
