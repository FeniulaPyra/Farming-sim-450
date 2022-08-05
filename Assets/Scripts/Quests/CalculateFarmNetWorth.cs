using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateFarmNetWorth : MonoBehaviour
{
    [SerializeField]
    int farmNetWorth;

    public int FarmNetWorth
    {
        get
        {
            return farmNetWorth;
        }
        set
        {
            farmNetWorth = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CalculateNetWorth(int value)
    {
        farmNetWorth += value;
    }

    public void SaveWorth(out int savedWorth)
    {
        savedWorth = farmNetWorth;
    }

    public void LoadWorth(int savedWorth)
    {
        farmNetWorth = savedWorth;
    }
}

[System.Serializable]
public class SaveWorth
{
    public int netWorth;

    public SaveWorth(int worth)
    {
        netWorth = worth;
    }
}
