using UnityEngine.Events;
using UnityEngine;
using System;

public class Monster : MonoBehaviour
{
    public static event Action<Monster> OnMonsterDied;
    public static event Action<Monster> OnMonsterReachedFinalWaypoint;

    Transform[] waypoints;
    int currentWaypointIndex = -1;

    [SerializeField]
    [Range(1,10)]
    float speed = 1f;
    [SerializeField] float speedMultiplier = 2f;

    private int _damage;
    public int damage {
        get => _damage;
        private set => _damage = value;
    }

    public int currencyToDrop = 1;

    [SerializeField] int startingHealth;
    [SerializeField] int health;

    [SerializeField] DamagePopup damagePopup;
    [SerializeField] HealthBar healthBar;

    float healthbarOffset = 0.2f;
    Bounds bounds;
    HealthBar activeHealthBar;

    private bool _hasDied;
    public bool hasDied
    {
        get => _hasDied;
        private set => _hasDied = value;
    }

    private void Awake()
    {
        bounds = GetComponent<Collider2D>().bounds;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length <= 0)
        {
            return;
        }

        if (currentWaypointIndex == -1)
        {
            currentWaypointIndex = 0; 
        }

        MoveTowardsWaypoint();
    }

    void MoveTowardsWaypoint()
    {
        if (currentWaypointIndex < 0 || currentWaypointIndex >= waypoints.Length)
        {
            Debug.LogWarning("No path set or end reached");
            Destroy(gameObject);
            return;
        }

        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, (speed * speedMultiplier) * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) == 0)
        {
            currentWaypointIndex += 1;
        }

        if (currentWaypointIndex >= waypoints.Length)
        {
            OnMonsterReachedFinalWaypoint?.Invoke(this);
            AudioManager.instance.Play(Sound.Name.Escape);
            Destroy(gameObject);
        }
    }

    public void Setup(Sprite sprite, int initialHealth, float initialSpeed, int initialDamage, int initialCurrencyToDrop, Transform[] newWaypoints)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        SetStartingHealth(initialHealth);
        speed = initialSpeed;
        damage = initialDamage;
        waypoints = newWaypoints;
        currencyToDrop = initialCurrencyToDrop;
    }


    public void SetStartingHealth(int healthToSet)
    {
        startingHealth = healthToSet;
        health = healthToSet;
        if (activeHealthBar != null)
        {
            Destroy(activeHealthBar.gameObject);
        }
        activeHealthBar = Instantiate(healthBar, transform.position + new Vector3(0, (bounds.size.y / 2) + healthbarOffset), Quaternion.identity, transform);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (activeHealthBar)
        {
            activeHealthBar.SetHealthPercentage((float)health / (float)startingHealth);
            DamagePopup createdDamagePopup = Instantiate(damagePopup, activeHealthBar.transform.position + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 1), Quaternion.identity);
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

        OnMonsterDied?.Invoke(this);

        Destroy(gameObject);
        hasDied = true;
    }
}
