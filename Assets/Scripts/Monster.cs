using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    Transform[] waypoints;

    [SerializeField]
    [Range(1,10)]
    int speed = 2;

    [SerializeField]
    int currentWaypointIndex = -1;

    void Update()
    {
        if (waypoints.Length <= 0)
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
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) == 0)
        {
            currentWaypointIndex += 1;
        }

        if (currentWaypointIndex >= waypoints.Length)
        {
            Destroy(this.gameObject);
        }
    }

    public void setWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
    }
}
