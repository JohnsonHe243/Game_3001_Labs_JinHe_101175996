using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDragScript : MonoBehaviour
{
    private bool isDragging = false;
    private Rigidbody2D currentDraggedObject;

    private Vector2 offset;

    // Update is called once per frame
    void Update()
    {
        // First we check if a player clicked on screen
        if(Input.GetMouseButtonDown(0))
        {
            // Raycast to check if the mouse is over the collider
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if(hit.collider != null) // we click to a object with collider
            {
                // Check if the clicked Gameobject has a rigidbody 2d
                Rigidbody2D rb2d = hit.collider.GetComponent<Rigidbody2D>(); // try to get rigidbody2d from the gameObject from it collider

                if(rb2d != null)
                {
                    // Start dragging only if no object is currently being dragged
                    isDragging = true; 
                    currentDraggedObject = rb2d;

                    offset =rb2d.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition); 

                }
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            currentDraggedObject = null;
        }

        // Move object according to mouse
        if(isDragging && currentDraggedObject != null) 
        {
            // Move the dragged GameObjected based on the mouse position
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentDraggedObject.MovePosition(mousePosition + offset);
        }
    }
}
