using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherFiringBehaviour : FiringBehaviour
{
    [SerializeField] float degreesBetweenShots = 25f;

    protected override void FireProjectile()
    {
        AudioManager.instance.Play(Sound.Name.Shoot);
        bool hasTripleShot = towerController.HasUnlockedUpgrade(UpgradeType.Archer_SpreadShot);
        if (!hasTripleShot)
        {
            base.FireProjectile();
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            // Spawn a new projectile with a rotation based on the space between shots and which shot it is so we get an even distribution with one centered shot
            SpawnProjectile(-degreesBetweenShots + degreesBetweenShots * i);
        }
    }

    protected override void SetupProjectile(Projectile projectile)
    {
        bool hasPierce = towerController.HasUnlockedUpgrade(UpgradeType.Archer_PiercingShot);
        bool hasReinforcedTips = towerController.HasUnlockedUpgrade(UpgradeType.Archer_ReinforcedTips);

        base.SetupProjectile(projectile);
        projectile.SetMaxMonstersHit(hasPierce ? 2 : 1);
    }
}
