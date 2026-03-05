using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private int spawnAmount;
    [SerializeField] private int spawnRadius;
    [SerializeField] private bool doSpawnDelay = false;
    [SerializeField] private float spawnDelayInSeconds = 0.2f;
    private int spawnIndex = 0;
    private float timer = 0;
    void Start()
    {
        if (!doSpawnDelay)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
                Vector3 r = Random.insideUnitSphere.normalized;
                Quaternion randomRotation = Quaternion.LookRotation(r, Vector3.up);
                GameObject newSpawn = Instantiate(spawnPrefab, randomPosition, randomRotation);
            }
        }
    }

    void Update()
    {
        if (doSpawnDelay && spawnIndex < spawnAmount)
        {
            timer += Time.deltaTime;
            if (timer >= spawnDelayInSeconds)
            {
                Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
                Vector3 r = Random.insideUnitSphere.normalized;
                Quaternion randomRotation = Quaternion.LookRotation(r, Vector3.up);
                GameObject newSpawn = Instantiate(spawnPrefab, randomPosition, randomRotation);
                spawnIndex++;
            }
        }
    }
}
