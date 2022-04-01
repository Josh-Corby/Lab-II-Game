using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Card
{
    public int cardNumber;
    public int damage;
    public int attackCircle;
    public int defenseCircle;
    public string cardDescription;
    public string colour;
    public string cardName;


    public Card(int CardNumber, string CardName, int AttackCircle, int DefenseCircle, int Damage, string CardDescription, string Colour)
    {
        cardNumber = CardNumber;
        cardName = CardName;
        attackCircle = AttackCircle;
        defenseCircle = DefenseCircle;
        damage = Damage;
        cardDescription = CardDescription;
        colour = Colour;
    }
}
