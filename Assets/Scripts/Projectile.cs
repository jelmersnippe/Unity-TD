using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private int speed;

    private Transform target;

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        RotateTowardsTarget();
        TravelTowardsTarget();
    }

    void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y, target.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void TravelTowardsTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.transform == target)
        {
            Destroy(gameObject);

            Damageable damageable = target.GetComponent<Damageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    public void setValues(int initialDamage, int initialSpeed, Transform initialTarget)
    {
        damage = initialDamage;
        speed = initialSpeed;
        target = initialTarget;
    }
}
