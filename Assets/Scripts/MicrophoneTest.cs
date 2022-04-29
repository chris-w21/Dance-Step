using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicrophoneTest : MonoBehaviour
{
    // Start is called before the first frame update


    public InputField BPMValue;
    public AudioClip TickSound;
    public AudioSource audioSource;
    public Animator FlashImage;
    public Text Status;
    bool startTicking;
    
    void Start()
    {
        audioSource.clip = TickSound;
        
    }

    public void StartTicking()
    {
        startTicking = true;
        StartCoroutine(PlayTheTick());
        
        Status.text = "Playing Now";
    }

    public void UserEnteringBPM()
    {
        startTicking = false;
        StopCoroutine(PlayTheTick());
        Status.text = "Stopped playing user entering bpm";
    }
    public IEnumerator PlayTheTick()
    {
        while (startTicking)
        {
            if (BPMValue.text != "")
            {
                float waitTime = int.Parse(BPMValue.text);
                yield return new WaitForSecondsRealtime(60/waitTime);
                FlashImage.Play("Flash",0,0);
                audioSource.Play();
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
            
        }
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
