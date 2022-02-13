using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GattlingTower : Tower
{
    [SerializeField]
    int consecutiveShots = 0;
    [SerializeField]
    int damagePerConsecutiveShot = 0;
    [SerializeField]
    int shotsToReachLowestTimeToFire = 6;
    [SerializeField]
    float lowestTimeToFire = 0.1f;
    [SerializeField]
    float unwindTime = 0.5f;

    float timeWithoutFiring = 0f;

    override protected void SeekingBehaviour()
    {
        // Reset consecutiveShots after x amount of not firing
        if (consecutiveShots > 0)
        {
            timeWithoutFiring += Time.deltaTime;

            if (timeWithoutFiring > unwindTime)
            {
                consecutiveShots = 0;
            }
        }

        base.SeekingBehaviour();
    }

    override protected void ResetTimeToFire()
    {
        consecutiveShots++;
        // The fire rate exponentially increases
        // Speeding up from the first shot to the last shot
        // Finishing at the lowest timeToFire possible after the required shots
        float calc = timeToFire - (timeToFire - lowestTimeToFire) * Mathf.Sqrt(Mathf.Min((float)consecutiveShots / (float)shotsToReachLowestTimeToFire, 1));
        currentTimeToFire = calc;
    }

    protected override void SpawnProjectile()
    {
        Projectile spawnedProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation, transform);
        spawnedProjectile.setValues(damage + (damagePerConsecutiveShot * consecutiveShots), projectileSpeed, currentTarget.transform, monsterLayerMask, null, null);
    }

    override public void ActivateUpgrade(Upgrade upgradeToActivate)
    {
        base.ActivateUpgrade(upgradeToActivate);

        switch (upgradeToActivate.type)
        {
            case "gattling_consecutive_damage_up":
                damagePerConsecutiveShot += 5;
                break;
            case "gattling_increased_max_firerate":
                lowestTimeToFire /= 2f;
                break;
            case "quicker_windup":
                shotsToReachLowestTimeToFire /= 2;
                break;
        }
    }
}
