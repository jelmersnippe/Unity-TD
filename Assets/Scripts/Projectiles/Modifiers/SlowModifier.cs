using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowModifier : IOnHitModifier
{
    float _duration;
    float _speedReduction;

    public SlowModifier(float duration, float speedReduction)
    {
        _duration = duration;
        _speedReduction = speedReduction;
    }

    public void Execute(MonsterController target)
    {
        target.ApplyEffect(new SlowEffect(_duration, _speedReduction));
    }
}
