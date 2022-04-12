using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum cardColour
{
    Yellow,
    Red,
    Gree,
    Blue
}

public enum type
{
    Attack,
    Defend
}
public enum effectType
{
    Single,
    Multi,
}

public enum attackType
{
    normal,
    pierce,
    colourSpecific
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public cardColour cardColour;
    public type type;
    public effectType effectType;
    public attackType attackType;

    public int id;
    public string cardName;
    public string[] attackColours = { };
    public string[] defenseColours = { };
    public string cardEffect;
    public int damageAmount;
    public int healAmount;

    public Sprite frontImage;
    public Sprite backImage;

    //public string attackColour;
    //public string defenseColour;
}
