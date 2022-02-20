using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GattlingIdleBahaviour : IdleBehaviour
{
    override public void Execute()
    {
        // Reset consecutiveShots after x amount of not firing
        //if (gattlingTowerController.gattlingFiringBehaviour.consecutiveShots > 0)
        //{
        //    timeWithoutFiring += Time.deltaTime;

        //    if (timeWithoutFiring > unwindTime)
        //    {
        //        consecutiveShots = 0;
        //    }
        //}

        base.Execute();
    }
}
