using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    public Material mat;

    public new Renderer renderer;

    public Material stepFull, stepHalf;

    [SerializeField] private State state;

    public State StepState
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
            switch (state)
            {
                case State.Off:
                    renderer.enabled = false;
                    break;
                case State.Half:
                    renderer.sharedMaterial = stepHalf;
                    renderer.enabled = true;
                    break;
                case State.Full:
                    renderer.sharedMaterial = stepFull;
                    renderer.enabled = true;
                    break;
            }
        }
    }

    [System.Serializable]
    public enum State
    {
        Off, Half, Full
    }
}
