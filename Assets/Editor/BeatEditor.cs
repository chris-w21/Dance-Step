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
    private Vector2 scrollPosition = new Vector2();

    private AudioClip clip;

    private EditMode editMode;

    double clickTime;
    double doubleClickTime = 0.3;

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
    }
    Vector3 a = Vector3.zero;
    private void OnSceneGUI()
    {
        BeatManager beatManager = (BeatManager)target;
        bool HasFocus = false;
        for (int i = 0; i < BeatManager.StepsManager.leadSteps.Count; i++)
        {
            if (BeatManager.StepsManager.leadSteps[i].window != null)
            {
                if (BeatManager.StepsManager.leadSteps[i].window.hasFocus)
                {
                    HasFocus = true;
                    var a = BeatManager.StepsManager.leadSteps[i].renderer.transform.position;
                    var b = BeatManager.StepsManager.leadSteps[i].renderer.transform.rotation;
                    Handles.TransformHandle(ref a,ref b);
                    if (a != BeatManager.StepsManager.leadSteps[i].renderer.transform.position || b != BeatManager.StepsManager.leadSteps[i].renderer.transform.rotation)
                    {
                        Undo.RecordObject(BeatManager.StepsManager, "Move" + "Step " + i.ToString());
                    }
                    BeatManager.StepsManager.leadSteps[i].renderer.transform.position = a;
                    BeatManager.StepsManager.leadSteps[i].renderer.transform.rotation = b;
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
        // Set the colour of the next handle to be drawn:

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

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    GUILayout.BeginVertical();
                    GUILayout.Label("Lead");
                    if (GUILayout.Button("Add Steps", GUILayout.Width(100f)))
                    {
                        int count = BeatManager.StepsManager.leadSteps.Count;
                        for (int i = 0; i < count; i++)
                        {
                            BeatManager.StepsManager.leadSteps.Add(new StepsManager.Step() { type = StepsManager.Type.Follow});
                        }
                    }
                    if (GUILayout.Button("Remove Steps", GUILayout.Width(100f)))
                    {
                        int count = BeatManager.StepsManager.leadSteps.Count / 2;
                        for (int i = 0; i < count; i++)
                        {
                            BeatManager.StepsManager.leadSteps.Remove(BeatManager.StepsManager.leadSteps[BeatManager.StepsManager.leadSteps.Count - 1]);
                        }
                    }
                    GUILayout.EndVertical();
                    for (int k = 0; k < BeatManager.StepsManager.leadSteps.Count; k++)
                    {
                        StepsManager.Step step = BeatManager.StepsManager.leadSteps[k];
                        if (GUILayout.Button(BeatManager.StepsManager.leadSteps[k].enabled ? beatManager.texture : beatManager.texture1, EditorStyles.largeLabel, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                        {
                            if ((EditorApplication.timeSinceStartup - clickTime) < doubleClickTime)
                            {
                                if (step.window == null)
                                {
                                    EditorWindow window = new SceneView();
                                    window.titleContent = new GUIContent() { text = step.type.ToString() + " " + k.ToString() };
                                    SceneView sceneView = EditorWindow.GetWindow<SceneView>(false, window.titleContent.text);
                                    window.minSize = new Vector2(400f, 400f);
                                    window.maxSize = new Vector2(400f, 400f);
                                    window.Show();
                                    sceneView.showGrid = false;
                                    sceneView.pivot = step.renderer.transform.position;
                                    sceneView.rotation = Quaternion.Inverse(Quaternion.LookRotation(step.renderer.transform.position - sceneView.camera.transform.position, sceneView.camera.transform.up));
                                    step.window = window;
                                }
                                else
                                {
                                    SceneView sceneView = EditorWindow.GetWindow<SceneView>(false, step.type.ToString() + " " + k.ToString());
                                    step.window.minSize = new Vector2(400f, 400f);
                                    step.window.maxSize = new Vector2(400f, 400f);
                                    step.window.Show();
                                    sceneView.showGrid = false;
                                    sceneView.pivot = step.renderer.transform.position;
                                    sceneView.rotation = Quaternion.Inverse(Quaternion.LookRotation(step.renderer.transform.position - sceneView.camera.transform.position, sceneView.camera.transform.up));
                                    step.window.Show();
                                }
                            }


                            clickTime = EditorApplication.timeSinceStartup;

                            step.enabled = !step.enabled;
                            BeatManager.StepsManager.leadSteps[k] = step;
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    GUILayout.BeginVertical();
                    GUILayout.Label("Follow");
                    if (GUILayout.Button("Add Steps", GUILayout.Width(100f)))
                    {
                        int count = BeatManager.StepsManager.followSteps.Count;
                        for (int i = 0; i < count; i++)
                        {
                            BeatManager.StepsManager.followSteps.Add(new StepsManager.Step() { type = StepsManager.Type.Follow });
                        }
                    }
                    if (GUILayout.Button("Remove Steps", GUILayout.Width(100f)))
                    {
                        int count = BeatManager.StepsManager.followSteps.Count / 2;
                        for (int i = 0; i < count; i++)
                        {
                            BeatManager.StepsManager.followSteps.Remove(BeatManager.StepsManager.followSteps[BeatManager.StepsManager.followSteps.Count - 1]);
                        }
                    }
                    GUILayout.EndVertical();
                    for (int k = 0; k < BeatManager.StepsManager.followSteps.Count; k++)
                    {
                        if (GUILayout.Button(BeatManager.StepsManager.followSteps[k].enabled ? beatManager.texture : beatManager.texture1, EditorStyles.largeLabel, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                        {
                            StepsManager.Step step = BeatManager.StepsManager.followSteps[k];
                            step.enabled = !step.enabled;
                            BeatManager.StepsManager.followSteps[k] = step;
                        }
                    }
                    GUILayout.EndHorizontal();
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
