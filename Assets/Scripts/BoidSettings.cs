using UnityEngine;

[CreateAssetMenu(fileName = "New Boid", menuName = "Scriptable Object/Boid")]
public class BoidSettings : ScriptableObject
{
    public LayerMask obstacleMask;

    [Range(1f, 5f)]
    public float MaxSpeed = 2f;

    [Range(0.1f, 1f)]
    public float MaxSteerForce = 0.5f;

    [Space]

    [Range(0.5f, 5f)]
    public float DetectionRange = 3f;

    [Range(60f, 240f)]
    public float FieldOfVision = 120f;

    [Range(10f, 30f)]
    public float DetectionPointDensity = 15f;

    [Space]

    [Range(0.01f, 1f)]
    public float SeparationWeight = 0.1f;

    [Range(0.01f, 1f)]
    public float AlignmentWeight = 0.1f;

    [Range(0.01f, 1f)]
    public float CohesionWeight = 0.1f;
}