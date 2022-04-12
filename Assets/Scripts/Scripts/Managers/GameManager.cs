using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameBehaviour<GameManager>
{
    #region inspector stuff
    [Header("References")]
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

    #endregion

    [HideInInspector]
    public int playerHealth;
    [HideInInspector]
    public int enemyHealth;

    private CardObject enemyCard;


    //private string enemyAttackColour;
    //private string enemyDefenseColour;
    public void Start()
    {
        playerHealth = 30;
        enemyHealth = 30;
    }

    public void DealCards()
    {
        _PCS.isOccupied = false;

        for (int i = 0; i < 3; i++)
        {
            int rand1 = Random.Range(0,PlayerDeck.Count);
            GameObject playerCard = Instantiate(PlayerDeck[rand1], PlayerCardAreas[i].transform);
            PlayerDealtCards.Add(playerCard);
            //PlayerDeck.Remove(PlayerDeck[rand]);

            int rand2 = Random.Range(0, PlayerDeck.Count);
            GameObject enemyCard = Instantiate(EnemyDeck[rand2], EnemyCardAreas[i].transform);
            enemyCard.transform.rotation = EnemyCardAreas[i].transform.rotation;
            EnemyDealtCards.Add(enemyCard);
        }
    }

    public void ClearDealtCards()
    {
        ClearList(PlayerDealtCards);
        ClearList(EnemyDealtCards);
    }
    public void ClearList(List<GameObject> cardArea)
    {
        for(int i=0; i<cardArea.Count; i++)
        {
            Destroy(cardArea[i]);
        }
        cardArea.Clear();
    }

    //passes through values of player card played
    public void PlayEnemyCard(CardObject _playerCard)
    {
        int rand = Random.Range(1, 3);
        enemyCard = EnemyDealtCards[rand].GetComponent<CardObject>();
        enemyCard.transform.position = _ECS.transform.position;
        enemyCard.transform.rotation = _ECS.transform.rotation;
        Debug.Log("Enemy Plays: " + enemyCard.name);



        //enemyAttackColour = enemyCardToPlay.card.attackColour;
        //enemyDefenseColour = enemyCardToPlay.card.defenseColour;

        BattlePhase(_playerCard);
    }

    public void BattlePhase(CardObject _playerCard)
    {
        PlayerAttackCheck(_playerCard);
        EnemyAttackCheck(_playerCard);
    }
    //public void BattlePhase(int playerDamage, string[] playerAttackColours, string[] playerDefenseColours)
    //{

    //    Attack(playerDamage, playerAttackColours, enemyDefenseColours, "Enemy", enemyHealth);
    //    Attack(enemyDamage, enemyAttackColours, playerDefenseColours, "Player", playerHealth);
    //}

    public void PlayerAttackCheck(CardObject _playerCard)
    {
        if (_playerCard.type == type.Attack)
            Attack(_playerCard, enemyCard, "Enemy");
        else
            Defend(_playerCard, enemyCard, "Enemy");
    }

    public void EnemyAttackCheck(CardObject _playerCard)
    {
        if (enemyCard.type == type.Attack)
            Attack(enemyCard, _playerCard, "Player");
        else
            Defend(enemyCard, _playerCard, "Player");
    }


    public void Attack(CardObject _cardPlayed, CardObject _opponentCard, string target)
    {
        if(_cardPlayed.attackType == attackType.pierce)
        {
            Success(_cardPlayed, target);
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            if(_cardPlayed.attackColours[i] != null)
            {
                if (_cardPlayed.attackColours[i] != _opponentCard.defenseColours[i])
                {
                    Success(_cardPlayed, target);
                    if (_cardPlayed.effectType == effectType.Single) return;
                }
            }
           
        }
    }

    public void Defend(CardObject _cardPlayed, CardObject _opponentCard, string target)
    {
        for (int i = 0; i < 4; i++)
        {
            if (_cardPlayed.defenseColours[i] == _opponentCard.attackColours[i])
            {
                Success(_cardPlayed, target);
                if (_cardPlayed.effectType == effectType.Single) return;
            }
        }
    }

    public void Success(CardObject _cardPlayed, string target)
    {
        if (target == "Player")
        {
            Debug.Log("Enemy Card Success");
            playerHealth -= _cardPlayed.card.damageAmount;
            enemyHealth += _cardPlayed.card.healAmount;
            _UI.UpdateHP(target, playerHealth);
            Debug.Log(playerHealth);
        }
        if (target == "Enemy")
        {
            Debug.Log("Player Card Success");
            enemyHealth -= _cardPlayed.card.damageAmount;
            playerHealth += _cardPlayed.card.healAmount;
            _UI.UpdateHP(target, enemyHealth);
            Debug.Log(enemyHealth);
        }
    }

}
