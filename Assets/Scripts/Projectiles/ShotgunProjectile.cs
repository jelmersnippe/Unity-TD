using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunProjectile : Projectile
{
    float lowestSpeed;
    float timeToLowestSpeed;
    float timeAlive;

    protected override void Update()
    {
        timeAlive += Time.deltaTime;
        base.Update();
    }

    protected override void TravelTowardsTarget()
    {
        float reducedSpeed = speed - (speed - lowestSpeed) * Mathf.Sqrt(Mathf.Min(timeAlive / timeToLowestSpeed, 1));
        float distance = reducedSpeed * Time.deltaTime;
        transform.position = target ? Vector2.MoveTowards(transform.position, target.position, distance) : transform.position + transform.right * distance;
    }

    public void setValues(int initialDamage, float initialSpeed, Transform initialTarget, LayerMask monsterLayerMask, float initialLowestSpeed, float initialTimeToLowestSpeed)
    {
        base.setValues(initialDamage, initialSpeed, initialTarget, monsterLayerMask);

        lowestSpeed = initialLowestSpeed;
        timeToLowestSpeed = initialTimeToLowestSpeed;
    }
}
