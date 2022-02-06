using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum State { Seeking, Firing }

    [SerializeField]
    private Transform towerGun;
    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    [Range(1,10)]
    private int damage = 2;

    [SerializeField]
    [Range(20, 3000)]
    private int roundsPerMinute = 20;

    [SerializeField]
    [Range(10, 50)]
    private int range = 25;

    [SerializeField]
    [Range(1, 100)]
    private int projectileSpeed = 1;

    [SerializeField]
    private Projectile projectile;

    private float timeToFire = 0;

    protected Transform currentTarget;
    protected State currentState = State.Seeking;

    public int projectilesFired = 0;

    private void Start()
    {
        EnterSeekingState();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Seeking:
                SeekingBehaviour();
                break;
            case State.Firing:
                FiringBehaviour();
                break;
        }
    }

    void SeekingBehaviour()
    {

    }

    void FiringBehaviour()
    {
        if (currentTarget == null || isTargetOutOfRange())
        {
            EnterSeekingState();
            return;
        }

        RotateToTarget();
        FireProjectile();
    }

    protected virtual void EnterSeekingState()
    {
        currentTarget = null;
        InvokeRepeating("CheckRange", 0, 0.5f);
        currentState = State.Seeking;
    }

    protected virtual void EnterFiringState(Transform newTarget)
    {
        CancelInvoke();
        currentTarget = newTarget;
        currentState = State.Firing;
    }

    protected virtual void CheckRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, range);

        if (hit)
        {
            EnterFiringState(hit.transform);
        }
    }

    bool isTargetOutOfRange()
    {
        return Vector2.Distance(transform.position, currentTarget.position) > range;
    }

    void RotateToTarget()
    {
        Vector3 targetDirection = currentTarget.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        towerGun.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void FireProjectile()
    {
        if (timeToFire > 0)
        {
            timeToFire -= Time.deltaTime;
            return;
        }

        Projectile spawnedProjectile = Instantiate(projectile, firePoint.position, towerGun.rotation, transform);
        spawnedProjectile.setValues(damage, projectileSpeed, currentTarget.transform);
        timeToFire = ((float)60 / (float)roundsPerMinute);
        projectilesFired++;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
