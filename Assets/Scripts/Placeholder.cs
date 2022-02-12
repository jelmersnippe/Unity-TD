using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeholder : MonoBehaviour
{
    Color initialColor;
    SpriteRenderer spriteRenderer;
    LayerMask blockedLayers;

    [SerializeField] Transform activeObject;
    [SerializeField] Transform placeholderObject;

    public int cost = 10;

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

    public void ConvertToActiveTower()
    {
        // Enable the tower script and object
        Tower tower = GetComponent<Tower>();
        tower.enabled = true;
        activeObject.gameObject.SetActive(true);

        // Disabled palceholder script and object
        placeholderObject.gameObject.SetActive(false);
        enabled = false;
    }
}
