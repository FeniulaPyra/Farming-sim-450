using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BasicPet : BasicEntity  
{

    [Header("Sprites")]
    [SerializeField] public Sprite normalImage;
    [SerializeField] public Sprite pettingImage;

    [Header("Inventory & Items")]
    [SerializeField] public Item petItem;
    [SerializeField] public FarmManager manager;

	private PlayerInventoryManager pim;
    //Being set in start. Should be fine to not be saved
    public PlayerInteraction owner;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
		pim = player.gameObject.GetComponent<PlayerInventoryManager>();
        manager = GameObject.Find("ManagerObject").GetComponent<FarmManager>();
        owner = FindObjectOfType<PlayerInteraction>();
    }

    // Update is called once per frame
    protected override void Update()
    {
		base.Update();
		if (size) this.transform.localScale = new Vector3(size.value, size.value, 1);
		if (distance > maxSeekDistance * 2)
		{
			transform.position = player.transform.position;
		}
	}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void OnMouseEnter()//private void OnMouseEnter()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = pettingImage;
    }

    protected void OnMouseExit()//private void OnMouseExit()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = normalImage;
    }

    void OnInteraction()
    {
        /*if (!pim.inv.IsTooFull(petItem, 1))
        {
            pim.inv.AddItems(petItem, 1);
            Object.Destroy(this.gameObject);

            owner.petCount -= 1;
            owner.ableToPlacePet = false;

        }*/
    }

    protected void OnMouseOver()//private void OnMouseOver()
    {
        //OnInteraction();

        if (Keyboard.current.spaceKey.wasPressedThisFrame == true || Mouse.current.leftButton.wasPressedThisFrame == true)
        {
            if (pim != null)
            {
                if (!pim.inv.IsTooFull(petItem, 1))
                {
                    pim.inv.AddItems(petItem, 1);
                    Object.Destroy(this.gameObject);

                    owner.petCount -= 1;
                    owner.ableToPlacePet = false;

                }
            }
        }
    }

    public void SavePet(out SavePet pet)
    {
        pet = new SavePet(movementSpeed, menu, size, speedCurve, maxSeekDistance, minSeekDistance, sr, player, rb, facing, gameObject, gameObject.transform.position, normalImage, pettingImage, petItem, manager, gameObject);
    }
}

[System.Serializable]
public class SavePet : SaveEntity
{

    public Sprite normal;
    public Sprite petting;
    public Item pet;
    public FarmManager manager;

    public SavePet(float mS, GameObject m, Slider s, AnimationCurve sC, float maxD, float minD, SpriteRenderer sR, Transform p, Rigidbody2D rB, Vector2 f, GameObject self, Vector3 pos, Sprite n, Sprite petting, Item pet, FarmManager manager, GameObject gameObject) : base(mS, m, s, sC, maxD, minD, sR, p, rB, f, self, pos, gameObject)
    {
        normal = n;
        this.petting = petting;
        this.pet = pet;
        this.manager = manager;
    }
}
