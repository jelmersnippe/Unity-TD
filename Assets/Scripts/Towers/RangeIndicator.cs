using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowerController))]
public class RangeIndicator : MonoBehaviour
{
    [SerializeField] Transform rangeIndicator;
    TowerController towerController;

    private void Awake()
    {
        towerController = GetComponent<TowerController>();
        SetRange(towerController.range);
    }

    public void SetRange(float range)
    {
        float scale = range * 2f;
        rangeIndicator.localScale = new Vector3(scale, scale, 0);
    } 

    public void SetActive(bool active)
    {
        rangeIndicator.gameObject.SetActive(active);
    }

    void OnDrawGizmosSelected()
    {
        if (towerController == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, towerController.range);
    }
}
