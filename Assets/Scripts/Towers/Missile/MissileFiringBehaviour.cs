using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileFiringBehaviour : FiringBehaviour
{
    [SerializeField] float splashRadius = 3f;
    [SerializeField] int maxSplashTargetCount = 4;

    override protected void SpawnProjectile()
    {
        SplashProjectile spawnedProjectile = Instantiate(towerController.projectile, towerController.firePoint.position, towerController.firePoint.rotation, transform) as SplashProjectile;
        spawnedProjectile.setValues(towerController.damage, towerController.projectileSpeed, currentTarget.transform, towerController.monsterLayerMask, splashRadius, towerController.damage, maxSplashTargetCount);
    }
}
