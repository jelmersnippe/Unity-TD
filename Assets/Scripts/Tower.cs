using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum State { Seeking, Firing, Testing }

    [SerializeField]
    private Transform towerGun;
    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    public Sprite uiSprite;

    [SerializeField]
    public int damage = 2;

    [SerializeField]
    public int roundsPerMinute = 20;

    [SerializeField]
    public int range = 25;

    [SerializeField]
    [Range(1, 100)]
    private int projectileSpeed = 1;

    [Min(0)]
    public int cost = 10;

    [SerializeField]
    private Projectile projectile;

    [SerializeField]
    LayerMask monsterLayerMask;

    float timeToFire;
    [SerializeField]
    private float currentTimeToFire = 0;

    protected Transform currentTarget;
    [SerializeField]
    protected State currentState = State.Seeking;

    [SerializeField]
    bool hasGattlingUnlocked = false;
    [SerializeField]
    int consecutiveShots = 0;
    [SerializeField]
    int shotsToReachLowestTimeToFire = 6;
    [SerializeField]
    float lowestTimeToFire = 0.1f;

    [SerializeField]
    bool hasSpreadShotUnlocked = false;
    [SerializeField]
    int pelletsToFire = 6;
    [SerializeField]
    float spread = 3f;
    [SerializeField]
    float lowestSpeed = 0.5f;
    [SerializeField]
    float timeToLowestSpeed = 0.5f;

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
        consecutiveShots = 0;
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

    void FireProjectile()
    {
        if (currentTimeToFire > 0)
        {
            return;
        }

        if (hasSpreadShotUnlocked)
        {
            for (int i = 0; i < pelletsToFire; i++)
            {
                Projectile spawnedProjectile = Instantiate(projectile, firePoint.position, Quaternion.Euler(firePoint.rotation.eulerAngles + (new Vector3(0,0,1) * Random.Range(-spread, spread))), transform);
                spawnedProjectile.setValues(damage, projectileSpeed * Random.Range(0.8f, 1.2f), currentTarget ? currentTarget.transform : null, monsterLayerMask, lowestSpeed, timeToLowestSpeed);
            }
        } 
        else
        {
            Projectile spawnedProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation, transform);
            spawnedProjectile.setValues(damage, projectileSpeed, currentTarget ? currentTarget.transform : null, monsterLayerMask, null, null);
        }

        if (hasGattlingUnlocked)
        {
            consecutiveShots++;
            // The fire rate exponentially increases
            // Speeding up from the first shot to the last shot
            // Finishing at the lowest timeToFire possible after the required shots
            float calc = timeToFire - (timeToFire - lowestTimeToFire) * Mathf.Sqrt(Mathf.Min((float)consecutiveShots / (float)shotsToReachLowestTimeToFire, 1));
            currentTimeToFire = calc;
        } 
        else
        {
            currentTimeToFire = timeToFire;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
