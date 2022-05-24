using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Hybrid;
using Unity.Entities.Serialization;
using Unity.Tiny.Audio;
using Unity.Tiny;
using System.Threading;

public class Test : SystemBase
{
    private new EntityManager EntityManager;

    protected override void OnStartRunning()
    {
        //RequireSingletonForUpdate<AudioClip>();
        if (HasSingleton<AudioClip>())
        {
            Debug.Log(true);
            Debug.Log(GetSingleton<AudioClip>());
        }
        else
        {
            Debug.Log(false);
        }
    }

    protected override void OnUpdate()
    {

    }
}