using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[CustomEditor(typeof(Dialogue))]
public class DialogueObjectCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Open Editor")) {
            DialogueObjectEditorWindow.Open((Dialogue)target);
        }
    }
}