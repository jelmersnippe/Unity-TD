using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    [Min(1)]
    int startingHealth = 1;

    [SerializeField]
    int health;

    [SerializeField]
    HealthBar healthBar;

    [SerializeField]
    DamagePopup damagePopup;

    [SerializeField]
    int currencyToDrop;

    bool hasDied = false;

    void Start()
    {
        health = startingHealth;
        healthBar = Instantiate(healthBar, transform);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (healthBar)
        {
            healthBar.setHealthPercentage((float)health / (float)startingHealth);
            DamagePopup createdDamagePopup = Instantiate(damagePopup, healthBar.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 1), Quaternion.identity);
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

        BuildingManager.instance.addPurchaseCurrency(currencyToDrop);
        Destroy(this.gameObject);
        Spawner.instance.ReduceCurrentMonstersAlive();
        hasDied = true;
    }
}
