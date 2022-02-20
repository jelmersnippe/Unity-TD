using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Upgrade upgrade;
    public int cost;
    [Range(0,4)]
    public int spot;
    public UpgradeType parentUpgrade;
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
        OnUpgradeActivated?.Invoke(upgradeTreeItem);
    }

    public bool HasUnlockedUpgrade(UpgradeType upgrade)
    {
        return unlockedUpgrades.Contains(upgrade);
    }
}