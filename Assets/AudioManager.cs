using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Sirenix.OdinInspector;

public class AudioManager : SerializedMonoBehaviour
{
    private static AudioSource audioSource;

    private static BeatManager beatManager;

    public static BeatBaseLine[] BeatLines
    {
        get
        {
            return BeatManager.beatLines;
        }
    }

    public static AudioSource Source
    {
        get
        {
            if (audioSource == null)
            {
                audioSource = FindObjectOfType<AudioManager>().gameObject.AddComponent<AudioSource>();
                return audioSource;
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

    public static float BPM
    {
        get
        {
            return BeatManager.bpm;
        }
    }

    public virtual void Play()
    {
        for (int i = 0; i < BeatLines.Length; i++)
        {
            for (int j = 0; j < BeatLines[i].beats.Length; j++)
            {
                BeatLines[i].beats[j].Play(this, BPM, Source, BeatManager[i, j].division);
            }
        }
    }

    public virtual void Stop()
    {
        for (int i = 0; i < BeatLines.Length; i++)
        {
            for (int j = 0; j < BeatLines[i].beats.Length; j++)
            {
                BeatLines[i].beats[j].Stop();
            }
        }
    }
}