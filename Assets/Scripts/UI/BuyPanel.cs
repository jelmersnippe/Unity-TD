using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPanel : MonoBehaviour
{
    [SerializeField] Tower[] items;
    [SerializeField] ShopItem purchaseButton;

    void Awake()
    {
        float panelWidth = GetComponent<RectTransform>().sizeDelta.x;
        float itemWidth = purchaseButton.GetComponent<RectTransform>().sizeDelta.x;
        for (int i = 0; i < items.Length; i++)
        {
            Tower item = items[i];
            ShopItem createdShopItem = Instantiate(purchaseButton, transform);
            createdShopItem.SetItem(item);
        }
    }
}
