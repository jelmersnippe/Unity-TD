using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileFiringBehaviour : FiringBehaviour
{
    [SerializeField] float splashRadius = 3f;
    [SerializeField] int maxSplashTargetCount = 4;

    protected override void SetupProjectile(Projectile projectile)
    {
        base.SetupProjectile(projectile);
        projectile.ApplyOnDestroyModifier(new SplashDamageModifier(splashRadius, towerController.damage, towerController.monsterLayerMask, maxSplashTargetCount));
    }
}
