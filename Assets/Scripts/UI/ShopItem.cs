using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [SerializeField] Tower item;
    [SerializeField] int cost;
    [SerializeField] TextMeshProUGUI costDisplay;
    [SerializeField] Image spriteDisplay;
    [SerializeField] Color unpurchaseableColor = new Color(255, 255, 255, 0.6f);
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetItem(Tower itemToSet)
    {
        item = itemToSet;
        cost = itemToSet.cost;
        // TODO: Use a actual event
        button.onClick.AddListener(delegate { BuildingManager.instance.SetTowerToPlace(item); });
        costDisplay.text = "$" + cost;
        spriteDisplay.sprite = item.uiSprite;
    }

    private void Update()
    {
        if (item == null)
        {
            return;
        }

        if (cost > GameManager.instance.purchaseCurrency)
        {
            costDisplay.color = Color.red;
            spriteDisplay.color = unpurchaseableColor;
            button.enabled = false;
            return;
        }

        if (cost <= GameManager.instance.purchaseCurrency)
        {
            costDisplay.color = Color.white;
            spriteDisplay.color = Color.white;
            button.enabled = true;
            return;
        }
    }

}
