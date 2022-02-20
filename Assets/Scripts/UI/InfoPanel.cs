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

        image.sprite = selectedTower.GetComponent<SpriteRenderer>().sprite;
        damageText.text = selectedTower.damage.ToString();
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
        foreach (Transform depth in upgradeSection)
        {
            foreach (Transform child in depth)
            {
                Destroy(child.gameObject);
            }
        }

        float padding = 10f;

        float panelWidth = GetComponent<RectTransform>().sizeDelta.x - padding * 2;
        float itemWidth = panelWidth / 5f;
        Dictionary<int, List<UpgradeTreeItem>> items = selectedTower.GetUpgrades();
        for (int depth = 0; depth < items.Count; depth++)
        {
            List<UpgradeTreeItem> upgrades = items[depth];

            Transform parent = upgradeSection.Find("Depth " + depth.ToString());
            float depthHeight = parent.GetComponent<RectTransform>().sizeDelta.y;

            for (int i = 0; i < upgrades.Count; i++)
            {
                UpgradeTreeItem currentUpgrade = upgrades[i];

                UpgradeItem createdUpgradeItem = Instantiate(purchaseButton, parent);
                createdUpgradeItem.transform.localPosition += new Vector3(padding + (itemWidth * currentUpgrade.spot), 0);
                createdUpgradeItem.SetItem(currentUpgrade);

                if (currentUpgrade.parentUpgrade != UpgradeType.None)
                {
                    UpgradeTreeItem parentItem = items[depth - 1].Find((upgrade) => upgrade.upgrade.type == currentUpgrade.parentUpgrade);

                    if (parentItem == null)
                    {
                        Debug.Log("Could not find parent for " + currentUpgrade.upgrade.displayName);
                        continue;
                    }

                    UILineRenderer lineRenderer = createdUpgradeItem.lineRenderer;
                    List<Vector2> lineRendererPoints = new List<Vector2>();

                    Vector3 centerOffset = new Vector3(0, createdUpgradeItem.GetComponent<RectTransform>().sizeDelta.y / 2);

                    lineRendererPoints.Add(centerOffset);
                    lineRendererPoints.Add(new Vector3((parentItem.spot - currentUpgrade.spot) * itemWidth, depthHeight) - centerOffset);

                    lineRenderer.points = lineRendererPoints;
                }
            }
        }
    }
}
