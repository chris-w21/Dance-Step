using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Tiny.Audio;

[GenerateAuthoringComponent]
public struct AudioClips : IComponentData
{
    public Entity Clip;
}
