using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] UpgradeButton upgradeButton;
    [SerializeField] float padding = 10f;

    void ResetView()
    {
        foreach (Transform depth in transform)
        {
            foreach (Transform child in depth)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void ShowUpgrades(TowerController tower)
    {
        if (tower == null || !tower.isActiveAndEnabled) return;

        ResetView();

        float panelWidth = transform.parent.GetComponent<RectTransform>().sizeDelta.x - (padding * 2);
        float itemWidth = panelWidth / 5f;
        Dictionary<int, List<UpgradeTreeItem>> items = tower.GetUpgrades();

        for (int depth = 0; depth < items.Count; depth++)
        {
            List<UpgradeTreeItem> upgrades = items[depth];

            Transform parent = transform.Find("Depth " + depth.ToString());
            float depthHeight = parent.GetComponent<RectTransform>().sizeDelta.y;

            for (int i = 0; i < upgrades.Count; i++)
            {
                UpgradeTreeItem currentUpgrade = upgrades[i];

                UpgradeButton createdUpgradeItem = Instantiate(upgradeButton, parent);
                createdUpgradeItem.transform.localPosition += new Vector3(padding + (itemWidth * currentUpgrade.spot), 0);
                createdUpgradeItem.SetItem(currentUpgrade);

                if (depth > 0)
                {
                    RenderConnections(createdUpgradeItem, currentUpgrade.parentUpgrade, items[depth - 1], currentUpgrade.spot, itemWidth, depthHeight);
                }
            }
        }
    }

    void RenderConnections(UpgradeButton upgradeButton, UpgradeType parentUpgradeType, List<UpgradeTreeItem> possibleParents, int upgradeSpot, float widthOffset, float heightOffset)
    {
        if (parentUpgradeType == UpgradeType.None) return;

        UpgradeTreeItem parentItem = possibleParents.Find((upgrade) => upgrade.blueprint.type == parentUpgradeType);

        if (parentItem == null)
        {
            return;
        }

        UILineRenderer lineRenderer = Instantiate(upgradeButton.lineRenderer, upgradeButton.transform);
        List<Vector2> lineRendererPoints = new List<Vector2>();

        Vector3 centerOffset = new Vector3(0, upgradeButton.GetComponent<RectTransform>().sizeDelta.y / 2);

        lineRendererPoints.Add(centerOffset);
        lineRendererPoints.Add(new Vector3((parentItem.spot - upgradeSpot) * widthOffset, heightOffset) - centerOffset);

        lineRenderer.points = lineRendererPoints;
    }
}
