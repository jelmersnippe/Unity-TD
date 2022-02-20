using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(IdleBehaviour))]
[RequireComponent(typeof(FiringBehaviour))]
[RequireComponent(typeof(PlacingBehaviour))]
[RequireComponent(typeof(RangeIndicator))]
public class TowerController : MonoBehaviour
{
    public enum TowerState { Idle, Firing, Placing }

    public Transform towerGun;
    public Transform firePoint;
    public Sprite sprite;

    public int damage = 100;
    public int roundsPerMinute = 30;
    public int range = 3;
    public int cost = 100;

    [Range(1, 100)]
    public int projectileSpeed = 60;
    public Projectile projectile;

    // TOD: Set as a global reference somewhere
    [SerializeField]
    public LayerMask monsterLayerMask;

    public float timeToFire;

    [SerializeField]
    public TowerState currentTowerState = TowerState.Placing;

    IdleBehaviour idleBehaviour;
    FiringBehaviour firingBehaviour;
    PlacingBehaviour placingBehaviour;

    public enum DefaultUpgradeType
    {
        Default_DamageUp,
        Default_PierceUp,
        Default_FireRateUp,
        Basic_TripleShot,
    }

    private void Awake()
    {
        idleBehaviour = GetComponent<IdleBehaviour>();
        firingBehaviour = GetComponent<FiringBehaviour>();
        placingBehaviour = GetComponent<PlacingBehaviour>();
        sprite = GetComponent<SpriteRenderer>().sprite;
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
                placingBehaviour.Execute();
                return;
            case TowerState.Idle:
                idleBehaviour.Execute();
                break;
            case TowerState.Firing:
                firingBehaviour.Execute();
                break;
        }
    }

    public virtual void EnterIdleState()
    {
        currentTowerState = TowerState.Idle;
        idleBehaviour.OnEnter();
    }

    public virtual void EnterFiringState(Transform newTarget)
    {
        currentTowerState = TowerState.Firing;
        firingBehaviour.OnEnter(newTarget);
    }

    public void ShowRange(bool showRange)
    {
        GetComponent<RangeIndicator>().rangeIndicator.gameObject.SetActive(showRange);
    }
}
