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
	protected Toggle friendlyMode;

	[SerializeField]
	public List<TimeManager.Season> preferredSeasons;
	public float offSeasonSpawnChance;

    // Start is called before the first frame update
    protected override void Start()
    {
        //debuff = gameObject.GetComponent<EnemyDebuffs>();
        target = FindObjectOfType<PlayerMovement>().gameObject.GetComponent<CombatantStats>();

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected  virtual void FixedUpdate()
    {
        base.FixedUpdate();

        //If player is closer than that, flee until at max distance

        /*if (attacked == false)
        {
            base.FixedUpdate();

            /*if (distance <= minSeekDistance)
            {
                attacked = true;
                fleeTimer = baseFleeTimer;
            }
        }*/

        /*if (attacked == true)
        {
            fleeTimer -= Time.deltaTime;

            if (fleeTimer > 0)
            {
                Flee(player);
            }
            else
            {
                attacked = false;
            }
        }*/
    }

    protected virtual void Flee(Transform target)
    {
        /*if (rb != null && rb.velocity != null && rb.velocity.magnitude > 0)
            rb.velocity += -rb.velocity * 2f;*/

        var dist = Vector2.Distance(target.position, transform.position);

        var desiredVelocity = (target.position - transform.position) * movementSpeed * dist;//speedCurve.Evaluate((dist / (maxSeekDistance - minSeekDistance)));
        desiredVelocity.z = 0;

        var move = new Vector2(-desiredVelocity.x, -desiredVelocity.y) - rb.velocity;

        if (rb.velocity.magnitude > 0.1)
            sr.flipX = rb.velocity.x < 0;

        rb.velocity += move;
    }
}
