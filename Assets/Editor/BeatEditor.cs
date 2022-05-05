using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Threading;
#if UNITY_EDITOR
[CustomEditor(typeof(BeatManager))]
#endif
public class BeatEditor :
#if UNITY_EDITOR
Editor
#endif
{
#if UNITY_EDITOR
    private Vector2 scrollPosition = new Vector2(), scroll2 = new Vector2();

    private AudioClip clip;

    private EditMode editMode;

    double clickTime;
    double doubleClickTime = 0.3;

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
    }

    private void OnSceneGUI()
    {
        BeatManager beatManager = (BeatManager)target;
        bool HasFocus = false;
        for (int i = 0; i < StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps.Count; i++)
        {
            if (StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps[i].window != null && StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps[i].enabled)
            {
                if (StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps[i].window.hasFocus)
                {
                    var a = StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps[i].renderer.transform.position;
                    var b = StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps[i].renderer.transform.rotation;
                    HasFocus = true;
                    Handles.TransformHandle(ref a, ref b);
                    StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps[i].renderer.transform.rotation = b;
                    StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps[i].renderer.transform.position = a;
                    GUILayout.BeginScrollView(scroll2);
                    if (GUILayout.Button("Reset"))
                    {
                        StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps[i].renderer.transform.position = Vector3.zero + Vector3.up * 0.015f;
                        StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps[i].renderer.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                        for (int j = 0; j < 2; j++)
                        {
                            StepsManager.LeftStep step = StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps[i];
                            SceneView sceneView = EditorWindow.GetWindow<SceneView>(false, i.ToString());
                            sceneView.pivot = step.renderer.transform.position + Vector3.up * 10f;
                            sceneView.camera.transform.position = step.renderer.transform.position + Vector3.up * 10f;
                            sceneView.rotation = Quaternion.LookRotation(step.renderer.transform.position - sceneView.pivot, sceneView.camera.transform.up);
                        }
                    }
                    GUILayout.EndScrollView();
                }
            }
        }

        for (int i = 0; i < StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps.Count; i++)
        {
            if (StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps[i].window != null && StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps[i].enabled)
            {
                if (StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps[i].window.hasFocus)
                {
                    var a = StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps[i].renderer.transform.position;
                    var b = StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps[i].renderer.transform.rotation;
                    HasFocus = true;
                    Handles.TransformHandle(ref a, ref b);
                    StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps[i].renderer.transform.rotation = b;
                    StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps[i].renderer.transform.position = a;
                    GUILayout.BeginScrollView(scroll2);
                    if (GUILayout.Button("Reset"))
                    {
                        StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps[i].renderer.transform.position = Vector3.zero + Vector3.up * 0.015f;
                        StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps[i].renderer.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                        for (int j = 0; j < 2; j++)
                        {
                            StepsManager.RightStep step = StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps[i];
                            SceneView sceneView = EditorWindow.GetWindow<SceneView>(false, i.ToString());
                            sceneView.pivot = step.renderer.transform.position + Vector3.up * 10f;
                            sceneView.camera.transform.position = step.renderer.transform.position + Vector3.up * 10f;
                            sceneView.rotation = Quaternion.LookRotation(step.renderer.transform.position - sceneView.pivot, sceneView.camera.transform.up);
                        }
                    }
                    GUILayout.EndScrollView();
                }
            }
        }
        if (HasFocus)
        {
            return;
        }
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.Space(50f);
        if (GUILayout.Button("Add New BeatBaseLine", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true), GUILayout.MaxHeight(100f), GUILayout.MaxWidth(150f)))
        {
            beatManager.AddBeatBaseLine();
        }

        if (beatManager.beatbaseLines.Length != 0)
        {
            for (int c = 0; c < beatManager.beatbaseLines.Length; c++)
            {
                if (GUILayout.Button("Add New Beat", GUILayout.MaxHeight(30f), GUILayout.MaxWidth(100f)))
                {
                    beatManager.AddNewBeat(c);
                }
                if (beatManager[c].beats.Length != 0)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    for (int i = 0; i < beatManager[c].beats.Length; i++)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);
                        GUILayout.BeginVertical();
                        beatManager[c].beats[i].clip = (AudioClip)EditorGUILayout.ObjectField(beatManager[c, i].clip, typeof(AudioClip), true, GUILayout.Width(100f));
                        if (GUILayout.Button("Clone", GUILayout.Width(100f)))
                        {
                            beatManager.CloneBeat(c, i);
                        }
                        if (GUILayout.Button("Remove", GUILayout.Width(100f)))
                        {
                            beatManager.RemoveBeat(c, i);
                        }
                        if (GUILayout.Button("Add Notes", GUILayout.Width(100f)))
                        {
                            beatManager.AddNotes(c, i);
                        }
                        if (GUILayout.Button("Remove Notes", GUILayout.Width(100f)))
                        {
                            if (beatManager[c, i].notes.Length > 8)
                            {
                                beatManager.RemoveNotes(c, i);
                            }
                        }
                        GUILayout.BeginHorizontal();
                        if (beatManager[c, i].division == 0)
                        {
                            beatManager.beatbaseLines[c].beats[i].division = 8;
                        }
                        if (GUILayout.Button("<<", GUILayout.Width(100f)))
                        {
                            if (beatManager[c, i].division > 8)
                            {
                                beatManager.beatbaseLines[c].beats[i].division /= 2;
                                beatManager.Divide(c, i);
                            }
                        }
                        if (GUILayout.Button(">>", GUILayout.Width(100f)))
                        {

                            if (beatManager[c, i].division < 64)
                            {
                                beatManager.beatbaseLines[c].beats[i].division *= 2;
                                beatManager.Divide(c, i);
                            }
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                        for (int j = 0; j < beatManager[c, i].notes.Length; j++)
                        {
                            if (GUILayout.Button(beatManager[c, i, j].enabled ? beatManager.texture : beatManager.texture1, EditorStyles.largeLabel, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                            {
                                beatManager[c].beats[i].notes[j].enabled = !beatManager[c, i, j].enabled;
                            }
                        }
                        GUILayout.EndHorizontal();
                    }

                    #region LeadSteps
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    GUILayout.BeginVertical();
                    GUILayout.Label("Lead");
                    if (GUILayout.Button("Add Steps", GUILayout.Width(100f)))
                    {
                        int count = StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps.Count;
                        for (int i = 0; i < count; i++)
                        {
                            StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps.Add(new StepsManager.LeftStep());
                        }
                    }
                    if (GUILayout.Button("Remove Steps", GUILayout.Width(100f)))
                    {
                        int count = StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps.Count / 2;
                        for (int i = 0; i < count; i++)
                        {
                            StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps.Remove(StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps[StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps.Count - 1]);
                        }
                    }
                    GUILayout.EndVertical();
                    for (int k = 0; k < StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps.Count; k++)
                    {
                        StepsManager.LeftStep step = StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps[k];
                        if (GUILayout.Button(StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps[k].enabled ? beatManager.texture : beatManager.texture1, EditorStyles.largeLabel, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                        {
                            if ((EditorApplication.timeSinceStartup - clickTime) < doubleClickTime)
                            {
                                if (step.window == null)
                                {
                                    EditorWindow window = new SceneView();
                                    SceneView sceneView1 = EditorWindow.GetWindow<SceneView>(false, window.titleContent.text);
                                    window.titleContent = new GUIContent() { text = k.ToString() };
                                    step.window = window;
                                }
                                SceneView sceneView = EditorWindow.GetWindow<SceneView >(false, step.window.titleContent.text);
                                step.window.minSize = new Vector2(400f, 400f);
                                step.window.maxSize = new Vector2(400f, 400f);
                                sceneView.showGrid = false;
                                sceneView.pivot = step.renderer.transform.position;
                                sceneView.rotation = Quaternion.Inverse(Quaternion.LookRotation(step.renderer.transform.position - sceneView.camera.transform.position, sceneView.camera.transform.up));
                                step.window.Show();
                            }


                            clickTime = EditorApplication.timeSinceStartup;

                            step.enabled = !step.enabled;
                            StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].leadSteps[k] = step;
                        }
                    }
                    GUILayout.EndHorizontal();
                    #endregion
                    #region FollowSteps
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    GUILayout.BeginVertical();
                    GUILayout.Label("Lead");
                    if (GUILayout.Button("Add Steps", GUILayout.Width(100f)))
                    {
                        int count = StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps.Count;
                        for (int i = 0; i < count; i++)
                        {
                            StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps.Add(new StepsManager.RightStep());
                        }
                    }
                    if (GUILayout.Button("Remove Steps", GUILayout.Width(100f)))
                    {
                        int count = StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps.Count / 2;
                        for (int i = 0; i < count; i++)
                        {
                            StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps.Remove(StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps[StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps.Count - 1]);
                        }
                    }
                    GUILayout.EndVertical();
                    for (int k = 0; k < StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps.Count; k++)
                    {
                        StepsManager.RightStep step = StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps[k];
                        if (GUILayout.Button(StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps[k].enabled ? beatManager.texture : beatManager.texture1, EditorStyles.largeLabel, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                        {
                            if ((EditorApplication.timeSinceStartup - clickTime) < doubleClickTime)
                            {
                                if (step.window == null)
                                {
                                    EditorWindow window = new SceneView();
                                    SceneView sceneView1 = EditorWindow.GetWindow<SceneView>(false, window.titleContent.text);
                                    window.titleContent = new GUIContent() { text = k.ToString() };
                                    step.window = window;
                                }
                                SceneView sceneView = EditorWindow.GetWindow<SceneView>(false, step.window.titleContent.text);
                                step.window.minSize = new Vector2(400f, 400f);
                                step.window.maxSize = new Vector2(400f, 400f);
                                sceneView.showGrid = false;
                                sceneView.pivot = step.renderer.transform.position;
                                sceneView.rotation = Quaternion.Inverse(Quaternion.LookRotation(step.renderer.transform.position - sceneView.camera.transform.position, sceneView.camera.transform.up));
                                step.window.Show();
                            }


                            clickTime = EditorApplication.timeSinceStartup;

                            step.enabled = !step.enabled;
                            StepsManager.StepsManager.stepsSequences[StepsManager.StepsManager.SelectedSequence].followSteps[k] = step;
                        }
                    }
                    GUILayout.EndHorizontal();
                    #endregion
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndScrollView();
        HandleUtility.AddDefaultControl(0);
    }

    public enum EditMode
    {
        Beat, Track
    }
#endif
}
