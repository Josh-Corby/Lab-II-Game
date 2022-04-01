using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : GameBehaviour
{
    public static List<Card> cardList = new List<Card>();

    private void Awake()
    {
        cardList.Add(new Card(0, "The Fool", "Red", "Red", "", 1));
        cardList.Add(new Card(1, "The Magician", "Blue", "Blue", "", 1));
        cardList.Add(new Card(2, "The High Priestess", "Red", "Red", "", 1));
        cardList.Add(new Card(3, "The Empress", "Blue", "Blue", "", 1));
        cardList.Add(new Card(4, "The Emperor", "Red", "Red", "", 1));
        cardList.Add(new Card(5, "The Hierophant", "Blue", "Blue", "", 1));
        cardList.Add(new Card(6, "The Lovers", "Red", "Red", "", 1));
        cardList.Add(new Card(7, "The Chariot", "Blue", "Blue", "", 1));
        cardList.Add(new Card(8, "Justice", "Red", "Red", "", 1));
        cardList.Add(new Card(9, "The Hermit", "Blue", "Blue", "", 1));
        cardList.Add(new Card(10, "Wheel of Fortune", "Red", "Red", "", 1));
        cardList.Add(new Card(11, "Strength", "Blue", "Blue", "", 1));
        cardList.Add(new Card(12, "The Hanged Man", "Red", "Red", "", 1));
        cardList.Add(new Card(13, "Death", "Blue", "Blue", "", 1));
        cardList.Add(new Card(14, "Temperance", "Red", "Red", "", 1));
        cardList.Add(new Card(15, "The Devil", "Blue", "Blue", "", 1));
        cardList.Add(new Card(16, "The Tower", "Red", "Red", "", 1));
        cardList.Add(new Card(17, "The Star", "Blue", "Blue", "", 1));
        cardList.Add(new Card(18, "The Moon", "Red", "Red", "", 1));
        cardList.Add(new Card(19, "The Sun", "Blue", "Blue", "", 1));
        cardList.Add(new Card(20, "Judgement", "Red", "Red", "", 1));
        cardList.Add(new Card(21, "The World", "Blue", "Blue", "", 1));

    }
}
