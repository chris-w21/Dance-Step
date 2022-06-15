using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Footwork))]
public class FootworkPatternEditor : Editor
{
    private Footwork footwork;

    public override void OnInspectorGUI()
    {
        footwork = (Footwork)target;
        base.OnInspectorGUI();
    }

    public void OnSceneGUI()
    {
        footwork = (Footwork)target;
        Event e = Event.current;
        for (int i = 0; i < footwork.leftSteps.Count; i++)
        {
            if (footwork.leftSteps[i] == null)
            {
                footwork.leftSteps.Remove(footwork.leftSteps[i]);
            }
        }
        if (e.type == EventType.MouseDown && e.shift && e.button == 0)
        {
            RaycastHit hit;
            Ray newRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(newRay, out hit, Mathf.Infinity) && hit.transform.CompareTag("Ground"))
            {
                GameObject spawnedStep = Instantiate(footwork.leftStepPrefab, new Vector3(hit.point.x, 0.01f, hit.point.z), new Quaternion(0.707106829f, 0, 0, 0.707106829f), footwork.transform);
                Undo.RegisterCreatedObjectUndo(spawnedStep, "Add Step (Left Mous Button)");
                footwork.leftSteps.Add(spawnedStep.GetComponent<Renderer>());
            }
        }

        for (int i = 0; i < footwork.rightSteps.Count; i++)
        {
            if (footwork.rightSteps[i] == null)
            {
                footwork.rightSteps.Remove(footwork.rightSteps[i]);
            }
        }
        if (e.type == EventType.MouseDown && e.shift && e.button == 1)
        {
            RaycastHit hit;
            Ray newRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(newRay, out hit, Mathf.Infinity) && hit.transform.CompareTag("Ground"))
            {
                GameObject spawnedStep = Instantiate(footwork.rightStepPrefab, new Vector3(hit.point.x, 0.01f, hit.point.z), new Quaternion(0.707106829f, 0, 0, 0.707106829f), footwork.transform);
                Undo.RegisterCreatedObjectUndo(spawnedStep, "Add Step (Left Mous Button)");
                footwork.rightSteps.Add(spawnedStep.GetComponent<Renderer>());
            }
        }
        HandleUtility.AddDefaultControl(0);
    }
}
