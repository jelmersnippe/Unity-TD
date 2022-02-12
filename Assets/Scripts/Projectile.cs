using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private float speed;
    private LayerMask targetMask;

    private bool isTargeted = false;
    private Transform target;

    private float? lowestSpeed;
    private float? timeToLowestSpeed;

    private float timeAlive;

    void Update()
    {
        timeAlive += Time.deltaTime;
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

    void TravelTowardsTarget()
    {
        if (lowestSpeed.HasValue && timeToLowestSpeed.HasValue)
        {
            float test = speed - (speed - lowestSpeed.Value) * Mathf.Sqrt(Mathf.Min(timeAlive / timeToLowestSpeed.Value, 1));
            float distance = test * Time.deltaTime;
            transform.position = target ? Vector2.MoveTowards(transform.position, target.position, distance) : transform.position + transform.right * distance;
        }
        else
        {
            float distance = speed * Time.deltaTime;
            transform.position = target ? Vector2.MoveTowards(transform.position, target.position, distance) : transform.position + transform.right * distance;
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        // If we have a target and we've hit it
        // Or we don't have a target and we've hit something in our targetmask
        if ((isTargeted && target != null && collider.transform == target) || (!isTargeted && (((1 << collider.gameObject.layer) & targetMask) != 0)))
        {
            Damageable damageable = collider.gameObject.GetComponent<Damageable>();

            if (damageable != null && !damageable.hasDied)
            {
                Destroy(gameObject);
                damageable.TakeDamage(damage);
            }
        }
    }

    public void setValues(int initialDamage, float initialSpeed, Transform initialTarget, LayerMask monsterLayerMask, float? initialLowestSpeed, float? initialTimeToLowestSpeed)
    {
        damage = initialDamage;
        speed = initialSpeed;
        target = initialTarget;
        targetMask = monsterLayerMask;
        lowestSpeed = initialLowestSpeed;
        timeToLowestSpeed = initialTimeToLowestSpeed;

        isTargeted = target != null;

        if (!isTargeted)
        {
            Destroy(gameObject, 1f);
        }
    }
}
