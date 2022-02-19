using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItem : MonoBehaviour
{
    [SerializeField] Upgrade item;
    [SerializeField] int cost;
    [SerializeField] TextMeshProUGUI costDisplay;
    [SerializeField] TextMeshProUGUI nameDisplay;
    [SerializeField] Image spriteDisplay;
    [SerializeField] Color unpurchaseableColor = new Color(255, 120, 120, 0.6f);
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetItem(Upgrade itemToSet)
    {
        item = itemToSet;
        cost = itemToSet.cost;
        // TODO: Use an actual event
        button.onClick.AddListener(delegate {
            BuildingManager.instance.selectedTower.ActivateUpgrade(itemToSet);
            InfoPanel.instance.UpdateInfo();
        });

        costDisplay.text = "$" + cost;
        nameDisplay.text = item.displayName;
        spriteDisplay.sprite = item.sprite;
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
