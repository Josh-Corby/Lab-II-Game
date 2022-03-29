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
    public GameObject PlayerCardSpot;

    public GameObject[] CardAreas;



    List<GameObject> cards = new List<GameObject>();

    public override void OnStartClient()
    {
        base.OnStartClient();

        GameObject cardArea1 = GameObject.Find("CardArea1");
        GameObject cardArea2 = GameObject.Find("CardArea2");
        GameObject cardArea3 = GameObject.Find("CardArea3");

        CardAreas[0] = cardArea1;
        CardAreas[1] = cardArea2;
        CardAreas[2] = cardArea3;

        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("EnemyArea");
        PlayerCardSpot = GameObject.Find("PlayerCardSpot");
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
                card.transform.SetParent(PlayerArea.transform, false);
            }
            else
            {
                card.transform.SetParent(EnemyArea.transform, false);
            }
        }
        else if (type == "Played")
        {
            card.transform.SetParent(PlayerCardSpot.transform, false);
        }
    }
}
