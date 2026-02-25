using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float turnSpeedModifier = 1.5f;
    [SerializeField] private Vector3 currentTarget;

    [Header("Hunting Behaviour")]
    [SerializeField] private bool canHunt = false;
    [SerializeField] private float huntSpeed = 2f;
    [SerializeField] private float huntingTurnSpeedModifier = 2f;
    [SerializeField] private GameObject huntingTarget;
    private bool isHunting = false;
    private bool hasTarget = false;
    private float currentMoveSpeed;
    private float currentTurnSpeedModifier;

    void Start()
    {
        currentMoveSpeed = moveSpeed;
        currentTurnSpeedModifier = turnSpeedModifier;
    }

    void Update()
    {
        if (!hasTarget)
        {
            currentTarget = FindWaypoint();
            hasTarget = true;
        }

        if (canHunt && isHunting)
        {
            HuntingBehaviour();
        }

        LookAtTarget();
        transform.position += transform.forward * currentMoveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, currentTarget) < 0.1f && !isHunting)
        {
            hasTarget = false;
        }
    }

    private Vector3 FindWaypoint()
    {
        Vector3 max = new Vector3(maxDistance, maxDistance, maxDistance);
        Vector3 waypoint = new Vector3(Random.Range(-max.x, max.x), Random.Range(-max.y, max.y), Random.Range(-max.z, max.z));
        return waypoint;
    }

    private void LookAtTarget()
    {
        float turnSpeed = moveSpeed * currentTurnSpeedModifier;

        Vector3 lookDirection = currentTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), turnSpeed * Time.deltaTime);
    }

    private void HuntingBehaviour()
    {
        if (huntingTarget == null)
        {
            huntingTarget = FindClosestBoid();
            hasTarget = true;
        }
        else
        {
            currentTarget = huntingTarget.transform.position;
            if (Vector3.Distance(transform.position, currentTarget) > 10f)
            {
                huntingTarget = FindClosestBoid();
            }
        }
    }

    private GameObject FindClosestBoid()
    {
        GameObject[] boids;
        boids = GameObject.FindGameObjectsWithTag("Boid");
        GameObject closestBoid = null;
        float distance = Mathf.Infinity;
        Vector3 pos = transform.position;
        foreach (GameObject boid in boids)
        {
            if (Vector3.Distance(pos, boid.transform.position) < distance)
            {
                distance = Vector3.Distance(pos, boid.transform.position);
                closestBoid = boid;
            }
        }

        return closestBoid;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("InteractionZone"))
        {
            isHunting = true;
            currentMoveSpeed = huntSpeed;
            currentTurnSpeedModifier = huntingTurnSpeedModifier;
        }
        else if (isHunting && other.gameObject == huntingTarget)
        {
            Destroy(other);
            huntingTarget = null;
            isHunting = false;
            hasTarget = false;
            currentMoveSpeed = moveSpeed;
            currentTurnSpeedModifier = turnSpeedModifier;
        }
    }
}
