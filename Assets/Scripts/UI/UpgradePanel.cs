using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] UpgradeItem purchaseButton;

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

                UpgradeItem createdUpgradeItem = Instantiate(purchaseButton, parent);
                createdUpgradeItem.transform.localPosition += new Vector3(padding + (itemWidth * currentUpgrade.spot), 0);
                createdUpgradeItem.SetItem(currentUpgrade);

                if (currentUpgrade.parentUpgrade != UpgradeType.None)
                {
                    UpgradeTreeItem parentItem = items[depth - 1].Find((upgrade) => upgrade.upgrade.type == currentUpgrade.parentUpgrade);

                    if (parentItem == null)
                    {
                        Debug.LogWarning("Could not find parent for " + currentUpgrade.upgrade.displayName);
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
