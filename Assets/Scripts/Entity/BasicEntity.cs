using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEntity : MonoBehaviour
{
    [SerializeField]
    protected float movementSpeed = 1;

	public GameObject menu;
	public Slider size;

    [SerializeField]
    protected AnimationCurve speedCurve;

    [Header("Seek Distnaces")]
    [SerializeField]
    protected float maxSeekDistance = 1;
    [SerializeField]
    protected float minSeekDistance = 1;


    [SerializeField]
    protected SpriteRenderer sr;

    [SerializeField]
    protected Transform player;

    protected Rigidbody2D rb;

    protected Vector2 facing;

    public Vector2 Facing => facing;

    public float distance;

    public void SaveEntity(out SaveEntity entity)
    {
        entity = new SaveEntity(movementSpeed, menu, size, speedCurve, maxSeekDistance, minSeekDistance, sr, player, rb, facing, gameObject, gameObject.transform.position, gameObject);
    }

    public void LoadEntity()
    {

    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        facing = new Vector2(1, 0);
		menu = GameObject.Find("Menus");
		size = menu.GetComponentInChildren<Slider>(true);
        player = GameObject.FindObjectOfType<PlayerMovement>().transform;
    }

    protected virtual void Update()
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
    protected virtual void FixedUpdate()
    {
        /*float*/ distance = Vector2.Distance(transform.position, player.position);
        if (distance < maxSeekDistance && distance > minSeekDistance)
            Seek(player);
        else
        {
            if (rb!= null && rb.velocity != null && rb.velocity.magnitude > 0)
                rb.velocity += -rb.velocity * 0.25f;
        }
    }

    protected virtual void Seek(Transform target)
    {
        var dist = Vector2.Distance(target.position, transform.position);

		var desiredVelocity = (target.position - transform.position) * movementSpeed * dist;//speedCurve.Evaluate((dist / (maxSeekDistance - minSeekDistance)));
        desiredVelocity.z = 0;

        var move = new Vector2(desiredVelocity.x, desiredVelocity.y) - rb.velocity;

        if (rb.velocity.magnitude > 0.1)
            sr.flipX = rb.velocity.x < 0;

        rb.velocity += move;
    }
}

[System.Serializable]
public class SaveEntity
{
	public float speed;
	public GameObject menu;
	public Slider size;
	public AnimationCurve curve;
	float maxDistance;
	float minDistance;
	public SpriteRenderer renderer;
	public Transform player;
	public Rigidbody2D body;
	Vector2 facing;

	//Getting the gameobject the script is on and its position to later instantiate it
	public GameObject self;
	public Vector3 pos;

	//For tracking the scene it's in; used for persistence and making sure it only spawns in inside of the scene it was left
	public string sceneName;

	public string type;
	public GameObject gameObject;//Gameobject this entity was on. Used for getting its type

	public SaveEntity(float mS, GameObject m, Slider s, AnimationCurve sC, float maxD, float minD, SpriteRenderer sR, Transform p, Rigidbody2D rB, Vector2 f, GameObject self, Vector3 pos, GameObject gameObject)
	{
		speed = mS;
		menu = m;
		size = s;
		curve = sC;
		maxDistance = maxD;
		minDistance = minD;
		renderer = sR;
		player = p;
		body = rB;
		facing = f;
		this.self = self;
		this.pos = pos;
		this.gameObject = gameObject;
	}
}

[System.Serializable]
public class SaveInventoryEntity : SaveEntity
{
	public List<int> inventory;
	public SaveInventoryEntity(float mS, GameObject m, Slider s, AnimationCurve sC, float maxD, float minD, SpriteRenderer sR, Transform p, Rigidbody2D rB, Vector2 f, GameObject self, Vector3 pos, GameObject gameObject, Inventory inv) : base(mS, m, s, sC, maxD, minD, sR, p, rB, f, self, pos, gameObject)
	{
		inventory = inv.GetSaveableInventory();
		Debug.Log(inventory);
	}
}