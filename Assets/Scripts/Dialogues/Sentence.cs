using UnityEngine;

[System.Serializable]
public class Sentence
{
    public string talker;

    [TextArea(3, 10)]
    public string sentence;
}
