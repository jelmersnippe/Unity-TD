using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowerController))]
public class FiringBehaviour : MonoBehaviour
{
    protected TowerController towerController;

    protected float currentTimeToFire = 0;
    protected Transform currentTarget;

    private void Awake()
    {
        towerController = GetComponent<TowerController>();
    }

    public void OnEnter(Transform target)
    {
        currentTarget = target;
    }

    public void OnExit()
    {
        CancelInvoke();
    }

    public virtual void Execute()
    {
        currentTimeToFire -= Time.deltaTime;

        if (currentTarget == null || isTargetOutOfRange())
        {
            towerController.EnterIdleState();
            return;
        }

        RotateToTarget();

        if (currentTimeToFire <= 0)
        {
            FireProjectile();
            ResetTimeToFire();
        }
    }

    bool isTargetOutOfRange()
    {
        return Vector2.Distance(transform.position, currentTarget.position) > towerController.range;
    }

    void RotateToTarget()
    {
        transform.rotation = Quaternion.Euler(0, currentTarget.transform.position.x < transform.position.x ? -180 : 0, 0);

        Vector3 targetDirection = currentTarget.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        towerController.firePoint.rotation = desiredRotation;
    }

    protected virtual void FireProjectile()
    {
        SpawnProjectile();
    }

    protected virtual void SpawnProjectile(float offset = 0)
    {
        Projectile spawnedProjectile = Instantiate(towerController.projectileBlueprint.prefab, towerController.firePoint.position, Quaternion.Euler(towerController.firePoint.rotation.eulerAngles + new Vector3(0, 0, offset)));
        SetupProjectile(spawnedProjectile);
    }

    protected virtual void SetupProjectile(Projectile projectile)
    {
        projectile.Setup(towerController.projectileBlueprint.sprite, towerController.damage, towerController.projectileBlueprint.speed, towerController.monsterLayerMask);
        projectile.SetHoming(towerController.projectileBlueprint.homing, currentTarget);
    }

    protected virtual void ResetTimeToFire()
    {
        currentTimeToFire = towerController.timeToFire;
        towerController.EnterIdleState();
    }
}
