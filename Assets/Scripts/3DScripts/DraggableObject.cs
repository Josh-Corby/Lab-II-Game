using UnityEngine;

public class DraggableObject : GameBehaviour
{
    private Rigidbody rigidbody;
    private float startYPos;
    private BoardController board;
    public bool canDrag;

    void Start()
    {
        board = GetComponentInParent<BoardController>();
        rigidbody = GetComponent<Rigidbody>();
        startYPos = 0; // Better to not hardcode that one but whatever
        canDrag = true;
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
        if (collision.gameObject.name == "PlayerDropZone")
        {
            Debug.Log("Card dropped");
            gameObject.transform.position = _PCS.transform.position;
            canDrag = false;
            _PCS.isOccupied = true;
        }
    }
}