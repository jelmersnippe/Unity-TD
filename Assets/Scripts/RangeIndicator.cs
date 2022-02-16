using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class RangeIndicator : MonoBehaviour
{
    public Transform rangeIndicator;
    Tower tower;

    private void Awake()
    {
        tower = GetComponent<Tower>();
        SetRange(tower.range);
    }

    public void SetRange(float range)
    {
        float scale = range * 2f;
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
