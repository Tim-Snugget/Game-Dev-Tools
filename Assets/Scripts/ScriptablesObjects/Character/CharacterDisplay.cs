using UnityEngine.UI;
using UnityEngine;

public class CharacterDisplay : MonoBehaviour
{
    public int artworkIt = 0;
    public Character character;

    public Text characterName;
    public Image characterImage;

    void Start()
    {
        characterName.text = character.characterName;

        characterImage.sprite = character.characterArtworks[artworkIt].artworkSprite;
    }
}
