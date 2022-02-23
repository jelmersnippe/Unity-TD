using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : IEffect
{
    float _totalDuration;
    public float _timeLeft { get; private set; }
    public float timeLeft { get => _timeLeft; set => _timeLeft = value; }

    public float speedReduction { get; private set; }

    public SlowEffect(float duration, float slow)
    {
        _totalDuration = duration;
        timeLeft = duration;
        speedReduction = slow;
    }

    public void Execute(MonsterController target)
    {
        timeLeft -= Time.deltaTime;
    }

    public bool OnActivate(MonsterController monster, List<IEffect> activeEffects)
    {
        SlowEffect activeSlowEffect = activeEffects.Find((effect) => effect.GetType() == this.GetType()) as SlowEffect;

        // No slow effect yet, apply new effect
        if (activeSlowEffect == null)
        {
            monster.ChangeSpeedModifier(-speedReduction);
            return true;
        }

        // Active slow is stronger than new slow, ignore this new effect
        if (activeSlowEffect.speedReduction > speedReduction)
        {
            return false;
        }
        // Active slow is equal, refresh duration
        if (activeSlowEffect.speedReduction == speedReduction)
        {
            activeSlowEffect.timeLeft = _totalDuration;
            return false;
        }

        // Active slow is weaker, remove effect and apply new
        monster.RemoveEffect(activeSlowEffect);
        monster.ChangeSpeedModifier(-speedReduction);

        return true;
    }

    public void OnDeactivate(MonsterController monster)
    {
        monster.ChangeSpeedModifier(+speedReduction);
    }
}
