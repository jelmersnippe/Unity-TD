using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeholder : MonoBehaviour
{
    Color initialColor;
    SpriteRenderer spriteRenderer;

    LayerMask blockedLayers;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
    }

    void Update()
    {
        if (BuildingManager.instance.towerToPlace == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mousePosition.x, mousePosition.y);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(((1 << collision.gameObject.layer) & blockedLayers) != 0)
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        spriteRenderer.color = initialColor;
    }

    public void setBlockedLayers(LayerMask newBlockedLayers)
    {
        blockedLayers = newBlockedLayers;
    }
}
