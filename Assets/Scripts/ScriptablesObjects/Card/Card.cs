using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New card", menuName = "Card/Card Object")]
public class Card : ScriptableObject
{
    public string cardName;
    public string cardDescription;

    public Sprite cardArtworks;
    public Sprite cardTemplate;

    public int cardCost;
    public int cardHealth;
    public int cardDamage;
}
