using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BasicEnemy
{
    public GameObject pellet;

    [SerializeField]
    float attackTimer;

    [SerializeField]
    float baseAttackTimer = 2.0f;

    void Attack()
    {
        //setting damage
        //pellet.damage = stat.strength
        //pellet.debuff = debuff;
        pellet.GetComponent<Projectile>().debuff = debuff;
        pellet.GetComponent<Projectile>().damage = stats.Strength;
        pellet.GetComponent<Projectile>().player = target;
        //Debug.Log($"Transfer?: {pellet.debuff.poison}");

        //Instantiate(pellet.gameObject, transform.position, transform.rotation);
        Instantiate(pellet, transform.position, transform.rotation);

        attackTimer = baseAttackTimer;
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        //Look towards player?
        transform.rotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
        //transform.rotation = new Quaternion(0, 0, transform.rotation.z, transform.rotation.w);

        //counts down to attack
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0.0f)
        {
            Attack();
        }

        distance = Vector2.Distance(transform.position, player.position);

        //ranged enemy seeks while farther than min distance
        if (distance > maxSeekDistance)
        {
            Seek(player);
        }
        else if (distance < minSeekDistance)
        {
            Flee(player);
        }
        else
        {
            if (rb != null && rb.velocity != null && rb.velocity.magnitude > 0)
                rb.velocity += -rb.velocity * 0.25f;
            //rb.velocity = Vector2.zero;
        }
    }
}
