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

    [Serializable]
    public class KeyValuePair
    {
        public string key;
        public GameObject value;
    }

    
    public List<KeyValuePair> mushroomList = new List<KeyValuePair>();
    Dictionary<string, GameObject> mushroomVariants = new Dictionary<string, GameObject>();

    

    private void Awake()
    {
        foreach (var kvp in mushroomList)
        {
            mushroomVariants[kvp.key] = kvp.value;
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
