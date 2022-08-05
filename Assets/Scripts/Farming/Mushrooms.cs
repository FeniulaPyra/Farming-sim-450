using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Mushrooms : Tile
{
    #region Life and Death Variables
    //constant that defines the greatest stage of growth. Once the mushroom is at this stage, it's fully grown
    //constant cannot be public, and therefore cannot be shared within another script
    public int maxGrowthStage;// = 3;

    public int GetMaxGrowthStage()
    {
        return maxGrowthStage;
    }

    //constant that defines how many days the mushroom can survive being neglected. If the number of days is ever greater than this, the mushroom has died
    public int maxDaysWithoutWater = 2;
	public int MaxDaysWithoutWater
	{
		get
		{
			return maxDaysWithoutWater + (int)pSkills.SumSkillsOfType<MushroomThirstSkill>(new List<GameObject> { pSkills.time.gameObject });
		}
	}
    //represents the mushroom's current stage of growth
    public float growthStage = 1.00f;
    //number of days without water
    public int daysWithoutWater;

    public int daysSinceFullyGrown;

    public bool readyToDie;
    #endregion

    #region Miscellaneous Variables 
    //The id/name of the mushroom, //The base monetary value of the mushroom
    public string ID = "Super Shroom";
    public int baseValue = 500;

    //For inventory interaction
    public GameObject mushroomItem;
    public Sprite spr;
    #endregion

    #region Hybridization

    //List of names of mushrooms you can hybridize with
    public List<string> mushroomsToHybridize = new List<string>();
    //List of gameobjects that are the resultant hybrids
    public List<GameObject> mushroomHybrids = new List<GameObject>();
    //Dictionary that uses the above to keep track of things
    public Dictionary<string, GameObject> hybridDictionary = new Dictionary<string, GameObject>();

	public PlayerSkills pSkills;

	#endregion

	private SpriteRenderer bubble;
	/// <summary>
	/// 0 - water
	/// 1 - sickle
	/// 3 - zzzz
	/// 4 - going to die
	/// </summary>
	public List<Sprite> popupBubbles;

	public Toggle ShowPopupBubblesToggle;

    // Start is called before the first frame update
    void Awake()
    {
		//Setting item's values
		/*mushroomItem = new Item();
        mushroomItem.name = ID;
        mushroomItem.spr = spr;*/

		bubble = this.gameObject.AddComponent<SpriteRenderer>();
		bubble.transform.position = new Vector3(bubble.transform.position.x, bubble.transform.position.y + .5f, 10);
		bubble.transform.localScale = new Vector3(.5f, .5f, 1);
		ShowPopupBubblesToggle = GameObject.Find("Menus").GetComponent<Menu>().BubbleToggle.GetComponent<Toggle>();
		pSkills = GameObject.Find("Player").GetComponent<PlayerSkills>();
		bubble.sortingLayerName = "Popup Bubbles";
		bubble.color = new Color(bubble.color.r, bubble.color.g, bubble.color.b, .75f);
		//populating hybrid dictionary
		for (int i = 0; i < mushroomsToHybridize.Count; i++)
        {
            hybridDictionary[mushroomsToHybridize[i]] = mushroomHybrids[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ScenePersistence.Instance.gamePaused == false)
        {
            //show bubble
            if (daysWithoutWater >= MaxDaysWithoutWater - 1 && !isMoist)
		    {
		    	bubble.sprite = popupBubbles[3];
		    }
		    else if(isMoist)
		    {
		    	bubble.sprite = popupBubbles[2];
		    }
		    else if (growthStage >= maxGrowthStage)
		    {
		    	bubble.sprite = popupBubbles[1];
		    }
		    else
		    {
		    	bubble.sprite = popupBubbles[0];
		    }
		    bubble.enabled = ShowPopupBubblesToggle.isOn;
        }
    }

    //Method where the mushrooms check to see if they grow or not
    public void GrowMushroom()
    {
        //Nested if branch to see what the mushroom does
        if (isMoist == true)
        {
            if (growthStage < maxGrowthStage)
            {
                growthStage++;
                //tileSprite = sprites[Mathf.FloorToInt(growthStage - 1)];
            }

            daysWithoutWater = 0;

            Debug.Log($"I'm at growth stage {growthStage} and it's been {daysWithoutWater} days since I've been watered.");
        }
        else
        {
            daysWithoutWater++;

            if (daysWithoutWater >= MaxDaysWithoutWater)
            {
				this.isTilled = false;
				this.isMoist = false;
				hasPlant = false;
                //Destroy(gameObject);
                readyToDie = true;
            }

            Debug.Log($"I'm at growth stage {growthStage} and it's been {daysWithoutWater} days since I've been watered.");
        }

        if (growthStage >= maxGrowthStage)
        {
            daysSinceFullyGrown++;
            tileSprite = sprites[Mathf.FloorToInt(growthStage - 1)];
        }
        #region Alternative mushroom growth
        //Mushrooms still grow if not moist, but more slowly
        /*else
        {
            if (growthStage < maxGrowthStage)
            {
                growthStage+= 0.5f;
            }

            daysWithoutWater++;

            if (daysWithoutWater > maxDaysWithoutWater)
            {
                Destroy(gameObject);
            }

            Debug.Log($"I'm at growth stage {growthStage} and it's been {daysWithoutWater} days since I've been watered.");
        }*/
        #endregion
    }

    public new MushroomSaveTile AsSaveTile()
    {
        return new MushroomSaveTile(
            isTilled, 
            isMoist, 
            hasPlant, 
            position,
            sprites.IndexOf(tileSprite),
            growthStage,
            maxGrowthStage,
            daysWithoutWater,
            maxDaysWithoutWater,
            readyToDie,
            ID,
            baseValue,
			daysSinceFullyGrown
            );
    }
}

[System.Serializable]
public class MushroomSaveTile : SaveTile
{
    public float growthStage;
    public int maxGrowthStage;
    public int daysWithoutWater;
	public int daysSinceFullyGrown;
    public int maxDaysWithoutWater;
    public bool readyToDie;
    public string ID;
    public int baseValue;

    public MushroomSaveTile(bool isTilled, bool isMoist, bool hasPlant, Vector3Int position, int spriteIndex, float growthStage, int maxGrowthStage, int daysWithoutWater, int maxDaysWithoutWater, bool readyToDie, string iD, int baseValue, int daysSinceFullyGrown) : base(isTilled, isMoist, hasPlant, position, spriteIndex)
    {
        this.growthStage = growthStage;
        this.maxGrowthStage = maxGrowthStage;
        this.daysWithoutWater = daysWithoutWater;
        this.daysSinceFullyGrown = daysSinceFullyGrown;
        this.maxDaysWithoutWater = maxDaysWithoutWater;
        this.readyToDie = readyToDie;
        ID = iD;
        this.baseValue = baseValue;
    }
}
