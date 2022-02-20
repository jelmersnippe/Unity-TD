using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public UpgradeBlueprint blueprint;
    public int cost;
    [Range(0, 4)]
    public int spot;
    public UpgradeType parentUpgrade;
    public Status status = Status.Locked;
}