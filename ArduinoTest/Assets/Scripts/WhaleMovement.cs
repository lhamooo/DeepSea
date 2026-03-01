using UnityEngine;

public class WhaleMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeedModifier = 0.5f;
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    [SerializeField] private float glowDuration;
    private float glowStrength = 0f;
    public bool isGlowing;

    void Start()
    {
        meshRenderer.materials[1] = new Material(meshRenderer.materials[1]);
    }

    void Update()
    {
        LookAtTarget();
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        if (isGlowing)
        {
            glowStrength += Time.deltaTime;
            if (meshRenderer.materials[1].GetFloat("_Fill") < 1f)
            {
                meshRenderer.materials[1].SetFloat("_Fill", glowStrength * 0.5f);
            }

            if (glowStrength > glowDuration)
            {
                isGlowing = false;
                glowStrength = 0f;
            }
        }
        else if (meshRenderer.materials[1].GetFloat("_Fill") > 0)
        {
            meshRenderer.materials[1].SetFloat("_Fill", meshRenderer.materials[1].GetFloat("_Fill") - Time.deltaTime * 0.5f);
        }
    }

    private void LookAtTarget()
    {
        float turnSpeed = moveSpeed * turnSpeedModifier;

        Vector3 lookDirection = target.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), turnSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("InteractionZone") && !isGlowing)
        {
            isGlowing = true;
        }
    }
}
