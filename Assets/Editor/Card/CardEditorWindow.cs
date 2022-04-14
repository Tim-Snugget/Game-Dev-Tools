using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CardEditorWindow : ExtendedCardEditorWindow
{
    public static void Open(Card card)
    {
        CardEditorWindow window = GetWindow<CardEditorWindow>("Card Window Editor");
        window.serializedObject = new SerializedObject(card);
    }

    private void OnGUI()
    {
        // currentProperty = serializedObject.FindProperty("cardName");

        // DrawProperties(currentProperty, true);
    }
}
