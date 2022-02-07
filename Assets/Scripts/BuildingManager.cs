using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;

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

    void Update()
    {
        if (towerToPlace == null)
        {
            return;
        }

        if (activePlaceholder == null && purchaseCurrency >= cost)
        {
            activePlaceholder = Instantiate(placeholder, transform);
            activePlaceholder.setBlockedLayers(blockedLayers);
        } 
        else if (activePlaceholder != null && cost > purchaseCurrency)
        {
            Destroy(activePlaceholder.gameObject);
        }

        if (Input.GetMouseButtonDown(0) && activePlaceholder != null && !Physics2D.IsTouchingLayers(activePlaceholder.GetComponent<BoxCollider2D>(), blockedLayers))
        {
            PlaceTower();
        }
    }

    void PlaceTower()
    {
        if (cost > purchaseCurrency)
        {
            return;
        }

        purchaseCurrency -= cost;

        Instantiate(towerToPlace, activePlaceholder.transform.position, towerToPlace.transform.rotation);
        Destroy(activePlaceholder.gameObject);
    }

    public void addPurchaseCurrency(int currency)
    {
        if (currency < 0)
        {
            return;
        }
        purchaseCurrency += currency;
    }
}
