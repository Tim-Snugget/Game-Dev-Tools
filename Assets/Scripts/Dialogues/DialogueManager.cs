using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Singleton

    private static DialogueManager _instance;

    public static DialogueManager instance
    {
        get
        {
            if (_instance == null) {
                _instance = FindObjectOfType<DialogueManager>();
                if (_instance == null)
                    _instance = new GameObject().AddComponent<DialogueManager>();
            }
            
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this);
        else {
            _instance = this;
        }
        DontDestroyOnLoad(this);
    }

    #endregion
    
    [SerializeField]
    private Queue<string> sentences;
    [Header("UIElements")]
    [SerializeField]
    private Text dialogueName;
    [SerializeField]
    private Text dialogueText;
    [SerializeField]
    private Animator dialogueBoxAnimator;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueBoxAnimator.SetBool("isOpen", true);
        sentences.Clear();

        dialogueName.text = dialogue.name;
        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }
        
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }
        
        string sentence = sentences.Dequeue();
        // dialogueText.text = sentence;

        // StopCoroutine(TypeSentence(sentence));
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence) {
            dialogueText.text += letter;
            yield return null;
        }
    }
    
    private void EndDialogue()
    {
        dialogueBoxAnimator.SetBool("isOpen", false);
        // dialogueText.text = "";
        // dialogueName.text = "";
    }
}
