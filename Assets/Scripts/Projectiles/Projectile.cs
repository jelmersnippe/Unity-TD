using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected int damage;
    protected float speed;
    protected LayerMask targetMask;
    protected int maxMonsterHits = 1;
    protected List<int> monstersHit = new List<int>();
    [SerializeField] protected float rotationSpeed = 10f;
    protected Transform target;

    float timeToLive = 2f;
    bool isDestroyed = false;

    protected void Start()
    {
        Destroy(gameObject, timeToLive);
    }

    protected virtual void Update()
    {
        RotateTowardsTarget();
        TravelForward();
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

    protected virtual void TravelForward()
    {
        float distance = speed * Time.deltaTime;
        transform.position = transform.position + transform.right * distance;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        // If we have a target and we've hit it
        // Or we don't have a target and we've hit something in our targetmask
        Damageable damageable = GetDamageableFromCollider(collision.collider);

        if (damageable != null && (target == null || damageable.gameObject == target.gameObject))
        {
            DealDamage(damageable);
        }
    }

    protected Damageable GetDamageableFromCollider(Collider2D collider) 
    {
        bool isValidTarget = ((1 << collider.gameObject.layer) & targetMask) != 0;
        if (!isValidTarget)
        {
            return null;
        }

        Damageable damageable = collider.gameObject.GetComponent<Damageable>();
        bool isDamageableAndNotDead = damageable != null && !damageable.hasDied;

        return isDamageableAndNotDead ? damageable : null;
    }

    protected virtual void DealDamage(Damageable damageable)
    {
        // If we've already hit the max amount we do nothing
        if (monstersHit.Count >= maxMonsterHits) return;

        int damageableInstanceId = damageable.GetInstanceID();

        // If we've already hit the damageable we do nothing
        if (monstersHit.Contains(damageableInstanceId)) return;

        damageable.TakeDamage(damage);
        monstersHit.Add(damageableInstanceId);

        if (monstersHit.Count >= maxMonsterHits)
        {
            Destroy(gameObject);
        }
    }

    public virtual void setValues(int initialDamage, float initialSpeed, Transform initialTarget, LayerMask monsterLayerMask, int initialEnemiesToHit = 1)
    {
        damage = initialDamage;
        speed = initialSpeed;
        target = initialTarget;
        targetMask = monsterLayerMask;
        maxMonsterHits = initialEnemiesToHit;
    }
}
