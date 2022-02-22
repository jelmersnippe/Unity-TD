using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : IEffect
{
    float _totalDuration;
    float _timeLeft;
    float _speedReduction;

    Color _initialSpriteColor;

    public SlowEffect(float duration, float speedReduction)
    {
        _totalDuration = duration;
        _timeLeft = duration;
        _speedReduction = speedReduction;
    }

    public float timeLeft { get => _timeLeft; set => _timeLeft = value; }

    public void Execute(MonsterController target)
    {
        timeLeft -= Time.deltaTime;
    }

    public void OnActivate(MonsterController monster)
    {
        _initialSpriteColor = monster.GetComponent<SpriteRenderer>().color;
        Color tempColor = _initialSpriteColor;
        tempColor.a = 0.8f;
        monster.GetComponent<SpriteRenderer>().color = tempColor;
        monster.speedMultiplier = Mathf.Max(monster.speedMultiplier - _speedReduction, 0.1f);
    }

    public void OnDeactivate(MonsterController monster)
    {
        monster.speedMultiplier += _speedReduction;
        monster.GetComponent<SpriteRenderer>().color = _initialSpriteColor;
    }
}
