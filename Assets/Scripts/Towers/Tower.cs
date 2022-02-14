using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum TowerState { Idle, Firing, Placing }

    [SerializeField]
    protected Transform towerGun;
    [SerializeField]
    protected Transform firePoint;

    [SerializeField]
    public Sprite uiSprite;

    public int damage = 100;
    public int roundsPerMinute = 30;
    public int range = 5;
    public int cost = 100;

    [SerializeField]
    [Range(1, 100)]
    protected int projectileSpeed = 60;

    [SerializeField]
    protected Projectile projectile;

    [SerializeField]
    protected LayerMask monsterLayerMask;

    protected float timeToFire;
    protected float currentTimeToFire = 0;

    protected Transform currentTarget;
    [SerializeField]
    protected TowerState currentTowerState = TowerState.Idle;

    public List<Upgrade> upgrades = new List<Upgrade>();
    public List<Upgrade> unlockedUpgrades = new List<Upgrade>();

    public virtual void ActivateUpgrade(Upgrade upgradeToActivate)
    {
        if (unlockedUpgrades.Contains(upgradeToActivate))
        {
            Debug.Log("Can only upgrade once");
            return;
        }

        switch (upgradeToActivate.type)
        {
            case "default_damage_up":
                damage += 50;
                break;
            case "default_firerate_up":
                roundsPerMinute += 30;
                break;
        }
        unlockedUpgrades.Add(upgradeToActivate);
        GameManager.instance.purchaseCurrency -= upgradeToActivate.cost;
    }

    private void Start()
    {
        timeToFire = (float)60 / (float)roundsPerMinute;
    }

    void Update()
    {
        switch (currentTowerState)
        {
            case TowerState.Placing:
                return;
            case TowerState.Idle:
                SeekingBehaviour();
                break;
            case TowerState.Firing:
                FiringBehaviour();
                break;
        }

        currentTimeToFire -= Time.deltaTime;
    }

    protected virtual void SeekingBehaviour()
    {
    }

    void FiringBehaviour()
    {
        if (currentTarget == null || isTargetOutOfRange())
        {
            EnterIdleState();
            return;
        }

        RotateToTarget();
        FireProjectile();
    }

    public virtual void EnterIdleState()
    {
        currentTarget = null;
        InvokeRepeating("CheckRange", 0, 0.5f);
        currentTowerState = TowerState.Idle;
    }

    protected virtual void EnterFiringState(Transform newTarget)
    {
        CancelInvoke();
        currentTarget = newTarget;
        currentTowerState = TowerState.Firing;
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
        spawnedProjectile.setValues(damage, projectileSpeed, currentTarget.transform, monsterLayerMask);
    }

    protected virtual void ResetTimeToFire()
    {
        currentTimeToFire = timeToFire;
    }

    public void ShowRange(bool showRange)
    {
        GetComponent<RangeIndicator>().rangeIndicator.gameObject.SetActive(showRange);
    }

    public void ConvertToActive()
    {
        // Disable the placeholder script and object
        Placeholder placeholder = GetComponent<Placeholder>();
        placeholder.enabled = false;
        placeholder.placeholderObject.gameObject.SetActive(false);

        // Enable the tower and object
        placeholder.activeObject.gameObject.SetActive(true);
        EnterIdleState();
    }
}
