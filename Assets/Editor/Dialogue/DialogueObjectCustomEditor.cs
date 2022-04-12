using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;


// public class AssetHandler
// {
//     [OnOpenAsset()]
//     public static bool OpenEditor(int instanceId, int line)
//     {
//         Dialogue obj = EditorUtility.InstanceIDToObject(instanceId) as Dialogue;
//         if (obj != null) {
//             DialogueObjectEditorWindow.Open(obj);
//             return true;
//         }
//         return false;
//     }
// }


[CustomEditor(typeof(Dialogue))]
public class DialogueObjectCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Open Editor")) {
            DialogueObjectEditorWindow.Open((Dialogue)target);
        }
    }
}