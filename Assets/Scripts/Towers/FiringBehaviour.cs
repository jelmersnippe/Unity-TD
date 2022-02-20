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
        FireProjectile();
    }

    bool isTargetOutOfRange()
    {
        return Vector2.Distance(transform.position, currentTarget.position) > towerController.range;
    }

    void RotateToMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        towerController.towerGun.rotation = desiredRotation;
    }

    void RotateToTarget()
    {
        transform.rotation = Quaternion.Euler(0, currentTarget.transform.position.x < transform.position.x ? -180 : 0, 0);
        return;
        Vector3 targetDirection = currentTarget.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        towerController.towerGun.rotation = desiredRotation;
    }

    protected virtual void FireProjectile()
    {
        if (currentTimeToFire > 0)
        {
            return;
        }

        SpawnProjectile();
        ResetTimeToFire();
    }

    protected virtual void SpawnProjectile()
    {
        Projectile spawnedProjectile = Instantiate(towerController.projectile, towerController.firePoint.position, towerController.firePoint.rotation, transform);
        spawnedProjectile.setValues(towerController.damage, towerController.projectileSpeed, currentTarget.transform, towerController.monsterLayerMask);
    }

    protected virtual void ResetTimeToFire()
    {
        currentTimeToFire = towerController.timeToFire;
    }
}
