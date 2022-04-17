using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Character", menuName = "Character/Character Object")]
public class Character : ScriptableObject
{
    public string characterName;

    public List<Artwork> characterArtworks;
}