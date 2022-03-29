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
    public GameObject PlayerDropZone;

    public GameObject[] CardAreas;
    public GameObject cardArea1;
    public GameObject cardArea2;
    public GameObject cardArea3;


    List<GameObject> cards = new List<GameObject>();

    public override void OnStartClient()
    {
        base.OnStartClient();

        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("EnemyArea");
        PlayerDropZone = GameObject.Find("PlayerDropZone");
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

        }
    }
}
