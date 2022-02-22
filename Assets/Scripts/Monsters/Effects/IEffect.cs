using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    public float timeLeft {get; set;}

    void OnActivate(MonsterController monster);
    void Execute(MonsterController monster);
    void OnDeactivate(MonsterController monster);
}
