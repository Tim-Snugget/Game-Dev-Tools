using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
public class ExtendedDialogueEditorWindow : ExtendedEditorWindow
{
    public ReorderableList list;
    public string selectedPropertyPath;
    public SerializedProperty selectedProperty;

    // Draws my contextual Sidebar instead of the one implemented in the Extended parent class
    public void DrawSidebar()
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("sentences"),
            true, true, true, true);

        list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Dialogue's order");
        list.drawElementCallback = (rect, index, active, focused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                new GUIContent(element.FindPropertyRelative("speaker").stringValue), GUIContent.none);
            EditorGUIUtility.editingTextField = false;
        };
        list.onSelectCallback += SelectItemFromList;

        if (!string.IsNullOrEmpty(selectedPropertyPath))
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);

        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    public void SelectItemFromList(ReorderableList l)
    {
        selectedPropertyPath = currentProperty.GetArrayElementAtIndex(l.index).propertyPath;

        if (!string.IsNullOrEmpty(selectedPropertyPath))
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
        Repaint();
    }

    public void CreateNewElement(SerializedProperty prop)
    {
        prop.arraySize++;
        prop.InsertArrayElementAtIndex(prop.arraySize);
    }
}
