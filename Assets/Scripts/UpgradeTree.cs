using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeTree<T> : MonoBehaviour
{
    public static event Action<Upgrade<T>> OnUpgradeActivated;

    public string treeName;
    public Dictionary<int, List<Upgrade<T>>> serializableUpgradeTreeItems = new Dictionary<int, List<Upgrade<T>>>();

    public UpgradeTree<T> upgradeTree;
    List<T> unlockedUpgrades = new List<T>();

    public virtual void ActivateUpgrade(Upgrade<T> upgradeTreeItem)
    {
        if (unlockedUpgrades.Contains(upgradeTreeItem.type))
        {
            Debug.LogWarning("Can only upgrade once");
            return;
        }

        unlockedUpgrades.Add(upgradeTreeItem.type);
        OnUpgradeActivated?.Invoke(upgradeTreeItem);
    }

    public bool HasUnlockedUpgrade(T upgrade)
    {
        return unlockedUpgrades.Contains(upgrade);
    }
}