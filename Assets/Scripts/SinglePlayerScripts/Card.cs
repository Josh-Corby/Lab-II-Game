using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Card
{
    public int id;
    public string cardName;
    public string attackColour;
    public string defenseColour;
    public string cardEffect;
    public int damage;

    public Sprite thisImage;

    public Card()
    {

    }

    public Card(int Id, string CardName, string AttackColour, string DefenseColour, string CardEffect, int Damage)
    {
        id = Id;
        cardName = CardName;
        attackColour = AttackColour;
        defenseColour = DefenseColour;
        cardEffect = CardEffect;
        damage = Damage;
    }
}
