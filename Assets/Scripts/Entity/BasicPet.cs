using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicPet : BasicEntity  
{

    [Header("Sprites")]
    [SerializeField] public Sprite normalImage;
    [SerializeField] public Sprite pettingImage;

    [Header("Inventory & Items")]
    [SerializeField] public Item petItem;
    [SerializeField] public FarmManager manager;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        manager = GameObject.Find("ManagerObject").GetComponent<FarmManager>();
    }

    // Update is called once per frame
    protected override void Update()
    {
		base.Update();
		if (size) this.transform.localScale = new Vector3(size.value, size.value, 1);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnMouseEnter()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = pettingImage;
    }

    private void OnMouseExit()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = normalImage;
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ItemStack items = new ItemStack(petItem, 1);
            if (!manager.playerInventory.IsTooFull(items))
            {
                manager.playerInventory.AddItems(items);
                Object.Destroy(this.gameObject);
            }
        }
    }

    public void SavePet(out SavePet pet)
    {
        pet = new SavePet(movementSpeed, menu, size, speedCurve, maxSeekDistance, minSeekDistance, sr, player, rb, facing, gameObject, gameObject.transform.position, normalImage, pettingImage, petItem, manager);
    }
}

[System.Serializable]
public class SavePet : SaveEntity
{

    public Sprite normal;
    public Sprite petting;
    public Item pet;
    public FarmManager manager;

    public SavePet(float mS, GameObject m, Slider s, AnimationCurve sC, float maxD, float minD, SpriteRenderer sR, Transform p, Rigidbody2D rB, Vector2 f, GameObject self, Vector3 pos, Sprite n, Sprite petting, Item pet, FarmManager manager) : base(mS, m, s, sC, maxD, minD, sR, p, rB, f, self, pos)
    {
        normal = n;
        this.petting = petting;
        this.pet = pet;
        this.manager = manager;
    }
}
