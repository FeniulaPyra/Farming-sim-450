using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MushroomManager : MonoBehaviour
{
    //Exists solely to have a bunch of Mushroom references
    //This way a prefab of all mushrooms types could be made and just saved as references here, then just grabbed
    //This one script then allows access to all Mushrooms, instead of Farm Manager needing a ton of them
    
    public List<GameObject> mushroomList = new List<GameObject>();
    public Dictionary<string, GameObject> mushroomVariants = new Dictionary<string, GameObject>();

    

    private void Awake()
    {
        //Sets the Mushroom Variant Key to the mushroom's ID
        foreach (GameObject shroom in mushroomList)
        {
            mushroomVariants[shroom.GetComponent<Mushrooms>().ID] = shroom;
        }

        //Printing the names of the mushrooms
        foreach (KeyValuePair<string, GameObject> kvp in mushroomVariants)
        {
            Debug.Log($"The value of {kvp.Key} is {kvp.Value.GetComponent<Mushrooms>().baseValue}");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
