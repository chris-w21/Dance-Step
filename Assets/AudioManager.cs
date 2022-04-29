using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioSource audioSource;

    private static BeatManager beatManager;

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

    public void Play()
    {
        for (int i = 0; i < beatLines.Length; i++)
        {
            for (int j = 0; j < beatLines[i].beats.Length; j++)
            {
                beatLines[i].beats[j].Play(this, BeatManager.bpm, Source, BeatManager[i, j].division);
            }
        }
    }

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