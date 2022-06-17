using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoUIManager : MonoBehaviour
{
    public Type type;
    public Role role;
    public Timing timing;
    public BPM bpm;

    public Footwork[] footworks;

    private int selectedFootwork;

    public void OnAnyDropDownChanged()
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
                            case Timing.On1:
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

        for (int i = 0; i < footworks.Length; i++)
        {
            footworks[i].Stop();
            footworks[i].gameObject.SetActive(false);
        }
        footworks[selectedFootwork].gameObject.SetActive(true);
        footworks[selectedFootwork].Play();
    }
}

public enum Type
{
    Basic, SideBasic
}

public enum Role
{
    Lead, Follow
}

public enum Timing
{
    On1, On2
}

public enum BPM
{
    BPM80, BPM90, BPM100
}