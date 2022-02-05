using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEntity : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 1;

    [SerializeField]
    private AnimationCurve speedCurve;

    [SerializeField]
    private float seekDistance = 1;

    [SerializeField]
    private SpriteRenderer sr;

    [SerializeField]
    private Transform player;

    private Rigidbody2D rb;

    private Vector2 facing;

    public Vector2 Facing => facing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        facing = new Vector2(1, 0);
    }

    void Update()
    {
        if (rb.velocity.magnitude > 0.1)
        {
            facing = rb.velocity.normalized;

            if (facing.y != 0 && facing.x != 0)
            {
                facing.y = 0;
            }
        }
    }
    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, player.position) < seekDistance)
            Seek(player);
        else
        {
            if (rb.velocity.magnitude > 0)
                rb.velocity += -rb.velocity * 0.25f;
        }
    }

    private void Seek(Transform target)
    {
        var dist = Vector2.Distance(target.position, transform.position);

        var desiredVelocity = (target.position - transform.position) * movementSpeed * speedCurve.Evaluate(1 - (dist / seekDistance));
        desiredVelocity.z = 0;

        var move = new Vector2(desiredVelocity.x, desiredVelocity.y) - rb.velocity;

        if (rb.velocity.magnitude > 0.1)
            sr.flipX = rb.velocity.x < 0;

        rb.velocity += move;
    }
}