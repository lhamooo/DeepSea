using System;
using UnityEngine;

public class SwarmMovement : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 currentTarget;
    public bool hasTarget = false;

    void Update()
    {
        if (hasTarget)
        {
            LookAtTarget();
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
            {
                hasTarget = false;
            }
        }
    }

    private void LookAtTarget()
    {
        float turnSpeed = moveSpeed * 1.5f;

        Vector3 lookDirection = currentTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), turnSpeed * Time.deltaTime);
    }

    public void GetNewWaypoint(bool isFleeing, Vector3 direction)
    {
        if (!isFleeing)
        {
            currentTarget = direction;
            hasTarget = true;
        }
        else
        {

        }
    }
}
