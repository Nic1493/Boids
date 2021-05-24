using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField] BoidSettings settings;
    [HideInInspector] public new Transform transform;

    Vector2 position;
    Vector2 velocity;

    List<Boid> neighbours = new List<Boid>();
    public bool HasNeighbours => neighbours.Count > 0;

    List<Vector3> detectionDirections;

    #region Initializiation

    void Awake()
    {
        transform = GetComponent<Transform>();
    }

    void Start()
    {
        position = transform.position;
        velocity = transform.up * settings.MaxSpeed;

        InitDetectionDirections();
    }

    void InitDetectionDirections()
    {
        detectionDirections = new List<Vector3>();

        detectionDirections.Add(Vector3.up);

        for (float i = settings.DetectionPointDensity; i <= settings.FieldOfVision / 2; i += settings.DetectionPointDensity)
        {
            Vector3 detectionDirection = new Vector3
                (
                Mathf.Sin(i * Mathf.Deg2Rad),
                Mathf.Cos(i * Mathf.Deg2Rad),
                0
                );
            detectionDirections.Add(detectionDirection);

            detectionDirection.x *= -1;
            detectionDirections.Add(detectionDirection);
        }
    }

    #endregion

    #region DEBUG
    /*
    void OnDrawGizmos()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            foreach (var d in detectionDirections)
            {
                Vector3 lineStart = transform.position;
                Vector3 lineEnd = lineStart + (transform.TransformDirection(d) * settings.DetectionRange);
                Debug.DrawLine(lineStart, lineEnd, Color.red);
            }
        }
        else
        {
            transform = GetComponent<Transform>();

            for (float i = -settings.FieldOfVision / 2; i <= settings.FieldOfVision / 2; i += settings.DetectionPointDensity)
            {
                Vector3 rayStart = transform.position;
                Vector3 rayEnd = rayStart + new Vector3(
                    Mathf.Sin((i - transform.eulerAngles.z) * Mathf.Deg2Rad),
                    Mathf.Cos((i - transform.eulerAngles.z) * Mathf.Deg2Rad),
                    0
                    ) * settings.DetectionRange;
                Debug.DrawLine(rayStart, rayEnd, Color.red);
            }
        }
    }
    */
    #endregion

    #region Public

    public void UpdateBoid(Vector2 targetDirection)
    {
        float dt = Time.deltaTime;

        Vector2 targetVelocity = targetDirection * settings.MaxSpeed;
        Vector2 targetSteerForce = (targetVelocity - velocity) * settings.MaxSteerForce;
        Vector2 acceleration = Vector2.ClampMagnitude(targetSteerForce, settings.MaxSteerForce);

        //print($"{name}:\n" +
        //    $"velocity: {velocity.ToString("F5")}\n" +
        //    $"acceleration: {acceleration.ToString("F5")}");

        velocity = Vector2.ClampMagnitude(velocity + acceleration, settings.MaxSpeed);

        position += velocity * dt;
        float zRotation = Mathf.Atan2(-velocity.x, velocity.y) * Mathf.Rad2Deg;

        transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, zRotation));
    }

    public void UpdateNeighbours(Boid[] boids)
    {
        neighbours.Clear();

        foreach (var boid in boids)
        {
            if (position != boid.position)
            {
                if (Vector3.Distance(position, boid.position) < settings.DetectionRange)
                {
                    neighbours.Add(boid);
                }
            }
        }
    }

    #region Movement methods

    public Vector3 Separate()
    {
        Vector3 separationDirection = Vector3.zero;
        if (!HasNeighbours) return separationDirection;

        for (int i = 0; i < neighbours.Count; i++)
        {
            Vector3 distance = transform.position - neighbours[i].transform.position;
            separationDirection += Vector3.Normalize(distance) / distance.sqrMagnitude;
        }

        return separationDirection * settings.SeparationWeight;
    }

    public Vector3 Align()
    {
        Vector3 alignmentDirection = Vector3.zero;
        if (!HasNeighbours) return alignmentDirection;

        foreach (var neighbour in neighbours)
        {
            alignmentDirection += (Vector3)neighbour.velocity;
        }

        alignmentDirection /= neighbours.Count;

        return alignmentDirection * settings.AlignmentWeight;
    }

    public Vector3 Cohere()
    {
        Vector3 cohesionDirection = Vector3.zero;
        if (!HasNeighbours) return cohesionDirection;

        foreach (var neighbour in neighbours)
        {
            cohesionDirection += neighbour.transform.position;
        }

        cohesionDirection /= neighbours.Count;
        cohesionDirection -= transform.position;

        return cohesionDirection * settings.CohesionWeight;
    }

    public Vector3 Avoidance()
    {
        Vector3 avoidDirection = transform.up;

        Vector3 rayOrigin = position;
        Vector3 rayDirection = transform.up;
        float rayDistance = velocity.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, settings.obstacleMask);

        if (hit)
        {
            //Debug.DrawLine(transform.position, hit.point, Color.red);

            for (int i = 0; i < detectionDirections.Count; i++)
            {
                rayDirection = transform.TransformDirection(detectionDirections[i]);
                hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, settings.obstacleMask);

                if (!hit)
                {
                    //Debug.DrawLine(transform.position, transform.position + rayDirection * rayDistance, Color.green);
                    avoidDirection = rayDirection;
                    break;
                }
            }

            //UnityEditor.EditorApplication.isPaused = true;
        }

        return avoidDirection * settings.MaxSteerForce;
    }

    public float AddVariance() => (Mathf.PerlinNoise(Time.time, 0f) - 0.5f) * 0.1f;

    #endregion

    #endregion
}