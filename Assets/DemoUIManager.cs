using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DemoUIManager : MonoBehaviour
{
    [SerializeField] private Toggle[] instrumentToggles;

    [SerializeField] private Transform instrumentTogglesContent;

    [SerializeField] private Toggle instrumentTogglePrefab;

    public Type type;
    public Role role;
    public Timing timing;
    public BPM bpm;

    public Toggle toggle;

    public Footwork[] footworks;

    private int selectedFootwork;

    private bool isPlaying = false;

    private void Start()
    {
        UpdateToggles();
    }

    public void ChangeType(int i)
    {
        type = (Type)i;
        OnAnyDropDownChanged();
    }

    public void ChangeRole(int i)
    {
        role = (Role)i; 
        OnAnyDropDownChanged();
    }

    public void ChangeTiming(int i)
    {
        timing = (Timing)i;
        OnAnyDropDownChanged();
    }

    public void ChangeBPM(int i)
    {
        bpm = (BPM)i;
        OnAnyDropDownChanged();
    }

    public void OnAnyDropDownChanged()
    {

        footworks[selectedFootwork].Stop();
        footworks[selectedFootwork].gameObject.SetActive(false);
        switch (type)
        {
            case Type.Basic:
                switch (role)
                {
                    case Role.Lead:
                        switch (timing)
                        {
                            case Timing.On1:
                                switch (bpm)
                                {
                                    case BPM.BPM80:
                                        selectedFootwork = 0;
                                        break;
                                    case BPM.BPM90:
                                        selectedFootwork = 1;
                                        break;
                                    case BPM.BPM100:
                                        selectedFootwork = 2;
                                        break;
                                }
                                break;
                            case Timing.On2:
                                switch (bpm)
                                {
                                    case BPM.BPM80:
                                        selectedFootwork = 3;
                                        break;
                                    case BPM.BPM90:
                                        selectedFootwork = 4;
                                        break;
                                    case BPM.BPM100:
                                        selectedFootwork = 5;
                                        break;
                                }
                                break;
                        }
                        break;
                    case Role.Follow:
                        switch (timing)
                        {
                            case Timing.On1:
                                switch (bpm)
                                {
                                    case BPM.BPM80:
                                        selectedFootwork = 6;
                                        break;
                                    case BPM.BPM90:
                                        selectedFootwork = 7;
                                        break;
                                    case BPM.BPM100:
                                        selectedFootwork = 8;
                                        break;
                                }
                                break;
                            case Timing.On2:
                                switch (bpm)
                                {
                                    case BPM.BPM80:
                                        selectedFootwork = 9;
                                        break;
                                    case BPM.BPM90:
                                        selectedFootwork = 10;
                                        break;
                                    case BPM.BPM100:
                                        selectedFootwork = 11;
                                        break;
                                }
                                break;
                        }
                        break;
                }
                break;
            case Type.SideBasic:
                switch (role)
                {
                    default:
                        switch (timing)
                        {
                            default:
                                switch (bpm)
                                {
                                    case BPM.BPM80:
                                        selectedFootwork = 12;
                                        break;
                                    case BPM.BPM90:
                                        selectedFootwork = 13;
                                        break;
                                    case BPM.BPM100:
                                        selectedFootwork = 14;
                                        break;
                                }
                                break;
                        }
                        break;
                }
                break;
        }

        //for (int i = 0; i < footworks.Length; i++)
        //{
        //    footworks[i].Stop();
        //    footworks[i].gameObject.SetActive(false);
        //}

        footworks[selectedFootwork].gameObject.SetActive(true);
        UpdateToggles();
        toggle.isOn = true;
    }

    private void UpdateToggles()
    {
        for (int i = 0; i < instrumentToggles.Length; i++)
        {
            if (instrumentToggles[i])
            {
                Destroy(instrumentToggles[i].gameObject);
            }
        }

        AudioSource[] sources = footworks[selectedFootwork].GetComponents<AudioSource>();

        instrumentToggles = new Toggle[sources.Length];
        for (int i = 0; i < instrumentToggles.Length; i++)
        {
            if (sources[i].volume != 0f)
            {
                instrumentToggles[i] = Instantiate(instrumentTogglePrefab, instrumentTogglesContent);
                var a = i;
                sources[i].volume = instrumentToggles[i].isOn ? 1 : 0;
                instrumentToggles[i].onValueChanged.AddListener(delegate
                {
                    ToggleInstrument(instrumentToggles[a].isOn, sources[a]);
                });
                instrumentToggles[i].GetComponentInChildren<TextMeshProUGUI>().text = sources[i].clip.name.Remove(0, 6);
            }
        }
    }

    private void ToggleInstrument(bool isOn, AudioSource source)
    {
        source.volume = isOn ? 1 : 0;
    }

    public void TogglePlaying()
    {
        isPlaying = !isPlaying;
        if (isPlaying)
        {
            if (footworks[selectedFootwork].isPlaying)
            {
                footworks[selectedFootwork].Unpause();
            }
            else
            {
                footworks[selectedFootwork].Play();
            }
            isPlaying = true;
        }
        else
        {
            footworks[selectedFootwork].Pause();
            isPlaying = false;
        }
    }
}

[System.Serializable]
public enum Type
{
    Basic, SideBasic
}

[System.Serializable]
public enum Role
{
    Lead, Follow
}

[System.Serializable]
public enum Timing
{
    On1, On2
}

[System.Serializable]
public enum BPM
{
    BPM80, BPM90, BPM100
}