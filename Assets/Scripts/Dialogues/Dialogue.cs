using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField]
    private string _name;
    public string name {
        get => _name;
        private set => _name = value;
    }
    [TextArea(3,10)]
    public string[] sentences;
}
