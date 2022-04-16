using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogues/Dialogue Object")]
public class Dialogue : ScriptableObject
{
    [Tooltip("No use, just so the user can note what's the topic of the dialogue.")]
    [TextArea(1, 3)]
    public string resume; // no use, just so the user can note what's the topic of the dialogue
    public List<Sentence> sentences = new List<Sentence>();

    public enum choices {
        no_choice, two_choice, three_choice, four_choice
    };
    public choices choiceSelection = choices.no_choice;
    public string firstChoice, secondChoice, thirdChoice, fourthChoice;
    public Dialogue firstChoiceObject, secondChoiceObject, thirdChoiceObject, fourthChoiceObject;
}
