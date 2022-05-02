using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StepsManager : AudioManager
{
    public List<Step> leadSteps;

    public List<Step> followSteps;

    public Transform platform;

    public GameObject followStepPrefab, leadStepPrefab;

    public Material followStepOff, followStepOn, leadStepOff, leadStepOn;

    public Texture2D openStepTex;

    [System.Serializable]
    public struct Step
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
                    Renderer = Instantiate(type == Type.Lead ? Instantiate(StepsManager.leadStepPrefab, new Vector3(0f, 0.015f, 0f), Quaternion.Euler(90f, 0f, 0f), StepsManager.transform) : Instantiate(StepsManager.followStepPrefab, new Vector3(0f, 0.015f, 0f), Quaternion.Euler(90f, 0f ,0f), StepsManager.transform)).gameObject.GetComponent<MeshRenderer>();
                }
                return Renderer;
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
                state = value;
                switch (type)
                {
                    case Type.Follow:
                        switch (state)
                        {
                            case State.Off:
                                renderer.sharedMaterial = StepsManager.followStepOff;
                                break;
                            case State.On:
                                renderer.sharedMaterial = StepsManager.followStepOn;
                                break;
                        }
                        break;
                    case Type.Lead:
                        switch (state)
                        {
                            case State.Off:
                                renderer.sharedMaterial = StepsManager.leadStepOff;
                                break;
                            case State.On:
                                renderer.sharedMaterial = StepsManager.leadStepOn;
                                break;
                        }
                        break;
                }
            }
        }
        public Type type;
        private State state;
        private Renderer Renderer;
    }

    [System.Serializable]
    public enum State
    {
        Off, On
    }

    [System.Serializable]
    public enum Type
    {
        Lead, Follow
    }
}
