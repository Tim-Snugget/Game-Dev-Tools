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

    // CONSTANT VARIABLES
    private const int DEFAULT_FONT_SIZE = 50;
    private const float DEFAULT_DISPLAY_SPEED = 1f;

    // PRIVATE VARIABLES
    private Dialogue loadedDialogue = null;
    private ChoicesPrefab loadedChoice = null;
    private Queue<Sentence> sentences;
    private Coroutine sentenceCoroutine = null;

    [Header("UIElements")]
    [SerializeField]
    private Text dialogueName;
    [SerializeField]
    private Text dialogueText;
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Animator dialogueBoxAnimator;
    [SerializeField]
    private Transform dialogueBoxLayerTransform;
    [SerializeField]
    [Range(0,0.1f)]
    public float dispSpeedVar;

    [Header("Choices Prefabs")]
    [SerializeField]
    private ChoicesPrefab twoChoicesPrefab;
    [SerializeField]
    private ChoicesPrefab threeChoicesPrefab;
    [SerializeField]
    private ChoicesPrefab fourChoicesPrefab;


    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<Sentence>();
    }


    public void StartDialogue(Dialogue dialogue)
    {
        loadedDialogue = dialogue;
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
            if (sentenceCoroutine != null)
                StopCoroutine(sentenceCoroutine);
            EndDialogue();
            return;
        }
        Sentence s = sentences.Dequeue();
        string sentence = s.sentence;
        
        dialogueName.text = s.speaker;
        dialogueText.fontSize = s.extraOptions.textSize == 0 ?
            DEFAULT_FONT_SIZE : s.extraOptions.textSize;
        
        if (Math.Abs(s.extraOptions.displaySpeed - 5f) <= 0) {
            StopCoroutine(sentenceCoroutine);
            dialogueText.text = s.sentence;
        } else {
            if (sentenceCoroutine != null)
                StopCoroutine(sentenceCoroutine);
            sentenceCoroutine = StartCoroutine(TypeSentence(sentence, s.extraOptions.displaySpeed));
            
        }
    }
    

    IEnumerator TypeSentence(string sentence, float timer)
    {
        if (timer == 0) timer = DEFAULT_DISPLAY_SPEED;
        dialogueText.text = "";

        foreach (char letter in sentence) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dispSpeedVar / (timer * 2));
        }
        yield return null;
    }
    
    private void EndDialogue()
    {
        switch (loadedDialogue.choiceSelection) {
            case Dialogue.choices.two_choice:
                loadedChoice = Instantiate(twoChoicesPrefab, dialogueBoxLayerTransform);
                break;
            case Dialogue.choices.three_choice:
                loadedChoice = Instantiate(threeChoicesPrefab, dialogueBoxLayerTransform);
                break;
            case Dialogue.choices.four_choice:
                loadedChoice = Instantiate(fourChoicesPrefab, dialogueBoxLayerTransform);
                break;
            default:
                dialogueBoxAnimator.SetBool("isOpen", false);
                return;
        }
        dialogueText.text = "";
        loadedChoice.InitChoices(loadedDialogue, continueButton);
    }
}
