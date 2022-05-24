using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Hybrid;
using Unity.Tiny.Audio;
using Unity.Tiny;
using System.Threading;

public class Test : ComponentSystem
{
    private new EntityManager EntityManager;

    protected override void OnStartRunning()
    {
        EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        Entities.ForEach((Entity e, ref AudioClips clips) =>
        {
            //EntityManager.AddComponentData(e, clips);
            AudioSource source = new AudioSource() { clip = clips.Clip, loop = true, volume = 1f};
            EntityManager.AddComponentData(e, source);
            EntityManager.AddComponent<AudioSourceStart>(e);
        });
    }

    protected override void OnUpdate()
    {
        //throw new System.NotImplementedException();
    }
}