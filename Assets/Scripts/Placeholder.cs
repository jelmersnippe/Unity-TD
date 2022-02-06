using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeholder : MonoBehaviour
{
    Color initialColor;
    SpriteRenderer spriteRenderer;

    Tower towerToPlace;
    bool hasPlaced = false;
    bool canPlace = true;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
    }

    void Update()
    {
        if (towerToPlace == null || hasPlaced) {
            return;
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xPosition = Mathf.Round(mousePosition.x * 2) / 2;
        float yPosition = Mathf.Round(mousePosition.y * 2) / 2;
        transform.position = new Vector2(xPosition, yPosition);

        if (canPlace && Input.GetMouseButtonDown(0))
        {
            Instantiate(towerToPlace, transform.position, towerToPlace.transform.rotation);

            hasPlaced = true;
            Destroy(gameObject);
        }
    }

    public void setTowerToPlace(Tower newTowerToPlace)
    {
        towerToPlace = newTowerToPlace;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        canPlace = false;
        spriteRenderer.color = Color.red;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canPlace = true;
        spriteRenderer.color = initialColor;
    }
}
