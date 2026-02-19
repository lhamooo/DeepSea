using System.Collections.Generic;
using UnityEngine;

public class SwarmController : MonoBehaviour
{
    [SerializeField] private float swarmMoveSpeed = 1;
    [SerializeField] private Vector3 movementArea = new Vector3(10, 10, 10);
    private SwarmMovement[] swarmElements;
    private bool isTargetReached = false;

    void Start()
    {
        swarmElements = GetComponentsInChildren<SwarmMovement>();
        foreach (var swarmElement in swarmElements)
        {
            swarmElement.moveSpeed = swarmMoveSpeed + Random.Range(-0.01f, 0.01f);
        }
    }

    void Update()
    {
        foreach (var swarmElement in swarmElements)
        {
            if (!swarmElement.hasTarget)
            {
                isTargetReached = true;
            }
        }

        if (isTargetReached)
        {
            Vector3 newTarget = FindWaypoint();
            foreach (var swarmElement in swarmElements)
            {
                swarmElement.GetNewWaypoint(false, newTarget);
            }
        }
    }

    private Vector3 FindWaypoint()
    {
        Vector3 m = movementArea;
        Vector3 waypoint = new Vector3(Random.Range(-m.x, m.x), Random.Range(-m.y, m.y), Random.Range(-m.z, m.z));
        return waypoint;
    }

}
