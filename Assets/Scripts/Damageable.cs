using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] int startingHealth;
    [SerializeField] int health;

    [SerializeField] DamagePopup damagePopup;
    [SerializeField] HealthBar healthBar;

    float offset = 0.2f;
    Bounds bounds;
    HealthBar activeHealthBar;

    public bool hasDied = false;

    private void Awake()
    {
        bounds = GetComponent<Collider2D>().bounds;
    }

    public void SetStartingHealth(int healthToSet)
    {
        startingHealth = healthToSet;
        health = healthToSet;
        if (activeHealthBar != null)
        {
            Destroy(activeHealthBar.gameObject);
        }
        activeHealthBar = Instantiate(healthBar, transform.position + new Vector3(0, (bounds.size.y / 2) + offset), Quaternion.identity, transform);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (activeHealthBar)
        {
            activeHealthBar.setHealthPercentage((float)health / (float)startingHealth);
            DamagePopup createdDamagePopup = Instantiate(damagePopup, activeHealthBar.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 1), Quaternion.identity);
            createdDamagePopup.setDamage(damage);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (hasDied)
        {
            return;
        }

        Monster monster = GetComponent<Monster>();
        if (monster != null)
        {
            GameManager.instance.purchaseCurrency += monster.currencyToDrop;
            Spawner.instance.ReduceCurrentMonstersAlive();
        }

        Destroy(gameObject);
        hasDied = true;
    }
}
