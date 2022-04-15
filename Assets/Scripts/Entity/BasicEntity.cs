using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEntity : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 1;

	public Slider size;

    [SerializeField]
    private AnimationCurve speedCurve;

    [Header("Seek Distnaces")]
    [SerializeField]
    private float maxSeekDistance = 1;
    [SerializeField]
    private float minSeekDistance = 1;


    [SerializeField]
    private SpriteRenderer sr;

    [SerializeField]
    private Transform player;

    [Header("Sprites")]
    [SerializeField] public Sprite normalImage;
    [SerializeField] public Sprite pettingImage;

    private Rigidbody2D rb;

    private Vector2 facing;

    public Vector2 Facing => facing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        facing = new Vector2(1, 0);

        player = GameObject.FindObjectOfType<PlayerMovement>().transform;
    }

    void Update()
    {
		if (size) this.transform.localScale = new Vector3(size.value, size.value, 1);

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
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < maxSeekDistance && distance > minSeekDistance)
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

        var desiredVelocity = (target.position - transform.position) * movementSpeed * speedCurve.Evaluate((dist / (maxSeekDistance - minSeekDistance)));
        desiredVelocity.z = 0;

        var move = new Vector2(desiredVelocity.x, desiredVelocity.y) - rb.velocity;

        if (rb.velocity.magnitude > 0.1)
            sr.flipX = rb.velocity.x < 0;

        rb.velocity += move;
    }

    private void OnMouseEnter()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = pettingImage;
    }

    private void OnMouseExit()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = normalImage;
    }
}
