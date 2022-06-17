using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo.Players;
using SonicBloom.Koreo;
using SonicBloom;

public class Footwork : MonoBehaviour
{
    [SerializeField, EventID] private string eventId = "";
    public List<Renderer> allSteps;
    public List<Renderer> leftSteps, rightSteps;
    public GameObject leftStepPrefab, rightStepPrefab;
    public bool linear = true;
    public bool isPlaying = false;
    public bool isPaused = false;
    [SerializeField] private Koreographer koreographer;
    [SerializeField] private MultiMusicPlayer multiMusicPlayer;
    [SerializeField] private Material leftStepOff, leftStepOn, rightStepOff, rightStepOn;
    [SerializeField] private float callbackError = 0.01f;
    [SerializeField] private bool PlayOnStart = false;
    private float lastLeftFootCallBackTime = 0f, lastRightFootCallBackTime = 0f;
    private int currentLeftStep = 0, currentRightStep = 0;
    private Vector3 originalStepScale = new Vector3(0.817920864f, 1.96672571f, 2.51049995f);
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPlaying)
            {
                isPlaying = false;
                Stop();
            }
            else
            {
                isPlaying = true;
                Play();
            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                isPaused = false;
                Unpause();
            }
            else
            {
                isPaused = true;
                Pause();
            }
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < leftSteps.Count; i++)
        {
            leftSteps[i].material = leftStepOff;
            leftSteps[i].transform.localScale = originalStepScale;
        }
        for (int i = 0; i < rightSteps.Count; i++)
        {
            rightSteps[i].material = rightStepOff;
            rightSteps[i].transform.localScale = originalStepScale;
        }
        if (PlayOnStart)
        {
            Play();
        }
    }

    public void Play()
    {
        for (int i = 0; i < leftSteps.Count; i++)
        {
            leftSteps[i].material = leftStepOff;
            leftSteps[i].transform.localScale = originalStepScale;
        }
        for (int i = 0; i < rightSteps.Count; i++)
        {
            rightSteps[i].material = rightStepOff;
            rightSteps[i].transform.localScale = originalStepScale;
        }

        if (allSteps[0].CompareTag("On"))
        {
            allSteps[0].material = allSteps[0].material.name.Contains("left") ? leftStepOn : rightStepOn;
        }

        koreographer.RegisterForEvents(eventId, leftFootCallBack);
        multiMusicPlayer.Play();
    }

    public void Stop()
    {
        currentLeftStep = 0;
        lastLeftFootCallBackTime = callbackError;
        koreographer.UnregisterForAllEvents(this);
        multiMusicPlayer.Stop();
    }

    public void Pause()
    {
        multiMusicPlayer.Pause();
    }

    public void Unpause()
    {
        multiMusicPlayer.Play();
    }

    private void leftFootCallBack(KoreographyEvent e)
    {
        if (allSteps.Count <= 0)
        {
            return;
        }
        if (Time.time - lastLeftFootCallBackTime <= callbackError)
        {
            lastLeftFootCallBackTime = Time.time;
            return;
        }
        else
        {
            for (int i = 0; i < allSteps.Count; i++)
            {
                if (allSteps[i].material.name.Contains("left") || allSteps[i].material.name.Contains("Left"))
                {
                    allSteps[i].material = leftStepOff;
                }
                else
                {
                    allSteps[i].material = rightStepOff;
                }
            }
            if (allSteps[currentLeftStep].CompareTag("On"))
            {
                if (allSteps[currentLeftStep].material.name.Contains("left") || allSteps[currentLeftStep].material.name.Contains("Left"))
                {
                    allSteps[currentLeftStep].material = leftStepOn;
                }
                else
                {
                    allSteps[currentLeftStep].material = rightStepOn;
                }
            }
            currentLeftStep = currentLeftStep == allSteps.Count - 1 ? 0 : currentLeftStep + 1;
        }
        lastLeftFootCallBackTime = Time.time;
    }

    public Material leftStepOffMat
    {
        get
        {
            return leftStepOff;
        }
    }

    public Material leftStepOnMat
    {
        get
        {
            return leftStepOn;
        }
    }

    public Material rightStepOffMat
    {
        get
        {
            return rightStepOff;
        }
    }

    public Material rightStepOnMat
    {
        get
        {
            return rightStepOn;
        }
    }
}
