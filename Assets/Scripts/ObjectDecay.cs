using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDecay : MonoBehaviour
{
    [SerializeField]
    private float maxAge = 5;
    private float age;

    void Update()
    {
        age += Time.deltaTime;

        if (age > maxAge) Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
