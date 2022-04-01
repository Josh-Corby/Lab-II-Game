using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DraggableObject : GameBehaviour
{
    public GameManager playerManager;
    private Rigidbody rigidbody;
    private BoardController board;
    public bool canDrag = true;
    public bool played = false;
    private float startYPos;


    void Start()
    {
        canDrag = true;
        board = GetComponentInParent<BoardController>();
        rigidbody = GetComponent<Rigidbody>();
        startYPos = 0; // Better to not hardcode that one but whatever

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
                played = true;
                Debug.Log("Card dropped");
                gameObject.transform.position = _PCS.transform.position;
                canDrag = false;
                _PCS.isOccupied = true;
                _GM.PlayEnemyCard();
            }
        }
    }
}