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
    [SerializeField] private GameObject prey;

    [Header("Particles")]
    [SerializeField] private float particleTimeInSeconds;
    [SerializeField] private new ParticleSystem particleSystem;
    private bool isHunting = false;
    private bool hasTarget = false;
    private float currentMoveSpeed;
    private float currentTurnSpeedModifier;
    private BehaviourTrigger behaviourTrigger;

    void Start()
    {
        currentMoveSpeed = moveSpeed;
        currentTurnSpeedModifier = turnSpeedModifier;
        if (canHunt)
        {
            behaviourTrigger = GetComponent<BehaviourTrigger>();
            behaviourTrigger.OnBehaviourTriggered += TriggerBehaviour;
            particleSystem.Stop();
            var main = particleSystem.main;
            main.duration = particleTimeInSeconds;
        }
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

        if (Vector3.Distance(transform.position, currentTarget) < 0.2f && !isHunting)
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
        if (prey == null)
        {
            prey = FindClosestBoid();
            hasTarget = true;
        }
        else
        {
            currentTarget = prey.transform.position;
            if (Vector3.Distance(transform.position, currentTarget) > 10f)
            {
                prey = FindClosestBoid();
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

    private void TriggerBehaviour(int strength)
    {
        float strengthModifier = 1f;
        switch (strength)
        {
            case 0:
                strengthModifier = 0.8f;
                break;
            case 1:
                strengthModifier = 1f;
                break;
            case 2:
                strengthModifier = 1.5f;
                break;
        }
        if (canHunt)
        {
            isHunting = true;
            currentMoveSpeed = huntSpeed * strengthModifier;
            currentTurnSpeedModifier = huntingTurnSpeedModifier * strengthModifier;
            Debug.Log("Triggered behaviour with Strength " + strength);

            if (!particleSystem.isPlaying)
            {
                var main = particleSystem.main;
                main.duration = particleTimeInSeconds * strengthModifier;
                particleSystem.Play();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isHunting && other.gameObject == prey)
        {
            Debug.Log("Prey reached");
            Destroy(other.gameObject);
            prey = null;
            isHunting = false;
            hasTarget = false;
            currentMoveSpeed = moveSpeed;
            currentTurnSpeedModifier = turnSpeedModifier;
        }
    }
}
