using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootJarEnemy : BasicEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        //Ready for death
        if (stats.Health <= 0)
        {
            KillEnemy();
        }
    }

    protected override void DropItems()
    {
        foreach (Item i in drops)
        {
            Instantiate(i.gameObject, transform.position, Quaternion.identity);
        }
    }
}
