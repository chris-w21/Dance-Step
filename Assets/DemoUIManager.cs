using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DemoUIManager : MonoBehaviour
{
    [SerializeField] private GameObject salsaObject, bachataObject;

    [SerializeField] private Slider[] instrumentVolumeSliders;

    [SerializeField] private Transform instrumentVolumeSlidersContent;

    [SerializeField] private Slider instrumentVolumeSliderPrefab;

    public Type type;
    public Role role;
    public Timing timing;
    public BPM bpm;

    public Toggle toggle;

    public Footwork[] footworks;

    private int lastSelectedSalsaFootwork = 0;

    private int selectedFootwork;

    private bool isPlaying = false;

    private bool salsa = true;

    private void Start()
    {
        UpdateToggles();
    }

    public void ChangeStyle(int _salsa)
    {
        footworks[selectedFootwork].Stop();
        if (_salsa == 0)
        {
            if (!salsa)
            {
                selectedFootwork = lastSelectedSalsaFootwork;
                bachataObject.SetActive(false);
                salsaObject.SetActive(true);
                salsa = true;
            }
        }
        else if (_salsa == 1)
        {
            if (salsa)
            {
                selectedFootwork = 15;
                salsaObject.SetActive(false);
                bachataObject.SetActive(true);
                salsa = false;
            }
        }
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
        if (salsa)
        {
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
                                        case BPM.Slow:
                                            selectedFootwork = 0;
                                            break;
                                        case BPM.Normal:
                                            selectedFootwork = 1;
                                            break;
                                        case BPM.Fast:
                                            selectedFootwork = 2;
                                            break;
                                    }
                                    break;
                                case Timing.On2:
                                    switch (bpm)
                                    {
                                        case BPM.Slow:
                                            selectedFootwork = 3;
                                            break;
                                        case BPM.Normal:
                                            selectedFootwork = 4;
                                            break;
                                        case BPM.Fast:
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
                                        case BPM.Slow:
                                            selectedFootwork = 6;
                                            break;
                                        case BPM.Normal:
                                            selectedFootwork = 7;
                                            break;
                                        case BPM.Fast:
                                            selectedFootwork = 8;
                                            break;
                                    }
                                    break;
                                case Timing.On2:
                                    switch (bpm)
                                    {
                                        case BPM.Slow:
                                            selectedFootwork = 9;
                                            break;
                                        case BPM.Normal:
                                            selectedFootwork = 10;
                                            break;
                                        case BPM.Fast:
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
                                        case BPM.Slow:
                                            selectedFootwork = 12;
                                            break;
                                        case BPM.Normal:
                                            selectedFootwork = 13;
                                            break;
                                        case BPM.Fast:
                                            selectedFootwork = 14;
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }
        else
        {
            switch (role)
            {
                case Role.Lead:
                    switch (bpm)
                    {
                        case BPM.Slow:
                            selectedFootwork = 15;
                            break;
                        case BPM.Normal:
                            selectedFootwork = 16;
                            break;
                        case BPM.Fast:
                            selectedFootwork = 17;
                            break;
                    }
                    break;
                case Role.Follow:
                    switch (bpm)
                    {
                        case BPM.Slow:
                            selectedFootwork = 18;
                            break;
                        case BPM.Normal:
                            selectedFootwork = 19;
                            break;
                        case BPM.Fast:
                            selectedFootwork = 20;
                            break;
                    }
                    break;
            }
            //for (int i = 0; i < footworks.Length; i++)
            //{
            //    footworks[i].Stop();
            //    footworks[i].gameObject.SetActive(false);
            //}
        }
        lastSelectedSalsaFootwork = selectedFootwork;
        footworks[selectedFootwork].gameObject.SetActive(true);
        UpdateToggles();
        toggle.isOn = true;
    }

    private void UpdateToggles()
    {
        for (int i = 0; i < instrumentVolumeSliders.Length; i++)
        {
            if (instrumentVolumeSliders[i])
            {
                Destroy(instrumentVolumeSliders[i].gameObject);
            }
        }

        AudioSource[] sources = footworks[selectedFootwork].GetComponents<AudioSource>();
        instrumentVolumeSliders = new Slider[sources.Length];
        for (int i = 0; i < instrumentVolumeSliders.Length; i++)
        {
            if (i > 1)
            {
                instrumentVolumeSliders[i] = Instantiate(instrumentVolumeSliderPrefab, instrumentVolumeSlidersContent);
                var a = i;
                instrumentVolumeSliders[i].onValueChanged.AddListener(delegate
                {
                    ChangeInstrumentVolume(instrumentVolumeSliders[a].value, sources[a]);
                });
                instrumentVolumeSliders[i].GetComponentInChildren<TextMeshProUGUI>().text = sources[i].clip.name;
            }
        }
    }

    //private void ToggleInstrument(bool isOn, AudioSource source)
    //{
    //    source.volume = isOn ? 1 : 0;
    //}

    private void ChangeInstrumentVolume(float newVolume, AudioSource source)
    {
        source.volume = newVolume;
    }

    public void TogglePlaying()
    {
        isPlaying = !isPlaying;
        if (isPlaying)
        {
            footworks[selectedFootwork].Play();
            isPlaying = true;
        }
        else
        {
            footworks[selectedFootwork].Stop();
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
    Slow, Normal, Fast
}