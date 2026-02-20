using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private int spawnAmount;
    [SerializeField] private int spawnRadius;
    void Start()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            GameObject newSpawn = Instantiate(spawnPrefab, randomPosition, Quaternion.identity);
        }
    }
}
