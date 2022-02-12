using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum State { Seeking, Firing, Testing }

    [SerializeField]
    protected Transform towerGun;
    [SerializeField]
    protected Transform firePoint;

    [SerializeField]
    public Sprite uiSprite;

    public int damage= 100;
    public int roundsPerMinute = 30;
    public int range = 5;

    [SerializeField]
    [Range(1, 100)]
    protected int projectileSpeed = 60;

    [SerializeField]
    protected Projectile projectile;

    [SerializeField]
    protected LayerMask monsterLayerMask;

    protected float timeToFire;
    [SerializeField]
    protected float currentTimeToFire = 0;

    protected Transform currentTarget;
    [SerializeField]
    protected State currentState = State.Seeking;

    public bool isTesting = true;

    private void Start()
    {
        timeToFire = (float)60 / (float)roundsPerMinute;

        EnterSeekingState();

        if (isTesting)
        {
            EnterTestingState();
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
            case State.Testing:
                TestingBehavior();
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

    void TestingBehavior()
    {
        RotateToMouse();
        
        if (Input.GetMouseButton(0))
        {
            FireProjectile();
        }
    }

    void EnterTestingState()
    {
        CancelInvoke();
        currentState = State.Testing;
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

    void RotateToMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        towerGun.rotation = desiredRotation;
    }

    void RotateToTarget()
    {
        Vector3 targetDirection = currentTarget.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        towerGun.rotation = desiredRotation;
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
        Projectile spawnedProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation, transform);
        spawnedProjectile.setValues(damage, projectileSpeed, currentTarget ? currentTarget.transform : null, monsterLayerMask, null, null);
    }

    protected virtual void ResetTimeToFire()
    {
        currentTimeToFire = timeToFire;
    }

    public void ShowRange(bool showRange)
    {
        GetComponent<RangeIndicator>().rangeIndicator.gameObject.SetActive(showRange);
    }
}
