using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TalkObjectEditorWindow : ExtendedTalkEditorWindow
{
    public static void Open(Talk talkObject)
    {
        TalkObjectEditorWindow window = GetWindow<TalkObjectEditorWindow>("Talk Window Editor");
        window.serializedObject = new SerializedObject(talkObject);
    }

    private void OnGUI()
    {
        currentProperty = serializedObject.FindProperty("sentences");
        EditorGUILayout.BeginHorizontal();
        
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
                DrawSidebar(currentProperty);
            EditorGUILayout.EndVertical();
        
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
                if (selectedProperty != null)
                    DrawProperties(selectedProperty, true);
                else
                    EditorGUILayout.LabelField("Select an item from the list");
            EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
        
        GUILayout.FlexibleSpace();
        
        if (GUILayout.Button("Save")) {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
