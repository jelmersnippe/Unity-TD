using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Tower
{
    [SerializeField]
    int pelletsToFire = 6;
    [SerializeField]
    float spread = 45f;
    [SerializeField]
    float lowestSpeed = 0.5f;
    [SerializeField]
    float timeToLowestSpeed = 0.5f;

    protected override void SpawnProjectile()
    {
        for (int i = 0; i < pelletsToFire; i++)
        {
            // Spawn a new projectile with a rotation based on the spread
            Projectile spawnedProjectile = Instantiate(projectile, firePoint.position, Quaternion.Euler(firePoint.rotation.eulerAngles + (new Vector3(0, 0, 1) * Random.Range(-spread, spread))), transform);

            // Set speed to a random value within a range so the shotgun pellets are spread out a bit
            // And don't provide a target because the shotgun should not be homign
            spawnedProjectile.setValues(damage, projectileSpeed * Random.Range(0.8f, 1.2f), null, monsterLayerMask, lowestSpeed, timeToLowestSpeed);
        }
    }
}
