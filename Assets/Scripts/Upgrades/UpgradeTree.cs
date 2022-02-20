using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTree : MonoBehaviour
{
    public static event Action<UpgradeTreeItem> OnUpgradeActivated;

    public List<UpgradeTreeItem> depth0Upgrades = new List<UpgradeTreeItem>();
    public List<UpgradeTreeItem> depth1Upgrades = new List<UpgradeTreeItem>();
    public List<UpgradeTreeItem> depth2Upgrades = new List<UpgradeTreeItem>();
    public List<UpgradeTreeItem> depth3Upgrades = new List<UpgradeTreeItem>();
    public List<UpgradeTreeItem> depth4Upgrades = new List<UpgradeTreeItem>();

    public Dictionary<int, List<UpgradeTreeItem>> upgrades = new Dictionary<int, List<UpgradeTreeItem>>();

    List<UpgradeType> unlockedUpgrades = new List<UpgradeType>();

    private void Start()
    {
        foreach (UpgradeTreeItem item in depth0Upgrades)
        {
            item.status = UpgradeTreeItem.Status.Unlocked;
        }
        upgrades.Add(0, depth0Upgrades);
        upgrades.Add(1, depth1Upgrades);
        upgrades.Add(2, depth2Upgrades);
        upgrades.Add(3, depth3Upgrades);
        upgrades.Add(4, depth4Upgrades);
    }

    public virtual void ActivateUpgrade(UpgradeTreeItem upgradeTreeItem)
    {
        if (unlockedUpgrades.Contains(upgradeTreeItem.blueprint.type))
        {
            Debug.LogWarning("Can only upgrade once");
            return;
        }

        unlockedUpgrades.Add(upgradeTreeItem.blueprint.type);
        UpdateUpgradesTree(upgradeTreeItem);

        OnUpgradeActivated?.Invoke(upgradeTreeItem);
    }

    public bool HasUnlockedUpgrade(UpgradeType upgrade)
    {
        return unlockedUpgrades.Contains(upgrade);
    }

    void UpdateUpgradesTree(UpgradeTreeItem purchasedUpgrade)
    {
        for (int depth = 0; depth < upgrades.Count; depth++)
        {
            List<UpgradeTreeItem> depthUpgrades = upgrades[depth];
            UpgradeTreeItem isPurchasedItemInDepth = depthUpgrades.Find((u) => u.blueprint.type == purchasedUpgrade.blueprint.type);

            foreach (UpgradeTreeItem upgrade in depthUpgrades)
            {
                if (upgrade.status == UpgradeTreeItem.Status.Blocked || upgrade.status == UpgradeTreeItem.Status.Purchased)
                {
                    continue;
                }

                if (isPurchasedItemInDepth != null)
                {
                    upgrade.status = upgrade.blueprint.type == purchasedUpgrade.blueprint.type ? UpgradeTreeItem.Status.Purchased : UpgradeTreeItem.Status.Blocked;
                }
                else if (upgrade.parentUpgrade == purchasedUpgrade.blueprint.type)
                {
                    upgrade.status = UpgradeTreeItem.Status.Unlocked;
                }
                else if (!IsConnected(upgrade, depth, purchasedUpgrade.blueprint.type))
                {
                    upgrade.status = UpgradeTreeItem.Status.Blocked;
                }
            }
        }
    } 

    bool IsConnected(UpgradeTreeItem upgrade, int depth, UpgradeType connectedTo)
    {
        if (upgrade.blueprint.type == connectedTo) return true;
        if (upgrade.status == UpgradeTreeItem.Status.Purchased || 
            upgrade.status == UpgradeTreeItem.Status.Blocked || 
            upgrade.blueprint.type == UpgradeType.None || 
            depth < 0) return false;

        int depthAbove = depth - 1;
        UpgradeTreeItem parentInDepthAbove = upgrades[depthAbove].Find((u) => u.blueprint.type == upgrade.parentUpgrade);
        return parentInDepthAbove != null ? IsConnected(parentInDepthAbove, depthAbove, connectedTo) : false;
    }
}