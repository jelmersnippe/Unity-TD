using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowerController))]
public class IdleBehaviour : MonoBehaviour
{
    protected TowerController towerController;

    private void Awake()
    {
        towerController = GetComponent<TowerController>();
    }

    public void OnEnter()
    {
        InvokeRepeating("CheckRange", 0, 0.5f);
    }

    public void OnExit()
    {
        CancelInvoke();
    }

    public virtual void Execute()
    {
    }

    protected virtual void CheckRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, towerController.range, towerController.monsterLayerMask);

        if (hit)
        {
            OnExit();
            towerController.EnterFiringState(hit.transform);
        }
    }
}
