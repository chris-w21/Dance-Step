using System.Collections;
using System.Collections.Generic;
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

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
    }

    private void OnSceneGUI()
    {
        BeatManager beatManager = (BeatManager)target;

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
                            if (GUILayout.Button(beatManager[c, i, j].enabled ? beatManager.texture : beatManager.texture1, EditorStyles.radioButton, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                            {
                                beatManager[c].beats[i].notes[j].enabled = !beatManager[c, i, j].enabled;
                            }
                        }
                        GUILayout.EndHorizontal();
                    }

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
