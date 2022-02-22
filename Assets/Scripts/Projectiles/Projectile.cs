using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected int _damage;
    protected float _speed;
    protected LayerMask _targetMask;
    protected int _maxMonsterHits = 1;
    protected List<int> _monstersHit = new List<int>();
    protected bool _homing = false;

    List<IOnHitModifier> _onHitModifiers = new List<IOnHitModifier>();
    List<IOnDestroyModifier> _onDestroyModifiers = new List<IOnDestroyModifier>();

    public Transform target { get; private set; }
    [SerializeField] float rotationSpeed = 20f;

    protected virtual void Update()
    {
        if (_homing) HomeToTarget();
        TravelForward();
    }

    protected virtual void TravelForward()
    {
        float distance = _speed * Time.deltaTime;
        transform.position = transform.position + transform.right * distance;
    }

    protected void HomeToTarget()
    {
        if (target == null)
        {
            InvokeRepeating("CheckRange", 0, 0.5f);
        }

        RotateTowardsTarget();
    }

    protected void RotateTowardsTarget()
    {
        if (target == null) return;

        Vector3 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }

    protected void CheckRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 5f, _targetMask);

        if (hit)
        {
            target = hit.transform;
            CancelInvoke();
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        // If we have a target and we've hit it
        // Or we don't have a target and we've hit something in our targetmask
        MonsterController monster = GetMonsterFromCollider(collision.collider);

        if (monster != null && (target == null || monster.gameObject == target.gameObject))
        {
            DealDamage(monster);
        }
    }

    public static MonsterController GetMonsterFromCollider(Collider2D collider) 
    {
        MonsterController monster = collider.gameObject.GetComponent<MonsterController>();
        bool isMonsterAndNotDead = monster != null && !monster.hasDied;

        return isMonsterAndNotDead ? monster : null;
    }

    protected virtual void DealDamage(MonsterController monster)
    {
        // If we've already hit the max amount we do nothing
        if (_monstersHit.Count >= _maxMonsterHits) return;

        int monsterInstanceId = monster.GetInstanceID();

        // If we've already hit the damageable we do nothing
        if (_monstersHit.Contains(monsterInstanceId)) return;

        monster.TakeDamage(_damage);

        // Execute all on hit modifiers
        foreach (IOnHitModifier onHitModifier in _onHitModifiers)
        {
            onHitModifier.Execute(monster);
        }

        _monstersHit.Add(monsterInstanceId);

        if (_monstersHit.Count >= _maxMonsterHits)
        {
            // Execute all on destroy modifiers
            foreach (IOnDestroyModifier onDestroyModifier in _onDestroyModifiers)
            {
                onDestroyModifier.Execute(transform.position);
            }
            Destroy(gameObject);
        }
    }

    public virtual void Setup(int damage, float speed, LayerMask targetMask, float timeToLive = 2f)
    {
        _damage = damage;
        _speed = speed;
        _targetMask = targetMask;

        SetTimeToLive(timeToLive);
    }

    void SetTimeToLive(float timeToLive)
    {
        Destroy(gameObject, timeToLive);
    }

    public void SetHoming(bool homing, Transform initialTarget = null)
    {
        _homing = homing;
        target = initialTarget;
    }

    public void SetMaxMonstersHit(int maxMonstersHit = 1)
    {
        _maxMonsterHits = maxMonstersHit;
    }

    public void ApplyOnDestroyModifier(IOnDestroyModifier modifier)
    {
        _onDestroyModifiers.Add(modifier);
    }

    public void ApplyOnHitModifier(IOnHitModifier modifier)
    {
        _onHitModifiers.Add(modifier);
    }
}
