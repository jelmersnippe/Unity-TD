using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    public float timeLeft {get; set;}

    bool OnActivate(MonsterController monster, List<IEffect> activeEffects);
    void Execute(MonsterController monster);
    void OnDeactivate(MonsterController monster);
}
