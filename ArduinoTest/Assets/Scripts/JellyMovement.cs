using UnityEngine;

public class JellyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float animLength = 4f;
    [SerializeField] private float maxDistanceFromZero = 10;
    private float timer = 0;
    private int count = 1;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= animLength)
        {
            timer = 0;
            rb.AddRelativeForce(0, moveSpeed, 0, ForceMode.Impulse);
            rb.AddTorque(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        }
        rb.linearDamping = timer;
        timer += Time.deltaTime;

        if (Vector3.Distance(transform.position, Vector3.zero) > maxDistanceFromZero)
        {
            transform.position *= -0.9f;
        }
    }
}
