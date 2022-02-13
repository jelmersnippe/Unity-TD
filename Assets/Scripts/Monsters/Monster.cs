using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Transform[] waypoints;
    int currentWaypointIndex = -1;

    [SerializeField]
    [Range(1,10)]
    float speed = 1f;
    [SerializeField] float speedMultiplier = 2f;

    [SerializeField] int damage = 1;

    public int currencyToDrop = 1;



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
            GameManager.instance.takeDamage(damage);
            Spawner.instance.ReduceCurrentMonstersAlive();
            Destroy(this.gameObject);
        }
    }

    public void Setup(Sprite sprite, int initialHealth, float initialSpeed, int initialDamage, int initialCurrencyToDrop, Transform[] newWaypoints)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        GetComponent<Damageable>().SetStartingHealth(initialHealth);
        speed = initialSpeed;
        damage = initialDamage;
        waypoints = newWaypoints;
        currencyToDrop = initialCurrencyToDrop;
    }
}
