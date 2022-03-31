using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DialogueContinueButton : MonoBehaviour
{
    private DialogueManager dialogueManager;
    [SerializeField]
    private Button continueBtn;

    void Start()
    {
        dialogueManager = DialogueManager.instance;
        
        if (dialogueManager == null)
            Debug.LogWarning("Error loading the DialogueManager in the DialogueContinueButton");

        if (continueBtn == null) {
            Debug.LogWarning("Careful, you forgot to assign the button from the Inspector in the DialogueContinueButton...!");
            continueBtn = gameObject.GetComponent<Button>();
        }
        continueBtn.onClick.AddListener(dialogueManager.DisplayNextSentence);
    }

    private void OnDestroy()
    {
        continueBtn.onClick.RemoveListener(dialogueManager.DisplayNextSentence);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
