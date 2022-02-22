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

    protected override void FireProjectile()
    {
        AudioManager.instance.Play(Sound.Name.Shotgun);

        for (int i = 0; i < pelletsToFire; i++)
        {
            // Spawn a new projectile with a rotation based on the spread
            base.SpawnProjectile(1 * Random.Range(-spread, spread));
        }
    }
}
