using UnityEngine;

[System.Serializable]
public class Sentence
{
    [System.Serializable]
    public class ExtraOptions
    {
        [Range(1, 5)]
        public float displaySpeed = 1f;
        [Range(50, 60)]
        public int textSize = 14;
    }
    
    public string speaker;

    [TextArea(3, 10)]
    public string sentence;
    public ExtraOptions extraOptions = new ExtraOptions();
}
