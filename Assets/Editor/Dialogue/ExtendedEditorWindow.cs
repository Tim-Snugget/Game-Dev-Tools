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

    protected void DrawProperties(SerializedProperty prop, bool drawChildren = true)
    {
        string lastPropPath = string.Empty;

        if (prop.propertyType == SerializedPropertyType.Generic) {
            foreach (SerializedProperty p in prop) {
                // if the property is an array, opens and unfolds it and its children
                Debug.Log("Prop " + p.displayName + " is of type " + p.propertyType);
                if (p.propertyType == SerializedPropertyType.Integer)
                    Debug.Log("~~~~ " + p.displayName + " is equal to " + p.intValue);
                if (p.propertyType == SerializedPropertyType.Float)
                    Debug.Log("~~~~ " + p.displayName + " is equal to " + p.floatValue);

                if (p.isArray && p.propertyType == SerializedPropertyType.Generic) {
                    Debug.Log("### " + p.displayName + " is a Generic Array");
                    EditorGUILayout.BeginHorizontal();
                    p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                    EditorGUILayout.EndHorizontal();

                    if (p.isExpanded) {
                        EditorGUI.indentLevel++;
                        DrawProperties(p, drawChildren);
                        EditorGUI.indentLevel--;
                    }
                } else {
                    if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) {
                        Debug.Log("we CONTINUE with lastPropPath == " + lastPropPath);
                        continue;
                    }
                    lastPropPath = p.propertyPath;
                    EditorGUILayout.PropertyField(p, drawChildren);
                }
            }
        } else {
            if (!string.IsNullOrEmpty(lastPropPath) && prop.propertyPath.Contains(lastPropPath))
                return;
            lastPropPath = prop.propertyPath;
            EditorGUILayout.PropertyField(prop, drawChildren);
            Debug.Log("here");
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
