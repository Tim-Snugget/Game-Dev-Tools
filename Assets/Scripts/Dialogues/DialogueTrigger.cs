using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private DialogueManager dialogueManager;
    public Dialogue dialogue;
 
    private void Awake()
    {
        dialogueManager = DialogueManager.instance;
    }

    public void TriggerDialogue()
    {
        if (dialogueManager == null)
            Debug.LogWarning("Error with the DialogueManager");
        dialogueManager.StartDialogue(dialogue);
    }
}
