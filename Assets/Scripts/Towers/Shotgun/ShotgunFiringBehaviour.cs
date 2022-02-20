using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunFiringBehaviour : FiringBehaviour
{
    [SerializeField] int pelletsToFire = 6;
    [SerializeField] float spread = 45f;
    [SerializeField] float lowestSpeed = 0.5f;
    [SerializeField] float timeToLowestSpeed = 0.5f;

    bool concentratedPellets = false;

    protected override void SpawnProjectile()
    {
        AudioManager.instance.Play(Sound.Name.Shotgun);

        int pelletCount = concentratedPellets ? pelletsToFire / 2 : pelletsToFire;
        int damagePerPellet = concentratedPellets ? towerController.damage * 2 : towerController.damage;
        for (int i = 0; i < pelletCount; i++)
        {
            // Spawn a new projectile with a rotation based on the spread
            ShotgunProjectile spawnedProjectile = Instantiate(towerController.projectile, towerController.firePoint.position, Quaternion.Euler(towerController.firePoint.rotation.eulerAngles + (new Vector3(0, 0, 1) * Random.Range(-spread, spread))), transform) as ShotgunProjectile;

            // Set speed to a random value within a range so the shotgun pellets are spread out a bit
            // And don't provide a target because the shotgun should not be homign
            spawnedProjectile.setValues(damagePerPellet, towerController.projectileSpeed * Random.Range(0.8f, 1.2f), null, towerController.monsterLayerMask, lowestSpeed, timeToLowestSpeed);
        }
    }
}