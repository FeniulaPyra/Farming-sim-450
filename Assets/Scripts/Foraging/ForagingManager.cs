using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForagingManager : MonoBehaviour
{
	public List<ForagingZone> zones;
	public PlayerMovement player;

	public void TrySpawn()
	{
		//if this scene has been visited already, just return
		if (player.visited.Contains(SceneManager.GetActiveScene().name)) return;
		//otherwise spawn items
		foreach(ForagingZone z in zones)
		{
			for(int i = 0; i < z.numToSpawn; i++)
			{
				z.SpawnRandom();
			}
		}
	}
}
