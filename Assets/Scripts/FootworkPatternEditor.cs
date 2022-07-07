using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(Footwork))]
public class FootworkPatternEditor : Editor
{
    private Footwork footwork;

    private Vector3 originalStepScale = new Vector3(0.817920864f, 1.96672571f, 2.51049995f);

    private Vector3 hoverStepScale = new Vector3(0.913669944f, 2.19695854f, 2.804389f);

    public override void OnInspectorGUI()
    {
        footwork = (Footwork)target;
        bool _linear = GUILayout.Toggle(footwork.linear, footwork.linear ? "Linear" : "Mono", GUILayout.Width(100), GUILayout.Height(25), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

        if (footwork.linear != _linear)
        {
            footwork.linear = _linear;
            HandleUtility.Repaint();
        }

        base.OnInspectorGUI();
    }

    public void OnSceneGUI()
    {
        footwork = (Footwork)target;
        Event e = Event.current;

        if (footwork.linear)
        {
            #region Linear
            for (int i = 0; i < footwork.allSteps.Count; i++)
            {
                if (footwork.allSteps[i] == null)
                {
                    footwork.leftSteps.Remove(footwork.allSteps[i]);
                    footwork.rightSteps.Remove(footwork.allSteps[i]);
                    footwork.allSteps.Remove(footwork.allSteps[i]);
                }
                else
                {
                    GUIStyle style = new GUIStyle();
                    style.alignment = TextAnchor.MiddleCenter;
                    style.normal.textColor = footwork.allSteps[i].sharedMaterial.name.Contains("left") || footwork.allSteps[i].sharedMaterial.name.Contains("Left") ? footwork.allSteps[i].sharedMaterial == footwork.leftStepOffMat ? new Color(0.254902f, 0.06666667f, 1f, 1f) : new Color(0.07239216f, 0.01893333f, 0.284f, 1f) : footwork.allSteps[i].sharedMaterial == footwork.rightStepOffMat ? new Color(1, 0.5490196f, 0.06666667f, 1) : new Color(0.574f, 0.3151373f, 0.03826667f, 1f);
                    style.fontSize = 25;
                    style.fontStyle = FontStyle.Bold;
                    Handles.Label(footwork.allSteps[i].transform.position + Vector3.forward * 0.35f - Vector3.right * 0.05f, (i + 1).ToString(), style);
                }
            }

            for (int i = 0; i < footwork.leftSteps.Count; i++)
            {
                if (!footwork.leftSteps[i].enabled)
                {
                    continue;
                }
                if (e.type == EventType.MouseDown)
                {
                    Undo.RegisterCompleteObjectUndo(footwork.leftSteps[i].transform, "Move");
                }
                try
                {
                    Vector3 relative = footwork.leftSteps[i].transform.position + new Vector3(0.5f, 0.01f, 0.75f);
                    relative = Handles.PositionHandle(relative, Quaternion.identity);
                    footwork.leftSteps[i].transform.position = relative - new Vector3(0.5f, 0.01f, 0.75f);
                }
                catch (MissingReferenceException)
                {
                    continue;
                }
            }

            for (int i = 0; i < footwork.rightSteps.Count; i++)
            {
                if (!footwork.rightSteps[i].enabled)
                {
                    continue;
                }
                if (e.type == EventType.MouseDown)
                {
                    Undo.RegisterCompleteObjectUndo(footwork.rightSteps[i].transform, "Move");
                }
                try
                {
                    Vector3 relative = footwork.rightSteps[i].transform.position + new Vector3(0.5f, 0.01f, 0.75f);
                    relative = Handles.PositionHandle(relative, Quaternion.identity);
                    footwork.rightSteps[i].transform.position = relative - new Vector3(0.5f, 0.01f, 0.75f);
                }
                catch (MissingReferenceException)
                {
                    continue;
                }
            }

            //Add Left
            if (e.type == EventType.MouseDown && e.shift && e.button == 0)
            {
                RaycastHit hit;
                Ray newRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (Physics.Raycast(newRay, out hit, Mathf.Infinity))
                {
                    switch (hit.transform.tag)
                    {
                        case "Ground":
                            AddStep(new Vector3(hit.point.x, 0.01f, hit.point.z), false, true);
                            break;
                        case "Off":
                            if (hit.transform.name.Contains("Left"))
                            {
                                AddStep(hit.transform.position, hit.transform.CompareTag("On"), true);
                                return;
                            }
                            else
                            {
                                AddStep(new Vector3(hit.point.x, 0.01f, hit.point.z), false, true);
                                break;
                            }
                            break;
                        case "On":
                            if (hit.transform.name.Contains("Left"))
                            {
                                AddStep(hit.transform.position, hit.transform.CompareTag("On"), true);
                                return;
                            }
                            else
                            {
                                AddStep(new Vector3(hit.point.x, 0.01f, hit.point.z), false, true);
                            }
                            break;
                    }
                }
            }

            //Add Right
            if (e.type == EventType.MouseDown && e.shift && e.button == 1)
            {
                RaycastHit hit;
                Ray newRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (Physics.Raycast(newRay, out hit, Mathf.Infinity) && hit.transform.CompareTag("Ground") || hit.transform.CompareTag("Off") || hit.transform.CompareTag("On"))
                {
                    switch (hit.transform.tag)
                    {
                        case "Ground":
                            AddStep(new Vector3(hit.point.x, 0.01f, hit.point.z), false, false);
                            break;
                        case "Off":
                            if (hit.transform.name.Contains("Right"))
                            {
                                AddStep(hit.transform.position, hit.transform.CompareTag("On"), false);
                                return;
                            }
                            else
                            {
                                AddStep(new Vector3(hit.point.x, 0.01f, hit.point.z), false, false);
                            }
                            break;
                        case "On":
                            if (hit.transform.name.Contains("Right"))
                            {
                                AddStep(hit.transform.position, hit.transform.CompareTag("On"), false);
                                return;
                            }
                            else
                            {
                                AddStep(new Vector3(hit.point.x, 0.01f, hit.point.z), false, false);
                            }
                            break;
                    }
                }
            }

            //Add Both
            if (e.type == EventType.MouseDown && e.shift && e.button == 2)
            {
                RaycastHit hit;
                Ray newRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (Physics.Raycast(newRay, out hit, Mathf.Infinity) && hit.transform.CompareTag("Ground"))
                {
                    GameObject spawnedLeftStep = Instantiate(footwork.leftStepPrefab, new Vector3(hit.point.x - 0.5f, 0.01f, hit.point.z), new Quaternion(0.707106829f, 0, 0, 0.707106829f), footwork.transform);
                    Undo.RegisterCreatedObjectUndo(spawnedLeftStep, "Add Both Steps (Middle Mouse Button + Shift)");
                    Undo.RegisterCompleteObjectUndo(footwork, "Add Left (Middle Mouse Button)");
                    footwork.leftSteps.Add(spawnedLeftStep.GetComponent<Renderer>());

                    GameObject spawnedRightStep = Instantiate(footwork.rightStepPrefab, new Vector3(hit.point.x + 0.5f, 0.01f, hit.point.z), new Quaternion(0.707106829f, 0, 0, 0.707106829f), footwork.transform);
                    Undo.RegisterCreatedObjectUndo(spawnedRightStep, "Add Both Steps (Middle Mouse Button + Shift)");
                    Undo.RegisterCompleteObjectUndo(footwork, "Add Right (Middle Mouse Button)");
                    footwork.rightSteps.Add(spawnedRightStep.GetComponent<Renderer>());
                }
            }

            RaycastHit hit1;
            Ray newRay1 = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(newRay1, out hit1, Mathf.Infinity))
            {
                if (hit1.transform.CompareTag("On") || hit1.transform.CompareTag("Off"))
                {
                    hit1.transform.localScale = Vector3.Lerp(hit1.transform.localScale, hoverStepScale, 5f * Time.deltaTime);
                    SceneView.RepaintAll();
                    //Toggle
                    if (e.type == EventType.MouseDown && e.button == 0 && !e.shift)
                    {
                        Undo.RegisterCompleteObjectUndo(hit1.transform.GetComponent<MeshRenderer>(), "Toggle Step (Left Mous Button)");
                        hit1.transform.tag = hit1.transform.CompareTag("On") ? hit1.transform.tag = "Off" : hit1.transform.tag = "On";
                        Material rend = hit1.transform.GetComponent<Renderer>().sharedMaterial;
                        if (hit1.transform.CompareTag("On"))
                        {
                            if (rend.name.Contains("left"))
                            {
                                hit1.transform.GetComponent<Renderer>().sharedMaterial = footwork.leftStepOnMat;
                            }
                            else
                            {
                                hit1.transform.GetComponent<Renderer>().sharedMaterial = footwork.rightStepOnMat;
                            }
                        }
                        else
                        {
                            if (rend.name.Contains("left"))
                            {
                                hit1.transform.GetComponent<Renderer>().sharedMaterial = footwork.leftStepOffMat;
                            }
                            else
                            {
                                hit1.transform.GetComponent<Renderer>().sharedMaterial = footwork.rightStepOffMat;
                            }
                        }
                    }
                    //Remove
                    else if (e.type == EventType.MouseDown && e.button == 1 && !e.shift)
                    {
                        if (hit1.transform.name.Contains("Left"))
                        {
                            Undo.RegisterCompleteObjectUndo(footwork, "Remove Left Step (Right Mouse Button)");
                            footwork.leftSteps.RemoveAt(footwork.leftSteps.IndexOf(hit1.transform.GetComponent<Renderer>()));
                        }
                        else
                        {
                            Undo.RegisterCompleteObjectUndo(footwork, "Remove Right Step (Right Mouse Button)");
                            footwork.rightSteps.RemoveAt(footwork.rightSteps.IndexOf(hit1.transform.GetComponent<Renderer>()));
                        }
                        Undo.DestroyObjectImmediate(hit1.transform.gameObject);
                    }
                }
                else
                {
                    for (int i = 0; i < footwork.leftSteps.Count; i++)
                    {
                        footwork.leftSteps[i].transform.localScale = Vector3.Lerp(footwork.leftSteps[i].transform.localScale, originalStepScale, 5f * Time.deltaTime); ;
                    }
                    for (int i = 0; i < footwork.rightSteps.Count; i++)
                    {
                        footwork.rightSteps[i].transform.localScale = Vector3.Lerp(footwork.rightSteps[i].transform.localScale, originalStepScale, 5f * Time.deltaTime); ;
                    }
                }
            }
            else
            {
                for (int i = 0; i < footwork.leftSteps.Count; i++)
                {
                    try
                    {
                        footwork.leftSteps[i].transform.localScale = Vector3.Lerp(footwork.leftSteps[i].transform.localScale, originalStepScale, 5f * Time.deltaTime);
                    }
                    catch (MissingReferenceException)
                    {
                        continue;
                    }
                }
                for (int i = 0; i < footwork.rightSteps.Count; i++)
                {
                    try
                    {
                        footwork.rightSteps[i].transform.localScale = Vector3.Lerp(footwork.rightSteps[i].transform.localScale, originalStepScale, 5f * Time.deltaTime);
                    }
                    catch (MissingReferenceException)
                    {
                        continue;
                    }
                }
            }

            HandleUtility.AddDefaultControl(0);
            #endregion Linear
        }
        else
        {
            #region Mono

            for (int i = 0; i < footwork.allSteps.Count; i++)
            {
                if (footwork.allSteps[i] == null)
                {
                    footwork.allSteps.Remove(footwork.allSteps[i]);
                }
            }

            for (int i = 0; i < footwork.leftSteps.Count; i++)
            {
                if (!footwork.leftSteps[i])
                {
                    footwork.leftSteps.Remove(footwork.leftSteps[i]);
                }
                else
                {
                    GUIStyle style = new GUIStyle();
                    style.alignment = TextAnchor.MiddleCenter;
                    style.normal.textColor = footwork.leftSteps[i].sharedMaterial == footwork.leftStepOffMat ? new Color(0.254902f, 0.06666667f, 1f, 1f) : new Color(0.07239216f, 0.01893333f, 0.284f, 1f);
                    style.fontSize = 25;
                    style.fontStyle = FontStyle.Bold;
                    if (footwork.leftSteps[i].gameObject.activeSelf)
                    {
                        Handles.Label(footwork.leftSteps[i].transform.position + Vector3.forward * 0.35f - Vector3.right * 0.05f, (i + 1).ToString(), style);
                        footwork.leftSteps[i].name = i + " ( Left " + footwork.leftSteps[i].tag + " )";
                    }
                }
            }

            for (int i = 0; i < footwork.rightSteps.Count; i++)
            {
                if (!footwork.rightSteps[i])
                {
                    footwork.rightSteps.Remove(footwork.rightSteps[i]);
                }
                else
                {
                    GUIStyle style = new GUIStyle();
                    style.alignment = TextAnchor.MiddleCenter;
                    style.normal.textColor = footwork.rightSteps[i].sharedMaterial == footwork.rightStepOffMat ? new Color(1, 0.5490196f, 0.06666667f, 1) : new Color(0.574f, 0.3151373f, 0.03826667f, 1f);
                    style.fontSize = 25;
                    style.fontStyle = FontStyle.Bold;
                    if (footwork.rightSteps[i].gameObject.activeSelf)
                    {
                        Handles.Label(footwork.rightSteps[i].transform.position + Vector3.forward * 0.35f - Vector3.right * 0.05f, (i + 1).ToString(), style);
                        footwork.rightSteps[i].name = i + " ( Right " + footwork.rightSteps[i].tag + " )";
                    }
                }
            }

            for (int i = 0; i < footwork.leftSteps.Count; i++)
            {
                if (!footwork.leftSteps[i].enabled)
                {
                    continue;
                }
                if (e.type == EventType.MouseDown)
                {
                    Undo.RegisterCompleteObjectUndo(footwork.leftSteps[i].transform, "Move");
                }
                try
                {
                    Vector3 relative = footwork.leftSteps[i].transform.position + new Vector3(0.5f, 0.01f, 0.75f);
                    if (footwork.leftSteps[i].gameObject.activeSelf)
                    {
                        relative = Handles.PositionHandle(relative, Quaternion.identity);
                    }
                    footwork.leftSteps[i].transform.position = relative - new Vector3(0.5f, 0.01f, 0.75f);
                }
                catch (MissingReferenceException)
                {
                    continue;
                }
            }

            for (int i = 0; i < footwork.rightSteps.Count; i++)
            {
                if (!footwork.rightSteps[i].enabled)
                {
                    continue;
                }
                if (e.type == EventType.MouseDown)
                {
                    Undo.RegisterCompleteObjectUndo(footwork.rightSteps[i].transform, "Move");
                }
                try
                {
                    Vector3 relative = footwork.rightSteps[i].transform.position + new Vector3(0.5f, 0.01f, 0.75f);
                    if (footwork.rightSteps[i].gameObject.activeSelf)
                    {
                        relative = Handles.PositionHandle(relative, Quaternion.identity);
                    }
                    footwork.rightSteps[i].transform.position = relative - new Vector3(0.5f, 0.01f, 0.75f);
                }
                catch (MissingReferenceException)
                {
                    continue;
                }
            }

            //Add Left
            if (e.type == EventType.MouseDown && e.shift && e.button == 0)
            {
                RaycastHit hit;
                Ray newRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (Physics.Raycast(newRay, out hit, Mathf.Infinity))
                {
                    switch (hit.transform.tag)
                    {
                        case "Ground":
                            AddStep(new Vector3(hit.point.x, 0.01f, hit.point.z), State.Off, true);
                            break;
                        case "Off":
                            if (hit.transform.name.Contains("Left"))
                            {
                                AddStep(hit.transform.position, State.Off, true);
                            }
                            else
                            {
                                AddStep(new Vector3(hit.point.x, 0.01f, hit.point.z), State.Off, true);
                            }
                            break;
                        case "On":
                            if (hit.transform.name.Contains("Left"))
                            {
                                AddStep(hit.transform.position, State.On, true);
                            }
                            else
                            {
                                AddStep(new Vector3(hit.point.x, 0.01f, hit.point.z), State.Off, true);
                            }
                            break;
                        case "In":
                            if (hit.transform.name.Contains("Left"))
                            {
                                AddStep(hit.transform.position, State.In, true);
                            }
                            else
                            {
                                AddStep(new Vector3(hit.point.x, 0.01f, hit.point.z), State.Off, true);
                            }
                            break;
                    }
                }
            }

            //Add Right
            if (e.type == EventType.MouseDown && e.shift && e.button == 1)
            {
                RaycastHit hit;
                Ray newRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (Physics.Raycast(newRay, out hit, Mathf.Infinity))
                {
                    switch (hit.transform.tag)
                    {
                        case "Ground":
                            AddStep(new Vector3(hit.point.x, 0.01f, hit.point.z), State.Off, false);
                            break;
                        case "Off":
                            if (hit.transform.name.Contains("Right"))
                            {
                                AddStep(hit.transform.position, State.Off, false);
                            }
                            else
                            {
                                AddStep(new Vector3(hit.point.x, 0.01f, hit.point.z), State.Off, false);
                            }
                            break;
                        case "In":
                            if (hit.transform.name.Contains("Right"))
                            {
                                AddStep(hit.transform.position, State.In, false);
                            }
                            else
                            {
                                AddStep(new Vector3(hit.point.x, 0.01f, hit.point.z), State.Off, false);
                            }
                            break;
                        case "On":
                            if (hit.transform.name.Contains("Right"))
                            {
                                AddStep(hit.transform.position, State.On, false);
                            }
                            else
                            {
                                AddStep(new Vector3(hit.point.x, 0.01f, hit.point.z), State.Off, false);
                            }
                            break;
                    }
                }
            }

            //Add Both
            if (e.type == EventType.MouseDown && e.shift && e.button == 2)
            {
                RaycastHit hit;
                Ray newRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (Physics.Raycast(newRay, out hit, Mathf.Infinity) && hit.transform.CompareTag("Ground"))
                {
                    GameObject spawnedLeftStep = Instantiate(footwork.leftStepPrefab, new Vector3(hit.point.x - 0.5f, 0.01f, hit.point.z), new Quaternion(0.707106829f, 0, 0, 0.707106829f), footwork.transform);
                    Undo.RegisterCreatedObjectUndo(spawnedLeftStep, "Add Both Steps (Middle Mouse Button + Shift)");
                    Undo.RegisterCompleteObjectUndo(footwork, "Add Left (Middle Mouse Button)");
                    footwork.leftSteps.Add(spawnedLeftStep.GetComponent<Renderer>());

                    GameObject spawnedRightStep = Instantiate(footwork.rightStepPrefab, new Vector3(hit.point.x + 0.5f, 0.01f, hit.point.z), new Quaternion(0.707106829f, 0, 0, 0.707106829f), footwork.transform);
                    Undo.RegisterCreatedObjectUndo(spawnedRightStep, "Add Both Steps (Middle Mouse Button + Shift)");
                    Undo.RegisterCompleteObjectUndo(footwork, "Add Right (Middle Mouse Button)");
                    footwork.rightSteps.Add(spawnedRightStep.GetComponent<Renderer>());
                }
            }

            RaycastHit hit1;
            Ray newRay1 = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(newRay1, out hit1, Mathf.Infinity))
            {
                if (hit1.transform.CompareTag("On") || hit1.transform.CompareTag("Off") || hit1.transform.CompareTag("In"))
                {
                    hit1.transform.localScale = Vector3.Lerp(hit1.transform.localScale, hoverStepScale, 5f * Time.deltaTime);
                    SceneView.RepaintAll();
                    //Toggle
                    if (e.type == EventType.MouseDown && e.button == 0 && !e.shift)
                    {
                        Undo.RegisterCompleteObjectUndo(hit1.transform.GetComponent<MeshRenderer>(), "Toggle Step (Left Mous Button)");
                        Renderer rend = hit1.transform.GetComponent<Renderer>();
                        switch (hit1.transform.tag)
                        {
                            case "Off":
                                hit1.transform.tag = "In";
                                if (rend.name.Contains("left") || rend.name.Contains("Left"))
                                {
                                    rend.sharedMaterial = footwork.leftStepOffMat;
                                }
                                else
                                {
                                    rend.sharedMaterial = footwork.rightStepOffMat;
                                }
                                break;
                            case "In":
                                hit1.transform.tag = "On";
                                if (rend.name.Contains("left") || rend.name.Contains("Left"))
                                {
                                    rend.sharedMaterial = footwork.leftStepOnMat;
                                }
                                else
                                {
                                    rend.sharedMaterial = footwork.rightStepOnMat;
                                }
                                break;
                            case "On":
                                hit1.transform.tag = "Off";
                                if (rend.name.Contains("left") || rend.name.Contains("Left"))
                                {
                                    rend.sharedMaterial = footwork.leftStepEmptyMat;
                                }
                                else
                                {
                                    rend.sharedMaterial = footwork.rightStepEmptyMat;
                                }
                                break;
                        }
                    }
                    //Remove
                    else if (e.type == EventType.MouseDown && e.button == 1 && !e.shift)
                    {
                        if (hit1.transform.name.Contains("Left"))
                        {
                            Undo.RegisterCompleteObjectUndo(footwork, "Remove Left Step (Right Mouse Button)");
                            footwork.leftSteps.RemoveAt(footwork.leftSteps.IndexOf(hit1.transform.GetComponent<Renderer>()));
                        }
                        else
                        {
                            Undo.RegisterCompleteObjectUndo(footwork, "Remove Right Step (Right Mouse Button)");
                            footwork.rightSteps.RemoveAt(footwork.rightSteps.IndexOf(hit1.transform.GetComponent<Renderer>()));
                        }
                        Undo.DestroyObjectImmediate(hit1.transform.gameObject);
                    }
                }
                else
                {
                    for (int i = 0; i < footwork.leftSteps.Count; i++)
                    {
                        footwork.leftSteps[i].transform.localScale = Vector3.Lerp(footwork.leftSteps[i].transform.localScale, originalStepScale, 5f * Time.deltaTime); ;
                    }
                    for (int i = 0; i < footwork.rightSteps.Count; i++)
                    {
                        footwork.rightSteps[i].transform.localScale = Vector3.Lerp(footwork.rightSteps[i].transform.localScale, originalStepScale, 5f * Time.deltaTime); ;
                    }
                }
            }
            else
            {
                for (int i = 0; i < footwork.leftSteps.Count; i++)
                {
                    try
                    {
                        footwork.leftSteps[i].transform.localScale = Vector3.Lerp(footwork.leftSteps[i].transform.localScale, originalStepScale, 5f * Time.deltaTime);
                    }
                    catch (MissingReferenceException)
                    {
                        continue;
                    }
                }
                for (int i = 0; i < footwork.rightSteps.Count; i++)
                {
                    try
                    {
                        footwork.rightSteps[i].transform.localScale = Vector3.Lerp(footwork.rightSteps[i].transform.localScale, originalStepScale, 5f * Time.deltaTime);
                    }
                    catch (MissingReferenceException)
                    {
                        continue;
                    }
                }
            }

            HandleUtility.AddDefaultControl(0);
            #endregion
        }
    }

    private void AddStep(Vector3 position, bool on, bool left)
    {
        switch (left)
        {
            case true:
                GameObject spawnedStep = Instantiate(footwork.leftStepPrefab, position, new Quaternion(0.707106829f, 0, 0, 0.707106829f), footwork.transform);
                spawnedStep.tag = on ? "On" : "Off";
                Renderer rend = spawnedStep.GetComponent<Renderer>();
                rend.sharedMaterial = on ? footwork.leftStepOnMat : footwork.leftStepOffMat;
                Undo.RegisterCreatedObjectUndo(spawnedStep, "Add Left Step (Left Mouse Button + Shift)");
                Undo.RegisterCompleteObjectUndo(footwork, "Predicate List");
                footwork.leftSteps.Add(spawnedStep.GetComponent<Renderer>());
                Undo.RegisterCompleteObjectUndo(footwork, "Predicate List");
                footwork.allSteps.Add(rend);
                break;
            case false:
                GameObject spawnedStep1 = Instantiate(footwork.rightStepPrefab, position, new Quaternion(0.707106829f, 0, 0, 0.707106829f), footwork.transform);
                spawnedStep1.tag = on ? "On" : "Off";
                Renderer rend1 = spawnedStep1.GetComponent<Renderer>();
                rend1.sharedMaterial = on ? footwork.rightStepOnMat : footwork.rightStepOffMat;
                Undo.RegisterCreatedObjectUndo(spawnedStep1, "Add Right Step (Right Mouse Button + Shift)");
                Undo.RegisterCompleteObjectUndo(footwork, "Predicate List");
                footwork.rightSteps.Add(spawnedStep1.GetComponent<Renderer>());
                Undo.RegisterCompleteObjectUndo(footwork, "Predicate List");
                footwork.allSteps.Add(rend1);
                break;
        }
    }

    private void AddStep(Vector3 position, State _state, bool left)
    {
        switch (left)
        {
            case true:
                GameObject spawnedStep = Instantiate(footwork.leftStepPrefab, position, new Quaternion(0.707106829f, 0, 0, 0.707106829f), footwork.transform);
                Renderer rend = spawnedStep.GetComponent<Renderer>();
                switch (_state)
                {
                    case State.On:
                        rend.sharedMaterial = footwork.leftStepOnMat;
                        break;
                    case State.In:
                        rend.sharedMaterial = footwork.leftStepOffMat;
                        break;
                    case State.Off:
                        rend.sharedMaterial = footwork.leftStepEmptyMat;
                        break;
                }
                rend.tag = _state.ToString();
                Undo.RegisterCreatedObjectUndo(spawnedStep, "Add Left Step (Left Mouse Button + Shift)");
                Undo.RegisterCompleteObjectUndo(footwork, "Predicate List");
                footwork.leftSteps.Add(spawnedStep.GetComponent<Renderer>());
                Undo.RegisterCompleteObjectUndo(footwork, "Predicate List");
                footwork.allSteps.Add(rend);
                break;
            case false:
                GameObject spawnedStep1 = Instantiate(footwork.rightStepPrefab, position, new Quaternion(0.707106829f, 0, 0, 0.707106829f), footwork.transform);
                Renderer rend1 = spawnedStep1.GetComponent<Renderer>();
                switch (_state)
                {
                    case State.On:
                        rend1.sharedMaterial = footwork.rightStepOnMat;
                        break;
                    case State.In:
                        rend1.sharedMaterial = footwork.rightStepOffMat;
                        break;
                    case State.Off:
                        rend1.sharedMaterial = footwork.rightStepEmptyMat;
                        break;
                }
                rend1.tag = _state.ToString();
                Undo.RegisterCreatedObjectUndo(spawnedStep1, "Add Right Step (Right Mouse Button + Shift)");
                Undo.RegisterCompleteObjectUndo(footwork, "Predicate List");
                footwork.rightSteps.Add(spawnedStep1.GetComponent<Renderer>());
                Undo.RegisterCompleteObjectUndo(footwork, "Predicate List");
                footwork.allSteps.Add(rend1);
                break;
        }
    }

    public enum State
    {
        On, In, Off
    }
}
#else
public class FootworkPatternEditor
{

}
#endif