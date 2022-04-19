using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameBehaviour<GameManager>
{
    #region inspector stuff
    [Header("References")]
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject PlayerDiscardPile;
    public GameObject EnemyDiscardPile;

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

        DealCards();
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
            //for (int p = 0; p < PlayerDeck.Count; p++)
            //{
            //    if (playerCard.GetComponent<Card>().id == PlayerDeck[p].GetComponent<Card>().id)
            //    {
            //        PlayerDeck.RemoveAt(p);
            //    }
            //}

            int rand2 = Random.Range(0, PlayerDeck.Count);
            GameObject enemyCard = Instantiate(EnemyDeck[rand2], EnemyCardAreas[i].transform);
            enemyCard.transform.rotation = EnemyCardAreas[i].transform.rotation;
            EnemyDealtCards.Add(enemyCard);

            //for (int e = 0; e < PlayerDeck.Count; e++)
            //{
            //    if (enemyCard.GetComponent<Card>().id == EnemyDeck[e].GetComponent<Card>().id)
            //    {
            //        EnemyDeck.RemoveAt(e);
            //    }
            //}
        }
    }

    public IEnumerator ClearCards()
    {
        ClearDealtCards();
        yield return new WaitForSeconds(4f);
        DealCards();
    }
    public void ClearDealtCards()
    {
        StartCoroutine(DiscardCards(PlayerDealtCards, PlayerDiscardPile));
        StartCoroutine(DiscardCards(EnemyDealtCards, EnemyDiscardPile));
    }
    //public void DiscardCards(List<GameObject> cardArea, GameObject discardpile)
    //{
    //    for(int i=0; i<cardArea.Count; i++)
    //    {
    //        MoveToDiscard(cardArea[i], discardpile);
    //    }
    //    cardArea.Clear();
    //}

    public void MoveToDiscard(GameObject card, GameObject discardpile)
    {
        card.transform.position = Vector3.MoveTowards(transform.position, discardpile.transform.position, 10f);
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

        StartCoroutine(BattlePhase(_playerCard));
    }

    #region Combat functions

    public IEnumerator BattlePhase(CardObject _playerCard)
    {
        yield return new WaitForSeconds(2f);
        PlayerAttackCheck(_playerCard);
        EnemyAttackCheck(_playerCard);
        StartCoroutine(ClearCards());
    }

    public IEnumerator DiscardCards(List<GameObject> cardArea, GameObject discardpile)
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < cardArea.Count; i++)
        {
            MoveToDiscard(cardArea[i], discardpile);
        }
        cardArea.Clear();
    }
    //public void BattlePhase(CardObject _playerCard)
    //{
    //    PlayerAttackCheck(_playerCard);
    //    EnemyAttackCheck(_playerCard);
    //}
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
            if(_cardPlayed.attackColours[i].ToString() != "None")
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
            if (playerHealth <= 0)
                _UI.GameOver("Enemy");
            enemyHealth += _cardPlayed.card.healAmount;
            if (enemyHealth >= 30)
                enemyHealth = 30;
            _UI.UpdateHP(target, playerHealth);
            Debug.Log(playerHealth);
        }
        if (target == "Enemy")
        {
            Debug.Log("Player Card Success");
            enemyHealth -= _cardPlayed.card.damageAmount;
            if (enemyHealth <= 0)
                _UI.GameOver("Player");
            playerHealth += _cardPlayed.card.healAmount;
            if (playerHealth >= 30)
                playerHealth = 30;
            _UI.UpdateHP(target, enemyHealth);
            Debug.Log(enemyHealth);
        }
    }

    #endregion
 
}
