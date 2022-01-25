using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmManager : MonoBehaviour
{
    //Tilemap from the thing it is one, to keep track of crops
    Tilemap farmField;
    //Testing purposes; list of gameobject children
    //List<GameObject> children = new List<GameObject>();
    List<Transform> plantedMushrooms = new List<Transform>();
    //Reference to the Mushroom Prefab for creating new mushrooms
    public GameObject mushromPrefab;

    // Start is called before the first frame update
    void Start()
    {
        farmField = gameObject.GetComponent<Tilemap>();

        //Can be used to figure out where the (0, 0) of the tilemap is, which could be useful
        Debug.Log($"The origin is: {farmField.origin}");
        //The bottom left point of a tile is its actual coordinates
        Debug.Log($"I am the tile: {farmField.GetTile(new Vector3Int(-11, 4, 0))}");

        foreach(Transform child in transform)
        {
            //Should only add mushrooms
            //Testing purposes; might have to be moved elsewhere
            if(child.GetComponent<Mushrooms>() != null)
            {
                plantedMushrooms.Add(child);
            }
            //Debug.Log($"I've added {child}");
            //Debug.Log($"It is worth {child.GetComponent<Mushrooms>().baseValue}");
        }

        //Testing purposes; This line of code actually puts the tile at that position
        farmField.SetTile(new Vector3Int(-11, 4, 0), plantedMushrooms[0].GetComponent<Mushrooms>().mushroomTile);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spreadMushroom()
    {
        //Nested For Loop, going through the x bound and y bounds of farmfield
        //Calls gettile at that point
        //If it exists, get the mushroom script it's attached to
        //see if it's fully grown
        //If it is, go back to tile, search four adjacent spaces to see if they're within bounds and gettile is null, meaning they're empty
        //If yes, instantiate mushroom prefab, set it's tile to that empty space, and add it as child to farmfield

        for (int x = 0; x < farmField.cellBounds.x; x++)
        {
            for (int y = 0; y < farmField.cellBounds.y; y++)
            {
                if(farmField.GetTile(new Vector3Int(x, y, 0)) != null)
                {

                }
            }
        }
    }
}
