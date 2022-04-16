using UnityEngine;

[System.Serializable]
public class Sentence
{
    [System.Serializable]
    public class ExtraOptions
    {
        [Tooltip("Couldn't put a default value to this field - 0 will be interpreted as 1f.")]
        [Range(1, 5)]
        public float displaySpeed = 1f;
        [Tooltip("Couldn't put a default value to this field - 0 will be interpreted as 50.")]
        [Range(50, 60)]
        public int textSize = 50;

    }
    
    public string speaker;

    [TextArea(3, 10)]
    public string sentence;
    public ExtraOptions extraOptions = new ExtraOptions();
}
