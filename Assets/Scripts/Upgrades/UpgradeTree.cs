using System;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    None,
    Upgrade1,
    Upgrade2,
    Upgrade3,
    Upgrade4,
    Upgrade5,
    Upgrade6,
    Upgrade7,
    Upgrade8,
    Upgrade9,
    Upgrade10,
}

[System.Serializable]
public class UpgradeTreeItem
{
    public enum Status
    {
        Locked,
        Unlocked,
        Blocked,
        Purchased,
    }

    public Upgrade upgrade;
    public int cost;
    [Range(0,4)]
    public int spot;
    public UpgradeType parentUpgrade;
    public Status status = Status.Locked;
} 

public class UpgradeTree : MonoBehaviour
{
    public static event Action<UpgradeTreeItem> OnUpgradeActivated;

    public string treeName;
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
        if (unlockedUpgrades.Contains(upgradeTreeItem.upgrade.type))
        {
            Debug.LogWarning("Can only upgrade once");
            return;
        }

        unlockedUpgrades.Add(upgradeTreeItem.upgrade.type);
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
            Debug.Log("Depth: " + depth);
            List<UpgradeTreeItem> depthUpgrades = upgrades[depth];
            UpgradeTreeItem isPurchasedItemInDepth = depthUpgrades.Find((u) => u.upgrade.type == purchasedUpgrade.upgrade.type);
            Debug.Log("Found item in current depth: " + isPurchasedItemInDepth);

            foreach (UpgradeTreeItem upgrade in depthUpgrades)
            {
                Debug.Log("Upgrade: " + upgrade.upgrade.displayName);
                if (upgrade.status == UpgradeTreeItem.Status.Blocked || upgrade.status == UpgradeTreeItem.Status.Purchased)
                {
                    Debug.Log("Already purchased or blocked");
                    continue;
                }

                Debug.Log("Parent of upgrade: " + upgrade.parentUpgrade);
                Debug.Log("Purchased upgrade: " + purchasedUpgrade.upgrade.type);
                if (isPurchasedItemInDepth != null)
                {
                    Debug.Log("On same level on current item, setting to: " + (upgrade.upgrade.type == purchasedUpgrade.upgrade.type ? UpgradeTreeItem.Status.Purchased : UpgradeTreeItem.Status.Blocked));
                    upgrade.status = upgrade.upgrade.type == purchasedUpgrade.upgrade.type ? UpgradeTreeItem.Status.Purchased : UpgradeTreeItem.Status.Blocked;
                }
                else if (upgrade.parentUpgrade == purchasedUpgrade.upgrade.type)
                {
                    Debug.Log("Parent is equal to purchased item, setting to Unlocked");
                    upgrade.status = UpgradeTreeItem.Status.Unlocked;
                }
                else if (!IsConnected(upgrade, depth, purchasedUpgrade.upgrade.type))
                {
                    Debug.Log("Not connected to the purchased item in any way");
                    upgrade.status = UpgradeTreeItem.Status.Blocked;
                }
            }
        }
    } 

    bool IsConnected(UpgradeTreeItem upgrade, int depth, UpgradeType connectedTo)
    {
        if (upgrade.upgrade.type == connectedTo) return true;
        if (upgrade.status == UpgradeTreeItem.Status.Purchased || 
            upgrade.status == UpgradeTreeItem.Status.Blocked || 
            upgrade.upgrade.type == UpgradeType.None || 
            depth < 0) return false;

        int depthAbove = depth - 1;
        UpgradeTreeItem parentInDepthAbove = upgrades[depthAbove].Find((u) => u.upgrade.type == upgrade.parentUpgrade);
        return parentInDepthAbove != null ? IsConnected(parentInDepthAbove, depthAbove, connectedTo) : false;
    }
}