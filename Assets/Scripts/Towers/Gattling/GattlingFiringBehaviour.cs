using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GattlingFiringBehaviour : FiringBehaviour
{
    [SerializeField]
    int consecutiveShots = 0;
    [SerializeField]
    int damagePerConsecutiveShot = 0;
    [SerializeField]
    int shotsToReachLowestTimeToFire = 6;
    [SerializeField]
    int maxRoundsPerMinute = 450;
    [SerializeField]
    float unwindTime = 0.5f;

    float timeWithoutFiring = 0f;

    override protected void ResetTimeToFire()
    {
        consecutiveShots++;
        float lowestTimeToFire = 60f / maxRoundsPerMinute;
        // The fire rate exponentially increases
        // Speeding up from the first shot to the last shot
        // Finishing at the lowest timeToFire possible after the required shots
        float calc = towerController.timeToFire - (towerController.timeToFire - lowestTimeToFire) * Mathf.Sqrt(Mathf.Min((float)consecutiveShots / (float)shotsToReachLowestTimeToFire, 1));
        currentTimeToFire = calc;
    }

    protected override void SpawnProjectile()
    {
        AudioManager.instance.Play(Sound.Name.Shoot);
        Projectile spawnedProjectile = Instantiate(towerController.projectile, towerController.firePoint.position, towerController.firePoint.rotation, transform);
        spawnedProjectile.setValues(towerController.damage + (damagePerConsecutiveShot * consecutiveShots), towerController.projectileSpeed, currentTarget.transform, towerController.monsterLayerMask);
    }
}
