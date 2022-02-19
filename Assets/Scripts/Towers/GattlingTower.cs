using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GattlingTower : TowerController
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
        float lowestTimeToFire = 60f / maxRoundsPerMinute;
        // The fire rate exponentially increases
        // Speeding up from the first shot to the last shot
        // Finishing at the lowest timeToFire possible after the required shots
        float calc = timeToFire - (timeToFire - lowestTimeToFire) * Mathf.Sqrt(Mathf.Min((float)consecutiveShots / (float)shotsToReachLowestTimeToFire, 1));
        currentTimeToFire = calc;
    }

    protected override void SpawnProjectile()
    {
        AudioManager.instance.Play(Sound.Name.Shoot);
        Projectile spawnedProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation, transform);
        spawnedProjectile.setValues(damage + (damagePerConsecutiveShot * consecutiveShots), projectileSpeed, currentTarget.transform, monsterLayerMask);
    }

    override public void ActivateUpgrade(Upgrade upgrade)
    {
        base.ActivateUpgrade(upgrade);

        switch (upgrade.type)
        {
            case Upgrade.Type.Gattling_ConsecutiveDamageUp:
                damagePerConsecutiveShot += 5;
                break;
            case Upgrade.Type.Gattling_MaxFireRateUp:
                maxRoundsPerMinute = 600;
                break;
            case Upgrade.Type.Gattling_QuickerWindup:
                shotsToReachLowestTimeToFire /= 2;
                break;
        }
    }
}
