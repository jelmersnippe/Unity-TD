using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherUpgradeTree : UpgradeTree
{
    public override void ActivateUpgrade(UpgradeTreeItem upgradeTreeItem)
    {
        base.ActivateUpgrade(upgradeTreeItem);

        if (upgradeTreeItem.blueprint.type == UpgradeType.Archer_ReinforcedTips)
        {
            towerController.damage += 100;
        }
    }
}
