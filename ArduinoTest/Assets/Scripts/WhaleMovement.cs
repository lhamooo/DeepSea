using FMODUnityResonance;
using UnityEngine;

public class WhaleMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeedModifier = 0.5f;

    [Header("Glow Shader")]
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private float glowDuration;
    [SerializeField] private float glowSpeed = 0.5f;
    public bool isGlowing;

    [Header("Sounds")]
    [SerializeField] private string[] fmodEvents;
    private float glowStrength = 0f;
    private BehaviourTrigger behaviourTrigger;

    void Start()
    {
        meshRenderer.materials[1] = new Material(meshRenderer.materials[1]);
        behaviourTrigger = GetComponent<BehaviourTrigger>();
        behaviourTrigger.OnBehaviourTriggered += TriggerBehaviour;
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
                meshRenderer.materials[1].SetFloat("_Fill", glowStrength * glowSpeed);
            }

            if (glowStrength > glowDuration)
            {
                isGlowing = false;
                glowStrength = 0f;
            }
        }
        else if (meshRenderer.materials[1].GetFloat("_Fill") > 0)
        {
            meshRenderer.materials[1].SetFloat("_Fill", meshRenderer.materials[1].GetFloat("_Fill") - Time.deltaTime * glowSpeed);
        }
    }

    private void LookAtTarget()
    {
        float turnSpeed = moveSpeed * turnSpeedModifier;

        Vector3 lookDirection = target.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), turnSpeed * Time.deltaTime);
    }

    private void TriggerBehaviour(int strength)
    {
        switch (strength)
        {
            case 0:
                glowDuration *= 1f;
                break;
            case 1:
                glowDuration *= 1.5f;
                break;
            case 2:
                glowDuration *= 2f;
                break;
        }
        int randomSoundIndex = Random.Range(0, fmodEvents.Length - 1);
        Debug.Log("Played sound: " + fmodEvents[randomSoundIndex]);
        FMODUnity.RuntimeManager.PlayOneShot(fmodEvents[randomSoundIndex], transform.position);
        if (!isGlowing)
        {
            isGlowing = true;
        }
    }
}
