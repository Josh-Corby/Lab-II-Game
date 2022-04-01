using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : GameBehaviour
{
    public static List<Card> cardList = new List<Card>();

    private void Awake()
    {
        cardList.Add(new Card(0, "Card1", 1, 1, 3, "None", "Red"));
        cardList.Add(new Card(1, "Card2", 1, 1, 3, "None", "Blue"));
    }
}
