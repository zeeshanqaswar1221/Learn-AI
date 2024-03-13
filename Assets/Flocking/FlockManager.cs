using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public int flockCount = 20;
    public GameObject fish;
    public Vector3 swimLimits = new Vector3(5,5,5);

    [Header("Fish Properties")]
    [Range(0.0f, 5f)] public float minSpeed;
    [Range(0.0f, 5f)] public float maxSpeed;
    [Range(0.0f, 10f)] public float neighbourDist;
    [Range(0.0f, 5f)] public float rotationSpeed;

    public GameObject[] spawnedFishes;

    public static FlockManager FM;

    public Vector3 goalPoint;

    private void Awake()
    {
        FM = this;
    }

    private void Start()
    {
        spawnedFishes = new GameObject[flockCount];
        for (int i = 0; i < spawnedFishes.Length; i++)
        {
            Vector3 pos = new Vector3 ( Random.Range(-swimLimits.x, swimLimits.x),
                                        Random.Range(-swimLimits.y, swimLimits.y),
                                        Random.Range(-swimLimits.z, swimLimits.z));

            spawnedFishes[i] = Instantiate(fish, pos, Quaternion.identity);
        }
    }


    private void Update()
    {
        if (Random.Range(0,1000) < 10)
        {
            goalPoint = new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                        Random.Range(-swimLimits.y, swimLimits.y),
                                        Random.Range(-swimLimits.z, swimLimits.z));
        }
    }
}
