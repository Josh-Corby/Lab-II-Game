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
    public List<GameObject> PlayerDeck = new List<GameObject>();
    public List<GameObject> EnemyDeck = new List<GameObject>();
    public List<GameObject> PlayerDealtCards = new List<GameObject>();
    public List<GameObject> EnemyDealtCards = new List<GameObject>();

    public bool cardPlayed = false;
    #endregion

    public int playerHealth;
    public int enemyHealth;

    public CardObject enemyCardToPlay;
    private int enemyDamage;
    private string enemyAttackColour;
    private string enemyDefenseColour;
    public void Start()
    {
        playerHealth = 30;
        enemyHealth = 30;
    }

    public void DealCards()
    {
        for (int i = 0; i < 3; i++)
        {
            int rand1 = Random.Range(0,PlayerDeck.Count);
            Debug.Log(rand1);
            GameObject playerCard = Instantiate(PlayerDeck[rand1], PlayerCardAreas[i].transform);
            PlayerDealtCards.Add(playerCard);
            //PlayerDeck.Remove(PlayerDeck[rand]);

            int rand2 = Random.Range(0, PlayerDeck.Count);
            Debug.Log(rand1);
            GameObject enemyCard = Instantiate(EnemyDeck[rand2], EnemyCardAreas[i].transform);
            enemyCard.transform.rotation = EnemyCardAreas[i].transform.rotation;
            EnemyDealtCards.Add(enemyCard);
 
        }
    }

    public void PlayPlayerCard(int playerDamage, string playerAttackColour, string playerDefenseColour)
    {
        _GM.PlayEnemyCard(playerDamage, playerAttackColour, playerDefenseColour);
        enemyHealth -= playerDamage;

    }
    public void PlayEnemyCard(int playerDamage, string playerAttackColour, string playerDefenseColour)
    {
        int rand = Random.Range(1, 3);
        Debug.Log(rand);
        enemyCardToPlay = EnemyDealtCards[rand].GetComponent<CardObject>();
        enemyCardToPlay.transform.position = _ECS.transform.position;
        enemyCardToPlay.transform.rotation = _ECS.transform.rotation;


        enemyDamage = enemyCardToPlay.card.damage;
        enemyAttackColour = enemyCardToPlay.card.attackColour;
        enemyDefenseColour = enemyCardToPlay.card.defenseColour;

        BattlePhase(playerDamage, playerAttackColour, playerDefenseColour, enemyDamage, enemyAttackColour, enemyDefenseColour);
    }

    public void BattlePhase(int playerDamage, string playerAttackColour, string playerDefenseColour,
                            int enemyDamage, string enemyAttackColour, string enemyDefenseColour)
    {
        if (playerDamage != 0)
        {
            Attack(playerDamage, playerAttackColour, enemyDefenseColour, "Enemy", enemyHealth);
        }

        if (enemyDamage != 0)
        {
            Attack(enemyDamage, enemyAttackColour, playerDefenseColour, "Player", playerHealth);
        }
    }

    public void Attack(int damage, string attackColour, string defenseColour, string target, int targetHealth)
    { 
        if (attackColour != defenseColour)
        {
            targetHealth -= damage;
            _UI.UpdateHP(target, targetHealth);
            
        }
    }

}
