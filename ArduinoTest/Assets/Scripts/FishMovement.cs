using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0;
    [SerializeField] private Vector3 movementArea = new Vector3(10, 10, 10);
    [SerializeField] private Vector3 currentTarget;
    private bool hasTarget = false;

    void Update()
    {
        if (!hasTarget)
        {
            currentTarget = FindWaypoint();
            hasTarget = true;
        }

        LookAtTarget();
        //transform.position = Vector3.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
        {
            hasTarget = false;
        }
    }

    private Vector3 FindWaypoint()
    {
        Vector3 m = movementArea;
        Vector3 waypoint = new Vector3(Random.Range(-m.x, m.x), Random.Range(-m.y, m.y), Random.Range(-m.z, m.z));
        return waypoint;
    }

    private void LookAtTarget()
    {
        float turnSpeed = moveSpeed * 1.5f;

        Vector3 lookDirection = currentTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), turnSpeed * Time.deltaTime);
    }
}
