using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected int damage;
    protected float speed;
    protected LayerMask targetMask;

    protected bool isTargeted = false;
    protected Transform target;

    protected virtual void Update()
    {
        if (isTargeted && target == null)
        {
            Destroy(gameObject);
            return;
        }

        RotateTowardsTarget();
        TravelTowardsTarget();
    }

    void RotateTowardsTarget()
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = desiredRotation;
    }

    protected virtual void TravelTowardsTarget()
    {
        float distance = speed * Time.deltaTime;
        transform.position = target ? Vector2.MoveTowards(transform.position, target.position, distance) : transform.position + transform.right * distance;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If we have a target and we've hit it
        // Or we don't have a target and we've hit something in our targetmask
        Damageable damageable = GetDamageableFromCollider(collision.collider);

        if (damageable != null && (!isTargeted || damageable.gameObject == target.gameObject))
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
        damageable.TakeDamage(damage);
        Destroy(gameObject);
    }

    public virtual void setValues(int initialDamage, float initialSpeed, Transform initialTarget, LayerMask monsterLayerMask)
    {
        damage = initialDamage;
        speed = initialSpeed;
        target = initialTarget;
        targetMask = monsterLayerMask;

        isTargeted = target != null;

        if (!isTargeted)
        {
            Destroy(gameObject, 1f);
        }
    }
}
