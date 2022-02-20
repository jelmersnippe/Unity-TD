using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItem : MonoBehaviour
{
    public UpgradeTreeItem item { get; private set; }
    [SerializeField] TextMeshProUGUI costDisplay;
    [SerializeField] TextMeshProUGUI nameDisplay;
    [SerializeField] Image spriteDisplay;
    [SerializeField] Color unpurchaseableColor = new Color(255, 120, 120, 0.6f);
    [SerializeField] public UILineRenderer lineRenderer;
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
        nameDisplay.text = item.upgrade.displayName;
        spriteDisplay.sprite = item.upgrade.sprite;
    }

    private void Update()
    {
        if (item == null)
        {
            return;
        }

        if (item.cost > GameManager.instance.purchaseCurrency)
        {
            costDisplay.color = Color.red;
            spriteDisplay.color = unpurchaseableColor;
            button.enabled = false;
            return;
        }

        if (item.cost <= GameManager.instance.purchaseCurrency)
        {
            costDisplay.color = Color.white;
            spriteDisplay.color = Color.white;
            button.enabled = true;
            return;
        }
    }
}
