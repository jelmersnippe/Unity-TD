using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gattling : Tower
{
    [SerializeField]
    int consecutiveShots = 0;
    [SerializeField]
    int shotsToReachLowestTimeToFire = 6;
    [SerializeField]
    float lowestTimeToFire = 0.1f;

    override protected void EnterSeekingState()
    {
        consecutiveShots = 0;
        base.EnterSeekingState();
    }

    override protected void ResetTimeToFire()
    {
        consecutiveShots++;
        // The fire rate exponentially increases
        // Speeding up from the first shot to the last shot
        // Finishing at the lowest timeToFire possible after the required shots
        float calc = timeToFire - (timeToFire - lowestTimeToFire) * Mathf.Sqrt(Mathf.Min((float)consecutiveShots / (float)shotsToReachLowestTimeToFire, 1));
        currentTimeToFire = calc;
    }
}
