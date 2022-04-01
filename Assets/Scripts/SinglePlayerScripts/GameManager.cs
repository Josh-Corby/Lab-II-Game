using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameBehaviour<GameManager>
{
    #region inspector stuff
    [Header("References")]
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerArea;
    public GameObject EnemyArea;

    [Header("Arrays")]
    public GameObject[] PlayerCardAreas;
    public GameObject[] EnemyCardAreas;

    [Header("Lists")]
    List<GameObject> cards = new List<GameObject>();
    public List<GameObject> PlayerDealtCards = new List<GameObject>();
    public List<GameObject> EnemyDealtCards = new List<GameObject>();

    public bool cardPlayed = false;
    #endregion

    public int playerHealth = 30;
    public int enemyHealth = 30;
    public void Start()
    {
        cards.Add(Card1);
        cards.Add(Card2);
    }

    public void DealCards()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject playerCard = Instantiate(cards[Random.Range(0, cards.Count)], PlayerCardAreas[i].transform);
            PlayerDealtCards.Add(playerCard);

            GameObject enemyCard = Instantiate(cards[Random.Range(0, cards.Count)], EnemyCardAreas[i].transform);
            enemyCard.transform.rotation = EnemyCardAreas[i].transform.rotation;
            EnemyDealtCards.Add(enemyCard);
 
        }
    }

    public void PlayEnemyCard()
    {
        int rand = Random.Range(1, 3);
        Debug.Log(rand);
        EnemyDealtCards[rand].gameObject.transform.position = _ECS.transform.position;
        EnemyDealtCards[rand].gameObject.transform.rotation = _ECS.transform.rotation;
    }

}
