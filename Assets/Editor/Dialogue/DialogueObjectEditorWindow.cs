using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class DialogueObjectEditorWindow : ExtendedEditorWindow
{
    private ReorderableList list;
    
    
    public static void Open(Dialogue dialogueObject)
    {
        DialogueObjectEditorWindow window = GetWindow<DialogueObjectEditorWindow>("Dialogue Window Editor");
        window.serializedObject = new SerializedObject(dialogueObject);
    }

    // Draws my contextual Sidebar instead of the one implemented in the Extended parent class
    private void DrawSidebar()
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("sentences"),
            true, true, true, true);

        list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Dialogue's order");
        // list.drawElementCallback = (rect, index, active, focused) =>
        // {
        //     var element = list.serializedProperty.GetArrayElementAtIndex(index);
        //     rect.y += 2;
        //     EditorGUI.PropertyField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
        //         element.FindPropertyRelative("Name"), GUIContent.none);
        // };
        list.onSelectCallback += SelectItemFromList;

        if (!string.IsNullOrEmpty(selectedPropertyPath))
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);

        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
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

    
    private void SelectItemFromList(ReorderableList l)
    {
        selectedPropertyPath = currentProperty.GetArrayElementAtIndex(l.index).propertyPath;

        if (!string.IsNullOrEmpty(selectedPropertyPath))
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
        Repaint();
    }

}
