using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PianoRollManager))]
public class PianoRollEditor : Editor
{
    public PianoRollManager pianoRollmanger;

    private Vector2 scrollPos;

    private void OnSceneGUI()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = guiEvent.mousePosition;
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUILayout.BeginVertical();
        for (int i = 0; i < PianoRoll.KeyNotes.Length; i++)
        {
            GUILayout.Button(PianoRoll.KeyNotes[(PianoRoll.KeyNotes.Length - 1) - i]);
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }
}