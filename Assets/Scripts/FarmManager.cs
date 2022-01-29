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

    //Testing only; attempt at making a dictionary
    //The key is the position of a tile, and the mushroom is that instance of the script it's supposed to kep track of
    public Dictionary<Vector3Int, Mushrooms> mushroomsAndTiles = new Dictionary<Vector3Int, Mushrooms>();

    // Start is called before the first frame update
    void Start()
    {
        farmField = gameObject.GetComponent<Tilemap>();

        //Can be used to figure out where the (0, 0) of the tilemap is, which could be useful
        Debug.Log($"The origin is: {farmField.origin}");
        //The bottom left point of a tile is its actual coordinates
        Debug.Log($"I am the tile: {farmField.GetTile(new Vector3Int(-11, 3, 0))}");

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
        //farmField.SetTile(new Vector3Int(-11, 3, 0), plantedMushrooms[0].GetComponent<Mushrooms>().mushroomTile);

        //At this specific position, you know about this specific script instance
        //mushroomsAndTiles[new Vector3Int(-11, 3, 0)] = plantedMushrooms[0].GetComponent<Mushrooms>();

    }

    // Update is called once per frame
    void Update()
    {
        //When mushroom destroys itself, the tile stays
        //Check the dictionary to see if the associated script still exists
        //if it doesn't, set tile to null
        foreach (KeyValuePair<Vector3Int, Mushrooms> shroom in mushroomsAndTiles)
        {
            if(shroom.Value == null)
            {
                farmField.SetTile(shroom.Key, null);
            }
        }
    }

    public void TileInteract(Vector3Int tile, string tool)
    {
        Debug.Log("Do interaction! @" + tile);

        if (!mushroomsAndTiles.ContainsKey(tile)) return;

        // Do interaction
    }

    public void SpreadMushroom()
    {
        Debug.Log("Beginning of SpreadMushroom");

        //The bounds of the crop field
        //The one in the current scene is at (-11, 0) and goes across 11 and up five
        //Since the entire point is to use the x and y to figure out exactly what tile you need to check
        //The for loops need to properly know what the bounds are
        //Which is why it's just taking the bounds and adding the size to get to the other end
        //The x range is now -11 -> 0, which are the x values the field actualy covers
        int leftBound = farmField.cellBounds.x;
        int bottomBound = farmField.cellBounds.y;

        int rightBound = leftBound + farmField.cellBounds.size.x;
        int topBound = farmField.cellBounds.size.y;

        //Nested For Loop, going through the x bound and y bounds of farmfield
        for (int x = leftBound; x < rightBound; x++)
        {
            for (int y = bottomBound; y < topBound; y++)
            {
                Vector3Int tileToTest = new Vector3Int(x, y, 0);
                
                //Calls gettile at that point
                if (farmField.GetTile(tileToTest) != null)
                {

                    Debug.Log($"Found a tile at {tileToTest}!");

                    //If it exists, get the mushroom script it's attached to
                    //see if it's fully grown
                    if (mushroomsAndTiles[tileToTest].growthStage >= mushroomsAndTiles[tileToTest].GetMaxGrowthStage())
                    {
                        Vector3Int above = new Vector3Int(tileToTest.x, tileToTest.y + 1, 0);
                        Vector3Int below = new Vector3Int(tileToTest.x, tileToTest.y - 1, 0);
                        Vector3Int left = new Vector3Int(tileToTest.x - 1, tileToTest.y, 0);
                        Vector3Int right = new Vector3Int(tileToTest.x + 1, tileToTest.y, 0);


                        //If it is, go back to tile, search four adjacent spaces to see if they're within bounds and gettile is null, meaning they're empty
                        //If yes, instantiate mushroom prefab, set it's tile to that empty space, and add it as child to farmfield
                        if (farmField.GetTile(above) == null && above.y <= topBound)
                        {
                            mushroomsAndTiles[above] = Instantiate(mushromPrefab, above, Quaternion.identity).GetComponent<Mushrooms>();
                            farmField.SetTile(above, mushroomsAndTiles[above].mushroomTile);
                            mushroomsAndTiles[above].transform.parent = this.transform;
                        }

                        if (farmField.GetTile(below) == null && below.y >= bottomBound)
                        {
                            mushroomsAndTiles[below] = Instantiate(mushromPrefab, below, Quaternion.identity).GetComponent<Mushrooms>();
                            farmField.SetTile(below, mushroomsAndTiles[below].mushroomTile);
                            mushroomsAndTiles[below].transform.parent = this.transform;
                        }

                        if (farmField.GetTile(left) == null && left.x >= leftBound)
                        {
                            mushroomsAndTiles[left] = Instantiate(mushromPrefab, left, Quaternion.identity).GetComponent<Mushrooms>();
                            farmField.SetTile(left, mushroomsAndTiles[left].mushroomTile);
                            mushroomsAndTiles[left].transform.parent = this.transform;
                        }

                        if (farmField.GetTile(right) == null && right.x <= rightBound)
                        {
                            mushroomsAndTiles[right] = Instantiate(mushromPrefab, right, Quaternion.identity).GetComponent<Mushrooms>();
                            farmField.SetTile(right, mushroomsAndTiles[right].mushroomTile);
                            mushroomsAndTiles[right].transform.parent = this.transform;
                        }
                    }
                }
            }
        }

        Debug.Log("End of SpreadMushroom");
    }
}
