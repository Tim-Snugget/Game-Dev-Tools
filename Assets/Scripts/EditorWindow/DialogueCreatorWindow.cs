using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueCreatorWindow : EditorWindow
{
    [System.Serializable]
    public class SentenceWindow
    {
        public enum characterEnum {Évy, Raoul, Mimolin, Archibald}
        public characterEnum character;
        [TextArea(3, 10)]
        public string sentence;

        public static Dialogue ConvertSentencesWindowToDialogue(List<SentenceWindow> sentences)
        {
            Dialogue res = new Dialogue();
            res.sentences = new List<Sentence>();

            foreach (SentenceWindow sw in sentences) {
                Sentence s = new Sentence();
                s.sentence = sw.sentence;
                s.talker = sw.character.ToString();
                res.sentences.Add(s);
            }
            return res;
        }
    }

    [SerializeField]
    List<SentenceWindow> sentences = new List<SentenceWindow>();


    [MenuItem("Window/Game Tools/Dialogue Creator")]
    private static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow<DialogueCreatorWindow>();
        window.Show();
    }


    private void OnGUI()
    {
        GUILayout.Label("Dialogue Creator", EditorStyles.boldLabel);
        GUILayout.BeginVertical();
            DrawSentences(this);

            GUILayout.FlexibleSpace();
        
            GUILayout.BeginHorizontal();
                if (GUILayout.Button("New")) {
                    Debug.Log("Click New");
                    sentences.Add(new SentenceWindow());
                }
                if (GUILayout.Button("Save")) {
                    Debug.Log("Click Save");
                    GameObject [] gos = Selection.gameObjects;

                    if (gos.Length != 1) {
                        this.ShowNotification(new GUIContent("Error - A GameObject containing a Dialogue Trigger must be selected to save the dialogue."), 2);
                    } else {

                        DialogueTrigger dialogueTrigger = gos[0].GetComponent<DialogueTrigger>();
                        if (dialogueTrigger == null) {
                            ShowNotification(new GUIContent("Error - Selected GameObject does not contain a Dialogue Trigger component."), 2);
                        } else {
                            Dialogue dialogueWindow = SentenceWindow.ConvertSentencesWindowToDialogue(sentences);
                            dialogueTrigger.dialogue = dialogueWindow;
                        }
                    }

                }
            GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    private void DrawSentences(UnityEngine.Object obj)
    {
        SerializedObject so = new SerializedObject(obj);
        SerializedProperty sp = so.FindProperty("sentences");
        EditorGUILayout.PropertyField(sp);
    }
}
