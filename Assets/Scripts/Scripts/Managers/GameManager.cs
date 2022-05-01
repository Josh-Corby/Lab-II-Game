using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Image[] EnemyCardColours;

    [Header("Lists")]
    public List<GameObject> PlayerDeck = new List<GameObject>();
    public List<GameObject> EnemyDeck = new List<GameObject>();
    public List<GameObject> PlayerDealtCards = new List<GameObject>();
    public List<GameObject> EnemyDealtCards = new List<GameObject>();

    public HealthBar playerHealthBar;
    public HealthBar enemyHealthBar;

    #endregion

    [HideInInspector]
    public int playerHealth;
    [HideInInspector]
    public int enemyHealth;
    [HideInInspector]
    public string enemyCardColour;
    [HideInInspector]
    public int id;

    private CardObject enemyCard;

    public AudioSource SFX;
    public AudioClip dealCards;
    public AudioClip cardPlay;
    public AudioClip buttonClick;
    public AudioClip damage;
    public AudioClip heal;

    //private string enemyAttackColour;
    //private string enemyDefenseColour;


    public void StartGame()
    {
        _UI.UpdateDeckCount(PlayerDeck.Count);
        playerHealth = 30;
        playerHealthBar.SetMaxHealth(playerHealth);
        enemyHealth = 30;
        enemyHealthBar.SetMaxHealth(enemyHealth);

        DealCards();
    }

    public void DealCards()
    {
        _PCS.isOccupied = false;
        SFX.clip = dealCards;
        SFX.Play();
        if (PlayerDeck.Count > 0)
        {

            for (int i = 0; i < 3; i++)
            {
                int rand1 = Random.Range(0, PlayerDeck.Count);
                GameObject playerCard = Instantiate(PlayerDeck[rand1], PlayerCardAreas[i].transform);
                PlayerDealtCards.Add(playerCard);

                for (int p = 0; p < PlayerDeck.Count; p++)
                {
                    if (playerCard.GetComponent<CardObject>().id == PlayerDeck[p].GetComponent<CardObject>().id)
                    {
                        PlayerDeck.RemoveAt(p);
                    }
                }
                _UI.UpdateDeckCount(PlayerDeck.Count);

                int rand2 = Random.Range(0, PlayerDeck.Count);
                GameObject enemyCard = Instantiate(EnemyDeck[rand2], EnemyCardAreas[i].transform);
                enemyCard.transform.rotation = EnemyCardAreas[i].transform.rotation;
                EnemyDealtCards.Add(enemyCard);


                //id = (enemyCard.GetComponent<CardObject>().id);
                //Debug.Log(id);
                //enemyCardColour = enemyCard.GetComponent<CardObject>().cardColour.ToString();
                //Debug.Log(enemyCard.GetComponent<CardObject>().cardColour);

                //if (enemyCardColour == "Red")
                //    EnemyCardColours[i].color = Color.red;
                //else if (enemyCardColour == "Yellow")
                //    EnemyCardColours[i].color = Color.yellow;
                //else if (enemyCardColour == "Green")
                //    EnemyCardColours[i].color = Color.green;
                //else if (enemyCardColour == "Blue")
                //    EnemyCardColours[i].color = Color.blue;

                for (int e = 0; e < EnemyDeck.Count; e++)
                {
                    if (enemyCard.GetComponent<CardObject>().id == EnemyDeck[e].GetComponent<CardObject>().id)
                    {
                        EnemyDeck.RemoveAt(e);
                    }
                }
            }
        }
        else
        {
            if (playerHealth > enemyHealth)
                _UI.GameOver("Player");
            if (playerHealth == enemyHealth)
                _UI.GameOver("Draw");
            if (playerHealth < enemyHealth)
                _UI.GameOver("Enemy");
        }
    }

    public IEnumerator ClearCards()
    {
        ClearDealtCards();
        yield return new WaitForSeconds(2f);
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
        card.transform.rotation = discardpile.transform.rotation;
    }

    public IEnumerator PlayEnemyCard(CardObject _playerCard)
    {
        yield return new WaitForSeconds(1.5f);
        int rand = Random.Range(1, 3);
        enemyCard = EnemyDealtCards[rand].GetComponent<CardObject>();
        enemyCard.transform.position = _ECS.transform.position;
        enemyCard.transform.rotation = _ECS.transform.rotation;
        //enemyCard.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
        Debug.Log("Enemy Plays: " + enemyCard.name);

        //enemyAttackColour = enemyCardToPlay.card.attackColour;
        //enemyDefenseColour = enemyCardToPlay.card.defenseColour;
        StartCoroutine(BattlePhase(_playerCard));
    }
    public IEnumerator DiscardCards(List<GameObject> cardArea, GameObject discardpile)
    {
        for (int i = 0; i <= 2; i++)
        {
            cardArea[i].gameObject.GetComponent<CardObject>().PlayParticles();
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i <= 2; i++)
        {
            Destroy(cardArea[i].gameObject);
            //MoveToDiscard(cardArea[i], discardpile);
        }
        cardArea.Clear();
    }
    /*
    //passes through values of player card played
    public void PlayEnemyCard(CardObject _playerCard)
    {
        int rand = Random.Range(1, 3);
        enemyCard = EnemyDealtCards[rand].GetComponent<CardObject>();
        enemyCard.transform.position = _ECS.transform.position;
        enemyCard.transform.rotation = _ECS.transform.rotation;
        //enemyCard.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
        Debug.Log("Enemy Plays: " + enemyCard.name);

        //enemyAttackColour = enemyCardToPlay.card.attackColour;
        //enemyDefenseColour = enemyCardToPlay.card.defenseColour;
        StartCoroutine(BattlePhase(_playerCard));
    }
    */

    #region Combat functions

    public IEnumerator BattlePhase(CardObject _playerCard)
    {
        yield return new WaitForSeconds(2f);
        PlayerAttackCheck(_playerCard);
        EnemyAttackCheck(_playerCard);
        StartCoroutine(ClearCards());
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
            Success(target, _cardPlayed.damageAmount, _cardPlayed.healAmount);
            return;
        }
        for (int i = 0; i < _cardPlayed.attackColours.Length; i++)
        {
            //check if card type is colour specific
            if (_cardPlayed.attackType == attackType.colourSpecific)
            {
                for (int c = 0; c < 4; c++)
                {
                    //check if attack colour is the same as colour specific colour
                    if (_cardPlayed.attackColours[c].ToString() == _cardPlayed.colourSpecificColour
                       && _cardPlayed.attackColours[c].ToString() != _opponentCard.defenseColours[c].ToString())
                    {
                        // run success with colour specific effect
                        Success(target, _cardPlayed.colourSpecificAmount, _cardPlayed.healAmount);
                        return;
                    }
                }
            }
            //if(_cardPlayed.attackColours[i].ToString() != "None")
            //{
            if (_cardPlayed.attackColours[i] != _opponentCard.defenseColours[i])
                {
                    Success(target, _cardPlayed.damageAmount, _cardPlayed.healAmount);
                    if (_cardPlayed.effectType == effectType.Single) return;
                }
            //}
           
        }
    }

    public void Defend(CardObject _cardPlayed, CardObject _opponentCard, string target)
    {
        for (int i = 0; i < 4; i++)
        {
            //check if block succeeds
            if (_cardPlayed.defenseColours[i].ToString() != "None" && _cardPlayed.defenseColours[i].ToString() == _opponentCard.attackColours[i].ToString())
            {


                //check if card type is colour specific
                if (_cardPlayed.attackType == attackType.colourSpecific)
                {
                    for (int c = 0; c < 4; c++)
                    {
                        //check if block colour is the same as colour specific colour
                        if (_cardPlayed.defenseColours[c].ToString() == _cardPlayed.colourSpecificColour 
                           && _cardPlayed.defenseColours[c].ToString() == _opponentCard.attackColours[c].ToString())
                        {
                            // run success with colour specific effect
                            Success(target, _cardPlayed.colourSpecificAmount, _cardPlayed.healAmount);
                            return;
                        }
                    }   
                }
                else
                {
                    Success(target, _cardPlayed.damageAmount, _cardPlayed.healAmount);
                    if (_cardPlayed.effectType == effectType.Single) return;
                }
            }
            else
                return;
        }
    }

    public void Success(string target, int damage, int heal)
    {
        if (target == "Player")
        {
            Debug.Log("Enemy Card Success");
            playerHealth -= damage;
            playerHealthBar.SetHealth(playerHealth);
            if (playerHealth <= 0)
                _UI.GameOver("Enemy");

            enemyHealth += heal;
            if (enemyHealth >= 30)
                enemyHealth = 30;
            enemyHealthBar.SetHealth(enemyHealth);
            _UI.UpdateHP(target, playerHealth);
            Debug.Log(playerHealth);
        }
        if (target == "Enemy")
        {
            Debug.Log("Player Card Success");
            enemyHealth -= damage;
            enemyHealthBar.SetHealth(enemyHealth);
            if (enemyHealth <= 0)
                _UI.GameOver("Player");

            playerHealth += heal;
            playerHealthBar.SetHealth(playerHealth);
            if (playerHealth >= 30)
                playerHealth = 30;
            _UI.UpdateHP(target, enemyHealth);
            Debug.Log(enemyHealth);
        }
    }

    #endregion
 
}
