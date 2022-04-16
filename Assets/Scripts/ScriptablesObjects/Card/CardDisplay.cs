using UnityEngine.UI;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{

    public Card card;

    public Text nameText;
    public Text descriptionText;
    public Text costText;
    public Text healthText;
    public Text damageText;

    public Image artworkImage;
    public Image templateImage;

    void Start()
    {
        nameText.text = card.cardName;
        descriptionText.text = card.cardDescription;
        costText.text = card.cardCost.ToString();
        healthText.text = card.cardHealth.ToString();
        damageText.text = card.cardDamage.ToString();

        artworkImage.sprite = card.cardArtworks;
        templateImage.sprite = card.cardTemplate;

    }
}
