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
            if (!manager.playerInventory.IsTooFull(petItem, 1))
            {
                manager.playerInventory.AddItems(petItem, 1);
                Object.Destroy(this.gameObject);
            }
        }
    }
}
