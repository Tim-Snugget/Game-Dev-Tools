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
    private readonly string [] options = { "0", "2", "3", "4" };
    private enum CHOICE : int {NONE, TWO_CHOICES, THREE_CHOICES, FOUR_CHOICES};
    public static void Open(Dialogue dialogueObject)
    {
        DialogueObjectEditorWindow window = GetWindow<DialogueObjectEditorWindow>("Dialogue Window Editor");

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


    private void DisplayExtraOptions(SerializedProperty p)
    {
        EditorGUIUtility.fieldWidth = 150;
        EditorGUILayout.PropertyField(p, true);
        EditorGUIUtility.fieldWidth = 0;
    }
    
    
    private new void DrawProperties(SerializedProperty prop, bool drawChildren = true)
    {
        string lastPropPath = string.Empty;
        
        if (prop.propertyType == SerializedPropertyType.Generic) {
            foreach (SerializedProperty p in prop) {
        
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
                    if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) {
                        continue;
                    }
                    lastPropPath = p.propertyPath;
                    if (p.displayName == "Extra Options")
                        DisplayExtraOptions(p);
                    else
                        EditorGUILayout.PropertyField(p, drawChildren);
                }
            }
        } else {
            if (!string.IsNullOrEmpty(lastPropPath) && prop.propertyPath.Contains(lastPropPath))
                return;
            lastPropPath = prop.propertyPath;
            EditorGUILayout.PropertyField(prop, drawChildren);
        }
    }

    
    private void DisplayChoiceSelection()
    {
        SerializedProperty choiceSelectionProperty = serializedObject.FindProperty("choiceSelection");
        SerializedProperty firstChoiceTextProperty = serializedObject.FindProperty("firstChoice");
        SerializedProperty secondChoiceTextProperty = serializedObject.FindProperty("secondChoice");
        SerializedProperty thirdChoiceTextProperty = serializedObject.FindProperty("thirdChoice");
        SerializedProperty fourthChoiceTextProperty = serializedObject.FindProperty("fourthChoice");
        SerializedProperty firstChoiceObjectProperty = serializedObject.FindProperty("firstChoiceObject");
        SerializedProperty secondChoiceObjectProperty = serializedObject.FindProperty("secondChoiceObject");
        SerializedProperty thirdChoiceObjectProperty = serializedObject.FindProperty("thirdChoiceObject");
        SerializedProperty fourthChoiceObjectProperty = serializedObject.FindProperty("fourthChoiceObject");
        
        if (choiceSelectionProperty.enumValueIndex == (int)CHOICE.NONE)
            return;
        EditorGUIUtility.labelWidth = 200;
        EditorGUILayout.BeginHorizontal();
            firstChoiceTextProperty.stringValue = EditorGUILayout.TextField("First Choice :", firstChoiceTextProperty.stringValue);
            firstChoiceObjectProperty.objectReferenceValue = (Dialogue)EditorGUILayout.ObjectField(firstChoiceObjectProperty.objectReferenceValue, typeof(Dialogue), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
            secondChoiceTextProperty.stringValue = EditorGUILayout.TextField("Second Choice :", secondChoiceTextProperty.stringValue);
            secondChoiceObjectProperty.objectReferenceValue = (Dialogue)EditorGUILayout.ObjectField(secondChoiceObjectProperty.objectReferenceValue, typeof(Dialogue), true);
        EditorGUILayout.EndHorizontal();
        if (choiceSelectionProperty.enumValueIndex >= (int)CHOICE.THREE_CHOICES) {
            EditorGUILayout.BeginHorizontal();
                thirdChoiceTextProperty.stringValue = EditorGUILayout.TextField("Third Choice :", thirdChoiceTextProperty.stringValue);
                thirdChoiceObjectProperty.objectReferenceValue = (Dialogue)EditorGUILayout.ObjectField(thirdChoiceObjectProperty.objectReferenceValue, typeof(Dialogue), true);
            EditorGUILayout.EndHorizontal();
        }
        if (choiceSelectionProperty.enumValueIndex == (int)CHOICE.FOUR_CHOICES) {
            EditorGUILayout.BeginHorizontal();
                fourthChoiceTextProperty.stringValue = EditorGUILayout.TextField("Fourth Choice :", fourthChoiceTextProperty.stringValue);
                fourthChoiceObjectProperty.objectReferenceValue = (Dialogue)EditorGUILayout.ObjectField(fourthChoiceObjectProperty.objectReferenceValue, typeof(Dialogue), true);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUIUtility.labelWidth = 0;
    }
    
    
    private void OnGUI()
    {
        currentProperty = serializedObject.FindProperty("sentences");
        EditorGUILayout.BeginHorizontal();
            // SIDEBAR
            EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(120),
                GUILayout.MaxWidth(200), GUILayout.ExpandHeight(true));
                DrawSidebar();
                if (!string.IsNullOrEmpty(selectedPropertyPath))
                    selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
            EditorGUILayout.EndVertical();
        
            // DIALOGUE DETAILS
            EditorGUILayout.BeginVertical("box",
                GUILayout.MaxWidth(Screen.currentResolution.width / 2 - 200),
                GUILayout.MinWidth(50),
                GUILayout.ExpandHeight(true));
                if (selectedProperty != null)
                    DrawProperties(selectedProperty, true);
                else
                    EditorGUILayout.LabelField("Select an item from the list");
            EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
        
        GUILayout.FlexibleSpace();

        // DIALOGUE CHOICES
        EditorGUIUtility.labelWidth = 300;
        SerializedProperty choiceSelectionProperty = serializedObject.FindProperty("choiceSelection");
        choiceSelectionProperty.enumValueIndex = EditorGUILayout.Popup("How many choice at the end of the dialogue :", choiceSelectionProperty.enumValueIndex, options);
        EditorGUIUtility.labelWidth = 0;
        DisplayChoiceSelection();
        
        // save choices
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
