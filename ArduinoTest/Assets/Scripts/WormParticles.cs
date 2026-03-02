using System.Collections;
using UnityEngine;

public class WormParticles : MonoBehaviour
{
    [SerializeField] private float particleTimeInSeconds;
    [SerializeField] private new ParticleSystem particleSystem;
    private bool isPlaying = false;

    void Start()
    {
        particleSystem.Stop();
        var main = particleSystem.main;
        main.duration = particleTimeInSeconds;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("InteractionZone") && !isPlaying)
        {
            particleSystem.Play();
        }
    }
}
