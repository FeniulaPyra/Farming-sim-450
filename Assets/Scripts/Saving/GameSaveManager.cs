using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

using System.IO;

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
        tileManager.SaveFieldObjects(out var farmland, out var mushrooms);
        save.farmTiles = farmland;
        save.mushrooms = mushrooms;
        save.inventory = farmManager.playerInventory.GetSaveableInventory();
        save.tutorialBools = new List<bool>();
        foreach (bool b in farmingTutorial.tutorialBools)
        {
            save.tutorialBools.Add(b);
        }
        save.tutorialObjective = farmingTutorial.objective.text;

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
        tileManager.LoadFieldObjects(save.farmTiles, save.mushrooms);
        farmManager.playerInventory.SetSaveableInventory(save.inventory);
        for (int i = 0; i < save.tutorialBools.Count; i++)
        {
            farmingTutorial.tutorialBools[i] = save.tutorialBools[i];
        }
        farmingTutorial.objective.text = save.tutorialObjective;

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
        public List<SaveTile> farmTiles;
        public List<MushroomSaveTile> mushrooms;
        public List<int> inventory;
        public List<bool> tutorialBools;
        public string tutorialObjective;
    }
}
