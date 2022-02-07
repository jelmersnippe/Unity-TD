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
    int currencyToDrop;

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
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        BuildingManager.instance.addPurchaseCurrency(currencyToDrop);
        Destroy(this.gameObject);
    }
}
