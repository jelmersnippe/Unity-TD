using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeTree : MonoBehaviour
{
    public string treeName;
    public List<Upgrade> serializableUpgradeTreeItems = new List<Upgrade>();
    public List<UpgradeTreeItem> upgradeTreeItems = new List<UpgradeTreeItem>();

    private void Start()
    {
        upgradeTreeItems = CreateTree();

        PrintTree(upgradeTreeItems);
    }

    void PrintTree(List<UpgradeTreeItem> tree, string prefix = "")
    {
        for (int i = 0; i < tree.Count; i++)
        {
            UpgradeTreeItem upgrade = tree[i];

            string upgradePrefix = prefix + i.ToString() + ". ";
            Debug.Log(upgradePrefix + upgrade.upgrade.displayName);
            PrintTree(upgrade.children, upgradePrefix);
        }
    }

    List<UpgradeTreeItem> CreateTree(Upgrade.Type parentType = Upgrade.Type.None)
    {
        List<UpgradeTreeItem> items = new List<UpgradeTreeItem>();
        List<Upgrade.Type> allUpgradeTypes = serializableUpgradeTreeItems.Select((item) => item.type).ToList();

        if (allUpgradeTypes.Count != allUpgradeTypes.Distinct().ToList().Count())
        {
            Debug.LogError("There is an upgrade that is present twice in the tree called " + treeName);
            return items;
        }

        foreach (Upgrade upgrade in serializableUpgradeTreeItems)
        {
            if (upgrade.parentUpgrade != Upgrade.Type.None && !serializableUpgradeTreeItems.Select((item) => item.type).Contains(upgrade.parentUpgrade))
            {
                Debug.LogError("There is an upgrade in the update tree called " + treeName + " with no connecting parent: " + upgrade.name + " with parent " + upgrade.parentUpgrade);
                return items;
            }

            if (upgrade.parentUpgrade != parentType)
            {
                continue;
            }

            items.Add(GenerateUpdateTreeItem(upgrade));
        }

        return items;
    }

    UpgradeTreeItem GenerateUpdateTreeItem(Upgrade upgrade)
    {
        UpgradeTreeItem upgradeTreeItem = new UpgradeTreeItem();

        upgradeTreeItem.upgrade = upgrade;
        upgradeTreeItem.cost = upgrade.cost;
        upgradeTreeItem.children = CreateTree(upgrade.type);

        return upgradeTreeItem;
    }
}

public class UpgradeTreeItem
{
    public Upgrade upgrade;
    public int cost;
    public Upgrade.Type conflictingUpgrade;
    public List<UpgradeTreeItem> children;
}