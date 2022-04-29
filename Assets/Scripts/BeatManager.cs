using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Unity.Burst;

public class BeatManager : AudioManager
{
    public static BeatManager Instance;

    public BeatBaseLine[] beatbaseLines;

    public AudioClip defaultClip;

    public Texture texture;

    public Texture texture1;

    public Texture playedTexture;

    public Texture plus, minus;

    public Texture2D backGroundTexture;

    public int bpm = 60;

    public bool paused = false;

    private void Start()
    {
        base.Play();
    }

    public void Pause()
    {
        paused = true;
    }

    public void Resume()
    {
        paused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            paused = !paused;
        }
    }

    public void ToggleBeat(int i)
    {
        beatbaseLines[0].beats[i].beatEnabled = !beatbaseLines[0].beats[i].beatEnabled;
    }

    public void AddBeatBaseLine()
    {
        if (beatbaseLines.Length != 0)
        {
            List<BeatBaseLine> beatBaseLines = new List<BeatBaseLine>();
            for (int i = 0; i < beatbaseLines.Length; i++)
            {
                beatBaseLines.Add(beatbaseLines[i]);
            }
            BeatBaseLine newBeatBaseLine = new BeatBaseLine()
            {
                beats = new Beat[0]
            };
            //newBeatBaseLine.beats[0].notes = new Note[0];
            newBeatBaseLine.enabled = true;
            beatBaseLines.Add(newBeatBaseLine);
            beatbaseLines = beatBaseLines.ToArray();
        }
        else
        {
            beatbaseLines = new BeatBaseLine[1];
            beatbaseLines[0].beats = new Beat[0];
            beatbaseLines[0].enabled = true;
        }
    }

    public void AddNewBeat(int beatBaseLineIndex)
    {
        Beat[] beats = beatbaseLines[beatBaseLineIndex].beats;
        beatbaseLines[beatBaseLineIndex].beats = new Beat[beatbaseLines[beatBaseLineIndex].beats.Length + 1];
        for (int i = 0; i < beats.Length; i++)
        {
            beatbaseLines[beatBaseLineIndex].beats[i] = beats[i];
        }
        Beat newBeat = new Beat();
        newBeat.clip = defaultClip;
        newBeat.beatEnabled = true;
        if (beatbaseLines[beatBaseLineIndex].beats.Length <= 1)
        {
            newBeat.notes = new Note[8];
        }
        else
        {
            newBeat.notes = new Note[beatbaseLines[(beatBaseLineIndex >= 1 ? beatBaseLineIndex - 1 : beatBaseLineIndex)].beats[beats.Length - 1].notes.Length];
        }
        beatbaseLines[beatBaseLineIndex].beats[beatbaseLines[beatBaseLineIndex].beats.Length - 1] = newBeat;
    }

    public void RemoveBeat(int beatBaseLineIndex, int beatIndex)
    {
        List<Beat> sampleBeats = new List<Beat>();
        sampleBeats.AddRange(beatbaseLines[beatBaseLineIndex].beats);
        sampleBeats.RemoveAt(beatIndex);
        beatbaseLines[beatBaseLineIndex].beats = sampleBeats.ToArray();
    }

    public void CloneBeat(int beatBaseLineIndex, int beatIndex)
    {

        Beat[] beats = beatbaseLines[beatBaseLineIndex].beats;
        beatbaseLines[beatBaseLineIndex].beats = new Beat[beats.Length + 1];
        int beatToClone = 0;
        for (int i = 0; i < beatbaseLines[beatBaseLineIndex].beats.Length; i++)
        {
            if (i < beatIndex)
            {
                beatToClone++;
            }
            if (i == beatIndex)
            {
                continue;
            }
            beatbaseLines[beatBaseLineIndex].beats[i] = beats[(i < beatIndex ? i : i - 1)];
        }
        Note[] notes = new Note[beats[beatIndex].notes.Length];
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].enabled = beats[beatIndex].notes[i].enabled;
        }
        beatbaseLines[beatBaseLineIndex].beats[beatToClone] = beats[beatIndex];
        beatbaseLines[beatBaseLineIndex].beats[beatToClone].notes = notes;
    }

    public void AddNotes(int beatBaseLineIndex, int beatIndex)
    {
        Note[] notes = beatbaseLines[beatBaseLineIndex].beats[beatIndex].notes;
        beatbaseLines[beatBaseLineIndex].beats[beatIndex].notes = new Note[notes.Length * 2];
        for (int i = 0; i < notes.Length; i++)
        {
            beatbaseLines[beatBaseLineIndex].beats[beatIndex].notes[i] = notes[i];
        }
        for (int i = 0; i < notes.Length; i++)
        {
            beatbaseLines[beatBaseLineIndex].beats[beatIndex].notes[i + notes.Length] = notes[i];
        }
    }

    public void RemoveNotes(int beatBaseLineIndex, int beatIndex)
    {
        Note[] sampleNotes = beatbaseLines[beatBaseLineIndex].beats[beatIndex].notes;
        Note[] removedNotes = new Note[sampleNotes.Length / 2];
        for (int i = 0; i < removedNotes.Length; i++)
        {
            removedNotes[i] = sampleNotes[i];
        }
        beatbaseLines[beatBaseLineIndex].beats[beatIndex].notes = removedNotes;
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
        beatbaseLines[beatBaseLineIndex].beats[beatIndex].notes = newNotes;
    }

    public BeatBaseLine this[int x]
    {
        get
        {
            return beatbaseLines[x];
        }
    }

    public Beat this[int x, int y]
    {
        get
        {
            return beatbaseLines[x].beats[y];
        }
    }

    public Note this[int x, int y, int z]
    {
        get
        {
            return beatbaseLines[x].beats[y].notes[z];
        }
        set
        {
            beatbaseLines[x].beats[y].notes[z] = value;
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
    public class Beat
    {
        public AudioClip clip;

        public Note[] notes;

        public bool beatEnabled = true;

        public int division;

        public float volume = 1f;

        private MonoBehaviour _caller;

        public void Play(MonoBehaviour caller, float BPM, AudioSource source, float division)
        {
            _caller = caller;
            caller.StartCoroutine(PlayBeat(caller, BPM, source, division));
        }

        public void Stop()
        {
            _caller.StopAllCoroutines();
        }

        [BurstCompile]
        private IEnumerator PlayBeat(MonoBehaviour caller, float BPM, AudioSource source, float division)
        {
            while (true)
            {
                if (BeatManager.bpm != 0f)
                {
                    for (int i = 0; i < notes.Length; i++)
                    {
                        yield return new WaitForSecondsRealtime((((60f / notes.Length)) /** i*/ / BeatManager.bpm) * notes.Length / division);
                        if (!BeatManager.paused && beatEnabled)
                        {
                            IEnumerator IE = Play(BeatManager.bpm, notes[i].enabled, notes.Length, clip, source, (float)i, division);
                            caller.StartCoroutine(IE);
                            caller.StartCoroutine(notes[i].a());
                        }
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
        public IEnumerator Play(float BPM, bool noteEnabled, float notesLen, AudioClip clip, UnityEngine.AudioSource source, float i, float division)
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
        [HideInInspector] public bool played;
        [HideInInspector] public bool enabled;

        public IEnumerator a()
        {
            played = true;
            yield return new WaitForSecondsRealtime(1f);
            played = false;
            Debug.Log(played);
            yield break;
        }
    }
}