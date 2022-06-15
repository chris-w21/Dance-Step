using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo.Players;
using SonicBloom.Koreo;
using SonicBloom;

public class Footwork : MonoBehaviour
{
    public List<Renderer> leftSteps, rightSteps;
    public GameObject leftStepPrefab, rightStepPrefab;
    [SerializeField, EventID] private string eventId = "";
    [SerializeField] private Koreographer koreographer;
    [SerializeField] private Material leftStepOff, leftStepOn, rightStepOff, rightStepOn;
    [SerializeField] private float callbackError = 0.01f;
    private float lastLeftFootCallBackTime = 0f, lastRightFootCallBackTime = 0f;
    private int currentLeftStep = 0, currentRightStep = 0;

    private void Start()
    {
        koreographer.RegisterForEvents(eventId, leftFootCallBack);
        koreographer.RegisterForEvents(eventId, rightFootCallBack);
    }

    private void leftFootCallBack(KoreographyEvent e)
    {
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
}
