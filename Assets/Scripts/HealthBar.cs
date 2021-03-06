using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    Transform bar;

    public void SetHealthPercentage(float percentage)
    {
        bar.localScale = new Vector2(percentage, bar.localScale.y);
    }
}
