using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5;
    public float MovementSpeed
    {
        get
        {
            return movementSpeed;
        }
        set
        {
            movementSpeed = value;
        }
    }

    [SerializeField]
    private SpriteRenderer sr;

    private Rigidbody2D rb;
    private Vector2 direction;

	public Sprite left;
	public Sprite right;
	public Sprite up;
	public Sprite down;

    private Vector2 facing;
    public Vector2 Facing => facing;

    private bool frozen;
    public bool Frozen
    {
        get => frozen;
        set
        {
            frozen = value;

            rb.velocity = Vector2.zero;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        facing = new Vector2(1, 0);
    }

    void OnMovement(InputValue value)
    {
        direction = new Vector2(value.Get<Vector2>().x, value.Get<Vector2>().y);

        if (value.Get<Vector2>().y == 1) sr.sprite = up;
        else if (value.Get<Vector2>().x == -1) sr.sprite = left;
        else if (value.Get<Vector2>().x == -1) sr.sprite = down;
        else if (value.Get<Vector2>().x == 1) sr.sprite = right;
    }

    void Update()
    {
        //direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (direction.magnitude > 1)
            direction = direction.normalized;

        if (frozen) return;

        if (direction.magnitude > 0.1)
        {
            facing = direction.normalized;

            if (facing.y != 0 && facing.x != 0)
            {
                if (facing.x < facing.y) facing.x = 0;
                else facing.y = 0;
            }
        }

		//matches sprite to movement
		Debug.Log("dir x " + direction.x);
		Debug.Log("dir Y " + direction.y);
		/*if (Input.GetKeyDown(KeyCode.W)) sr.sprite = up;
		else if (Input.GetKeyDown(KeyCode.A)) sr.sprite = left;
		else if (Input.GetKeyDown(KeyCode.S)) sr.sprite = down;
		else if (Input.GetKeyDown(KeyCode.D)) sr.sprite = right;*/

    }

    private void FixedUpdate()
    {
        if (frozen) return;

        var desiredVelocity = direction * movementSpeed;
        var move = desiredVelocity - rb.velocity;
		/*
        if (rb.velocity.magnitude > 0.1)
            sr.flipX = rb.velocity.x < 0;*/

        rb.velocity += move;
    }
}
