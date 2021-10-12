using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCard : MonoBehaviour
{
    public float returnSpeed = 5f;
    public GameObject heroPrefab;

    private bool isDragging;
    private bool withinZoneA;
    private Vector3 originalPosition;
    private bool returning;

    private void Start()
    {
        originalPosition = transform.position;
    }

    //mouse drag started
    public void OnMouseDown()
    {
        isDragging = true;
        returning = false;
    }

    //mouse drag release
    public void OnMouseUp()
    {
        isDragging = false;
        if(withinZoneA)
        {
            //remove card from stack
            Destroy(gameObject);

            //spawn hero
            if (heroPrefab != null)
                Instantiate(heroPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            //return to original position
            returning = true;
        }
    }

    void Update()
    {
        //if dragging, update position to the point of the cursor
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }

        //if card is not released on a drop zone, return the card to the stack
        if(returning)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if the card is within a drop zone
        if(collision.gameObject.tag == "Drop Zone A")
        {
            withinZoneA = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //check if the card has left a drop zone
        if (collision.gameObject.tag == "Drop Zone A")
        {
            withinZoneA = false;
        }
    }
}