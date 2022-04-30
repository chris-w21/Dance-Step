using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutineManager : MonoBehaviour
{
    private readonly string[] leftFootSequence = new string[4] { "1", "0", "0", "0"};
    private readonly string[] rightFootSequence = new string[4] { "0", "0", "-1", "0"};

    [SerializeField] private int leftStep = 0;
    [SerializeField] private int rightStep = 0;

    [SerializeField] private float delay = 1f;

    [SerializeField] private float waitTime = 0f;

    [SerializeField] private Step[] stepsLeft;
    [SerializeField] private Step[] stepsRight;

    public float magnitude = 2.5f;

    private void Update()
    {
        waitTime += Time.deltaTime;
        if (waitTime >= delay)
        {
            waitTime = 0f;
            leftStep = leftStep < leftFootSequence.Length - 1 ? leftStep + 1 : 0;
            rightStep = rightStep < rightFootSequence.Length - 1? rightStep + 1 : 0;
            switch (int.Parse(leftFootSequence[leftStep]))
            {
                case -1:
                    stepsLeft[0].StepState = Step.State.Full;
                    stepsLeft[1].StepState = Step.State.Half;
                    stepsLeft[2].StepState = Step.State.Half;
                    break;
                case 0:
                    stepsLeft[0].StepState = Step.State.Half;
                    stepsLeft[1].StepState = Step.State.Full;
                    stepsLeft[2].StepState = Step.State.Half;
                    break;
                case 1:
                    stepsLeft[0].StepState = Step.State.Half;
                    stepsLeft[1].StepState = Step.State.Half;
                    stepsLeft[2].StepState = Step.State.Full;
                    break;
            }
            switch (int.Parse(rightFootSequence[rightStep]))
            {
                case -1:
                    stepsRight[0].StepState = Step.State.Full;
                    stepsRight[1].StepState = Step.State.Half;
                    stepsRight[2].StepState = Step.State.Half;
                    break;
                case 0:
                    stepsRight[0].StepState = Step.State.Half;
                    stepsRight[1].StepState = Step.State.Full;
                    stepsRight[2].StepState = Step.State.Half;
                    break;
                case 1:
                    stepsRight[0].StepState = Step.State.Half;
                    stepsRight[1].StepState = Step.State.Half;
                    stepsRight[2].StepState = Step.State.Full;
                    break;
            }
        }

    }
}
