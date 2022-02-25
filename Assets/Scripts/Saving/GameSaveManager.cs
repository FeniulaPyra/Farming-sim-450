using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class GameSaveManager : MonoBehaviour
{
    [Header("Save Info")]
    public string saveName;

    [Header("Data to Save")]
    public GameObject player;
    public TimeManager timeManager;
    public PlayerInteraction playerInteraction;

    private string constantPath;

    private bool displayLoadMenu;
    private string[] saves;

    // Start is called before the first frame update
    void Start()
    {
        constantPath = Application.persistentDataPath;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            SaveGame(saveName + "-at-" + timeManager.DayNumber + "." + timeManager.DateNumber + "." + timeManager.YearNumber);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            displayLoadMenu = true;
            saves = FindAllSaves();
        }
    }

    void SaveGame(string saveName)
    {
        // Here is where saving happens...
        var folderPath = constantPath + "/saves/";

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var path = constantPath + "/saves/" + saveName;
        Debug.Log("Saving game to " + path);

        StreamWriter sw = new StreamWriter(path);

        var pTransform = player.transform;
        var pPosition = pTransform.position;
        var pRotation = pTransform.rotation.eulerAngles;
        sw.WriteLine($"{pPosition.x},{pPosition.y},{pPosition.z}");
        sw.WriteLine($"{pRotation.x},{pRotation.y},{pRotation.z}");
        sw.WriteLine($"{timeManager.DayNumber},{timeManager.DateNumber},{timeManager.YearNumber},{timeManager.SeasonNumber}");
        sw.WriteLine($"{playerInteraction.PlayerStamina}");

        sw.Close();

        saves = FindAllSaves();
    }

    void LoadGame(string saveName)
    {
        // Here is where loading happens...
        var path = constantPath + "/saves/" + saveName;
        Debug.Log("Loading game from " + path);

        StreamReader sr = new StreamReader(path);

        var readPos = sr.ReadLine().Split(',');
        var pos = new Vector3(
            float.Parse(readPos[0]),
            float.Parse(readPos[1]),
            float.Parse(readPos[2])
            );

        var readRot = sr.ReadLine().Split(',');
        var rot = Quaternion.Euler(
            float.Parse(readRot[0]),
            float.Parse(readRot[1]),
            float.Parse(readRot[2])
            );

        player.transform.position = pos;
        player.transform.rotation = rot;

        var readDate = sr.ReadLine().Split(',');
        timeManager.SetDate(
            int.Parse(readDate[0]),
            int.Parse(readDate[1]),
            int.Parse(readDate[2]),
            int.Parse(readDate[3])
            ); 

        var readStamina = sr.ReadLine();
        playerInteraction.SetStamina(int.Parse(readStamina));
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
}
