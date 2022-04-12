using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class ExtendedDialogueEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;
    
    protected string selectedPropertyPath;
    protected SerializedProperty selectedProperty;
    
    private ReorderableList list;

    // private void OnEnable()
    // {
    //     list.onSelectCallback += SelectItemFromList;
    // }

    private void OnDisable()
    {
        if (list.onSelectCallback != null)
            list.onSelectCallback -= SelectItemFromList;
    }

    protected void DrawProperties(SerializedProperty prop, bool drawChildren)
    {
        string lastPropPath = String.Empty;

        foreach (SerializedProperty p in prop) {
            // if the property is an array, opens and unfolds it and its children
            if (p.isArray && p.propertyType == SerializedPropertyType.Generic) {
                EditorGUILayout.BeginHorizontal();
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                EditorGUILayout.EndHorizontal();

                if (p.isExpanded) {
                    EditorGUI.indentLevel++;
                    DrawProperties(p, drawChildren);
                    EditorGUI.indentLevel--;
                }
            } else {
                if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath))
                    continue;
                lastPropPath = p.propertyPath;
                EditorGUILayout.PropertyField(p, drawChildren);
            }
        }
    }

    protected void DrawCustomSidebar(SerializedObject obj)
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("sentences"),
            true, true, true, true);

        list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Dialogue's order");
        list.drawElementCallback = (rect, index, active, focused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Name"), GUIContent.none);
        };
        list.onSelectCallback += SelectItemFromList;

        if (!string.IsNullOrEmpty(selectedPropertyPath))
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);

        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void SelectItemFromList(ReorderableList l)
    {
        Debug.Log("current property : " + currentProperty.displayName);
        selectedPropertyPath = currentProperty.GetArrayElementAtIndex(l.index).propertyPath;

        if (!string.IsNullOrEmpty(selectedPropertyPath))
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
        Debug.Log("sPP : " + selectedPropertyPath + " | sP : " + selectedProperty);
        Repaint();
    }

    // old way shown in the tutorial
    protected void DrawSidebar(SerializedProperty prop)
    {
        foreach (SerializedProperty p in prop) {
            if (GUILayout.Button(p.displayName)) {
                selectedPropertyPath = p.propertyPath;
                
                EditorGUIUtility.editingTextField = false;
            }
        }
        
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Entry")) {
            prop.InsertArrayElementAtIndex(prop.arraySize);
        }
        
        if (!string.IsNullOrEmpty(selectedPropertyPath))
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
    }

    private void CreateNewElement(SerializedProperty prop)
    {
        prop.arraySize++;
        prop.InsertArrayElementAtIndex(prop.arraySize);
    }
}
