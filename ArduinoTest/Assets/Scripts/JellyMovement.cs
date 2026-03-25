using System.Collections;
using UnityEngine;

public class JellyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float animLength = 4f;
    [SerializeField] private float maxDistanceFromZero = 10;
    [SerializeField] private float idleTorqueStrength = 10f;
    [SerializeField] private float activeTorqueStrength = 20f;
    [SerializeField] private Color passiveColour;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private float colourChangeDuration = 1f;
    private float torque;
    private float timer = 0;
    private Rigidbody rb;
    private BehaviourTrigger behaviourTrigger;
    private bool behaviourActive = false;
    private Color activeColour;

    void Start()
    {
        meshRenderer.materials[1] = new Material(meshRenderer.materials[1]);
        Color c = meshRenderer.materials[1].GetColor("_textureColor");
        Debug.Log(c);
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
            AddTorqueToJelly(torque);
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
        if (!behaviourActive)
        {
            float behaviourDuration = 5f;
            switch (strength)
            {
                case 0:
                    strengthModifier = 1f;
                    behaviourDuration = 4f;
                    break;
                case 1:
                    strengthModifier = 1.4f;
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
        StartCoroutine(ChangeColour(duration));
        torque = activeTorqueStrength * strengthModifier;
        yield return new WaitForSeconds(duration);
        torque = idleTorqueStrength;
        behaviourActive = false;
    }

    private IEnumerator ChangeColour(float duration)
    {
        Debug.Log("Balls");
        float timer = 0;
        activeColour = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
        while (timer < colourChangeDuration)
        {
            timer += Time.deltaTime;
            Color lerpColour = Color.Lerp(passiveColour, activeColour, timer / colourChangeDuration);
            meshRenderer.materials[1].SetColor("_textureColor", lerpColour);
            yield return null;
        }
        yield return new WaitForSeconds(duration - (colourChangeDuration * 2));
        timer = 0;
        while (timer < colourChangeDuration)
        {
            timer += Time.deltaTime;
            Color lerpColour = Color.Lerp(activeColour, passiveColour, timer / colourChangeDuration);
            meshRenderer.materials[1].SetColor("_textureColor", lerpColour);
            yield return null;
        }
        //meshRenderer.materials[1].SetColor("_textureColor", passiveColour);
    }

    private void AddTorqueToJelly(float t)
    {
        rb.AddTorque(Random.Range(-t, t), Random.Range(-t, t), Random.Range(-t, t));
    }
}
