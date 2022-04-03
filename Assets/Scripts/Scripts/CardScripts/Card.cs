using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public int id;
    public string cardName;
    public string attackColour;
    public string defenseColour;
    public string cardEffect;
    public int damage;
    public Sprite frontImage;
    public Sprite backImage;
}
