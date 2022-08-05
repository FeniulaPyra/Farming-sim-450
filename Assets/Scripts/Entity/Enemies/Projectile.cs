using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour
{

    //direction to go in
    public Vector2 dir;
    //speed to travel at
    public float speed;
    //damage to deal (will be passed in using ranged enemy's stat for strength)
    public int damage;
    //For moving the pellet
    [SerializeField]
    Rigidbody2D rb;

    //How long the projectile exists
    [SerializeField]
    float lifespan;

    [SerializeField]
    Vector2 force;

    [SerializeField]
    public EnemyDebuffs debuff; //inherited from enemy that fires it.

	public GameObject origin;

    //public CombatantStats player; //For changing their HP on hit

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Am I Poison?: {debuff != null && debuff.poison}");
		transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(dir.x, -dir.y));
    }

    private void FixedUpdate()
    {
        //Vector2 desiredVelocity = dir * speed;

        //rb.velocity += desiredVelocity;
        force = /*transform.forward*/ dir.normalized * speed;


        rb.AddForce(force, ForceMode2D.Impulse);

        Destroy(gameObject, lifespan);
    }

    //If entering the player
    //Some method for collision with player
    //Would be where damage actually happens
    private void OnTriggerEnter2D(Collider2D collision)
    {
		CombatantStats target = collision.gameObject.GetComponent<CombatantStats>();
		if (target != null && collision.gameObject != origin)
        {
            //Before destroying self, inflict damage to player

            //Potentially Applying Debuff
            if (debuff != null)
            {
                int random = Random.Range(0, 101);

                if (random <= debuff.activateChance)
                {
                    debuff.ApplyDebuff();
                }
            }

            target.TakeDamage(damage, false);

            Destroy(gameObject);

            Debug.Log("Hit");
        }
        else if (collision.gameObject.GetComponent<TilemapCollider2D>() != null)
        {
            Destroy(gameObject);
        }
    }
}
