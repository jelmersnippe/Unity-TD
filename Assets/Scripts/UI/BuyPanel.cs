using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class BuyPanel : MonoBehaviour
{
    [SerializeField] TowerController[] items;
    [SerializeField] ShopItem purchaseButton;

    void Start()
    {
        float panelWidth = GetComponent<RectTransform>().sizeDelta.x;
        float itemWidth = purchaseButton.GetComponent<RectTransform>().sizeDelta.x;
        for (int i = 0; i < items.Length; i++)
        {
            TowerController item = items[i];
            ShopItem createdShopItem = Instantiate(purchaseButton, transform);
            createdShopItem.SetItem(item);
        }
    }
}
