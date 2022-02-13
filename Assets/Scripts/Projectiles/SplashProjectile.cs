using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashProjectile : Projectile
{
    float splashRadius;
    int splashDamage;
    int maxTargetCount;
    int targetsHit = 0;

    public void setValues(int initialDamage, float initialSpeed, Transform initialTarget, LayerMask monsterLayerMask, float initialSplashRadius, int initialSplashDamage, int initialMaxTargetCount)
    {
        base.setValues(initialDamage, initialSpeed, initialTarget, monsterLayerMask);

        splashRadius = initialSplashRadius;
        splashDamage = initialSplashDamage;
        maxTargetCount = initialMaxTargetCount;
    }

    protected override void DealDamage(Damageable target)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, splashRadius, targetMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];

            Damageable damageable = GetDamageableFromCollider(collider);
            if (damageable != null)
            {
                DealSplashDamage(damageable);
            }

            if (targetsHit >= maxTargetCount)
            {
                break;
            }
        }

        Destroy(gameObject);
    }

    void DealSplashDamage(Damageable damageable)
    {
        damageable.TakeDamage(splashDamage);
        targetsHit++;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}
