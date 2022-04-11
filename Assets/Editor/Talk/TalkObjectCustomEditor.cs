using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;


public class AssetHandler
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceId, int line)
    {
        Talk obj = EditorUtility.InstanceIDToObject(instanceId) as Talk;
        if (obj != null) {
            TalkObjectEditorWindow.Open(obj);
            return true;
        }
        return false;
    }
}


[CustomEditor(typeof(Talk))]
public class TalkObjectCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Open Editor")) {
            TalkObjectEditorWindow.Open((Talk)target);
        }
    }
}