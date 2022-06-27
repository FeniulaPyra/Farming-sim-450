using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class EntityManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GlobalGameSaving.Instance != null)
        {
            if (GlobalGameSaving.Instance.loadingSave == true)
            {
				//deletes any existing entities
				List<BasicEntity> entities = FindObjectsOfType<BasicEntity>().ToList();
				foreach(BasicEntity e in entities)
				{
					if(e is InventoryEntity || e is BasicPet)
					{
						Destroy(e.gameObject);
					}
				}
    
				//instantiates all the entities from the save.
				if (GlobalGameSaving.Instance.pets.Count > 0)
                {
                    for (int i = 0; i < GlobalGameSaving.Instance.pets.Count; i++)
                    {
                        //Close to the player's position, so the pet spawns near them
                        Vector3 playerPos = GameObject.Find("Player").transform.position;
                        Vector3 pos = new Vector3(playerPos.x + 2, playerPos.y, playerPos.z);
                        Instantiate(Resources.Load($"Prefabs/Pets/{GlobalGameSaving.Instance.petNames[i]}"), pos, Quaternion.identity);
                    }
                }
    
                if (GlobalGameSaving.Instance.livestockPets.Count > 0)
                {
                    for (int i = 0; i < GlobalGameSaving.Instance.livestockPets.Count; i++)
                    {
                        //Close to the player's position, so the pet spawns near them
                        Vector3 playerPos = GameObject.Find("Player").transform.position;
                        Vector3 pos = new Vector3(playerPos.x + 2, playerPos.y, playerPos.z);
                        GameObject g = (GameObject)Instantiate(Resources.Load($"Prefabs/Pets/{GlobalGameSaving.Instance.livestockPetNames[i]}"), pos, Quaternion.identity);
                        LivestockPet l = g.GetComponent<LivestockPet>();
                        l.timer = GlobalGameSaving.Instance.livestockPets[i].timer;
                    }
                }
    
                if (GlobalGameSaving.Instance.buffPets.Count > 0)
                {
                    for (int i = 0; i < GlobalGameSaving.Instance.buffPets.Count; i++)
                    {
                        //Close to the player's position, so the pet spawns near them
                        Vector3 playerPos = GameObject.Find("Player").transform.position;
                        Vector3 pos = new Vector3(playerPos.x + 2, playerPos.y, playerPos.z);
                        GameObject g = (GameObject)Instantiate(Resources.Load($"Prefabs/Pets/{GlobalGameSaving.Instance.buffPetNames[i]}"), pos, Quaternion.identity);
                        BuffPet b = g.GetComponent<BuffPet>();
                        b.timer = GlobalGameSaving.Instance.buffPets[i].timer;
                        b.buffTimer = GlobalGameSaving.Instance.buffPets[i].buffTimer;
                        b.buffApplied = GlobalGameSaving.Instance.buffPets[i].buffApplied;
                    }
                }
				Debug.Log($"LEP2738 LOADING THING: {GlobalGameSaving.Instance.entities.Count}");
    
				if (GlobalGameSaving.Instance.entities.Count > 0)
                {
                    for (int i = 0; i < GlobalGameSaving.Instance.entities.Count; i++)
                    {
						Debug.Log($"LEP2738 LOADING THING: {GlobalGameSaving.Instance.entityNames[i]}");
                        //Close to the player's position, so the pet spawns near them
                        /*Vector3 playerPos = GameObject.Find("Player").transform.position;
                        Vector3 pos = new Vector3(playerPos.x + 2, playerPos.y, playerPos.z);*/
                        if (SceneManager.GetActiveScene().name == GlobalGameSaving.Instance.entities[i].sceneName)
                        {
							
							switch (GlobalGameSaving.Instance.entities[i].type)
                            {
                                case "tool":
                                    Instantiate(Resources.Load($"Prefabs/Tools/{GlobalGameSaving.Instance.entityNames[i]}"), GlobalGameSaving.Instance.entities[i].pos, Quaternion.identity);
                                    break;
                                case "mushroom":
                                    Instantiate(Resources.Load($"Prefabs/MushroomPrefabs/{GlobalGameSaving.Instance.entityNames[i]}"), GlobalGameSaving.Instance.entities[i].pos, Quaternion.identity);
                                    break;
                                default:
                                    break;
                            }
    
                            GlobalGameSaving.Instance.entities.RemoveAt(GlobalGameSaving.Instance.entities.IndexOf(GlobalGameSaving.Instance.entities[i]));
                            GlobalGameSaving.Instance.entityNames.RemoveAt(GlobalGameSaving.Instance.entityNames.IndexOf(GlobalGameSaving.Instance.entityNames[i]));
    
                        }
                    }
    
                    //Clears after spawning them, so you can't go back and forth for infinite entities
                    //ScenePersistence.Instance.entities.Clear();
                    //ScenePersistence.Instance.entityNames.Clear();
                }
				
				if(GlobalGameSaving.Instance.inventoryEntities.Count > 0)
				{
					for (int i = 0; i < GlobalGameSaving.Instance.inventoryEntities.Count; i++)
					{
						Debug.Log($"LEP2738 LOADING THING: {GlobalGameSaving.Instance.inventoryEntityNames[i]}");
						//Close to the player's position, so the pet spawns near them
						/*Vector3 playerPos = GameObject.Find("Player").transform.position;
                        Vector3 pos = new Vector3(playerPos.x + 2, playerPos.y, playerPos.z);*/
						if (SceneManager.GetActiveScene().name == GlobalGameSaving.Instance.inventoryEntities[i].sceneName)
						{
							GameObject inv = (GameObject)Instantiate(Resources.Load($"Prefabs/Interactable Objects/Chest"), GlobalGameSaving.Instance.inventoryEntities[i].pos, Quaternion.identity);
							inv.GetComponent<InventoryEntity>().inv.SetSaveableInventory(GlobalGameSaving.Instance.inventoryEntities[i].inventory);
							inv.transform.parent = GameObject.Find("PlayerInputManager").transform;
							GlobalGameSaving.Instance.inventoryEntities.RemoveAt(GlobalGameSaving.Instance.inventoryEntities.IndexOf(GlobalGameSaving.Instance.inventoryEntities[i]));
							GlobalGameSaving.Instance.inventoryEntityNames.RemoveAt(GlobalGameSaving.Instance.inventoryEntityNames.IndexOf(GlobalGameSaving.Instance.inventoryEntityNames[i]));
						}
					}
				}
			}
            else if (ScenePersistence.Instance != null)
            {
				List<BasicEntity> entities = FindObjectsOfType<BasicEntity>().ToList();
				foreach (BasicEntity e in entities)
				{
					if (e is InventoryEntity || e is BasicPet)
					{
						Destroy(e.gameObject);
					}
				}
				if (ScenePersistence.Instance.pets.Count > 0)
                {
                    for (int i = 0; i < ScenePersistence.Instance.pets.Count; i++)
                    {
                        //Close to the player's position, so the pet spawns near them
                        Vector3 playerPos = GameObject.Find("Player").transform.position;
                        Vector3 pos = new Vector3(playerPos.x + 2, playerPos.y, playerPos.z);
                        Instantiate(Resources.Load($"Prefabs/Pets/{ScenePersistence.Instance.petNames[i]}"), pos, Quaternion.identity);
                    }
                }
    
                if (ScenePersistence.Instance.livestockPets.Count > 0)
                {
                    for (int i = 0; i < ScenePersistence.Instance.livestockPets.Count; i++)
                    {
                        //Close to the player's position, so the pet spawns near them
                        Vector3 playerPos = GameObject.Find("Player").transform.position;
                        Vector3 pos = new Vector3(playerPos.x + 2, playerPos.y, playerPos.z);
                        GameObject g = (GameObject)Instantiate(Resources.Load($"Prefabs/Pets/{ScenePersistence.Instance.livestockPetNames[i]}"), pos, Quaternion.identity);
                        LivestockPet l = g.GetComponent<LivestockPet>();
                        l.timer = ScenePersistence.Instance.livestockPets[i].timer;
                    }
                }
    
                if (ScenePersistence.Instance.buffPets.Count > 0)
                {
                    for (int i = 0; i < ScenePersistence.Instance.buffPets.Count; i++)
                    {
                        //Close to the player's position, so the pet spawns near them
                        Vector3 playerPos = GameObject.Find("Player").transform.position;
                        Vector3 pos = new Vector3(playerPos.x + 2, playerPos.y, playerPos.z);
                        GameObject g = (GameObject)Instantiate(Resources.Load($"Prefabs/Pets/{ScenePersistence.Instance.buffPetNames[i]}"), pos, Quaternion.identity);
                        BuffPet b = g.GetComponent<BuffPet>();
                        b.timer = ScenePersistence.Instance.buffPets[i].timer;
                        b.buffTimer = ScenePersistence.Instance.buffPets[i].buffTimer;
                        b.buffApplied = ScenePersistence.Instance.buffPets[i].buffApplied;
                    }
                }
    
				if(ScenePersistence.Instance.inventoryEntities.Count > 0)
				{
					for (int i = 0; i < ScenePersistence.Instance.inventoryEntities.Count; i++)
					{
						Debug.Log($"LEP2738 LOADING THING: {ScenePersistence.Instance.inventoryEntityNames[i]}");
						//Close to the player's position, so the pet spawns near them
						/*Vector3 playerPos = GameObject.Find("Player").transform.position;
                        Vector3 pos = new Vector3(playerPos.x + 2, playerPos.y, playerPos.z);*/
						if (SceneManager.GetActiveScene().name == ScenePersistence.Instance.inventoryEntities[i].sceneName)
						{
							GameObject inv = (GameObject)Instantiate(Resources.Load($"Prefabs/Interactable Objects/Chest"), ScenePersistence.Instance.inventoryEntities[i].pos, Quaternion.identity);
							inv.GetComponent<InventoryEntity>().inv.SetSaveableInventory(ScenePersistence.Instance.inventoryEntities[i].inventory);
							inv.transform.parent = GameObject.Find("PlayerInputManager").transform;
							ScenePersistence.Instance.inventoryEntities.RemoveAt(ScenePersistence.Instance.inventoryEntities.IndexOf(ScenePersistence.Instance.inventoryEntities[i]));
							ScenePersistence.Instance.inventoryEntityNames.RemoveAt(ScenePersistence.Instance.inventoryEntityNames.IndexOf(ScenePersistence.Instance.inventoryEntityNames[i]));
						}
					}
				}
    
                if (ScenePersistence.Instance.entities.Count > 0)
                {
                    for (int i = 0; i < ScenePersistence.Instance.entities.Count; i++)
                    {
                        //Close to the player's position, so the pet spawns near them
                        /*Vector3 playerPos = GameObject.Find("Player").transform.position;
                        Vector3 pos = new Vector3(playerPos.x + 2, playerPos.y, playerPos.z);*/
                        if (SceneManager.GetActiveScene().name == ScenePersistence.Instance.entities[i].sceneName)
                        {
                            switch (ScenePersistence.Instance.entities[i].type)
                            {
                                case "tool":
                                    Instantiate(Resources.Load($"Prefabs/Tools/{ScenePersistence.Instance.entityNames[i]}"), ScenePersistence.Instance.entities[i].pos, Quaternion.identity);
                                    break;
                                case "mushroom":
                                    Instantiate(Resources.Load($"Prefabs/MushroomPrefabs/{ScenePersistence.Instance.entityNames[i]}"), ScenePersistence.Instance.entities[i].pos, Quaternion.identity);
                                    break;
                                default:
                                    break;
                            }
    
                            ScenePersistence.Instance.entities.RemoveAt(ScenePersistence.Instance.entities.IndexOf(ScenePersistence.Instance.entities[i]));
                            ScenePersistence.Instance.entityNames.RemoveAt(ScenePersistence.Instance.entityNames.IndexOf(ScenePersistence.Instance.entityNames[i]));
    
                        }
                    }
    
                    //Clears after spawning them, so you can't go back and forth for infinite entities
                    ScenePersistence.Instance.pets.Clear();
                    ScenePersistence.Instance.petNames.Clear();
                    ScenePersistence.Instance.livestockPets.Clear();
                    ScenePersistence.Instance.livestockPetNames.Clear();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveLivestockPets()
    {
        //Get rid of any pets already in the list, so it doesn't spawn two dogs when the player goes through two scenes with their singular dog
        ScenePersistence.Instance.livestockPets.Clear();
        ScenePersistence.Instance.livestockPetNames.Clear();

        List<LivestockPet> pets = FindObjectsOfType<LivestockPet>().ToList();

        foreach (LivestockPet p in pets)
        {
            p.SaveLivestockPet(out var pet);
            ScenePersistence.Instance.livestockPets.Add(pet);
            if (ScenePersistence.Instance.livestockPets[ScenePersistence.Instance.livestockPets.Count - 1].self.name.Contains('('))
            {
                string[] name = ScenePersistence.Instance.livestockPets[ScenePersistence.Instance.livestockPets.Count - 1].self.name.Split('(');
                ScenePersistence.Instance.livestockPetNames.Add(name[0]);
            }
            else
            {
                ScenePersistence.Instance.livestockPetNames.Add(ScenePersistence.Instance.livestockPets[ScenePersistence.Instance.pets.Count - 1].self.name);
            }
        }
    }

    public void SavePets()
    {
        //Get rid of any pets already in the list, so it doesn't spawn two dogs when the player goes through two scenes with their singular dog
        ScenePersistence.Instance.pets.Clear();
        ScenePersistence.Instance.petNames.Clear();
        ScenePersistence.Instance.livestockPets.Clear();
        ScenePersistence.Instance.livestockPetNames.Clear();

        List<BasicPet> pets = FindObjectsOfType<BasicPet>().ToList();

        foreach (BasicPet p in pets)
        {
            if (p is LivestockPet)
            {
                LivestockPet l = (LivestockPet)p;
                l.SaveLivestockPet(out var pet);
                ScenePersistence.Instance.livestockPets.Add(pet);
                if (ScenePersistence.Instance.livestockPets[ScenePersistence.Instance.livestockPets.Count - 1].self.name.Contains('('))
                {
                    string[] name = ScenePersistence.Instance.livestockPets[ScenePersistence.Instance.livestockPets.Count - 1].self.name.Split('(');
                    ScenePersistence.Instance.livestockPetNames.Add(name[0]);
                }
                else
                {
                    ScenePersistence.Instance.livestockPetNames.Add(ScenePersistence.Instance.livestockPets[ScenePersistence.Instance.livestockPets.Count - 1].self.name);
                }
            }
            else
            {
                p.SavePet(out var pet);
                ScenePersistence.Instance.pets.Add(pet);
                if (ScenePersistence.Instance.pets[ScenePersistence.Instance.pets.Count - 1].self.name.Contains('('))
                {
                    string[] name = ScenePersistence.Instance.pets[ScenePersistence.Instance.pets.Count - 1].self.name.Split('(');
                    ScenePersistence.Instance.petNames.Add(name[0]);
                }
                else
                {
                    ScenePersistence.Instance.petNames.Add(ScenePersistence.Instance.pets[ScenePersistence.Instance.pets.Count - 1].self.name);
                }
            }
        }
    }

    public void SaveEntities(string what)
    {
        //Get rid of any pets already in the list, so it doesn't spawn two dogs when the player goes through two scenes with their singular dog
        //ScenePersistence.Instance.entities.Clear();
        //ScenePersistence.Instance.entityNames.Clear();

        List<BasicEntity> entities = FindObjectsOfType<BasicEntity>().ToList();

		foreach (BasicEntity e in entities)
		{
			Debug.Log("LEP2738 SAVING ENTITY: " + e.name);
			if (e is BasicPet)
			{
				BasicPet p = (BasicPet)e;

				if (p is LivestockPet)
				{
					if (what == "persist")
					{
						LivestockPet l = (LivestockPet)p;
						l.SaveLivestockPet(out var pet);
						ScenePersistence.Instance.livestockPets.Add(pet);
						if (ScenePersistence.Instance.livestockPets[ScenePersistence.Instance.livestockPets.Count - 1].self.name.Contains('('))
						{
							string[] name = ScenePersistence.Instance.livestockPets[ScenePersistence.Instance.livestockPets.Count - 1].self.name.Split('(');
							ScenePersistence.Instance.livestockPetNames.Add(name[0]);
						}
						else
						{
							ScenePersistence.Instance.livestockPetNames.Add(ScenePersistence.Instance.livestockPets[ScenePersistence.Instance.livestockPets.Count - 1].self.name);
						}
					}
					else if (what == "save")
					{
						LivestockPet l = (LivestockPet)p;
						l.SaveLivestockPet(out var pet);
						GlobalGameSaving.Instance.livestockPets.Add(pet);
						if (GlobalGameSaving.Instance.livestockPets[GlobalGameSaving.Instance.livestockPets.Count - 1].self.name.Contains('('))
						{
							string[] name = GlobalGameSaving.Instance.livestockPets[GlobalGameSaving.Instance.livestockPets.Count - 1].self.name.Split('(');
							GlobalGameSaving.Instance.livestockPetNames.Add(name[0]);
						}
						else
						{
							GlobalGameSaving.Instance.livestockPetNames.Add(GlobalGameSaving.Instance.livestockPets[GlobalGameSaving.Instance.livestockPets.Count - 1].self.name);
						}
					}

				}
				else if (p is BuffPet)
				{
					if (what == "persist")
					{
						BuffPet b = (BuffPet)p;
						b.SaveBuffPet(out var pet);
						ScenePersistence.Instance.buffPets.Add(pet);
						if (ScenePersistence.Instance.buffPets[ScenePersistence.Instance.buffPets.Count - 1].self.name.Contains('('))
						{
							string[] name = ScenePersistence.Instance.buffPets[ScenePersistence.Instance.buffPets.Count - 1].self.name.Split('(');
							ScenePersistence.Instance.buffPetNames.Add(name[0]);
						}
						else
						{
							ScenePersistence.Instance.buffPetNames.Add(ScenePersistence.Instance.buffPets[ScenePersistence.Instance.buffPets.Count - 1].self.name);
						}
					}
					else if (what == "save")
					{
						BuffPet b = (BuffPet)p;
						b.SaveBuffPet(out var pet);
						GlobalGameSaving.Instance.buffPets.Add(pet);
						if (GlobalGameSaving.Instance.buffPets[GlobalGameSaving.Instance.buffPets.Count - 1].self.name.Contains('('))
						{
							string[] name = GlobalGameSaving.Instance.buffPets[GlobalGameSaving.Instance.buffPets.Count - 1].self.name.Split('(');
							GlobalGameSaving.Instance.buffPetNames.Add(name[0]);
						}
						else
						{
							GlobalGameSaving.Instance.buffPetNames.Add(GlobalGameSaving.Instance.buffPets[GlobalGameSaving.Instance.buffPets.Count - 1].self.name);
						}
					}
				}
				else
				{
					if (what == "persist")
					{
						p.SavePet(out var pet);
						ScenePersistence.Instance.pets.Add(pet);
						if (ScenePersistence.Instance.pets[ScenePersistence.Instance.pets.Count - 1].self.name.Contains('('))
						{
							string[] name = ScenePersistence.Instance.pets[ScenePersistence.Instance.pets.Count - 1].self.name.Split('(');
							ScenePersistence.Instance.petNames.Add(name[0]);
						}
						else
						{
							ScenePersistence.Instance.petNames.Add(ScenePersistence.Instance.pets[ScenePersistence.Instance.pets.Count - 1].self.name);
						}
					}
					else if (what == "save")
					{
						p.SavePet(out var pet);
						GlobalGameSaving.Instance.pets.Add(pet);
						if (GlobalGameSaving.Instance.pets[GlobalGameSaving.Instance.pets.Count - 1].self.name.Contains('('))
						{
							string[] name = GlobalGameSaving.Instance.pets[GlobalGameSaving.Instance.pets.Count - 1].self.name.Split('(');
							GlobalGameSaving.Instance.petNames.Add(name[0]);
						}
						else
						{
							GlobalGameSaving.Instance.petNames.Add(GlobalGameSaving.Instance.pets[GlobalGameSaving.Instance.pets.Count - 1].self.name);
						}
					}

				}
			}
			else if (e is InventoryEntity)
			{

				if (what == "persist")
				{
					((InventoryEntity)e).SaveEntity(out SaveInventoryEntity entity);
					entity.type = "inventory";
					entity.sceneName = SceneManager.GetActiveScene().name;
					ScenePersistence.Instance.inventoryEntities.Add(entity);
					if (ScenePersistence.Instance.inventoryEntities[ScenePersistence.Instance.inventoryEntities.Count - 1].self.name.Contains('('))
					{
						string[] name = ScenePersistence.Instance.inventoryEntities[ScenePersistence.Instance.inventoryEntities.Count - 1].self.name.Split('(');
						ScenePersistence.Instance.inventoryEntityNames.Add(name[0]);
					}
					else
					{
						ScenePersistence.Instance.inventoryEntityNames.Add(ScenePersistence.Instance.inventoryEntities[ScenePersistence.Instance.inventoryEntities.Count - 1].self.name);
					}
				}
				else if (what == "save")
				{
					((InventoryEntity)e).SaveEntity(out SaveInventoryEntity entity);
					entity.type = "inventory";
					entity.sceneName = SceneManager.GetActiveScene().name;
					GlobalGameSaving.Instance.inventoryEntities.Add(entity);
					if (GlobalGameSaving.Instance.inventoryEntities[GlobalGameSaving.Instance.inventoryEntities.Count - 1].self.name.Contains('('))
					{
						string[] name = GlobalGameSaving.Instance.inventoryEntities[GlobalGameSaving.Instance.inventoryEntities.Count - 1].self.name.Split('(');
						GlobalGameSaving.Instance.inventoryEntityNames.Add(name[0]);
					}
					else
					{
						GlobalGameSaving.Instance.inventoryEntityNames.Add(GlobalGameSaving.Instance.inventoryEntities[GlobalGameSaving.Instance.inventoryEntities.Count - 1].self.name);
					}
				}
			}
            else if(!(e is NPCEntity))
            {
                if (what == "persist")
                {
                    e.SaveEntity(out SaveEntity entity);
                    entity.type = entity.gameObject.GetComponent<Item>().type;
                    entity.sceneName = SceneManager.GetActiveScene().name;
                    ScenePersistence.Instance.entities.Add(entity);
                    if (ScenePersistence.Instance.entities[ScenePersistence.Instance.entities.Count - 1].self.name.Contains('('))
                    {
                        string[] name = ScenePersistence.Instance.entities[ScenePersistence.Instance.entities.Count - 1].self.name.Split('(');
                        ScenePersistence.Instance.entityNames.Add(name[0]);
                    }
                    else
                    {
                        ScenePersistence.Instance.entityNames.Add(ScenePersistence.Instance.entities[ScenePersistence.Instance.entities.Count - 1].self.name);
                    }
                }
                else if (what == "save")
                {
                    e.SaveEntity(out SaveEntity entity);
                    entity.type = entity.gameObject.GetComponent<Item>().type;
                    entity.sceneName = SceneManager.GetActiveScene().name;
                    GlobalGameSaving.Instance.entities.Add(entity);
                    if (GlobalGameSaving.Instance.entities[GlobalGameSaving.Instance.entities.Count - 1].self.name.Contains('('))
                    {
                        string[] name = GlobalGameSaving.Instance.entities[GlobalGameSaving.Instance.entities.Count - 1].self.name.Split('(');
                        GlobalGameSaving.Instance.entityNames.Add(name[0]);
                    }
                    else
                    {
                        GlobalGameSaving.Instance.entityNames.Add(GlobalGameSaving.Instance.entities[GlobalGameSaving.Instance.entities.Count - 1].self.name);
                    }
                }
                
            }
        }
    }
}
