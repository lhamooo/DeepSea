using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionRadius : MonoBehaviour
{
    [SerializeField] private SwarmManager[] swarmManagers;
    [SerializeField] private float swarmActivationTime;
    [SerializeField] private string pumpEventPath;
    private float expansionStrength = 1f;
    private float expansionDuration = 10f;
    public bool isExpanding = false;
    private int timer = 0;
    private SphereCollider sphereCollider;
    private int currentStrength;
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed && !isExpanding)
        {
            isExpanding = true;
            sphereCollider.enabled = true;
            FMODUnity.RuntimeManager.PlayOneShot(pumpEventPath, transform.position);
        }
        if (isExpanding)
        {
            if (timer == 0)
            {
                foreach (var swarmManager in swarmManagers)
                {
                    swarmManager.ActivateSwarm(swarmActivationTime);
                }
            }

            if (timer < expansionDuration * 10)
            {
                timer++;
                Vector3 s = transform.localScale;
                Vector3 newScale = new Vector3(s.x + expansionStrength, s.y + expansionStrength, s.z + expansionStrength);
                transform.localScale = newScale;
            }
            else
            {
                timer = 0;
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                isExpanding = false;
                sphereCollider.enabled = false;
            }
        }
    }

    public void StartInteraction(ArduinoResult result)
    {
        if (result.geschwindigkeit == "slow")
        {
            currentStrength = 0;
        }
        else if (result.geschwindigkeit == "middle")
        {
            currentStrength = 1;
        }
        else if (result.geschwindigkeit == "fast")
        {
            currentStrength = 2;
        }

        expansionStrength = result.leds / 10;

        if (!isExpanding)
        {
            expansionDuration = result.leds;
            isExpanding = true;
            sphereCollider.enabled = true;
            FMODUnity.RuntimeManager.PlayOneShot(pumpEventPath, transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag + " activated!");
        if (other.gameObject.CompareTag("Behaviour") && isExpanding)
        {
            other.GetComponent<BehaviourTrigger>().TriggerBehaviour(currentStrength);
        }
    }
}
