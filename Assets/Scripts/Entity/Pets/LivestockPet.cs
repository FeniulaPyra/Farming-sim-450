using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivestockPet : BasicPet
{
    //Item they spawn
    public GameObject item;

    //Timer on which the item spawns
    /*public float timer;
    public float baseTimer;*/

    //chance of item spawning
    public int spawnChance;

    [SerializeField]
    //PlayerInteraction interaction;

    public int staminaExpended;
    public int staminaThreshold;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        //Allows it to be set on a pet by pet basis, but default of 10
        if (staminaThreshold <= 0)
        {
            staminaThreshold = 20;
        }

        FindObjectOfType<PlayerInteraction>().livestocks.Add(this);

        if (GlobalGameSaving.Instance != null && ScenePersistence.Instance != null)
        {
            if (GlobalGameSaving.Instance.loadingSave == false && ScenePersistence.Instance.changingScene == false)
            {
                //timer = baseTimer;
                staminaExpended = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        //Stamina spawning
        if (staminaExpended >= staminaThreshold)
        {
            Instantiate(item, transform.position, Quaternion.identity);

            staminaExpended -= staminaThreshold;
        }

        //timer and spawning
        /*timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            int rand = Random.Range(0, 101);

            if (rand <= spawnChance)
            {
                Instantiate(item, transform.position, Quaternion.identity);
            }

            timer = baseTimer;
        }*/
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnMouseEnter()//private void OnMouseEnter()
    {
        base.OnMouseEnter();
    }

    private void OnMouseExit()//private void OnMouseExit()
    {
        base.OnMouseExit();
    }

    private void OnMouseOver()//private void OnMouseOver()
    {
        base.OnMouseOver();
    }

    public void SaveLivestockPet(out SaveLivestockPet livestockPet)
    {
        //livestockPet = new SaveLivestockPet(movementSpeed, menu, size, speedCurve, maxSeekDistance, minSeekDistance, sr, player, rb, facing, gameObject, gameObject.transform.position, normalImage, pettingImage, petItem, manager, gameObject, item, timer, baseTimer, spawnChance);
        livestockPet = new SaveLivestockPet(movementSpeed, menu, size, speedCurve, maxSeekDistance, minSeekDistance, sr, player, rb, facing, gameObject, gameObject.transform.position, normalImage, pettingImage, petItem, manager, gameObject, item, spawnChance);
    }
}

[System.Serializable]
public class SaveLivestockPet : SavePet
{
    //Item they spawn
    public GameObject item;
    //Timer on which the item spawns
    public float timer;
    public float baseTimer;
    //chance of item spawning
    public int spawnChance;

    //public SaveLivestockPet(float mS, GameObject m, Slider s, AnimationCurve sC, float maxD, float minD, SpriteRenderer sR, Transform p, Rigidbody2D rB, Vector2 f, GameObject self, Vector3 pos, Sprite n, Sprite petting, Item pet, FarmManager manager, GameObject gameObject, GameObject i, float t, float bT, int chance) : base(mS, m, s, sC, maxD, minD, sR, p, rB, f, self, pos, n, petting, pet, manager, gameObject)
    public SaveLivestockPet(float mS, GameObject m, Slider s, AnimationCurve sC, float maxD, float minD, SpriteRenderer sR, Transform p, Rigidbody2D rB, Vector2 f, GameObject self, Vector3 pos, Sprite n, Sprite petting, Item pet, FarmManager manager, GameObject gameObject, GameObject i, int chance, float t = 0, float bT = 0) : base(mS, m, s, sC, maxD, minD, sR, p, rB, f, self, pos, n, petting, pet, manager, gameObject)
    {
        item = i;
        timer = t;
        baseTimer = bT;
        spawnChance = chance;
    }
}
