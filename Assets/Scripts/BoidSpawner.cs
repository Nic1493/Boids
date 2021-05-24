using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [Range(0, 64)]
    [SerializeField] int BoidCount = 8;

    [SerializeField] GameObject boidPrefab;
    [SerializeField] Transform boidParentObject;

    void Start()
    {
        //SpawnUnitTest();
        SpawnBoids();
    }

    void SpawnBoids()
    {
        for (int i = 0; i < BoidCount; i++)
        {
            GameObject newBoid = Instantiate(boidPrefab, boidParentObject) as GameObject;

            newBoid.transform.position = Random.insideUnitCircle * 5;
            newBoid.transform.eulerAngles = Vector3.forward * Random.Range(0, 360);

            newBoid.name = "Boid " + i;
        }
    }

    void SpawnUnitTest()
    {
        GameObject b1 = Instantiate(boidPrefab, boidParentObject) as GameObject;
        b1.transform.Translate(0, -4, 0);
        b1.name = "Boid 1";

        GameObject b2 = Instantiate(boidPrefab, boidParentObject) as GameObject;
        b2.transform.Translate(0, 4, 0);
        b2.transform.eulerAngles = Vector3.forward * 180f;
        b2.name = "Boid 2";
    }
}