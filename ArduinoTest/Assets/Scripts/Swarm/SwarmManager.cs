using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwarmManager : MonoBehaviour
{
    [SerializeField] private GameObject boidPrefab;
    [SerializeField] private int swarmSize = 30;
    [SerializeField] private Boid[] boids;
    [SerializeField] private float cohesionWeight = 1f;
    [SerializeField] private float separationWeight = 1f;
    [SerializeField] private float alignmentWeight = 1f;
    [SerializeField] private float separationRadius = 2f;
    [SerializeField] private float maxDistanceFromZero = 30f;

    void Start()
    {
        boids = new Boid[swarmSize];
        for (int i = 0; i < swarmSize; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * 10;
            GameObject boid = Instantiate(boidPrefab, randomPosition, Quaternion.identity);
            boids[i] = boid.GetComponent<Boid>();
            boids[i].velocity = Random.insideUnitSphere.normalized * boids[i].maxSpeed;
        }
    }

    void Update()
    {
        for (int i = 0; i < swarmSize; i++)
        {
            if (boids[i] == null)
            {
                Vector3 randomPosition = Random.insideUnitSphere * 10;
                GameObject boid = Instantiate(boidPrefab, randomPosition, Quaternion.identity);
                boids[i] = boid.GetComponent<Boid>();
                boids[i].velocity = Random.insideUnitSphere.normalized * boids[i].maxSpeed;
            }
        }

        foreach (Boid boid in boids)
        {
            Boid[] neighbors = FindNeighbors(boid, 5f);

            Vector3 cohesion = boid.Cohesion(neighbors) * cohesionWeight;
            Vector3 separation = boid.Separation(neighbors, separationRadius) * separationWeight;
            Vector3 alignment = boid.Alignment(neighbors) * alignmentWeight;

            if (Vector3.Distance(boid.transform.position, Vector3.zero) > maxDistanceFromZero)
            {
                boid.transform.position *= -0.9f;
            }

            boid.velocity += cohesion + separation + alignment;
            boid.velocity = Vector3.ClampMagnitude(boid.velocity, boid.maxSpeed);
            boid.transform.position += boid.velocity * Time.deltaTime;
            boid.transform.rotation = Quaternion.LookRotation(boid.velocity);
        }
    }

    Boid[] FindNeighbors(Boid boid, float radius)
    {
        List<Boid> neighbors = new List<Boid>();
        foreach (Boid otherBoid in boids)
        {
            if (otherBoid != boid && Vector3.Distance(boid.transform.position, otherBoid.transform.position) < radius && boids.Contains(otherBoid))
            {
                neighbors.Add(otherBoid);
            }
        }
        return neighbors.ToArray();
    }
}
