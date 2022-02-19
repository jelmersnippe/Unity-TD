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

    override public void ActivateUpgrade(Upgrade upgrade)
    {
        base.ActivateUpgrade(upgrade);

        switch (upgrade.type)
        {
            case Upgrade.Type.Default_DamageUp:
                damage += 50;
                break;
            case Upgrade.Type.Splash_RadiusUp:
                splashRadius += 1;
                break;
            case Upgrade.Type.Splash_TargetCountUp:
                maxSplashTargetCount += 2;
                break;
        }
    }
}
