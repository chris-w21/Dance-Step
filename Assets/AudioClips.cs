using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClips : MonoBehaviour
{
    public static AudioClips Instance;

    public void OnEnable()
    {
        Instance = this;
    }

    public AudioClip clip;
}
