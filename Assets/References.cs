using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class References : MonoBehaviour
{
    public static References Instance;

    public static AudioClips parent;

    private void OnEnable()
    {
        //Debug.Log(parent.GetComponent<AudioClips>());
        Instance = this;
    }

    public Entity Clip;
}
