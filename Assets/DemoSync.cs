using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo.Players;
using SonicBloom.Koreo;
using SonicBloom;

public class DemoSync : MonoBehaviour
{
    [EventID]
    //name of the sample track that we made
    public string eventID;

    public Koreographer koreographer;

    public SimpleMusicPlayer musicPlayer;

    public Rigidbody sphereRigidbody;

    public float upwardsForce = 5f;

    private void Start()
    {
        koreographer.RegisterForEvents(eventID, Jump);
        musicPlayer.Play();
    }

    public void Jump(KoreographyEvent @event)
    {
        sphereRigidbody.AddForce(Vector3.up * upwardsForce);
    }

    private void OnDisable()
    {
        koreographer.UnregisterForAllEvents(this);
    }
}
