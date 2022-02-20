using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(RectTransform))]
public class InfoPanel : MonoBehaviour
{
    public static InfoPanel instance;

    [SerializeField] TowerController selectedTower;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI fireRateText;

    [SerializeField] GameObject infoSection;
    [SerializeField] UpgradePanel upgradeSection;

    private void OnEnable()
    {
        UpgradeTree.OnUpgradeActivated += (UpgradeTreeItem item) => upgradeSection.ShowUpgrades(selectedTower);
        GameManager.OnCurrencyUpdate += (int currency) => upgradeSection.ShowUpgrades(selectedTower);
    }

    private void OnDisable()
    {
        UpgradeTree.OnUpgradeActivated -= (UpgradeTreeItem item) => upgradeSection.ShowUpgrades(selectedTower);
        GameManager.OnCurrencyUpdate -= (int currency) => upgradeSection.ShowUpgrades(selectedTower);
    }

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

        image.sprite = selectedTower.GetComponent<SpriteRenderer>().sprite;
        damageText.text = selectedTower.damage.ToString();
        fireRateText.text = selectedTower.roundsPerMinute.ToString();

        infoSection.SetActive(true);
        upgradeSection.gameObject.SetActive(selectedTower.isActiveAndEnabled);

        upgradeSection.ShowUpgrades(selectedTower);
    }
    
}
