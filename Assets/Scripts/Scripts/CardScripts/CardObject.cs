using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardObject : GameBehaviour
{
    public GameManager playerManager;
    private Rigidbody rigidbody;
    private BoardController board;
    private bool canDrag = true;
    private bool played = false;
    private float startYPos;

    public Card card;
    private int id;
    private string cardName;
    private int damage;
    private string[] attackColours = { };
    private string[] defenseColours = { };
    private string cardEffect;
    
    private Sprite frontImage;
    private Sprite backImage;

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
        attackColours = card.attackColours;
        defenseColours = card.defenseColours;
        cardEffect = card.cardEffect;
        damage = card.damage;

        frontImage = card.frontImage;
        backImage = card.backImage;

        //attackColour = card.attackColour;
        //defenseColour = card.defenseColour;
    }

    private void OnMouseDrag()
    {
        if (canDrag == true && _PCS.isOccupied == false)
        {
            Vector3 newWorldPosition = new Vector3(board.CurrentMousePosition.x, startYPos + 1, board.CurrentMousePosition.z);

            var difference = newWorldPosition - transform.position;

            var speed = 10 * difference;
            rigidbody.velocity = speed;
            //_rigidbody.rotation = Quaternion.Euler(new Vector3(speed.z, 0, -speed.x));
        }
        else return;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!played)
        {
            if (collision.gameObject.name == "PlayerDropZone" && !played)
            {
                _GM.PlayPlayerCard(damage, attackColours, defenseColours);
                gameObject.transform.position = _PCS.transform.position;
                played = true;
                Debug.Log("Card dropped");
                canDrag = false;
                _PCS.isOccupied = true;             
            }
        }
    }
}