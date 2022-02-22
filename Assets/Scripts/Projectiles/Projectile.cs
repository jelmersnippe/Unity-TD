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

    List<OnHitModifier> _onHitModifiers = new List<OnHitModifier>();
    List<OnDestroyModifier> _onDestroyModifiers = new List<OnDestroyModifier>();

    public Transform target { get; private set; }
    [SerializeField] float rotationSpeed = 10f;

    float timeToLive = 2f;

    protected void Start()
    {
        Destroy(gameObject, timeToLive);
    }

    protected virtual void Update()
    {
        RotateTowardsTarget();
        TravelForward();
    }

    protected virtual void TravelForward()
    {
        float distance = _speed * Time.deltaTime;
        transform.position = transform.position + transform.right * distance;
    }

    protected void RotateTowardsTarget()
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
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
        foreach (OnHitModifier onHitModifier in _onHitModifiers)
        {
            onHitModifier.Execute(monster);
        }

        _monstersHit.Add(monsterInstanceId);

        if (_monstersHit.Count >= _maxMonsterHits)
        {
            // Execute all on destroy modifiers
            foreach (OnDestroyModifier onDestroyModifier in _onDestroyModifiers)
            {
                onDestroyModifier.Execute(transform.position);
            }
            Destroy(gameObject);
        }
    }

    public virtual void Setup(int damage, float speed, LayerMask targetMask)
    {
        _damage = damage;
        _speed = speed;
        _targetMask = targetMask;
    }

    public void SetMaxMonstersHit(int maxMonstersHit = 1)
    {
        _maxMonsterHits = maxMonstersHit;
    }

    public void ApplyOnDestroyModifier(OnDestroyModifier modifier)
    {
        _onDestroyModifiers.Add(modifier);
    }

    public void ApplyOnHitModifier(OnHitModifier modifier)
    {
        _onHitModifiers.Add(modifier);
    }
}
