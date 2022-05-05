using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;

public class AudioManager : MonoBehaviour
{
    private static AudioSource audioSource;

    private static BeatManager beatManager;

    private static StepsManager stepsManager;

    public static BeatManager.BeatBaseLine[] beatLines
    {
        get
        {
            return BeatManager.beatbaseLines;
        }
    }

    public static AudioSource Source
    {
        get
        {
            if (audioSource == null)
            {
                audioSource = FindObjectOfType<AudioSource>();
            }
            return audioSource;
        }
    }

    public static BeatManager BeatManager
    {
        get
        {
            if (beatManager == null)
            {
                beatManager = FindObjectOfType<BeatManager>();
            }
            return beatManager;
        }
    }

    public static StepsManager StepsManager
    {
        get
        {
            if (stepsManager == null)
            {
                stepsManager = FindObjectOfType<StepsManager>();
            }
            return stepsManager;
        }
    }

    [BurstCompile]
    public void Play()
    {
        StartCoroutine(StepsManager.PlayLead());
        StartCoroutine(StepsManager.PlayFollow());
        for (int i = 0; i < beatLines.Length; i++)
        {
            for (int j = 0; j < beatLines[i].beats.Length; j++)
            {
                beatLines[i].beats[j].Play(this, BeatManager.bpm, Source, BeatManager[i, j].division);
            }
        }
    }

    [BurstCompile]
    private void Stop()
    {
        for (int i = 0; i < beatLines.Length; i++)
        {
            for (int j = 0; j < beatLines[i].beats.Length; j++)
            {
                beatLines[i].beats[j].Stop();
            }
        }
    }
}