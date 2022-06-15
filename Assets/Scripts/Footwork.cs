using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo.Players;
using SonicBloom.Koreo;
using SonicBloom;

public class Footwork : MonoBehaviour
{
    [SerializeField, EventID] private string eventId = "";
    public List<Renderer> leftSteps, rightSteps;
    public GameObject leftStepPrefab, rightStepPrefab;
    [SerializeField] private Koreographer koreographer;
    [SerializeField] private MultiMusicPlayer multiMusicPlayer;
    [SerializeField] private Material leftStepOff, leftStepOn, rightStepOff, rightStepOn;
    [SerializeField] private float callbackError = 0.01f;
    private float lastLeftFootCallBackTime = 0f, lastRightFootCallBackTime = 0f;
    private int currentLeftStep = 0, currentRightStep = 0;
    private Vector3 originalStepScale = new Vector3(0.817920864f, 1.96672571f, 2.51049995f);

    private void Start()
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

        for (int i = 0; i < leftSteps.Count; i++)
        {
            leftSteps[i].material = leftStepOff;
        }
        if (leftSteps[currentLeftStep].CompareTag("On"))
        {
            leftSteps[currentLeftStep].material = leftStepOn;
        }
        currentLeftStep = currentLeftStep == leftSteps.Count - 1 ? 0 : currentLeftStep + 1;

        for (int i = 0; i < rightSteps.Count; i++)
        {
            rightSteps[i].material = rightStepOff;
        }
        if (rightSteps[currentRightStep].CompareTag("On"))
        {
            rightSteps[currentRightStep].material = rightStepOn;
        }
        currentRightStep = currentRightStep == rightSteps.Count - 1 ? 0 : currentRightStep + 1;

        koreographer.RegisterForEvents(eventId, leftFootCallBack);
        koreographer.RegisterForEvents(eventId, rightFootCallBack);
        multiMusicPlayer.Play();
    }

    private void leftFootCallBack(KoreographyEvent e)
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
                leftSteps[i].material = leftStepOff;
            }
            if (leftSteps[currentLeftStep].CompareTag("On"))
            {
                leftSteps[currentLeftStep].material = leftStepOn;
            }
            currentLeftStep = currentLeftStep == leftSteps.Count - 1 ? 0 : currentLeftStep + 1;
        }
        lastLeftFootCallBackTime = Time.time;
    }

    private void rightFootCallBack(KoreographyEvent e)
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
                rightSteps[i].material = rightStepOff;
            }
            if (rightSteps[currentRightStep].CompareTag("On"))
            {
                rightSteps[currentRightStep].material = rightStepOn;
            }
            currentRightStep = currentRightStep == rightSteps.Count - 1 ? 0 : currentRightStep + 1;
        }
        lastRightFootCallBackTime = Time.time;
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
