using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;

public class AudioManager : MonoBehaviour
{
    private static AudioSource audioSource;

    private static BeatManager beatManager;

    private static StepsManager stepsManager;

    private static StyleManager styleManager;

    private static UIManager uiManager;

    public static int SelectedBeatBaseLines
    {
        get
        {
            return BeatManager.selectedBeatBaseLines;
        }
    }

    public static int SelectedBeatBaseLine
    {
        get
        {
            return BeatManager.selectedBeatBaseLine;
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

    public static StyleManager StyleManager
    {
        get
        {
            if (styleManager == null)
            {
                styleManager = FindObjectOfType<StyleManager>();
            }
            return styleManager;
        }
    }

    public static int SelectedStyle
    {
        get
        {
            return StyleManager.selectedStyle;
        }
    }

    public static UIManager UIManager
    {
        get
        {
            if (uiManager == null)
            {
                uiManager = FindObjectOfType<UIManager>();
            }
            return uiManager;
        }
    }

    public static bool paused { get; private set; }

    public static bool isPlaying = false;

    private void Start()
    {
        isPlaying = false;
    }

    public void Pause()
    {
        paused = true;
    }

    public void Resume()
    {
        paused = false;
    }

    [BurstCompile]
    public void Play()
    {
        isPlaying = true;
        if (BeatManager.setOfBeatbaseLines[SelectedBeatBaseLines].beatBaseLines[SelectedBeatBaseLine].beats.Length > 0)
        {
            StartCoroutine(StepsManager.PlayLead());
            StartCoroutine(StepsManager.PlayFollow());
            for (int i = 0; i < BeatManager.setOfBeatbaseLines[SelectedBeatBaseLines].beatBaseLines[SelectedBeatBaseLine].beats.Length; i++)
            {
                BeatManager.setOfBeatbaseLines[SelectedBeatBaseLines].beatBaseLines[SelectedBeatBaseLine].beats[i].Play(this, BeatManager.bpm, Source, BeatManager[SelectedBeatBaseLine, i].division);
            }
        }
    }

    [BurstCompile]
    public void Stop()
    {
        isPlaying = false;
        if (BeatManager.setOfBeatbaseLines[SelectedBeatBaseLines].beatBaseLines[SelectedBeatBaseLine].beats.Length > 0)
        {
            for (int i = 0; i < BeatManager.setOfBeatbaseLines[SelectedBeatBaseLines].beatBaseLines[SelectedBeatBaseLine].beats.Length; i++)
            {
                BeatManager.setOfBeatbaseLines[SelectedBeatBaseLines].beatBaseLines[SelectedBeatBaseLine].beats[i].Stop(this);
            }
        }
        Source.Stop();
    }
}