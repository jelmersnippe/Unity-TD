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

    protected override void TravelForward()
    {
        float reducedSpeed = speed - (speed - lowestSpeed) * Mathf.Sqrt(Mathf.Min(timeAlive / timeToLowestSpeed, 1));
        float distance = reducedSpeed * Time.deltaTime;
        transform.position = transform.position + transform.right * distance;
    }

    public void setValues(int initialDamage, float initialSpeed, Transform initialTarget, LayerMask monsterLayerMask, float initialLowestSpeed, float initialTimeToLowestSpeed)
    {
        base.setValues(initialDamage, initialSpeed, initialTarget, monsterLayerMask);

        lowestSpeed = initialLowestSpeed;
        timeToLowestSpeed = initialTimeToLowestSpeed;
    }
}
