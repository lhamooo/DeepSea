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
        float strengthModifier = 1f;
        float behaviourDuration = 5f;
        switch (strength)
        {
            case 0:
                strengthModifier = 0.8f;
                behaviourDuration = 3f;
                break;
            case 1:
                strengthModifier = 1f;
                behaviourDuration = 5f;
                break;
            case 2:
                strengthModifier = 1.5f;
                behaviourDuration = 8f;
                break;
        }
        StartCoroutine(RechargeActivation(behaviourDuration, strengthModifier));
    }

    private IEnumerator RechargeActivation(float duration, float strengthModifier)
    {
        torque = activeTorqueStrength * strengthModifier;
        yield return new WaitForSeconds(duration);
        torque = idleTorqueStrength;
    }
}
