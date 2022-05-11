using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class CardObject : GameBehaviour
{
    private Rigidbody rigidbody;
    private BoardController board;
    private bool canDrag = true;
    private bool played = false;
    private float startYPos;

    private ParticleSystem playParticles;



    //Values from scriptable object to be assigned to CardObject
    public TMP_Text cardNameText;
    public TMP_Text cardEffectText;
    public Card card;
    [HideInInspector]
    public int id, damageAmount, healAmount, effectAmount;
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

        #region Card Values
        //values from scriptable objects assigned to card object
        id = card.id;
        cardName = card.cardName;
        cardNameText.text = card.cardName;
        cardEffectText.text = card.cardEffect;
        attackColours = card.attackColours;
        defenseColours = card.defenseColours;
        cardEffect = card.cardEffect;
        damageAmount = card.damageAmount;
        healAmount = card.healAmount;
        cardColour = card.cardColour;
        type = card.type;
        effectType = card.effectType;
        attackType = card.attackType;
        effectAmount = card.effectAmount;
        colourSpecificColour = card.colourSpecificColour;
        #endregion
        playParticles = gameObject.GetComponentInChildren<ParticleSystem>();

        /*
        attackColour = card.attackColour;
        defenseColour = card.defenseColour;

        for(int i=0; i<4; i++)
        {
        attackColours[i] = card.attackColours[i];
         defenseColours[i] = card.defenseColours[i];
        }
        */
    }

    //Function for clicking and tragging the card
    private void OnMouseDrag()
    {
        //Check if there isn't a card in the played card spot
        if (canDrag == true && _PCS.isOccupied == false)
        {  
            //Change the scale of the card
            gameObject.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            //Change position of card to position of mouse but contained to the board
            Vector3 newWorldPosition = new Vector3(board.CurrentMousePosition.x, startYPos + 1, board.CurrentMousePosition.z);

            var difference = newWorldPosition - transform.position;

            var speed = 10 * difference;
            rigidbody.velocity = speed;
            //_rigidbody.rotation = Quaternion.Euler(new Vector3(speed.z, 0, -speed.x));
        }
        else return;
    }

    //When the mouse hovers over a card
    private void OnMouseEnter()
    {
        //Play sound effect
        _GM.HoverSound();
        //rigidbody.useGravity = false;
        //Increase scale and rotate card towards camera
        gameObject.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        gameObject.transform.DORotate(new Vector3(-18.295f, -180, 0), 0.2f);
        //gameObject.transform.Rotate(0f, 180f, 0f, Space.Self);
    }

    private void OnMouseExit()
    {
        rigidbody.useGravity = true;
        //Reduce scale to original size and rotate back to original rotation
        gameObject.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        gameObject.transform.DORotate(new Vector3(-90, -90, -90), 0.2f);
        //gameObject.transform.Rotate(0f, 0f, 0f, Space.Self);
    }

    //Function for when card enters the dropzone
    private void OnCollisionEnter(Collision collision)
    {
        if (!played)
        {
            if (collision.gameObject.name == "PlayerDropZone" && !played)
            {
                //Play sound effect
                _GM.PlaceSound();
                //Start function for playing enemy card
                StartCoroutine(_GM.PlayEnemyCard(this));
                
                //set position of card to played card position
                gameObject.transform.position = _PCS.transform.position;
                played = true;

                //change bools so cards can't be dragged until bools are changed again
                canDrag = false;
                _PCS.isOccupied = true;
                //PlayParticles();
                //Debug.Log("Player plays: " + card.cardName);
            }
        }
    }

    public void PlayParticles()
    {
        playParticles.Play();
    }
}