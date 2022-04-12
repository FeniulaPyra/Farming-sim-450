using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateFarmNetWorth : MonoBehaviour
{
    [SerializeField]
    int farmNetWorth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalculateNetWorth(int value)
    {
        farmNetWorth += value;
    }
}
