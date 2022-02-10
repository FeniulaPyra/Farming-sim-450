using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sounds
{
    Footstep
}

public class AudioManager : MonoBehaviour
{

    private string[] sounds = new string[]
    {
        "footstep"
    };

    /// <summary>
    /// Plays a sound at a position... Essentially just instantiates a prefab
    /// </summary>
    /// <param name="pos">Position to instantiate</param>
    /// <param name="sound">Sound to play</param>
    public void PlaySoundAtPosition(Vector3 pos, Sounds sound)
    {
        GameObject soundEffect = Instantiate((GameObject)Resources.Load(sounds[0]), pos, Quaternion.identity, transform);

        // If there are settings apply
    }
}
