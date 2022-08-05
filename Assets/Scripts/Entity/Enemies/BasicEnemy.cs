using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemy : BasicEntity
{
    //This is where the stats variable will eventually go

    //distance between enemy and player
    //[SerializeField]
    //float distance;

    [SerializeField]
    protected bool attacked;

    [SerializeField]
    protected float fleeTimer;

    [SerializeField]
    protected float baseFleeTimer = 2.00f;

    [SerializeField]
    protected EnemyDebuffs debuff; //It's on this prefab; just drag it in

    [SerializeField]
    protected CombatantStats stats; //It's on this prefab; just drag it in
    [SerializeField]
    protected CombatantStats target; //The player

    [SerializeField]
    protected List<Item> drops = new List<Item>();

    //Enemy spawning chest testing
    [SerializeField]
    protected InventoryEntity chest;
    //I don't know what's happening
    bool dead;

	[SerializeField]
	protected Toggle friendlyMode;

	[SerializeField]
	public List<TimeManager.Season> preferredSeasons;
	public float offSeasonSpawnChance;

    // Start is called before the first frame update
    protected override void Start()
    {
        //debuff = gameObject.GetComponent<EnemyDebuffs>();
        target = FindObjectOfType<PlayerMovement>().gameObject.GetComponent<CombatantStats>();

        //Go to the PlayerInputManager, which should have everything for controller support. Then just go down the list until Friendly Mode
        //friendlyMode = GameObject.Find("PlayerInputManager").transform.Find("Menus").transform.Find("Settings").transform.Find("Friendly Mode").GetComponent<Toggle>();

        //Menus is active, so something should be returned. Then just go down the list until Friendly Mode
        friendlyMode = GameObject.Find("Menus").transform.Find("Settings").transform.Find("Friendly Mode").GetComponent<Toggle>();

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (ScenePersistence.Instance.gamePaused == false)
        {
            //Ready for death
            if (stats.Health <= 0 && dead == false)
            {
                KillEnemy();
            }

            base.Update();
        }

    }

    protected  virtual void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void KillEnemy()
    {
        dead = true;
        //experience
        target.IncreaseExp(stats.Experience);
        //drops
        if (drops.Count > 0)
        {
            DropItems();
        }
        //Quests
        UpdateQuest();
        //death
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Upon death, update the player's search and destroy quest, if necessary
    /// </summary>
    protected virtual void UpdateQuest()
    {
        foreach (Quests q in target.gameObject.GetComponent<PlayerInteraction>().playerQuests)
        {
            if (q.questType == Quests.QuestType.SearchAndDestroy || q.questType == Quests.QuestType.TimedSearchAndDestroy)
            {
                if (this.GetType() == q.TargetEnemy.GetType())
                {
                    q.AmountKilled++;
                }
            }
        }
    }

    /// <summary>
    /// Randomly chooses something from the enemy's item list to instantiate where they die
    /// </summary>
    protected virtual void DropItems()
    {
        //randomly select an item to spawn
        int index;
        index = Random.Range(0, drops.Count);
        if (index == drops.Count)
        {
            //Spawn no individual item; spawn whole chest
            chest.items.Clear();
            chest.itemAmounts.Clear();
            chest.Manager = FindObjectOfType<ItemManager>();
            chest.items.Add(chest.Manager.gameItems[0]);
            chest.items.Add(chest.Manager.gameItems[1]);
            chest.items.Add(chest.Manager.gameItems[2]);
            chest.PopulateMe();
            GameObject chestObject = Instantiate(chest.gameObject, transform.position, Quaternion.identity);
        }
        else
        {
            Item drop = drops[index];
            //spawn that item's gameobject at the enemy's position
            Instantiate(drop.gameObject, transform.position, Quaternion.identity);
        }
    }

    protected virtual void Flee(Transform target)
    {
        var dist = Vector2.Distance(target.position, transform.position);

        var desiredVelocity = (target.position - transform.position) * movementSpeed * dist;
        desiredVelocity.z = 0;

        var move = new Vector2(-desiredVelocity.x, -desiredVelocity.y) - rb.velocity;

        if (rb.velocity.magnitude > 0.1)
            sr.flipX = rb.velocity.x < 0;

        rb.velocity += move;
    }
}
