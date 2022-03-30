using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public PlayerCardSpot playerCardSpot;

    public CardArea[] CardAreas;
    public CardArea cardArea1;
    public CardArea cardArea2;
    public CardArea cardArea3;


    List<GameObject> cards = new List<GameObject>();

    public override void OnStartClient()
    {
        base.OnStartClient();

        cardArea1 = GameObject.Find("CardArea1").GetComponent<CardArea>();
        cardArea2 = GameObject.Find("CardArea2").GetComponent<CardArea>();
        cardArea3 = GameObject.Find("CardArea3").GetComponent<CardArea>();

        CardAreas[0] = cardArea1;
        CardAreas[1] = cardArea2;
        CardAreas[2] = cardArea3;

        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("EnemyArea");
        playerCardSpot = GameObject.Find("PlayerCardSpot").GetComponent<PlayerCardSpot>();
    }


    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();

        cards.Add(Card1);
        cards.Add(Card2);
    }

    [Command]
    public void CmdDealCards()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject card = Instantiate(cards[Random.Range(0, cards.Count)], CardAreas[i].transform);
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, "Dealt");
        }
    }

    public void PlayCard(GameObject card)
    {
        CmdPlayCard(card);
    }

    [Command]
    void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card, "Played");
    }

    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        if (type == "Dealt")
        {
            if (hasAuthority)
            {
                if (cardArea1.isOccupied == false)
                {
                    card.transform.SetParent(CardAreas[0].transform, false);
                    cardArea1.isOccupied = true;
                }

                else if (cardArea2.isOccupied == false)
                {
                    card.transform.SetParent(CardAreas[1].transform, false);
                    cardArea2.isOccupied = true;
                }

                else if (cardArea3.isOccupied == false)
                {
                    card.transform.SetParent(CardAreas[2].transform, false);
                    cardArea3.isOccupied = true;
                }
            }
            else
            {
                card.transform.SetParent(EnemyArea.transform, false);
            }
        }
        else if (type == "Played")
        {
            card.transform.SetParent(playerCardSpot.transform, true);
            playerCardSpot.isOccupied = true;
        }
    }
}
