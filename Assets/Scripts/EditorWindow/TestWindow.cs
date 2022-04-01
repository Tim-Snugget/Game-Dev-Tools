using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestWindow : EditorWindow
{
    /// example of guilayout
    private string hw = "Hello World!";
    bool groupEnabled;
    private bool testEnabled = true;
    private float testFloat = 1.23f;
    
    [MenuItem("Window/TestWindow")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow<TestWindow>();
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        hw = EditorGUILayout.TextField("Text Field", hw);
        
        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        testEnabled = EditorGUILayout.Toggle("Toggle", testEnabled);
        testFloat = EditorGUILayout.FloatField("Float", testFloat);
        testFloat = EditorGUILayout.Slider("Float Slider", testFloat, -3f, 3f);
        
        EditorGUILayout.EndToggleGroup();
    }
}
