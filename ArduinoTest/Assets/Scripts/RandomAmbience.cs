using UnityEngine;

public class RandomAmbience : MonoBehaviour
{
    [SerializeField] private string[] ambienceEvents;
    [SerializeField] private Vector2 timeBetweenEventsInSeconds;
    [SerializeField] private float maxEventDistance;
    private float currentWaitDuration;
    private float timer;

    void Start()
    {
        currentWaitDuration = Random.Range(timeBetweenEventsInSeconds.x, timeBetweenEventsInSeconds.y);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= currentWaitDuration)
        {
            Vector3 eventPosition = Random.insideUnitSphere * maxEventDistance;
            int ambienceIndex = Random.Range(0, ambienceEvents.Length - 1);
            FMODUnity.RuntimeManager.PlayOneShot(ambienceEvents[ambienceIndex], eventPosition);
            currentWaitDuration = Random.Range(timeBetweenEventsInSeconds.x, timeBetweenEventsInSeconds.y);
            timer = 0;
        }
    }
}
