using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;

    [SerializeField]
    int startingHealth;

    [SerializeField]
    int health;

    [SerializeField]
    Tower[] towers;

    public Tower towerToPlace;
    [SerializeField]
    int cost = 0;

    [SerializeField]
    Placeholder placeholder;

    Placeholder activePlaceholder;

    [SerializeField]
    int purchaseCurrency;

    [SerializeField]
    LayerMask blockedLayers;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        health = startingHealth;
    }

    void Update()
    {
        TowerSelection();

        if (towerToPlace == null)
        {
            return;
        }

        if (activePlaceholder != null && cost > purchaseCurrency)
        {
            towerToPlace = null;
            Destroy(activePlaceholder.gameObject);
            return;
        }

        if (towerToPlace != null && activePlaceholder == null && purchaseCurrency >= cost)
        {
            activePlaceholder = Instantiate(placeholder, transform);
            activePlaceholder.setBlockedLayers(blockedLayers);
        } 

        if (Input.GetMouseButtonDown(0) && activePlaceholder != null && !Physics2D.IsTouchingLayers(activePlaceholder.GetComponent<BoxCollider2D>(), blockedLayers))
        {
            PlaceTower();
        }
    }

    void TowerSelection()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            towerToPlace = null;
            Destroy(activePlaceholder.gameObject);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q) && towers.Length > 0)
        {
            towerToPlace = towers[0];
        }

    }

    void PlaceTower()
    {
        if (cost > purchaseCurrency || activePlaceholder == null)
        {
            return;
        }

        purchaseCurrency -= cost;

        Instantiate(towerToPlace, activePlaceholder.transform.position, towerToPlace.transform.rotation, transform);
    }

    public void addPurchaseCurrency(int currency)
    {
        if (currency < 0)
        {
            return;
        }
        purchaseCurrency += currency;
    }

    public void takeDamage(int damage)
    {
        health = Mathf.Max(health -= damage, 0);
    }
}
