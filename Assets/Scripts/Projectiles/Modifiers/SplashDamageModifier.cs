using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDamageModifier : IOnDestroyModifier
{
    float radius;
    int damage;
    LayerMask targetMask;
    int maxTargetCount;

    int targetsHit = 0;

    public SplashDamageModifier(float radius, int damage, LayerMask targetMask, int maxTargetCount)
    {
        this.radius = radius;
        this.damage = damage;
        this.targetMask = targetMask;
        this.maxTargetCount = maxTargetCount;
    }

    public void Execute(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius, targetMask);

        AudioManager.instance.Play(Sound.Name.Explosion);

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];

            MonsterController monster = Projectile.GetMonsterFromCollider(collider);
            if (monster != null)
            {
                DealSplashDamage(monster);
            }

            if (targetsHit >= maxTargetCount)
            {
                break;
            }
        }
    }

    void DealSplashDamage(MonsterController monster)
    {
        monster.TakeDamage(damage);
        targetsHit++;
    }
}
