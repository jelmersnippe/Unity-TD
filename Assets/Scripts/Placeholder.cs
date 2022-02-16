using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Placeholder : MonoBehaviour
{
    Color initialColor;
    SpriteRenderer spriteRenderer;
    LayerMask blockedLayers;

    public Transform activeObject;
    public Transform placeholderObject;

    void Awake()
    {
        activeObject = transform.Find("ActiveObject");
        placeholderObject = transform.Find("Placeholder");
    }

    private void Start()
    {
        spriteRenderer = placeholderObject.GetComponent<SpriteRenderer>();
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

    private void OnCollisionStay2D(Collision2D collider)
    {
        if (((1 << collider.gameObject.layer) & blockedLayers) != 0)
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void OnCollisionExit2D(Collision2D collider)
    {
        spriteRenderer.color = initialColor;
    }

    public void setBlockedLayers(LayerMask newBlockedLayers)
    {
        blockedLayers = newBlockedLayers;
    }
}
