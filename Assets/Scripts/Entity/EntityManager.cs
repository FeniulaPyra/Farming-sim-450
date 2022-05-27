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
            for (int i = 0; i < ScenePersistence.Instance.pets.Count; i++)
            {
                //Close to the player's position, so the pet spawns near them
                Vector3 playerPos = GameObject.Find("Player").transform.position;
                Vector3 pos = new Vector3(playerPos.x + 2, playerPos.y, playerPos.z);
                Instantiate(Resources.Load($"Prefabs/Pets/{ScenePersistence.Instance.livestockPetNames[i]}"), pos, Quaternion.identity);
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
            //ScenePersistence.Instance.entities.Clear();
            //ScenePersistence.Instance.entityNames.Clear();
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

    public void SaveEntities()
    {
        //Get rid of any pets already in the list, so it doesn't spawn two dogs when the player goes through two scenes with their singular dog
        //ScenePersistence.Instance.entities.Clear();
        //ScenePersistence.Instance.entityNames.Clear();

        List<BasicEntity> entities = FindObjectsOfType<BasicEntity>().ToList();

        foreach (BasicEntity e in entities)
        {
            if (e is BasicPet)
            {
                BasicPet p = (BasicPet)e;

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
            else
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
        }
    }
}
