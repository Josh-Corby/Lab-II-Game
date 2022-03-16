using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    private Vector3 startPosition;
    private bool isDragging = false;
    private bool isOverDropZone = false;
    private GameObject dropZone;

    void Update()
    {
        if(isDragging)
        {
            transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    public void StartDrag()
    {
        startPosition = transform.position;
        isDragging = true;
    }

    public void EndDrag()
    {
        isDragging = false;
        if (isOverDropZone)
        {
            transform.SetParent(dropZone.transform);
        }
        else
        {
            transform.position = startPosition;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isOverDropZone = true;
        dropZone = collision.gameObject;
    }

    private void OnCollisionExit(Collision collision)
    {
        isOverDropZone = false;
        dropZone = null;
    }
}
