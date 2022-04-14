using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class DialogueObjectEditorWindow : ExtendedEditorWindow
{
    private ReorderableList list = null;
    
    
    public static void Open(Dialogue dialogueObject)
    {
        DialogueObjectEditorWindow window = GetWindow<DialogueObjectEditorWindow>("Dialogue Window Editor");

        // DialogueObjectEditorWindow window = 
        //     GetWindowWithRect<DialogueObjectEditorWindow>(
        //         new Rect(0, 0, Screen.currentResolution.width / 2, Screen.currentResolution.height), 
        //         false, "Dialogue Editor Window");
        window.serializedObject = new SerializedObject(dialogueObject);
    }
    

    // Draws my contextual Sidebar instead of the one implemented in the Extended parent class
    private void DrawSidebar()
    {
        if (list == null)
            PrepareList();
        
        if (!string.IsNullOrEmpty(selectedPropertyPath))
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);

        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void OnGUI()
    {
        currentProperty = serializedObject.FindProperty("sentences");
        EditorGUILayout.BeginHorizontal();
        
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(200), GUILayout.ExpandHeight(true));
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

        serializedObject.ApplyModifiedProperties();
    }


    #region listMethods
    
    private void PrepareList()
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
        };
        list.onAddCallback = AddItemToList;
        list.onRemoveCallback = RemoveItemFromList;
        list.onSelectCallback = SelectItemFromList;
        
        list.DoLayoutList();
    }
    
    private void SelectItemFromList(ReorderableList l)
    {
        selectedPropertyPath = currentProperty.GetArrayElementAtIndex(l.index).propertyPath;

        if (!string.IsNullOrEmpty(selectedPropertyPath))
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
    }



    private void AddItemToList(ReorderableList l)
    {
        var index = l.serializedProperty.arraySize;
        l.serializedProperty.arraySize++;
        l.index = index;
        var element = l.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("speaker").stringValue = string.Empty;
        element.FindPropertyRelative("sentence").stringValue = string.Empty;
        
        // Sentence.ExtraOptions extraOptions = new Sentence.ExtraOptions();
        // element.FindPropertyRelative("extraOptions").objectReferenceValue = extraOptions as GameObject;
        SelectItemFromList(l);
    }


    private void RemoveItemFromList(ReorderableList l)
    {
        if (EditorUtility.DisplayDialog("Warning!",
                "Are you sure you want to delete the dialogue?", "Yes", "No")) {
            ReorderableList.defaultBehaviours.DoRemoveButton(l);
            SelectItemFromList(l);
        }
    }
    
    #endregion // listMethods
}
