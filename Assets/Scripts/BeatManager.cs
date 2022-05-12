﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Unity.Burst;

public class BeatManager : AudioManager
{
    public SetOfBeatBaseLines[] setOfBeatbaseLines;

    public Texture texture;

    public Texture texture1;

    public AudioClip defaultClip;

    public Texture2D backGroundTexture;

    public float bpm = 60;

    public int selectedBeatBaseLine = 0;

    public int selectedBeatBaseLines = 0;

    public Text t;

    public static float time = 0;

    public void BPM(float newBPM)
    {
        t.text = newBPM.ToString();
        bpm = newBPM;
    }

    public void ToggleBeat(int i)
    {
        float vol = setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines[selectedBeatBaseLine].beats[i].source.volume;
        setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines[selectedBeatBaseLine].beats[i].source.volume = vol == 1 ? 0 : 1;
    }

    public void TogglePaused()
    {
        if (!isPlaying)
        {
            Play();
        }
        switch (!paused)
        {
            case true:
                for (int i = 0; i < BeatManager.setOfBeatbaseLines[SelectedBeatBaseLines].beatBaseLines[SelectedBeatBaseLine].beats.Length; i++)
                {
                    BeatManager.setOfBeatbaseLines[SelectedBeatBaseLines].beatBaseLines[SelectedBeatBaseLine].beats[i].source.Pause();
                }
                break;
            case false:
                for (int i = 0; i < BeatManager.setOfBeatbaseLines[SelectedBeatBaseLines].beatBaseLines[SelectedBeatBaseLine].beats.Length; i++)
                {
                    BeatManager.setOfBeatbaseLines[SelectedBeatBaseLines].beatBaseLines[SelectedBeatBaseLine].beats[i].source.UnPause();
                }
                break;
        }
        paused = !paused;
    }

    private void Update()
    {
        if (paused)
        {
            time += Time.deltaTime;
            Debug.Log(time);
        }
    }

    public void AddBeatBaseLine()
    {
        if (setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines.Length != 0)
        {
            List<BeatBaseLine> beatBaseLines = new List<BeatBaseLine>();
            for (int i = 0; i < setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines.Length; i++)
            {
                beatBaseLines.Add(this[i]);
            }
            BeatBaseLine newBeatBaseLine = new BeatBaseLine()
            {
                beats = new Beat[0]
            };
            //newBeatBaseLine.beats[0].notes = new Note[0];
            newBeatBaseLine.enabled = true;
            beatBaseLines.Add(newBeatBaseLine);
            setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines = beatBaseLines.ToArray();
        }
        else
        {
            setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines = new BeatBaseLine[1];
            setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines[selectedBeatBaseLine].beats = new Beat[0];
            setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines[selectedBeatBaseLine].enabled = true;
        }
    }

    public void AddNewBeat(int beatBaseLineIndex)
    {
        Beat[] beats = this[beatBaseLineIndex].beats;
        setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines[beatBaseLineIndex].beats = new Beat[this[beatBaseLineIndex].beats.Length + 1];
        for (int i = 0; i < beats.Length; i++)
        {
            this[beatBaseLineIndex, i] = beats[i];
        }
        Beat newBeat = new Beat();
        newBeat.clip = defaultClip;
        newBeat.beatEnabled = true;
        if (this[beatBaseLineIndex].beats.Length <= 1)
        {
            newBeat.notes = new Note[8];
        }
        else
        {
            newBeat.notes = new Note[this[(beatBaseLineIndex >= 1 ? beatBaseLineIndex - 1 : beatBaseLineIndex)].beats[beats.Length - 1].notes.Length];
        }
        this[beatBaseLineIndex, this[beatBaseLineIndex].beats.Length - 1] = newBeat;
    }

    public void RemoveBeat(int beatBaseLineIndex, int beatIndex)
    {
        List<Beat> sampleBeats = new List<Beat>();
        sampleBeats.AddRange(this[beatBaseLineIndex].beats);
        sampleBeats.RemoveAt(beatIndex);
        setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines[beatBaseLineIndex].beats = sampleBeats.ToArray();
    }

    public void CloneBeat(int beatBaseLineIndex, int beatIndex)
    {

        Beat[] beats = this[beatBaseLineIndex].beats;
        setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines[beatBaseLineIndex].beats = new Beat[beats.Length + 1];
        int beatToClone = 0;
        for (int i = 0; i < this[beatBaseLineIndex].beats.Length; i++)
        {
            if (i < beatIndex)
            {
                beatToClone++;
            }
            if (i == beatIndex)
            {
                continue;
            }
            this[beatBaseLineIndex, i] = beats[(i < beatIndex ? i : i - 1)];
        }
        Note[] notes = new Note[beats[beatIndex].notes.Length];
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].enabled = beats[beatIndex].notes[i].enabled;
        }
        this[beatBaseLineIndex, beatToClone] = beats[beatIndex];
        this[beatBaseLineIndex, beatToClone].notes = notes;
    }

    public void AddNotes(int beatBaseLineIndex, int beatIndex)
    {
        Note[] notes = this[beatBaseLineIndex, beatIndex].notes;
        this[beatBaseLineIndex, beatIndex].notes = new Note[notes.Length * 2];
        for (int i = 0; i < notes.Length; i++)
        {
            this[beatBaseLineIndex, beatIndex, i] = notes[i];
        }
        for (int i = 0; i < notes.Length; i++)
        {
            this[beatBaseLineIndex, beatIndex, i + notes.Length] = notes[i];
        }
    }

    public void RemoveNotes(int beatBaseLineIndex, int beatIndex)
    {
        Note[] sampleNotes = this[beatBaseLineIndex, beatIndex].notes;
        Note[] removedNotes = new Note[sampleNotes.Length / 2];
        for (int i = 0; i < removedNotes.Length; i++)
        {
            removedNotes[i] = sampleNotes[i];
        }
        this[beatBaseLineIndex, beatIndex].notes = removedNotes;
    }

    public void Divide(int beatBaseLineIndex, int beatIndex)
    {
        Note[] notes = this[beatBaseLineIndex, beatIndex].notes;
        Note[] newNotes = new Note[8 * (int)(this[beatBaseLineIndex, beatIndex].division / 8f)];
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
        this[beatBaseLineIndex, beatIndex].notes = newNotes;
    }

    public BeatBaseLine this[int x]
    {
        get
        {
            return setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines[x];
        }
        private set
        {
            setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines[x] = value;
        }
    }

    public Beat this[int x, int y]
    {
        get
        {
            return setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines[x].beats[y];
        }
        set
        {
            setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines[x].beats[y] = value;
        }
    }

    public Note this[int x, int y, int z]
    {
        get
        {
            return setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines[x].beats[y].notes[z];
        }
        set
        {
            setOfBeatbaseLines[selectedBeatBaseLines].beatBaseLines[x].beats[y].notes[z] = value;
        }
    }

    [Serializable]
    public struct SetOfBeatBaseLines
    {
        public BeatBaseLine[] beatBaseLines;
    }

    [Serializable]
    public struct BeatBaseLine
    {
        public string name;
        public Beat[] beats;
        public bool enabled;
    }


    [Serializable]
    public class Beat
    {
        public AudioClip clip;

        public Note[] notes;

        public bool beatEnabled = true;

        public int division;

        public float volume = 1f;

        public AudioSource source;

        private MonoBehaviour _caller;

        public void Play(MonoBehaviour caller, float BPM, float division)
        {
            _caller = caller;
            caller.StartCoroutine(PlayBeat(caller, BPM, division));
        }

        public void Stop(MonoBehaviour caller)
        {
            caller.StopAllCoroutines();
        }

        [BurstCompile]
        private IEnumerator PlayBeat(MonoBehaviour caller, float BPM, float division)
        {
            while (true)
            {
                if (BeatManager.bpm != 0f)
                {
                    if (!BeatManager.paused && beatEnabled)
                    {
                        IEnumerator IE = Play(BeatManager.bpm, notes[0].enabled, notes.Length, clip, (float)0, division);
                        caller.StartCoroutine(IE);
                        yield return new WaitForSecondsRealtime(clip.length);
                        yield return new WaitForSecondsRealtime(time);
                        time = 0;
                    }
                }
                else
                {
                    yield return new WaitForSecondsRealtime(60f * notes.Length / division / BeatManager.bpm);
                }
                yield return new WaitForEndOfFrame();
            }
        }

        [BurstCompile]
        public IEnumerator Play(float BPM, bool noteEnabled, float notesLen, AudioClip clip, float i, float division)
        {
            if (!BeatManager.paused)
            {
                if (!noteEnabled)
                {
                    yield break;
                }
                if (beatEnabled && !BeatManager.paused)
                {
                    source.PlayOneShot(clip, volume);
                }
            }
            yield break;
        }
    }

    [Serializable]
    public struct Note
    {
        [HideInInspector] public bool enabled;
    }
}