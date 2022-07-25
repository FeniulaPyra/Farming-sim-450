using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    //All waves; designers should be able to just change this in editor as they see fit
    public List<Wave> waves = new List<Wave>();

    //List of spawners, which are just empty gameObjects so their transforms can be used
    public List<GameObject> spawners = new List<GameObject>();

    //Doors that keep the player from leaving until all enemies have been killed
    public List<GameObject> Doors = new List<GameObject>();

    //Chest to spawn when all enemies are dead; make sure to use an "if not null" check
    public GameObject chest;

    [SerializeField]
    Wave currentWave;

    //Enemies in the wave; its Count will determine when things spawn
    public List<BasicEnemy> waveEnemies = new List<BasicEnemy>();

    public int waveCounter = 0;

    //Counts the number of enemies currently in the scene
    public int waveEnemyCounter;

    //purely for testing
    [SerializeField]
    bool waveStartedSpawn;
    [SerializeField]
    bool waveFinishedSpawn;

    //if, for whatever reason, there are 0 enemies in a wave list, it will default to this
    public BasicEnemy defaultEnemy;

    //Player; when they are close enough, starts the spawning
    [SerializeField]
    GameObject player;
    [SerializeField]
    PlayerInteraction playerInteraction;

    //distance between player and spawner; might have to be different on a case to case basis
    public float spawnDistance;
    public float distance; //For testing purposes, to easily see what the distance is

    GameObject spawner;

	public TimeManager timeManager;

    [System.Serializable]
    public class Wave
    {
        //How many enemies to spawn
        public int numToSpawn;

        //Time between each enemy passing
        public float enemySpawnDelay;

        //Weights for enemies; how often each one should spawn
        public List<int> enemyWeights = new List<int>();

        //list of enemies this wave could spawn; must be in order from rarest to most common to work with the above
        public List<BasicEnemy> enemies = new List<BasicEnemy>();

        public Wave(BasicEnemy basic, int num = 1, float delay = 1)
        {
            numToSpawn = num;
            enemySpawnDelay = delay;
            enemyWeights.Add(100);
            enemies.Add(basic);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (waves.Count > 0)
        {
            currentWave = waves[0];
        }
        //Just making an empty wave to avoid errors, if one isn't made in inspector, for some reason
        else
        {
            waves.Add(new Wave(defaultEnemy));
            currentWave = waves[0];
        }

        //If it's 0, or somehow less than 0, make it a number
        if (spawnDistance <= 0)
        {
            spawnDistance = 2.5f;
        }

        playerInteraction = FindObjectOfType<PlayerInteraction>();
        player = playerInteraction.gameObject;
        timeManager = FindObjectOfType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < spawners.Count; i++)
        {
            distance = Vector2.Distance(player.transform.position, spawners[i].transform.position);
            if (distance < spawnDistance && waveStartedSpawn == false)//if (Vector2.Distance(player.transform.position, transform.position) < spawnDistance)
            {
                spawner = spawners[i];
                StartCoroutine(SpawnWave(currentWave.enemySpawnDelay, spawner));
            }

            if (distance < spawnDistance / 3)
            {
                foreach (GameObject d in Doors)
                {
                    if (d.activeInHierarchy == false)
                    {
                        d.SetActive(true);
                    }
                }
            }
        }

        //Loop through the list of enemies, and if an index is null because the enemy is dead, 
        for (int i = 0; i < waveEnemies.Count; i++)
        {
            if (waveEnemies[i] == null)
            {
                waveEnemies.RemoveAt(i);
                waveEnemyCounter--;
            }
        }

        //In the event of five, all must be dead if it was <=, then it would still happen at only one left, which would immediately happen
        if (waveEnemyCounter == 0 && waveFinishedSpawn == true)
        //if (waveEnemyCounter == ((currentWave.numToSpawn / 2) - 1) && waveSpawned == true)
        {
            if (waveCounter < waves.Count - 1)
            {
                waveStartedSpawn = false;
                waveFinishedSpawn = false;
                waveCounter++;
                currentWave = waves[waveCounter];
                StartCoroutine(SpawnWave(currentWave.enemySpawnDelay, spawner));
            }
            else
            {
                foreach (GameObject d in Doors)
                {
                    d.SetActive(false);
                }

                if (chest != null)
                {
                    if (chest.activeInHierarchy == false)
                    {
                        chest.SetActive(true);
                    }
                }
            }
        }
    }

    IEnumerator SpawnWave(float enemySpawnDelay, GameObject spawner)
    {
        if (waveStartedSpawn == false)
        {
            waveStartedSpawn = true;
        }

        currentWave.enemyWeights.Sort();

        //if things are the same, force them to not be
        for (int i = 0, j = currentWave.enemyWeights.Count; i < j; i++)
        {
            if (i != j - 1)
            {
                if (currentWave.enemyWeights[i] == currentWave.enemyWeights[i + 1])
                {
                    for (int k = i + 1; k < j; k++)
                    {
                        currentWave.enemyWeights[k] += 20;
                    }
                }
            }
        }

        //If final enemy index is 2, but final weight index is 3, it will spawn enemy[2]. There is no way to account for 3 enemies, but only two weights
        //Making up weights, in the event that there are just too few, for whatever reason.
        if (currentWave.enemyWeights.Count < currentWave.enemies.Count)
        {
            if (currentWave.enemyWeights.Count == 0)
            {
                currentWave.enemyWeights.Add(10);
            }

            while (currentWave.enemyWeights.Count < currentWave.enemies.Count)
            {
                currentWave.enemyWeights.Add(currentWave.enemyWeights[currentWave.enemyWeights.Count - 1] + 10);
            }
        }

        //int highestWeight = currentWave.enemyWeights[currentWave.enemyWeights.Count - 1];
        int highestWeight = currentWave.enemyWeights[currentWave.enemies.Count - 1]; // If there happen to be mroe weights than enemies, highest weight is tied to last enemy

        for (int i = 0; i < currentWave.numToSpawn; i++)
        {
            BasicEnemy enemyToSpawn = defaultEnemy;

            //picking a spawner
            /*if (spawners.Count > 1)
            {
                int index = Random.Range(0, spawners.Count);
                spawner = spawners[index];
                Debug.Log($"Spawning at Spawner {index}");
            }
            else
            {
                spawner = spawners[0];
            }*/

            //picking an enemy; would need to be tweaked to account for weight
            if (currentWave.enemies.Count > 1)
            {
                //enemyToSpawn = currentWave.enemies[Random.Range(0, currentWave.enemies.Count - 1)];
                for (int j = 0; j < currentWave.enemyWeights.Count; j++)
                {
                    int weightCheck = Random.Range(0, highestWeight + 1);

					Debug.Log($"Spawn weight is {weightCheck}; j is {j}");

                    if (weightCheck <= currentWave.enemyWeights[j])
                    {
                        if (currentWave.enemies[j] == null)
                        {
                            enemyToSpawn = currentWave.enemies[currentWave.enemies.Count - 1];
                        }
                        else
                        {
                            enemyToSpawn = currentWave.enemies[j];
                            Debug.Log($"Spawning Enemy {j}");
                        }

                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else if (currentWave.enemies.Count == 1)
            {
                enemyToSpawn = currentWave.enemies[0];
            }

            GameObject enemyObject;
			//if it is the incorrect season for the enemy dont spawn it unless random chance says so
			if (enemyToSpawn != null && (enemyToSpawn.preferredSeasons.Contains((TimeManager.Season)timeManager.SeasonNumber) //is enemy in the right season?
				|| Random.value < enemyToSpawn.offSeasonSpawnChance)) //or if it isnt do a random check to see if the  monster will spawn anyway (this way the monster will spawn but just less 
			{
                if (spawners.Count > 1 && i > 0)
                {
                    spawner = spawners[Random.Range(0, spawners.Count)];
                }

                //Have enemies scale with player, but still be slightly weaker than them
                int newLevel = GameObject.Find("Player").GetComponent<CombatantStats>().Level - 1;

                if (newLevel < 2)
                {
                    newLevel = 1;
                }

                enemyToSpawn.gameObject.GetComponent<CombatantStats>().Level = newLevel;

				//instantiate enemy
				enemyObject = Instantiate(enemyToSpawn.gameObject, spawner.transform.position, Quaternion.identity);
                //7/15/22 Playtesting purposes
                if (spawner.GetComponent<SpriteRenderer>() != null)
                {
                    spawner.GetComponent<SpriteRenderer>().enabled = false;
                }

				//enemyObject.GetComponent<Enemy>().myController = GetComponent<EnemyController>();

				waveEnemies.Add(enemyObject.GetComponent<BasicEnemy>());

				waveEnemyCounter++;
				yield return new WaitForSeconds(enemySpawnDelay);

			}
			else
			{
				yield return new WaitForSeconds(0);
				i--;
			}

        }

        /*if (waveCounter < waves.Count - 1)
        {
            waveSpawned = false;
            waveCounter++;
            currentWave = waves[waveCounter];
            //StartCoroutine(SpawnWave(currentWave.enemySpawnDelay));
        }*/

        if (waveFinishedSpawn == false)
        {
            waveFinishedSpawn = true;
        }

        /*if (waveSpawned == false)
        {
            waveSpawned = true;
        }*/
    }
}
