using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

using System.IO;
using System.Linq;
using TMPro;

public class GameSaveManager : MonoBehaviour
{
    [Header("Save Info")]
    public string saveName;

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

    [SerializeField]
    Flowchart flowchart;

    // Start is called before the first frame update
    void Start()
    {
        constantPath = Application.persistentDataPath;

        flowchart = transform.Find("SaveFlowchart").GetComponent<Flowchart>();

        originalPath = constantPath + "/saves/";
        path = originalPath;
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

    public void SaveGame(string saveName)
    {
        // Here is where saving happens...
        var folderPath = constantPath + "/saves/";

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        //var path = constantPath + "/saves/" + saveName;
        var path = originalPath + saveName;
        Debug.Log("Saving game to " + path);

        var save = new GameSave();
        save.position = player.transform.position;
        save.isNight = timeManager.isNight;
        save.date = new Vector4(
            timeManager.DayNumber,
            timeManager.DateNumber,
            timeManager.YearNumber,
            timeManager.SeasonNumber
            );
        save.stamina = playerInteraction.PlayerStamina;
        save.gold = playerInteraction.playerGold;
        tileManager.SaveFieldObjects(out var farmland, out var mushrooms);
        save.farmTiles = farmland;
        save.mushrooms = mushrooms;
        save.inventory = farmManager.playerInventory.GetSaveableInventory();

        if (farmingTutorial != null)
        {
            foreach (bool b in farmingTutorial.tutorialBools)
            {
                save.tutorialBools.Add(b);
            }
            save.tutorialObjective = farmingTutorial.objective.text;
        }

        timeManager.netWorth.SaveWorth(out var savedWorth);
        save.farmNetWorth = savedWorth;


        for (int i = 0; i < timeManager.NPCList.Count; i++)
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
        }

        //saving all entities and pets
        List<BasicEntity> entities = FindObjectsOfType<BasicEntity>().ToList();

        foreach (BasicEntity e in entities)
        {
            if (e is BasicPet)
            {
                BasicPet b = (BasicPet)e;
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
        }


        var json = JsonUtility.ToJson(save);

        StreamWriter sw = new StreamWriter(path);
        sw.WriteLine(json);
        sw.Close();

        saves = FindAllSaves();
    }

    void LoadGame(string saveName)
    {
        // Here is where loading happens...
        var path = constantPath + "/saves/" + saveName;
        Debug.Log("Loading game from " + path);

        StreamReader sr = new StreamReader(path);

        var json = sr.ReadLine();
        var save = JsonUtility.FromJson<GameSave>(json);

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

        for (int i = 0; i < entities.Count; i++)
        {
            if (entities[i] != null)
            {
                Destroy(entities[i].gameObject);
            }
        }


        sr.Close();
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
        for(int i = 0; i < saves.Length; i++)
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
        } else
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
            } else
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

    [System.Serializable]
    private class GameSave
    {
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
        public List<SaveEntity> allEntities = new List<SaveEntity>(); //all entities in scene
        public List<SaveEntity> entities = new List<SaveEntity>(); //all non pet entities that are in the list of all entities
        public List<string> entityNames = new List<string>();
        public List<SavePet> pets = new List<SavePet>(); //all pets that are in the list of all entities
        public List<string> petNames = new List<string>();
    }
}
