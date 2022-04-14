using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private const int DEFAULT_FONT_SIZE = 50;
    private const float DEFAULT_DISPLAY_SPEED = 1f;
    
    private Queue<Sentence> sentences;
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
        sentences = new Queue<Sentence>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueBoxAnimator.SetBool("isOpen", true);
        sentences.Clear();

        foreach (Sentence sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        } // needs clear?
        
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }
        
        // string sentence = sentences.Dequeue();
        Sentence s = sentences.Dequeue();
        string sentence = s.sentence;
        
        dialogueName.text = s.speaker;

        dialogueText.fontSize = s.extraOptions.textSize == 0 ?
            DEFAULT_FONT_SIZE : s.extraOptions.textSize;
        
        if (Math.Abs(s.extraOptions.displaySpeed - 5f) < 0) {
            dialogueText.text = s.sentence;
        } else {
            // StopCoroutine(TypeSentence(sentence));
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence, s.extraOptions.displaySpeed));
        }
    }
    
    [Range(0,1)]
    public float tacticalFloat;

    IEnumerator TypeSentence(string sentence, float timer)
    {
        if (timer == 0) timer = DEFAULT_DISPLAY_SPEED;
        int length = sentence.Length;
        dialogueText.text = "";
        float waitingTime = (length / timer) / length;
        Debug.Log("Sentence : " + sentence + "(" + sentence.Length + ") has a timer of " + timer + " which gives a waitingTime of " + waitingTime);

        foreach (char letter in sentence) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(tacticalFloat / timer);
            // yield return new WaitForSeconds(waitingTime);
        }
    }
    
    private void EndDialogue()
    {
        dialogueBoxAnimator.SetBool("isOpen", false);
        // dialogueText.text = "";
        // dialogueName.text = "";
    }
}
