using UnityEngine;

public class JellyMovement : MonoBehaviour
{
    private int count;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (count > 1000)
        {
            rb.AddForce(0, 0.2f, 0);
            count = 0;
        }
        count++;
    }
}
