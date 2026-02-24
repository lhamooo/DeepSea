using UnityEngine;

public class WhaleMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeedModifier = 0.5f;

    void Update()
    {
        LookAtTarget();
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void LookAtTarget()
    {
        float turnSpeed = moveSpeed * turnSpeedModifier;

        Vector3 lookDirection = target.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), turnSpeed * Time.deltaTime);
    }
}
