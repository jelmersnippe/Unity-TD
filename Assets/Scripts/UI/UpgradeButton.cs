using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public UpgradeTreeItem item { get; private set; }
    [SerializeField] TextMeshProUGUI costDisplay;
    [SerializeField] TextMeshProUGUI nameDisplay;
    [SerializeField] Image blockedDisplay;
    [SerializeField] Image spriteDisplay;
    [SerializeField] Color unpurchaseableColor = new Color(255, 120, 120, 0.6f);
    public UILineRenderer lineRenderer;
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetItem(UpgradeTreeItem itemToSet)
    {
        item = itemToSet;
        // TODO: Use an actual event
        button.onClick.AddListener(delegate {
            BuildingManager.instance.selectedTower.ActivateUpgrade(itemToSet);
            InfoPanel.instance.UpdateInfo();
        });

        costDisplay.text = "$" + item.cost;
        nameDisplay.text = item.blueprint.displayName;
        spriteDisplay.sprite = item.blueprint.sprite;

        SetDisplayStatus();
    }

    void SetDisplayStatus()
    {
        if (item == null)
        {
            return;
        }

        switch (item.status)
        {
            case UpgradeTreeItem.Status.Purchased:
                costDisplay.enabled = false;
                button.enabled = false;
                break;
            case UpgradeTreeItem.Status.Unlocked:
                spriteDisplay.color = item.cost > GameManager.instance.purchaseCurrency ? unpurchaseableColor : Color.white;
                costDisplay.color = item.cost > GameManager.instance.purchaseCurrency ? Color.red : Color.white;
                button.enabled = item.cost <= GameManager.instance.purchaseCurrency;
                break;
            case UpgradeTreeItem.Status.Locked:
                spriteDisplay.color = Color.red;
                costDisplay.color = Color.red;
                button.enabled = false;
                break;
            case UpgradeTreeItem.Status.Blocked:
                costDisplay.enabled = false;
                button.enabled = false;
                blockedDisplay.enabled = true;
                break;
        }
    }
}
