//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Mirror;

//public class NetworkDraggableObject : NetworkBehaviour
//{
    //public PlayerManager playerManager;
    //private Rigidbody rigidbody;
    //private BoardController board;
    //public NetworkPlayerCardSpot playerCardSpot;

    //public bool canDrag = true;
    //private float startYPos;
    
    
    

    //void Start()
    //{
    //    canDrag = true;
    //    playerCardSpot = GameObject.Find("PlayerCardSpot").GetComponent<NetworkPlayerCardSpot>();
    //    board = GetComponentInParent<BoardController>();
    //    rigidbody = GetComponent<Rigidbody>();
    //    startYPos = 0; // Better to not hardcode that one but whatever

    //    if (!hasAuthority)
    //    {
    //        canDrag = false;
    //    }
    //    else return;
    //}

    //private void OnMouseDrag()
    //{
    //    if (canDrag == true && playerCardSpot.isOccupied == false)
    //    {
    //        Vector3 newWorldPosition = new Vector3(board.CurrentMousePosition.x, startYPos + 1, board.CurrentMousePosition.z);

    //        var difference = newWorldPosition - transform.position;

    //        var speed = 10 * difference;
    //        rigidbody.velocity = speed;
    //        //_rigidbody.rotation = Quaternion.Euler(new Vector3(speed.z, 0, -speed.x));
    //    }
    //    else return;
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.name == "PlayerDropZone")
    //    {
    //        Debug.Log("Card dropped");
    //        gameObject.transform.position = playerCardSpot.transform.position;
    //        canDrag = false;
    //        playerCardSpot.isOccupied = true;
    //        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
    //        playerManager = networkIdentity.GetComponent<PlayerManager>();
    //        playerManager.PlayCard(gameObject);
//        }
//    }
//}