using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class ExtendedEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;
    
    protected string selectedPropertyPath;
    protected SerializedProperty selectedProperty;

    protected void DrawProperties(SerializedProperty prop, bool drawChildren)
    {
        Debug.Log("property : " + prop.displayName);
        string lastPropPath = string.Empty;

        // checks if the property is an array - then draws it
        if (prop.isArray && prop.propertyType == SerializedPropertyType.Generic) {
            foreach (SerializedProperty p in prop) {
                // if the property is an array, opens and unfolds it and its children
                Debug.Log(">>> p is " + p.displayName + " <<<");
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
                    Debug.Log("===== last prop " + lastPropPath);
                    if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath))
                        continue;
                    Debug.Log("passed check as p = " + p.displayName);
                    lastPropPath = p.propertyPath;
                    EditorGUILayout.PropertyField(p, drawChildren);
                }
            }
        } else { // if prop is not an array, simply draws the corresponding prop field
            lastPropPath = prop.propertyPath;
            EditorGUILayout.PropertyField(prop, drawChildren);
        }
    }


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
