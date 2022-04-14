using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetHandler
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceId, int line)
    {
        Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceId) as Dialogue;
        Card card = EditorUtility.InstanceIDToObject(instanceId) as Card;
        
        if (dialogue != null)
        {
            DialogueObjectEditorWindow.Open(dialogue);
            return true;
        }
        else if (card != null)
        {
            CardEditorWindow.Open(card);
            return (true);
        }
        return (false);
    }
}