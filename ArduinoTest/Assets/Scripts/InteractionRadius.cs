using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionRadius : MonoBehaviour
{
    [SerializeField] private SwarmManager[] swarmManagers;
    [SerializeField] private float swarmActivationTime;
    private float expansionStrength;
    private float expansionDuration = 10f;
    public bool isExpanding = false;
    private int timer;
    private SphereCollider sphereCollider;
    private int currentStrength;
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed && timer == 0)
        {
            isExpanding = true;
            sphereCollider.enabled = true;
            expansionStrength++;
        }

        else if (isExpanding)
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
                Vector3 newScale = new Vector3(s.x + 0.005f * expansionStrength, s.y + 0.005f * expansionStrength, s.z + 0.005f * expansionStrength);
                transform.localScale = newScale;
            }
            else
            {
                timer = 0;
                expansionStrength = 0;
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

        expansionDuration = result.leds;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag + " activated!");
        if (other.gameObject.CompareTag("Behaviour"))
        {
            other.GetComponent<BehaviourTrigger>().TriggerBehaviour(currentStrength);
        }
    }
}
