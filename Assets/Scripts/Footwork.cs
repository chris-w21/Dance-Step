using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SonicBloom.Koreo.Players;
using SonicBloom.Koreo;
using SonicBloom;

public class Footwork : MonoBehaviour
{
    [SerializeField, EventID] private string eventId = "", eventId2 = "";
    [SerializeField, EventID] private string vocalsEventId = "";
    public List<Renderer> allSteps;
    public List<Renderer> leftSteps, rightSteps;
    public GameObject leftStepPrefab, rightStepPrefab;
    public bool isPlaying = false;
    public bool isPaused = false;
    [HideInInspector]public bool linear = true;
    [SerializeField] private Toggle[] vocalToggles;
    [SerializeField] private Image[] vocalIndicators;
    [SerializeField] private AudioSource vocalsAudioSource;
    [SerializeField] private Koreographer koreographer;
    [SerializeField] private MultiMusicPlayer multiMusicPlayer;
    [SerializeField] private Material leftStepOff, leftStepOn, rightStepOff, rightStepOn, leftStepEmpty, rightStepEmpty;
    [SerializeField] private bool PlayOnStart = false;
    private Vector3 originalStepScale = new Vector3(0.817920864f, 1.96672571f, 2.51049995f);
    private float callbackError = 0.05f;
    private float lastLeftFootCallBackTime = 0f, lastRightFootCallBackTime = 0f, lastVocalCallBackTime = 0f;
    private int currentLeftStep = 0, currentRightStep = 0;
    private int currentVocal = 0;
    
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
        if (linear)
        {
            koreographer.RegisterForEvents(eventId, LinearCallBack);
            koreographer.RegisterForEvents(vocalsEventId, VocalCallBack);
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
        }
        else
        {
            koreographer.RegisterForEvents(eventId, LeftFootCallBack);
            koreographer.RegisterForEvents(eventId2, RightFootCallBack);
            koreographer.RegisterForEvents(vocalsEventId, VocalCallBack);
            for (int i = 0; i < leftSteps.Count; i++)
            {
                leftSteps[i].material = leftStepEmptyMat;
                leftSteps[i].transform.localScale = originalStepScale;
            }
            for (int i = 0; i < rightSteps.Count; i++)
            {
                rightSteps[i].material = rightStepEmptyMat;
                rightSteps[i].transform.localScale = originalStepScale;
            }

            if (leftSteps[0].CompareTag("On"))
            {
                leftSteps[0].material = leftStepOn;
            }
            else if (leftSteps[0].CompareTag("In"))
            {
                leftSteps[0].material = leftStepOff;
            }
            else if (leftSteps[0].CompareTag("Off"))
            {
                leftSteps[0].material = leftStepEmpty;
            }
            if (rightSteps[0].CompareTag("On"))
            {
                rightSteps[0].material = rightStepOn;
            }
            else if (rightSteps[0].CompareTag("In"))
            {
                rightSteps[0].material = rightStepOff;
            }
            else if (rightSteps[0].CompareTag("Off"))
            {
                rightSteps[0].material = rightStepEmpty;
            }
        }

        if (PlayOnStart)
        {
            currentLeftStep += 1;
            currentRightStep += 1;
            currentVocal += 1;
            Play();
        }
    }

    private void OnDisable()
    {
        koreographer.UnregisterForAllEvents(this);
    }

    public void Play()
    {
        if (linear)
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
        }
        else
        {
            if (PlayOnStart)
            {
                if (leftSteps[0].CompareTag("On"))
                {
                    leftSteps[0].material = leftStepOn;
                }
                else if (leftSteps[0].CompareTag("In"))
                {
                    leftSteps[0].material = leftStepOff;
                }
                else if (leftSteps[0].CompareTag("Off"))
                {
                    leftSteps[0].material = leftStepEmpty;
                }
                if (rightSteps[0].CompareTag("On"))
                {
                    rightSteps[0].material = rightStepOn;
                }
                else if (rightSteps[0].CompareTag("In"))
                {
                    rightSteps[0].material = rightStepOff;
                }
                else if (rightSteps[0].CompareTag("Off"))
                {
                    rightSteps[0].material = rightStepEmpty;
                }
            }
        }

        multiMusicPlayer.Play();
    }

    public void Stop()
    {
        currentLeftStep = 0;
        currentRightStep = 0;
        currentVocal = 0;
        lastLeftFootCallBackTime = callbackError;
        lastRightFootCallBackTime = callbackError;
        lastVocalCallBackTime = callbackError;
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

    private void LinearCallBack(KoreographyEvent e)
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

    private void LeftFootCallBack(KoreographyEvent e)
    {
        if (leftSteps.Count <= 0)
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
            for (int i = 0; i < leftSteps.Count; i++)
            {
                leftSteps[i].material = leftStepEmpty;
            }
            if (leftSteps[currentLeftStep].CompareTag("On"))
            {
                leftSteps[currentLeftStep].material = leftStepOn;
            }
            else if (leftSteps[currentLeftStep].CompareTag("In"))
            {
                leftSteps[currentLeftStep].material = leftStepOff;
            }
            currentLeftStep = currentLeftStep == leftSteps.Count - 1 ? 0 : currentLeftStep + 1;
        }
        lastLeftFootCallBackTime = Time.time;
    }

    private void RightFootCallBack(KoreographyEvent e)
    {
        if (rightSteps.Count <= 0)
        {
            return;
        }
        if (Time.time - lastRightFootCallBackTime <= callbackError)
        {
            lastRightFootCallBackTime = Time.time;
            return;
        }
        else
        {
            for (int i = 0; i < rightSteps.Count; i++)
            {
                rightSteps[i].material = rightStepEmpty;
            }
            if (rightSteps[currentRightStep].CompareTag("On"))
            {
                rightSteps[currentRightStep].material = rightStepOn;
            }
            else if (rightSteps[currentRightStep].CompareTag("In"))
            {
                rightSteps[currentRightStep].material = rightStepOff;
            }
            currentRightStep = currentRightStep == rightSteps.Count - 1 ? 0 : currentRightStep + 1;
        }
        lastRightFootCallBackTime = Time.time;
    }

    private void VocalCallBack(KoreographyEvent e)
    {
        if (vocalToggles.Length <= 0)
        {
            return;
        }
        if (Time.time - lastVocalCallBackTime <= callbackError)
        {
            lastVocalCallBackTime = Time.time;
            return;
        }
        else
        {
            for (int i = 0; i < vocalIndicators.Length; i++)
            {
                vocalIndicators[i].enabled = false;
            }
            if (vocalToggles[currentVocal].isOn)
            {
                vocalsAudioSource.volume = 1f;
                vocalIndicators[currentVocal].enabled = true;
            }
            else
            {
                vocalsAudioSource.volume = 0f;
            }
            currentVocal = currentVocal == vocalToggles.Length - 1 ? 0 : currentVocal + 1;
        }
        lastVocalCallBackTime = Time.time;
    }

    public Material leftStepEmptyMat
    {
        get
        {
            return leftStepEmpty;
        }
    }

    public Material rightStepEmptyMat
    {
        get
        {
            return rightStepEmpty;
        }
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
