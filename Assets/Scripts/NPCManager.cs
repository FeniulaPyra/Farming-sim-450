using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class NPCManager : MonoBehaviour
{
    //The NPC's Start Block. The rest will just naturally follow, so this is the only one it needs to know

    Flowchart myFlowchart;

    public Flowchart MyFlowchart
    {
        get
        {
            return myFlowchart;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myFlowchart = transform.Find("Start").GetComponent<Flowchart>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
