using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerEnemy : BasicEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (attacked == false)
        {
            base.FixedUpdate();

            if (distance <= minSeekDistance)
            {
                target.TakeDamage(stats.Strength, false);

                attacked = true;
                fleeTimer = baseFleeTimer;
            }
        }
        else
        {
            fleeTimer -= Time.deltaTime;

            if (fleeTimer <= 0.0f)
            {
                attacked = false;
            }
        }
    }
}