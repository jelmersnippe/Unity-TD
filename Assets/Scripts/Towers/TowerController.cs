using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(IdleBehaviour))]
[RequireComponent(typeof(FiringBehaviour))]
[RequireComponent(typeof(PlacingBehaviour))]
[RequireComponent(typeof(RangeIndicator))]
[RequireComponent(typeof(UpgradeTree))]
public class TowerController : MonoBehaviour
{
    public enum TowerState { Idle, Firing, Placing }

    [HideInInspector] public SpriteRenderer towerGunSpriteRenderer;

    public Transform towerGun;
    public Transform firePoint;

    // TODO: struct
    public int damage = 100;
    public int roundsPerMinute = 45;
    public float range = 3f;
    public int cost = 100;

    public ProjectileBlueprint projectileBlueprint;

    public LayerMask monsterLayerMask;
    public float timeToFire { get; private set; }

    public TowerState currentTowerState { get; private set; } = TowerState.Placing;

    IdleBehaviour idleBehaviour;
    FiringBehaviour firingBehaviour;
    PlacingBehaviour placingBehaviour;
    RangeIndicator rangeIndicator;
    UpgradeTree upgradeTree;

    private void Awake()
    {
        towerGunSpriteRenderer = towerGun.GetComponent<SpriteRenderer>();

        idleBehaviour = GetComponent<IdleBehaviour>();
        firingBehaviour = GetComponent<FiringBehaviour>();
        placingBehaviour = GetComponent<PlacingBehaviour>();
        rangeIndicator = GetComponent<RangeIndicator>();
        upgradeTree = GetComponent<UpgradeTree>();
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

    private void OnMouseDown()
    {
        BuildingManager.instance.AttemptToSetSelectedTower(this);
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
        rangeIndicator.SetActive(showRange);
    }

    public Dictionary<int, List<UpgradeTreeItem>> GetUpgrades()
    {
        return upgradeTree.upgrades;
    }

    public void ActivateUpgrade(UpgradeTreeItem upgrade)
    {
        upgradeTree.ActivateUpgrade(upgrade);
    }

    public bool HasUnlockedUpgrade(UpgradeType upgrade)
    {
        return upgradeTree.HasUnlockedUpgrade(upgrade);
    }
}
