using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    public Transform rangeIndicator;

    Tower tower;

    private void Awake()
    {
        tower = GetComponent<Tower>();
    }

    private void OnEnable()
    {
        int scale = tower.range * 2;
        rangeIndicator.localScale = new Vector3(scale, scale, 0);
    }

    void OnDrawGizmosSelected()
    {
        if (tower == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tower.range);
    }
}
