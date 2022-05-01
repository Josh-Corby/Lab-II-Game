using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardObject : GameBehaviour
{
    private Rigidbody rigidbody;
    private BoardController board;
    private bool canDrag = true;
    private bool played = false;
    private float startYPos;

    private ParticleSystem playParticles;
    public TMP_Text cardNameText;
    public TMP_Text cardEffectText;

    public Card card;
    public int id;
    [HideInInspector]
    public int damageAmount, healAmount, colourSpecificAmount;
    [HideInInspector]
    public string cardName, cardEffect, colourSpecificColour;

    public cardColour cardColour;
    [HideInInspector]
    public type type;
    [HideInInspector]
    public effectType effectType;
    [HideInInspector]
    public attackType attackType;
    
    public cardColour[] attackColours;
    
    public cardColour[] defenseColours;
    //[HideInInspector]
    //public Sprite frontImage, backImage;

    //private string attackColour;
    //private string defenseColour;
    

    void Start()
    {
        canDrag = true;
        board = GetComponentInParent<BoardController>();
        rigidbody = GetComponent<Rigidbody>();
        startYPos = 0; // Better to not hardcode that one but whatever

        id = card.id;
        cardName = card.cardName;
        cardNameText.text = card.cardName;
        cardEffectText.text = card.cardEffect;

        attackColours = card.attackColours;
        defenseColours = card.defenseColours;
        /*
        for(int i=0; i<4; i++)
        {
            attackColours[i] = card.attackColours[i];
            defenseColours[i] = card.defenseColours[i];
        }
        */

        cardEffect = card.cardEffect;
        damageAmount = card.damageAmount;
        healAmount = card.healAmount;

        cardColour = card.cardColour;
        type = card.type;
        effectType = card.effectType;
        attackType = card.attackType;

        colourSpecificAmount = card.colourSpecificAmount;
        colourSpecificColour = card.colourSpecificColour;

        playParticles = gameObject.GetComponentInChildren<ParticleSystem>();
        
        //attackColour = card.attackColour;
        //defenseColour = card.defenseColour;
    }

    private void OnMouseDrag()
    {
        if (canDrag == true && _PCS.isOccupied == false)
        {
            gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Vector3 newWorldPosition = new Vector3(board.CurrentMousePosition.x, startYPos + 1, board.CurrentMousePosition.z);

            var difference = newWorldPosition - transform.position;

            var speed = 10 * difference;
            rigidbody.velocity = speed;
            //_rigidbody.rotation = Quaternion.Euler(new Vector3(speed.z, 0, -speed.x));
        }
        else return;
    }

    private void OnMouseEnter()
    {
        gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        //gameObject.transform.Rotate(0f, 180f, 0f, Space.Self);
    }

    private void OnMouseExit()
    {
        gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //gameObject.transform.Rotate(0f, 0f, 0f, Space.Self);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!played)
        {
            if (collision.gameObject.name == "PlayerDropZone" && !played)
            {
                StartCoroutine(_GM.PlayEnemyCard(this));
                gameObject.transform.position = _PCS.transform.position;
                played = true;
                canDrag = false;
                _PCS.isOccupied = true;
                PlayParticles();
                Debug.Log("Player plays: " + card.cardName);
            }
        }
    }

    public void PlayParticles()
    {
        playParticles.Play();
    }
}