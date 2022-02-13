using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTower : Tower
{
    [SerializeField] float splashRadius;
    [SerializeField] int splashDamage;
    [SerializeField] int maxSplashTargetCount;

    override protected void SpawnProjectile()
    {
        SplashProjectile spawnedProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation, transform) as SplashProjectile;
        spawnedProjectile.setValues(damage, projectileSpeed, currentTarget.transform, monsterLayerMask, splashRadius, splashDamage, maxSplashTargetCount);
    }
}
