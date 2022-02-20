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
    //[SerializeField] UpgradeItem purchaseButton;

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

        //if (selectedTower.isActiveAndEnabled)
        //{
        //    ShowUpgrades();
        //}
    }
    //public void ShowUpgrades()
    //{
    //    foreach (Transform depth in upgradeSection)
    //    {
    //        foreach (Transform child in depth)
    //        {
    //            Destroy(child.gameObject);
    //        }
    //    }

    //    Dictionary<int, List<UpgradeItem>> spawnedUpgrades = new Dictionary<int, List<UpgradeItem>>();

    //    float padding = 10f;

    //    float panelWidth = GetComponent<RectTransform>().sizeDelta.x - padding * 2;
    //    float itemWidth = panelWidth / 5f;
    //    List<Upgrade> items = selectedTower.upgradeTree.serializableUpgradeTreeItems.OrderBy((upgrade) => upgrade.depth).ToList();
    //    for (int i = 0; i < items.Count; i++)
    //    {
    //        Upgrade item = items[i];

    //        Transform parent = upgradeSection.Find("Depth " + item.depth.ToString());

    //        UpgradeItem createdUpgradeItem = Instantiate(purchaseButton, parent);
    //        createdUpgradeItem.transform.localPosition += new Vector3(padding + (itemWidth * item.spot), 0);
    //        createdUpgradeItem.SetItem(item);

    //        if (!spawnedUpgrades.ContainsKey(item.depth))
    //        {
    //            spawnedUpgrades.Add(item.depth, new List<UpgradeItem>());
    //        }

    //        spawnedUpgrades[item.depth].Add(createdUpgradeItem);

    //        if (item.parentUpgrade != Upgrade.Type.None)
    //        {
    //            UpgradeItem parentItem = spawnedUpgrades[item.depth - 1].Find((upgrade) => upgrade.item.type == item.parentUpgrade);

    //            if (parentItem == null)
    //            {
    //                Debug.Log("Could not find parent for " + item.displayName);
    //                continue;
    //            }

    //            UILineRenderer lineRenderer = createdUpgradeItem.lineRenderer;
    //            List<Vector2> lineRendererPoints = new List<Vector2>();

    //            Vector3 centerOffset = new Vector3(0, createdUpgradeItem.GetComponent<RectTransform>().sizeDelta.y / 2);

    //            lineRendererPoints.Add(centerOffset);
    //            lineRendererPoints.Add(parentItem.transform.position - createdUpgradeItem.transform.position - centerOffset);

    //            lineRenderer.points = lineRendererPoints;
    //        }
    //    }
    //}
}
