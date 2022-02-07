using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedTower : MonoBehaviour
{
    public enum State { Seeking, Firing }

    [SerializeField]
    private Transform towerGun;
    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    [Range(1, 10)]
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

    [SerializeField]
    LayerMask[] monsterLayerMasks;
    LayerMask monsterLayerMask;

    float timeToFire;
    [SerializeField]
    private float currentTimeToFire = 0;

    protected Transform currentTarget;
    protected State currentState = State.Seeking;

    [SerializeField]
    int firingAnimationFrames;

    [SerializeField]
    float firingAnimationTime;

    [SerializeField]
    bool willFire = false;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        firingAnimationTime = (float)firingAnimationFrames / (float)30;

        timeToFire = (float)60 / (float)roundsPerMinute;
        currentTimeToFire = timeToFire;

        if (timeToFire <= firingAnimationTime)
        {
            Debug.LogError(gameObject.name + " has higher fire rate than it's animation can handle");
        }

        EnterSeekingState();

        if (monsterLayerMasks.Length > 0)
        {
            monsterLayerMask = monsterLayerMasks[0];

            for (int i = 1; i < monsterLayerMasks.Length; i++)
            {
                monsterLayerMask |= monsterLayerMasks[i];
            }
        }
    }

    void Update()
    {
        currentTimeToFire -= Time.deltaTime;

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
        if (currentTarget == null || (!willFire && isTargetOutOfRange() && !willFire))
        {
            EnterSeekingState();
            return;
        }

        RotateToTarget();
        AttemptToFireProjectile();
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
        if (currentTimeToFire < firingAnimationTime)
        {
            currentTimeToFire = firingAnimationTime;
        }
        currentState = State.Firing;
    }

    protected virtual void CheckRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, range, monsterLayerMask);

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

    void AttemptToFireProjectile()
    {
        if (currentTarget != null && !willFire && currentTimeToFire < firingAnimationTime)
        {
            willFire = true;
            animator.SetBool("WillFire", true);
        }
    }

    void FireProjectile()
    {
        willFire = false;
        animator.SetBool("WillFire", false);
        currentTimeToFire = (float)60 / (float)roundsPerMinute;

        Projectile spawnedProjectile = Instantiate(projectile, firePoint.position, towerGun.rotation, transform);
        spawnedProjectile.setValues(damage, projectileSpeed, currentTarget.transform);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
