using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTower : TowerController
{
    [SerializeField] float splashRadius = 3f;
    [SerializeField] int maxSplashTargetCount = 4;

    override protected void SpawnProjectile()
    {
        SplashProjectile spawnedProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation, transform) as SplashProjectile;
        spawnedProjectile.setValues(damage, projectileSpeed, currentTarget.transform, monsterLayerMask, splashRadius, damage, maxSplashTargetCount);
    }

    override public void ActivateUpgrade(Upgrade upgradeToActivate)
    {
        base.ActivateUpgrade(upgradeToActivate);

        switch (upgradeToActivate.type)
        {
            case "missile_damage_up":
                damage += 50;
                break;
            case "missile_splash_radius_up":
                splashRadius += 1;
                break;
            case "missile_target_count_up":
                maxSplashTargetCount += 2;
                break;
        }
    }
}
