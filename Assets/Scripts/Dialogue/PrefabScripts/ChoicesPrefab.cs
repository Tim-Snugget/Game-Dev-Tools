using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChoicesPrefab : MonoBehaviour
{
    [SerializeField]
    private Button firstChoice, secondChoice, thirdChoice, fourthChoice;
    [SerializeField]
    private Text firstChoiceText, secondChoiceText, thirdChoiceText, fourthChoiceText;
    
    public List<Dialogue> dialoguesChoices;
    private Button continueButton;

    private void OnEnable()
    {
        firstChoice.onClick.AddListener(delegate{SelectDialogue(0);});
        secondChoice.onClick.AddListener(delegate{SelectDialogue(1);});
        if (thirdChoice != null)
            thirdChoice.onClick.AddListener(delegate{SelectDialogue(2);});
        if (fourthChoice != null)
            fourthChoice.onClick.AddListener(delegate{SelectDialogue(3);});
    }


    private void OnDisable()
    {
        firstChoice.onClick.RemoveListener(delegate{SelectDialogue(0);});
        secondChoice.onClick.RemoveListener(delegate{SelectDialogue(1);});
        if (thirdChoice != null)
            thirdChoice.onClick.RemoveListener(delegate{SelectDialogue(2);});
        if (fourthChoice != null)
            fourthChoice.onClick.RemoveListener(delegate{SelectDialogue(3);});
    }


    public void InitChoices(Dialogue dialogue, Button continueBtn)
    {
        continueButton = continueBtn;
        continueButton.gameObject.SetActive(false);
        firstChoiceText.text = dialogue.firstChoice;
        secondChoiceText.text = dialogue.secondChoice;
        dialoguesChoices.Add(dialogue.firstChoiceObject);
        dialoguesChoices.Add(dialogue.secondChoiceObject);
        if (dialogue.choiceSelection >= Dialogue.choices.three_choice) {
            thirdChoiceText.text = dialogue.thirdChoice;
            dialoguesChoices.Add(dialogue.thirdChoiceObject);
        }
        if (dialogue.choiceSelection == Dialogue.choices.four_choice) {
            fourthChoiceText.text = dialogue.fourthChoice;
            dialoguesChoices.Add(dialogue.fourthChoiceObject);
        }
    }
    

    private void SelectDialogue(int i)
    {
        DialogueManager dialogueManager = DialogueManager.instance;
        
        if (dialogueManager != null && dialoguesChoices.Count >= i + 1 && dialoguesChoices[i] != null)
            dialogueManager.StartDialogue(dialoguesChoices[i]);
        Destroy(gameObject);
        continueButton.gameObject.SetActive(true);
    }
}
