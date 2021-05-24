using UnityEngine;

/// <summary>
/// Manages Boid system advancement
/// </summary>
public class BoidManager : MonoBehaviour
{
    Boid[] boids;

    void Start()
    {
        boids = FindObjectsOfType<Boid>();
    }

    void Update()
    {
        UpdateBoids();
    }

    void UpdateBoids()
    {
        foreach (var boid in boids)
        {
            boid.UpdateNeighbours(boids);

            Vector3 avoidanceDirection = boid.Avoidance();
            Vector3 targetDirection = avoidanceDirection;

            if (boid.HasNeighbours)
            {
                Vector3 separationDirection = boid.Separate();
                Vector3 alignmentDirection = boid.Align();
                Vector3 cohesionDirection = boid.Cohere();

                targetDirection +=
                separationDirection +
                alignmentDirection +
                cohesionDirection;

                //if (Time.frameCount % 15 == 0)
                //{
                //print($"{boid.name}:\n" +
                //$"sep: {separationForce.ToString("F5")}\n" +
                //$"ali: {alignmentForce.ToString("F5")}\n" +
                //$"coh: {cohesionForce.ToString("F5")}\n" +
                //$"dir: {targetDirection.normalized.ToString("F5")}");
                //}

            }

            boid.UpdateBoid(targetDirection.normalized);
        }
    }
}