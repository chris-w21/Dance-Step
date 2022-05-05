using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StepsManager : AudioManager
{
    public List<StepsSequence> stepsSequences;

    [SerializeField] private int selectedSequence = 0;

    public int SelectedSequence
    {
        get
        {
            return selectedSequence;
        }
    }

    public Transform platform;

    public GameObject followStepPrefab, leadStepPrefab;

    public Material followStepOff, followStepOn, leadStepOff, leadStepOn;

    public Texture2D openStepTex;

    public int division = 8;

    [BurstCompile]
    public IEnumerator PlayLead()
    {
        while (true)
        {
            if (BeatManager.bpm != 0f)
            {
                for (int i = 0; i < StepsManager.stepsSequences[StepsManager.SelectedSequence].leadSteps.Count; i++)
                {
                    yield return new WaitForSecondsRealtime((((60f / StepsManager.stepsSequences[StepsManager.SelectedSequence].leadSteps.Count)) /** i*/ / BeatManager.bpm) * StepsManager.stepsSequences[StepsManager.SelectedSequence].leadSteps.Count / division);
                    if (!BeatManager.paused && StepsManager.stepsSequences[StepsManager.SelectedSequence].leadSteps[i].enabled)
                    {
                        StepsManager.stepsSequences[StepsManager.SelectedSequence].ActivateLead(i);
                    }
                }
            }
            else
            {
                yield return new WaitForSecondsRealtime(60f * StepsManager.stepsSequences[StepsManager.SelectedSequence].leadSteps.Count / division / BeatManager.bpm);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    [BurstCompile]
    public IEnumerator PlayFollow()
    {
        while (true)
        {
            if (BeatManager.bpm != 0f)
            {
                for (int i = 0; i < StepsManager.stepsSequences[StepsManager.SelectedSequence].followSteps.Count; i++)
                {
                    yield return new WaitForSecondsRealtime((((60f / StepsManager.stepsSequences[StepsManager.SelectedSequence].followSteps.Count)) /** i*/ / BeatManager.bpm) * StepsManager.stepsSequences[StepsManager.SelectedSequence].followSteps.Count / division);
                    if (!BeatManager.paused && StepsManager.stepsSequences[StepsManager.SelectedSequence].followSteps[i].enabled)
                    {
                        StepsManager.stepsSequences[StepsManager.SelectedSequence].ActivateFollow(i);
                    }
                }
            }
            else
            {
                yield return new WaitForSecondsRealtime(60f * StepsManager.stepsSequences[StepsManager.SelectedSequence].followSteps.Count / division / BeatManager.bpm);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    [System.Serializable]
    public struct LeftStep
    {
#if UNITY_EDITOR
        public EditorWindow window;
#endif
        public Renderer renderer
        {
            get
            {
                if (Renderer == null)
                {
                    Renderer = Instantiate(StepsManager.leadStepPrefab, new Vector3(0f, 0.015f, 0f), Quaternion.Euler(90f, 0f, 0f), StepsManager.transform);
                }
                return Renderer.GetComponent<Renderer>();
            }
        }
        public bool enabled;
        public State State
        {
            get
            {
                return state;
            }
            set
            {
                if (enabled)
                {
                    state = value;
                    switch (state)
                    {
                        case State.Off:
                            renderer.sharedMaterial = StepsManager.leadStepOff;
                            break;
                        case State.On:
                            renderer.sharedMaterial = StepsManager.leadStepOn;
                            break;
                    }
                }
            }
        }
        private State state;
        [SerializeField] private GameObject Renderer;
    }

    [System.Serializable]
    public struct RightStep
    {
#if UNITY_EDITOR
        public EditorWindow window;
#endif
        public Renderer renderer
        {
            get
            {
                if (Renderer == null)
                {
                    Renderer = Instantiate(StepsManager.followStepPrefab, new Vector3(0f, 0.015f, 0f), Quaternion.Euler(90f, 0f, 0f), StepsManager.transform);
                }
                return Renderer.GetComponent<MeshRenderer>();
            }
        }
        public bool enabled;
        public State State
        {
            get
            {
                return state;
            }
            set
            {
                if (enabled)
                {
                    state = value;
                    switch (state)
                    {
                        case State.Off:
                            renderer.sharedMaterial = StepsManager.followStepOff;
                            break;
                        case State.On:
                            renderer.sharedMaterial = StepsManager.followStepOn;
                            break;
                    }
                }
            }
        }
        private State state;
        [SerializeField] private GameObject Renderer;
    }

    [System.Serializable]
    public class StepsSequence
    {
        public List<LeftStep> leadSteps;
        public List<RightStep> followSteps;

        public int L
        {
            get
            {
                int a = 0;
                ActiveLead(out a);
                return a;
            }
            set
            {
                if (leadSteps[value].enabled)
                {
                    ActivateLead(value);
                }
            }
        }

        public bool ActiveLead(out int i)
        {
            bool oneWasActive = false;
            i = 0;
            for (int k = 0; k < leadSteps.Count; k++)
            {
                if (leadSteps[k].State == State.On)
                {
                    i = k;
                    oneWasActive = true;
                }
            }
            return oneWasActive;
        }

        public void ActivateLead(int i)
        {
            for (int k = 0; k < leadSteps.Count; k++)
            {
                LeftStep step = leadSteps[k];
                step.State = State.Off;
                leadSteps[k]= step;
            }
            LeftStep step1 = leadSteps[i];
            if (!followSteps[i].enabled)
            {
                DeactivateAllFollow();
            }
            step1.State = State.On;
            leadSteps[i] = step1;
        }

        public void DeactivateAllLead()
        {
            for (int k = 0; k < leadSteps.Count; k++)
            {
                LeftStep step = leadSteps[k];
                step.State = State.Off;
                leadSteps[k] = step;
            }
        }

        public bool ActiveFollow(out int i)
        {
            bool oneWasActive = false;
            i = 0;
            for (int k = 0; k < followSteps.Count; k++)
            {
                if (followSteps[k].State == State.On)
                {
                    i = k;
                    oneWasActive = true;
                }
            }
            return oneWasActive;
        }

        public void ActivateFollow(int i)
        {
            for (int k = 0; k < followSteps.Count; k++)
            {
                RightStep step = followSteps[k];
                step.State = State.Off;
                followSteps[k] = step;
            }
            RightStep step1 = followSteps[i];
            if (!leadSteps[i].enabled)
            {
                DeactivateAllLead();
            }
            step1.State = State.On;
            followSteps[i] = step1;
        }

        public void DeactivateAllFollow()
        {
            for (int k = 0; k < followSteps.Count; k++)
            {
                RightStep step = followSteps[k];
                step.State = State.Off;
                followSteps[k] = step;
            }
        }
    }

    [System.Serializable]
    public enum Type
    {
        Lead, Follow
    }

    [System.Serializable]
    public enum State
    {
        Off, On
    }
}