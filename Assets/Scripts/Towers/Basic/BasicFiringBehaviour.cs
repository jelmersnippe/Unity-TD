using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFiringBehaviour : FiringBehaviour
{
    [SerializeField] float degreesBetweenShots = 45f;
    bool hasTripleShot = false;
    bool hasPierce = false;

    protected override void SpawnProjectile()
    {
        AudioManager.instance.Play(Sound.Name.Shoot);

        if (!hasTripleShot && !hasPierce)
        {
            base.SpawnProjectile();
            return;
        }

        int shotCount = hasTripleShot ? 3 : 1;
        for (int i = 0; i < shotCount; i++)
        {
            // Spawn a new projectile with a rotation based on the space between shots and which shot it is so we get an even distribution with one centered shot
            Projectile spawnedProjectile = Instantiate(
                towerController.projectile,
                towerController.firePoint.position,
                hasTripleShot
                    ? Quaternion.Euler(towerController.firePoint.rotation.eulerAngles - new Vector3(0, 0, degreesBetweenShots) + (new Vector3(0, 0, degreesBetweenShots) * i))
                    : Quaternion.Euler(towerController.firePoint.rotation.eulerAngles),
                transform);
            spawnedProjectile.setValues(towerController.damage, towerController.projectileSpeed, null, towerController.monsterLayerMask, hasPierce ? 2 : 1);
        }
    }
}
