using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogues/Dialogue Object")]
public class Dialogue : ScriptableObject
{
    public string resume;
    public List<Sentence> sentences = new List<Sentence>();
}
