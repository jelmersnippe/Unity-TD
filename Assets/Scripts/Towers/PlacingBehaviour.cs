using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingBehaviour : MonoBehaviour
{
    Color initialColor;
    SpriteRenderer spriteRenderer;
    LayerMask blockedLayers;

    TowerController towerController;

    private void Start()
    {
        towerController = GetComponent<TowerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
    }

    public void Execute()
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
        if (towerController.currentTowerState != TowerController.TowerState.Placing) return;

        if (((1 << collider.gameObject.layer) & blockedLayers) != 0)
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void OnCollisionExit2D(Collision2D collider)
    {
        if (towerController.currentTowerState != TowerController.TowerState.Placing) return;

        spriteRenderer.color = initialColor;
    }

    public void setBlockedLayers(LayerMask newBlockedLayers)
    {
        blockedLayers = newBlockedLayers;
    }
}
