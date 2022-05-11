using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Base colour of cards
public enum cardColour
{
    None,
    Yellow,
    Red,
    Green,
    Blue
}

//Type of card
public enum type
{
    Attack,
    Defend
}

//What kind of effect the card has
public enum effectType
{
    Single,
    Multi,
}

//What kind of attack/defend the card has
public enum attackType
{
    normal,
    pierce,
    colourSpecific
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    //values to send to card objects
    public cardColour cardColour;
    public type type;
    public effectType effectType;
    public attackType attackType;

    public int id;
    public string cardName;

    public cardColour[] attackColours;
    public cardColour[] defenseColours;

    public string colourSpecificColour;
    public int effectAmount;
    public string cardEffect;
    public int damageAmount;
    public int healAmount;

    //public string attackColour;
    //public string defenseColour;
}
