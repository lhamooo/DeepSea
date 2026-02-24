using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float turnSpeedModifier = 1.5f;
    [SerializeField] private Vector3 currentTarget;
    [SerializeField] private bool moveOnTrack = false;
    [SerializeField] private float trackDeviation = 10f;
    private bool hasTarget = false;
    Vector3 previousTarget;
    void Start()
    {
        if (moveOnTrack)
        {
            Vector3 max = new Vector3(maxDistance, maxDistance, maxDistance);
            currentTarget = new Vector3(Random.Range(-max.x, max.x), Random.Range(-max.y, max.y), Random.Range(-max.z, max.z));
            hasTarget = true;
        }
    }

    void Update()
    {
        if (!hasTarget)
        {
            currentTarget = FindWaypoint();
            hasTarget = true;
        }

        LookAtTarget();
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
        {
            previousTarget = currentTarget;
            hasTarget = false;
        }
    }

    private Vector3 FindWaypoint()
    {
        Vector3 waypoint = Vector3.zero;
        Vector3 max = new Vector3(maxDistance, maxDistance, maxDistance);
        if (!moveOnTrack)
        {
            waypoint = new Vector3(Random.Range(-max.x, max.x), Random.Range(-max.y, max.y), Random.Range(-max.z, max.z));
        }
        else
        {
            waypoint = new Vector3(previousTarget.x + Random.Range(-trackDeviation, trackDeviation), previousTarget.y + Random.Range(-trackDeviation, trackDeviation), transform.forward.z * 10);
        }
        return waypoint;
    }

    private void LookAtTarget()
    {
        float turnSpeed = moveSpeed * turnSpeedModifier;

        Vector3 lookDirection = currentTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), turnSpeed * Time.deltaTime);
    }
}
