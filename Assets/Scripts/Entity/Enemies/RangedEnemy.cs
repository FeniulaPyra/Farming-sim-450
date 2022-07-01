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
		if (friendlyMode.isOn) return;
		//setting damage
		//pellet.damage = stat.strength
		//pellet.debuff = debuff;
		Projectile projectileScript = pellet.GetComponent<Projectile>();
		projectileScript.debuff = debuff;
		projectileScript.damage = stats.Strength;
		projectileScript.dir = new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));//gameObject.transform.eulerAngles;
		projectileScript.origin = gameObject;
		//pellet.GetComponent<Projectile>().player = target;
		//Debug.Log($"Transfer?: {pellet.debuff.poison}");

		//Instantiate(pellet.gameObject, transform.position, transform.rotation);
		Instantiate(pellet, transform.position, Quaternion.identity/*transform.rotation*/);

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
		Vector2 vec = (Vector2)player.position - (Vector2)transform.position;
		float theta = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
		//https://forum.unity.com/threads/rotating-a-2d-object.483830/
		transform.eulerAngles = Vector3.forward * theta;//Quaternion.LookRotation(player.position - transform.position, new Vector3(0, 1, 0));//Vector3.up);
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
