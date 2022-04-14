using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class DialogueObjectEditorWindow : ExtendedDialogueEditorWindow
{

    public static void Open(Dialogue dialogueObject)
    {
        DialogueObjectEditorWindow window = GetWindow<DialogueObjectEditorWindow>("Dialogue Window Editor");
        window.serializedObject = new SerializedObject(dialogueObject);
    }

    private void OnGUI()
    {
        currentProperty = serializedObject.FindProperty("sentences");
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        DrawSidebar();
        if (!string.IsNullOrEmpty(selectedPropertyPath))
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        if (selectedProperty != null)
            DrawProperties(selectedProperty, true);
        else
            EditorGUILayout.LabelField("Select an item from the list");
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        Repaint();
        serializedObject.ApplyModifiedProperties();
    }
}
