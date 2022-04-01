using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ThisCard : GameBehaviour
{
    public List<Card> thisCard = new List<Card>();
    public int thisId;

    public int id;
    public string cardName;
    public string cardEffect;

    public TMP_Text nameText;
    public TMP_Text cardEffectText;

    public Sprite thisSprite;
    public Image thatImage;

    public GameObject Hand;

    public int numberOfCardsInDeck;

    void Start()
    {
        thisCard[0] = CardDatabase.cardList[thisId];
        numberOfCardsInDeck = PlayerDeck.deckSize;
    }

    void Update()
    {
        id = thisCard[0].id;
        cardName = thisCard[0].cardName;
        cardEffect = thisCard[0].cardEffect;

        thisSprite = thisCard[0].thisImage;

        nameText.text = "" + cardName;
        cardEffectText.text = "" + cardEffect;

        thatImage.sprite = thisSprite;


        if (this.tag == "Clone")
        {
            thisCard[0] = PlayerDeck.staticDeck[numberOfCardsInDeck - 1];
            numberOfCardsInDeck -= 1;
            PlayerDeck.deckSize -= 1;
            this.tag = "untagged";
        }
    }
}
