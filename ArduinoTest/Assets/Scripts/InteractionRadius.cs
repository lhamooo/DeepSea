using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionRadius : MonoBehaviour
{
    [SerializeField] private SwarmManager[] swarmManagers;
    [SerializeField] private float swarmActivationTime;
    private float expansionStrength;
    private bool isExpanding = false;
    private int timer;
    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed && timer == 0)
        {
            isExpanding = true;
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

            if (timer < expansionStrength)
            {
                timer++;
                Vector3 s = transform.localScale;
                Vector3 newScale = new Vector3(s.x + 0.001f * expansionStrength, s.y + 0.001f * expansionStrength, s.z + 0.001f * expansionStrength);
                transform.localScale = newScale;
            }
            else
            {
                timer = 0;
                expansionStrength = 0;
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                isExpanding = false;
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag + " activated!");
    }
}
