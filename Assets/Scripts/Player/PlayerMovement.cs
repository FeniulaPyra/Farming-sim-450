using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5;
    public float MovementSpeed
    {
        get
        {
			return movementSpeed * (1 + gameObject.GetComponent<PlayerSkills>().SumSkillsOfType<IncreaseSpeedSkill>(null));
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

	public List<string> visited;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
	{
		//rb = GetComponent<Rigidbody2D>();
		facing = new Vector2(1, 0);
		//visited = new List<string>();
		if (GlobalGameSaving.Instance != null)
		{
			if (GlobalGameSaving.Instance.loadingSave == true)
			{
				visited = GlobalGameSaving.Instance.mapsVisited;
			}
			
		}

		if (ScenePersistence.Instance.playerDropPoint.x < Int32.MaxValue)
		{
			//+ new vector because for some reason it is moving the player down by this amoutnt every time the scene loads :(
			transform.position = ScenePersistence.Instance.playerDropPoint + new Vector2(2, 3);
		}
		if (ScenePersistence.Instance.changingScene)
		{
			visited = ScenePersistence.Instance.mapsVisited;
		}

		

		//enables the foraging only after visited maps have been loaded.
		ForagingManager forageManager = GameObject.FindObjectOfType<ForagingManager>();
		if(forageManager != null) forageManager.TrySpawn();

		Scene currentScene = SceneManager.GetActiveScene();
		if (!visited.Contains("" + currentScene.name))
		{
			visited.Add(SceneManager.GetActiveScene().name);
			//do thing like advance day and create foragables
			switch(currentScene.name)
			{
				case "GroundScene":
					GameObject.FindObjectOfType<TimeManager>().UpdateField();
					break;
				default:
					//foragables, reset npcs, etc
					break;
			}
		}
    }

	public void SaveVisited()
	{
		GlobalGameSaving.Instance.mapsVisited = visited;
	}

    void OnMovement(InputValue value)
    {
        direction = new Vector2(value.Get<Vector2>().x, value.Get<Vector2>().y);

        if (value.Get<Vector2>().y == 1) sr.sprite = up;
        else if (value.Get<Vector2>().x == -1) sr.sprite = left;
        else if (value.Get<Vector2>().y == -1) sr.sprite = down;
        else if (value.Get<Vector2>().x == 1) sr.sprite = right;

        if (frozen) return;

        var desiredVelocity = direction * MovementSpeed;
        var move = desiredVelocity - rb.velocity;
        /*
        if (rb.velocity.magnitude > 0.1)
            sr.flipX = rb.velocity.x < 0;*/

        rb.velocity += move;
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
        /*if (frozen) return;

        var desiredVelocity = direction * movementSpeed;
        var move = desiredVelocity - rb.velocity;
		/*
        if (rb.velocity.magnitude > 0.1)
            sr.flipX = rb.velocity.x < 0;*/

        //rb.velocity += move;
    }
}
