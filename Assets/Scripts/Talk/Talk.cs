using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Talk Object")]
public class Talk : ScriptableObject
{
    public string resume;
    public List<Sentence> sentences = new List<Sentence>();
}
