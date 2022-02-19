using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
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
                projectile, 
                firePoint.position,
                hasTripleShot 
                    ? Quaternion.Euler(firePoint.rotation.eulerAngles - new Vector3(0, 0, degreesBetweenShots) + (new Vector3(0, 0, degreesBetweenShots) * i))
                    : Quaternion.Euler(firePoint.rotation.eulerAngles),
                transform);
            spawnedProjectile.setValues(damage, projectileSpeed, null, monsterLayerMask, hasPierce ? 2 : 1);
        }
    }

    override public void ActivateUpgrade(Upgrade upgradeToActivate)
    {
        base.ActivateUpgrade(upgradeToActivate);

        switch (upgradeToActivate.type)
        {
            case "basic_triple_shot":
                hasTripleShot = true;
                roundsPerMinute -= 15;
                break;
            case "basic_pierce_up":
                hasPierce = true;
                break;
        }
    }
}
