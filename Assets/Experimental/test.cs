using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Hybrid;
using Unity.Tiny.Audio;
using Unity.Tiny;
using System.Threading;

public class Test : ComponentSystem
{
    private Unity.Tiny.Audio.AudioSource source;

    private Entity eSource;

    private EntityManager entityManager;

    private Entity eClip;

    protected override void OnStartRunning()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Thread createEntityThread = new Thread(() =>
        {
            for (int i = 0; i < 1000000; i++)
            {
                entityManager.CreateEntity();
            }
        });
        createEntityThread.Start();
    }
    protected override void OnUpdate()
    {

    }
}