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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        torque = idleTorqueStrength;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("InteractionZone"))
        {
            StartCoroutine(RechargeActivation());
        }
    }

    private IEnumerator RechargeActivation()
    {
        torque = activeTorqueStrength;
        yield return new WaitForSeconds(5f);
        torque = idleTorqueStrength;
    }
}
