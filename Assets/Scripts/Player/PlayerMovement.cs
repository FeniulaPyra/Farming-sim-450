using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5;

    [SerializeField]
    private SpriteRenderer sr;

    private Rigidbody2D rb;
    private Vector2 direction;

    private Vector2 facing;

    public Vector2 Facing => facing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        facing = Vector2.one;
    }

    void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (direction.magnitude > 1)
            direction = direction.normalized;

        if (rb.velocity.magnitude > 0.1)
            facing = rb.velocity.normalized;
    }

    private void FixedUpdate()
    {
        var desiredVelocity = direction * movementSpeed;
        var move = desiredVelocity - rb.velocity;

        sr.flipX = desiredVelocity.x < 0;

        rb.velocity += move;
    }
}
