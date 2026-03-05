using System.Collections;
using UnityEngine;

public class JellyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float animLength = 4f;
    [SerializeField] private float maxDistanceFromZero = 10;
    [SerializeField] private float idleTorqueStrength = 10f;
    [SerializeField] private float activeTorqueStrength = 20f;
    private float torque;
    private float timer = 0;
    private Rigidbody rb;
    private BehaviourTrigger behaviourTrigger;
    private bool behaviourActive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        torque = idleTorqueStrength;
        behaviourTrigger = GetComponent<BehaviourTrigger>();
        behaviourTrigger.OnBehaviourTriggered += TriggerBehaviour;
    }

    void Update()
    {
        if (timer >= animLength)
        {
            timer = 0;
            rb.AddRelativeForce(0, moveSpeed, 0, ForceMode.Impulse);
            rb.AddTorque(Random.Range(-torque, torque), Random.Range(-torque, torque), Random.Range(-torque, torque));
        }
        rb.linearDamping = timer;
        timer += Time.deltaTime;

        if (Vector3.Distance(transform.position, Vector3.zero) > maxDistanceFromZero)
        {
            transform.position *= -0.9f;
        }
    }

    private void TriggerBehaviour(int strength)
    {
        if (!behaviourActive)
        {
            float strengthModifier = 1f;
            float behaviourDuration = 5f;
            switch (strength)
            {
                case 0:
                    strengthModifier = 0.8f;
                    behaviourDuration = 4f;
                    break;
                case 1:
                    strengthModifier = 1.2f;
                    behaviourDuration = 6f;
                    break;
                case 2:
                    strengthModifier = 2f;
                    behaviourDuration = 10f;
                    break;
            }
            StartCoroutine(RechargeActivation(behaviourDuration, strengthModifier));
            behaviourActive = true;
        }
    }

    private IEnumerator RechargeActivation(float duration, float strengthModifier)
    {
        torque = activeTorqueStrength * strengthModifier;
        yield return new WaitForSeconds(duration);
        torque = idleTorqueStrength;
        behaviourActive = false;
    }
}
