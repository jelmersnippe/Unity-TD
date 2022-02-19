using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class InfoPanel : MonoBehaviour
{
    public static InfoPanel instance;

    [SerializeField] TowerController selectedTower;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI rangeText;
    [SerializeField] TextMeshProUGUI fireRateText;

    [SerializeField] GameObject infoSection;
    [SerializeField] Transform upgradeSection;
    [SerializeField] UpgradeItem purchaseButton;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetSelectedTower(TowerController tower)
    {
        selectedTower = tower;
        UpdateInfo();
    }

    public void DeselectSelectedTower()
    {
        selectedTower = null;
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        if (selectedTower == null)
        {
            infoSection.SetActive(false);
            upgradeSection.gameObject.SetActive(false);
            return;
        }

        image.sprite = selectedTower.uiSprite;
        damageText.text = selectedTower.damage.ToString();
        rangeText.text = selectedTower.range.ToString();
        fireRateText.text = selectedTower.roundsPerMinute.ToString();

        infoSection.SetActive(true);
        upgradeSection.gameObject.SetActive(selectedTower.isActiveAndEnabled);

        if (selectedTower.isActiveAndEnabled)
        {
            ShowUpgrades();
        }
    }
    public void ShowUpgrades()
    {
        foreach (Transform child in upgradeSection)
        {
            Destroy(child.gameObject);
        }

        float panelHeight = GetComponent<RectTransform>().sizeDelta.y;
        List<Upgrade> items = selectedTower.upgrades;
        float itemHeight = purchaseButton.GetComponent<RectTransform>().sizeDelta.y;
        for (int i = 0; i < items.Count; i++)
        {
            Upgrade item = items[i];
            if (selectedTower.unlockedUpgrades.Contains(item))
            {
                continue;
            }

            UpgradeItem createdUpgradeItem = Instantiate(purchaseButton, upgradeSection);
            createdUpgradeItem.SetItem(item);
        }
    }
}
