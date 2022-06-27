using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    //direction to go in
    Vector2 dir;
    //speed to travel at
    public float speed;
    //damage to deal (will be passed in using ranged enemy's stat for strength)
    int damage;
    //For moving the pellet
    [SerializeField]
    Rigidbody2D rb;

    //How long the projectile exists
    [SerializeField]
    float lifespan;

    [SerializeField]
    Vector2 force;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //Vector2 desiredVelocity = dir * speed;

        //rb.velocity += desiredVelocity;
        force = transform.forward * speed;


        rb.AddForce(force, ForceMode2D.Impulse);

        Destroy(gameObject, lifespan);
    }

    //If entering the player
    //Some method for collision with player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            //Before destroying self, inflict damage to player

            Destroy(gameObject);

            Debug.Log("Hit");
        }
    }
}
