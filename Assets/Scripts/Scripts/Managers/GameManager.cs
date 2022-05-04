using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : GameBehaviour<GameManager>
{
    #region inspector stuff
    [Header("References")]
    public GameObject PlayerArea;
    public GameObject PlayerDiscardPile;
    public HealthBar playerHealthBar;

    public GameObject EnemyArea;
    public GameObject EnemyDiscardPile;
    public HealthBar enemyHealthBar;
    [Header("Vectors")]
    public Vector3 PCCombatSpot;
    public Vector3 PCCombatMove;
    public Vector3 ECCombatSpot;
    public Vector3 ECCombatMove;

    [Header("Arrays")]
    public GameObject[] PlayerCardAreas;
    public GameObject[] EnemyCardAreas;
    public Image[] EnemyCardColours;

    [Header("Lists")]
    public List<GameObject> PlayerDeck = new List<GameObject>();
    public List<GameObject> EnemyDeck = new List<GameObject>();
    public List<GameObject> PlayerDealtCards = new List<GameObject>();
    public List<GameObject> EnemyDealtCards = new List<GameObject>();

    public Tween tweening;

    [HideInInspector]
    public int playerHealth;
    [HideInInspector]
    public int enemyHealth;

    [HideInInspector]
    public int id;

    private int temperanceCounter = 0;

    private CardObject enemyCard;

    [Header("Audio")]
    public AudioSource SFX;
    public AudioClip dealCardSound;
    public AudioClip cardPlaySound;
    public AudioClip damageSound;
    public AudioClip healSound;

    #endregion


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
        SFX.clip = dealCardSound;
        SFX.Play();
        if (PlayerDeck.Count > 0)
        {

            for (int i = 0; i < 3; i++)
            {
                //select a random card from the player deck and instantiate it
                int rand1 = Random.Range(0, PlayerDeck.Count);
                GameObject playerCard = Instantiate(PlayerDeck[rand1], PlayerCardAreas[i].transform);
                //add the card to list of drawn cards
                PlayerDealtCards.Add(playerCard);

                //select a random card from the player deck and instantiate it
                int rand2 = Random.Range(0, PlayerDeck.Count);
                GameObject enemyCard = Instantiate(EnemyDeck[rand2], EnemyCardAreas[i].transform);
                enemyCard.transform.rotation = EnemyCardAreas[i].transform.rotation;
                //add the card to list of drawn cards
                EnemyDealtCards.Add(enemyCard);

                //fetch the values of the cards in another function. (waiting one frame so the values that need to be found exist)
                StartCoroutine(CardSetup(playerCard, enemyCard));

                //enemyCard = EnemyDealtCards[i].GetComponent<CardObject>();
                //Debug.Log(EnemyDealtCards[i].GetComponent<CardObject>().cardColour);
                //Debug.Log(EnemyDealtCards[i].GetComponent<CardObject>().cardEffect);
                //Debug.Log(EnemyCardColours[i].color);
            }
        }

        //if there are no cards left in the deck at the start of the draw phase, find the winner by checking health of both players
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
    //assign values of cards and remove the cards from the decks
    IEnumerator CardSetup(GameObject playerCard, GameObject enemyCard)
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < PlayerDeck.Count; i++)
        {
            if (playerCard.GetComponent<CardObject>().id == PlayerDeck[i].GetComponent<CardObject>().id)
            {
                PlayerDeck.RemoveAt(i);
            }
            if (enemyCard.GetComponent<CardObject>().id == EnemyDeck[i].GetComponent<CardObject>().id)
            {
                EnemyDeck.RemoveAt(i);
            }
        }
        //display the colours of the cards in enemy hand on the colour display
        for (int i = 0; i < 3; i++)
        {
            EnemyCardColours[i].color = GetCardColour(EnemyDealtCards[i].GetComponent<CardObject>().cardColour);
        }
        _UI.UpdateDeckCount(PlayerDeck.Count);
    }
    //fetch the colour from cardcomponent
    public Color GetCardColour(cardColour colour)
    {
        //Debug.Log(colour);
        switch (colour)
        {
            case cardColour.Red:
                return Color.red;
            case cardColour.Green:
                return Color.green;
            case cardColour.Blue:
                return Color.blue;
            case cardColour.Yellow:
                return Color.yellow;
            default:
                return Color.white;
        }
    }
    public IEnumerator ClearCards()
    {
        //clear the cards from the table and draw more cards
        ClearDealtCards();
        yield return new WaitForSeconds(2f);
        DealCards();
    }
    public void ClearDealtCards()
    {
        //clear player and enemy cards
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

    //move the cards to the discard pile
    public void MoveToDiscard(GameObject card, GameObject discardpile)
    {
        card.transform.position = Vector3.MoveTowards(transform.position, discardpile.transform.position, 10f);
        card.transform.rotation = discardpile.transform.rotation;
    }

    //play a random enemy card when the player has played a card
    public IEnumerator PlayEnemyCard(CardObject _playerCard)
    {
        yield return new WaitForSeconds(0.7f);
        int rand = Random.Range(1, 3);
        enemyCard = EnemyDealtCards[rand].GetComponent<CardObject>();
        enemyCard.transform.position = _ECS.transform.position;
        enemyCard.transform.rotation = _ECS.transform.rotation;
        //enemyCard.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
        enemyCard.GetComponent<Rigidbody>().useGravity = false;
        //Debug.Log("Enemy Plays: " + enemyCard.name);

        //enemyAttackColour = enemyCardToPlay.card.attackColour;
        //enemyDefenseColour = enemyCardToPlay.card.defenseColour;
        yield return new WaitForSeconds(0.7f);
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

    //base function that starts the process of the battle phase
    public IEnumerator BattlePhase(CardObject _playerCard)
    {
        CombatPosition(_playerCard, enemyCard);
        yield return new WaitForSeconds(1f);

        PlayerAttackCheck(_playerCard);
        yield return new WaitForSeconds(1f);

        EnemyAttackCheck(_playerCard);
        yield return new WaitForSeconds(1f);

        StartCoroutine(ClearCards());
    }

    public void CombatPosition(CardObject _playerCard, CardObject _enemyCard)
    {
        _playerCard.transform.DOMove(PCCombatSpot, 1, false);
        _playerCard.transform.DORotate(new Vector3(0, -180, 0), 1);
        _playerCard.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 1);

        _enemyCard.transform.DOMove(ECCombatSpot, 1, false);
        _enemyCard.transform.DORotate(new Vector3(0, -180, 0), 1);
        _enemyCard.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 1);
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

    //check if the player card is an attacking card or a defending card and run the respective function

    public void PlayerAttackCheck(CardObject _playerCard)
    {
        StartCoroutine(CardAnimation(_playerCard, PCCombatSpot, PCCombatMove));
        //StartCoroutine(PlayerCardAnimation(_playerCard));
        if (_playerCard.type == type.Attack)
            Attack(_playerCard, enemyCard, "Enemy");
        if (_playerCard.type == type.Defend)
            Defend(_playerCard, enemyCard, "Enemy");
    }

    public void EnemyAttackCheck(CardObject _playerCard)
    {
        StartCoroutine(CardAnimation(enemyCard, ECCombatSpot, ECCombatMove));
        //StartCoroutine(EnemyCardAnimation(enemyCard));
        if (enemyCard.type == type.Attack)
            Attack(enemyCard, _playerCard, "Player");
        if (enemyCard.type == type.Defend)
            Defend(enemyCard, _playerCard, "Player");
    }

    IEnumerator CardAnimation(CardObject card, Vector3 startPos, Vector3 movePos)
    {
        card.transform.DOMoveX(movePos.x, .2f, false);
        yield return new WaitForSeconds(0.2f);
        card.transform.DOMoveX(startPos.x, .2f, false);
        yield return new WaitForSeconds(0.1f);
    }
    /*
    IEnumerator PlayerCardAnimation(CardObject _playerCard)
    {
        _playerCard.transform.DOMoveX(0.5f, .2f, false);
        yield return new WaitForSeconds(0.2f);
        _playerCard.transform.DOMoveX(PCCombatSpot.x, .2f, false);
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator EnemyCardAnimation(CardObject _enemyCard)
    {
        _enemyCard.transform.DOMoveX(-0.5f, .2f, false);
        yield return new WaitForSeconds(0.2f);
        _enemyCard.transform.DOMoveX(ECCombatSpot.x, .2f, false);
        yield return new WaitForSeconds(0.1f);
    }
    */
    //check if the player card is an attacking card or a defending card and run the respective function
    //public void PlayerAttackCheck(CardObject _playerCard)
    //{

    //    if (_playerCard.type == type.Attack)
    //        Attack(_playerCard, enemyCard, "Enemy");
    //    else
    //        Defend(_playerCard, enemyCard, "Enemy");
    //}



    //check if the enemy card is an attacking card or a defending card and run the respective function
    //public void EnemyAttackCheck(CardObject _playerCard)
    //{
    //    if (enemyCard.type == type.Attack)
    //        Attack(enemyCard, _playerCard, "Player");
    //    else
    //        Defend(enemyCard, _playerCard, "Player");
    //}

    public void Attack(CardObject _cardPlayed, CardObject _opponentCard, string target)
    {
        if (_cardPlayed.attackType == attackType.pierce)
        {
            Success(target, _cardPlayed.damageAmount, _cardPlayed.healAmount);
            return;
        }

        if (_cardPlayed.name == "Temperance")
        {
            for (int c = 0; c < _cardPlayed.attackColours.Length; c++)
            {
                if (_cardPlayed.attackColours[c].ToString() != "None")
                {
                    if (_cardPlayed.attackColours[c] != _opponentCard.attackColours[c])
                    {
                        temperanceCounter += 1;
                    }
                }
            }
            if (temperanceCounter == 3)
            {
                Success(target, _cardPlayed.effectAmount, _cardPlayed.healAmount);
                temperanceCounter = 0;
                return;
            }
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
                        Success(target, _cardPlayed.effectAmount, _cardPlayed.healAmount);
                        return;
                    }
                }
            }
            if (_cardPlayed.attackColours[i] != _opponentCard.defenseColours[i])
            {

                Success(target, _cardPlayed.damageAmount, _cardPlayed.healAmount);
                if (_cardPlayed.effectType == effectType.Single) return;
            }
        }
    }

    public void Defend(CardObject _cardPlayed, CardObject _opponentCard, string target)
    {
        for (int i = 0; i < 4; i++)
        {
            //check if block succeeds
            if (_cardPlayed.defenseColours[i].ToString() != "None")
            {         
                if (_cardPlayed.defenseColours[i].ToString() == _opponentCard.attackColours[i].ToString())
                {
                    Debug.Log("Block success");
                    if (_cardPlayed.attackType == attackType.normal)
                    {
                        if (_cardPlayed.effectType == effectType.Multi)
                            Success(target, _cardPlayed.damageAmount, _cardPlayed.healAmount);

                        if (_cardPlayed.effectType == effectType.Single)
                        {
                            Success(target, _cardPlayed.damageAmount, _cardPlayed.healAmount);
                            return;
                        }

                    }
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
                                Success(target, _cardPlayed.effectAmount, _cardPlayed.healAmount);
                                return;
                            }
                        }
                    }
                }
                else return;
            }
        }
    }
    public void Success(string target, int damage, int heal)
    {
        if (target == "Player")
        {
            if (damage != 0)
            {
                SFX.clip = damageSound;
                SFX.Play();
            }
            Debug.Log("Enemy Card Success");
            playerHealth -= damage;
            playerHealthBar.SetHealth(playerHealth);
            if (playerHealth <= 0)
                _UI.GameOver("Enemy");

            if (heal != 0)
            {
                SFX.clip = healSound;
                SFX.Play();
            }
            enemyHealth += heal;
            if (enemyHealth >= 30)
                enemyHealth = 30;
            enemyHealthBar.SetHealth(enemyHealth);
            _UI.UpdateHP(target, playerHealth);
            Debug.Log(playerHealth);
        }
        if (target == "Enemy")
        {
            if (damage != 0)
            {
                SFX.clip = damageSound;
                SFX.Play();
            }
            Debug.Log("Player Card Success");
            enemyHealth -= damage;
            enemyHealthBar.SetHealth(enemyHealth);
            if (enemyHealth <= 0)
                _UI.GameOver("Player");

            if (heal != 0)
            {
                SFX.clip = healSound;
                SFX.Play();
            }
            enemyHealth += heal;
            playerHealth += heal;
            playerHealthBar.SetHealth(playerHealth);
            if (playerHealth >= 30)
                playerHealth = 30;
            _UI.UpdateHP(target, enemyHealth);
            Debug.Log(enemyHealth);
        }
    }
    //public void Success(string target, int damage, int heal)
    //{
    //    if (damage != 0)
    //    {
    //        SFX.clip = damageSound;
    //        SFX.Play();

    //        if (target == "Player")
    //        {
    //            DamagePlayer(damage);
    //        }


    //        if (target == "Enemy")
    //            DamageEnemy(damage);
    //    }

    //    if (heal != 0)
    //    {
    //        SFX.clip = healSound;
    //        SFX.Play();

    //        if (target == "Enemy")
    //            HealPlayer(heal);

    //        if (target == "Player")
    //            HealEnemy(heal);
    //    }

    //}

    //void DamagePlayer(int damage)
    //{
    //    playerHealth -= damage;
    //    SetHealth("Player");
    //    if (playerHealth <= 0)
    //        _UI.GameOver("Enemy");
    //}

    //void HealPlayer(int heal)
    //{
    //    playerHealth += heal;
    //    if (playerHealth >= 30)
    //    {
    //        playerHealth = 30;
    //        SetHealth("Player");
    //    }
    //}

    //void DamageEnemy(int damage)
    //{
    //    enemyHealth -= damage;
    //    SetHealth("Enemy");
    //    if (enemyHealth <= 0)
    //        _UI.GameOver("Player");
    //}

    //void HealEnemy(int heal)
    //{
    //    enemyHealth += heal;
    //    if (enemyHealth >= 30)
    //    {
    //        enemyHealth = 30;
    //        SetHealth("Enemy");
    //    }
    //}

    void SetHealth(string target)
    {
        if (target == "Player")
        {
            playerHealthBar.SetHealth(playerHealth);
            _UI.UpdateHP("Player", playerHealth);
        }

        if (target == "Enemy")
        {
            enemyHealthBar.SetHealth(enemyHealth);
            _UI.UpdateHP("Enemy", enemyHealth);
        }
    }

    #endregion



}