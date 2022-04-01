using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : GameBehaviour
{
    public List<Card> deck = new List<Card>();
    public int x;
    public static int deckSize;
    public List<Card> container = new List<Card>();
    public static List<Card> staticDeck = new List<Card>();

    public GameObject[] Clones;
    void Start()
    {
        x = 0;
        deckSize = 22;
        for (int i = 0; i < deckSize-1; i++)
        {

            deck[i] = CardDatabase.cardList[i];

        }
    }

    private void Update()
    {
        staticDeck = deck;
    }
    public void Shuffle()
    {
        for (int i = 0; i < deckSize; i++)
        {
            container[0] = deck[i];
            int randomIndex = Random.Range(i, deckSize);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container[0];
        }
    }
}
