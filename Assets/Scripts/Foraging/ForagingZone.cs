using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForagingZone : MonoBehaviour
{

	public int numToSpawn;

	public List<GameObject> itemsToSpawn;
	public List<int> weights;

	[SerializeField]
	private RectTransform ZoneBounds;
	private int totalWeight;

	public void Awake()
	{
        //I forgot to change the weights, and it threw me an error, so here's a safeguard
        if (weights.Count > itemsToSpawn.Count)
        {
            List<int> tempList = new List<int>();
            for (int i = 0; i < itemsToSpawn.Count; i++)
            {
                tempList.Add(weights[i]);
            }

            weights.Clear();

            foreach (int i in tempList)
            {
                weights.Add(i);
            }
        }

        foreach (int i in weights)
		{
			totalWeight += i;
		}
	}

	//https://stackoverflow.com/questions/1761626/weighted-random-numbers
	public void SpawnRandom()
	{
		int rand = Random.Range(0, totalWeight);
        
		for (int i = 0; i < weights.Count; i++)
		{
			if (rand < weights[i])
			{
				//spawn item;
				GameObject item = Instantiate(itemsToSpawn[i], transform);//, new Vector3(RandomX(), RandomY()), Quaternion.identity);
				item.transform.position = item.transform.position + new Vector3(RandomX(), RandomY());
				item.transform.localScale = new Vector3(1.25f, 1.25f);
				return;
			}
				
			rand -= weights[i];
		}
	}

	public float RandomX()
	{
		return Random.Range(-ZoneBounds.rect.width / 2, ZoneBounds.rect.width / 2);// + ZoneBounds.rect.width/2;// + ZoneBounds.rect.position.x + gameObject.transform.parent.position.x;
	}
	public float RandomY()
	{
		return Random.Range(-ZoneBounds.rect.height / 2, ZoneBounds.rect.height / 2);// - ZoneBounds.rect.height / 2; //+ ZoneBounds.rect.position.y + gameObject.transform.parent.position.y;
	}

}
