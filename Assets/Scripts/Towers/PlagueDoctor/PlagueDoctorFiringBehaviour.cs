using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueDoctorFiringBehaviour : FiringBehaviour
{
    protected override void SetupProjectile(Projectile projectile)
    {
        base.SetupProjectile(projectile);
        projectile.ApplyOnHitModifier(new SlowModifier(10f, 1f));
    }
}
