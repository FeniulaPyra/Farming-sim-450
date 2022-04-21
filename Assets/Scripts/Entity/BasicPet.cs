using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
