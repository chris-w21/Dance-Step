using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

public class BeatManager : AudioManager
{
    public AudioClip defaultClip;

    public Texture texture;

    public Texture texture1;

    public Texture playedTexture;

    public Texture plus, minus;

    public Texture2D backGroundTexture;

    public BeatBaseLine[] beatLines;

    [HideInInspector] public bool isOn = false;

    public float bpm = 60f;

    public override void Play()
    {
        base.Play();
    }

    public override void Stop()
    {
        base.Stop();
    }

    private void Start()
    {
        Play();
    }

    [SerializeField] private bool clone = false;

    private void OnValidate()
    {
        if (clone)
        {

            clone = false;
            //CloneBeat(0, 0);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Play();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Stop();
        }
    }

    public void AddBeatBaseLine()
    {
        if (beatLines.Length != 0)
        {
            List<BeatBaseLine> beatBaseLines = new List<BeatBaseLine>();
            for (int i = 0; i < beatLines.Length; i++)
            {
                beatBaseLines.Add(beatLines[i]);
            }
            BeatBaseLine newBeatBaseLine = new BeatBaseLine()
            {
                beats = new Beat[0]
            };
            //newBeatBaseLine.beats[0].notes = new Note[0];
            newBeatBaseLine.enabled = true;
            beatBaseLines.Add(newBeatBaseLine);
            beatLines = beatBaseLines.ToArray();
        }
        else
        {
            beatLines = new BeatBaseLine[1];
            beatLines[0].beats = new Beat[0];
            beatLines[0].enabled = true;
        }
    }

    public void AddNewBeat(int beatBaseLineIndex)
    {
        Beat[] beats = beatLines[beatBaseLineIndex].beats;
        beatLines[beatBaseLineIndex].beats = new Beat[beatLines[beatBaseLineIndex].beats.Length + 1];
        for (int i = 0; i < beats.Length; i++)
        {
            beatLines[beatBaseLineIndex].beats[i] = beats[i];
        }
        Beat newBeat = new Beat();
        newBeat.clip = defaultClip;
        newBeat.enabled = true;
        if (beatLines[beatBaseLineIndex].beats.Length <= 1)
        {
            newBeat.notes = new Note[8];
        }
        else
        {
            newBeat.notes = new Note[beatLines[(beatBaseLineIndex >= 1 ? beatBaseLineIndex - 1 : beatBaseLineIndex)].beats[beats.Length - 1].notes.Length];
        }
        beatLines[beatBaseLineIndex].beats[beatLines[beatBaseLineIndex].beats.Length - 1] = newBeat;
    }

    public void RemoveBeat(int beatBaseLineIndex, int beatIndex)
    {
        List<Beat> sampleBeats = new List<Beat>();
        sampleBeats.AddRange(beatLines[beatBaseLineIndex].beats);
        sampleBeats.RemoveAt(beatIndex);
        beatLines[beatBaseLineIndex].beats = sampleBeats.ToArray();
    }

    public void CloneBeat(int beatBaseLineIndex, int beatIndex)
    {

        Beat[] beats = beatLines[beatBaseLineIndex].beats;
        beatLines[beatBaseLineIndex].beats = new Beat[beats.Length + 1];
        int beatToClone = 0;
        for (int i = 0; i < beatLines[beatBaseLineIndex].beats.Length; i++)
        {
            if (i < beatIndex)
            {
                beatToClone++;
            }
            if (i == beatIndex)
            {
                continue;
            }
            beatLines[beatBaseLineIndex].beats[i] = beats[(i < beatIndex ? i : i - 1)];
        }
        Note[] notes = new Note[beats[beatIndex].notes.Length];
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].enabled = beats[beatIndex].notes[i].enabled;
        }
        beatLines[beatBaseLineIndex].beats[beatToClone] = beats[beatIndex];
        beatLines[beatBaseLineIndex].beats[beatToClone].notes = notes;
    }

    public void AddNotes(int beatBaseLineIndex, int beatIndex)
    {
        Note[] notes = beatLines[beatBaseLineIndex].beats[beatIndex].notes;
        beatLines[beatBaseLineIndex].beats[beatIndex].notes = new Note[notes.Length * 2];
        for (int i = 0; i < notes.Length; i++)
        {
            beatLines[beatBaseLineIndex].beats[beatIndex].notes[i] = notes[i];
        }
        for (int i = 0; i < notes.Length; i++)
        {
            beatLines[beatBaseLineIndex].beats[beatIndex].notes[i + notes.Length] = notes[i]; 
        }
    }

    public void RemoveNotes(int beatBaseLineIndex, int beatIndex)
    {
        Note[] sampleNotes = beatLines[beatBaseLineIndex].beats[beatIndex].notes;
        Note[] removedNotes = new Note[sampleNotes.Length / 2];
        for (int i = 0; i < removedNotes.Length; i++)
        {
            removedNotes[i] = sampleNotes[i];
        }
        beatLines[beatBaseLineIndex].beats[beatIndex].notes = removedNotes;
    }

    public Note[] notes;

    public Note[] newNotes;

    public void Divide(int beatBaseLineIndex, int beatIndex)
    {
        notes = this[beatBaseLineIndex, beatIndex].notes;
        newNotes = new Note[8 * (int)(this[beatBaseLineIndex, beatIndex].division / 8f)];
        if (this[beatBaseLineIndex, beatIndex].division > notes.Length)
        {
            for (int i = 0; i < notes.Length; i++)
            {
                Debug.Log(newNotes[i * 2]);
                newNotes[i * 2].enabled = notes[i].enabled;
            }
        }
        else
        {
            for (int i = 0; i < newNotes.Length; i++)
            {
                newNotes[i].enabled = notes[i * 2].enabled;
            }
        }
        beatLines[beatBaseLineIndex].beats[beatIndex].notes = newNotes;
    }

    public BeatBaseLine this[int x]
    {
        get
        {
            return beatLines[x];
        }
    }

    public Beat this[int x, int y]
    {
        get
        {
            return beatLines[x].beats[y];
        }
    }

    public Note this[int x, int y, int z]
    {
        get
        {
            return beatLines[x].beats[y].notes[z];
        }
        set
        {
            beatLines[x].beats[y].notes[z] = value;
        }
    }
}

[Serializable]
public struct BeatBaseLine
{
    public string name;
    public Beat[] beats;
    public bool enabled;
}

[Serializable]
public struct Beat
{
    public AudioClip clip;

    public Note[] notes;

    public bool enabled;

    public int division;

    private MonoBehaviour _caller;

    public void Play(MonoBehaviour caller, float BPM, AudioSource source, float division)
    {
        _caller = caller;
        if (!enabled)
        {
            return;
        }
        caller.StartCoroutine(PlayBeat(caller, BPM, source, division));
    }

    public void Stop()
    {
        _caller.StopAllCoroutines();
    }

    private IEnumerator PlayBeat(MonoBehaviour caller, float BPM, AudioSource source, float division)
    {
        while (true)
        {
            if (BPM != 0f)
            {
                for (int i = 0; i < notes.Length; i++)
                {
                    IEnumerator IE = notes[i].Play(BPM, notes.Length, clip, source, (float)i, division);
                    caller.StartCoroutine(IE);
                }
                yield return new WaitForSecondsRealtime(60f * notes.Length / division / BPM);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}

[Serializable]
public struct Note
{
    [HideInInspector] public bool enabled;

    [SerializeField, HideInInspector]
    public IEnumerator Play(float BPM, float notesLen, AudioClip clip, UnityEngine.AudioSource source, float i, float division)
    {
        if (!enabled)
        {
            yield break;
        }
        float time = (((60f / notesLen)) * i / BPM) * notesLen / division;
        yield return new UnityEngine.WaitForSecondsRealtime(time);
        source.PlayOneShot(clip);
    }
}