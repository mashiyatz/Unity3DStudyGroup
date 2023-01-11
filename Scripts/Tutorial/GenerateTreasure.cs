using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTreasure : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float interval;
    public float maxDistance;
    private float timeSinceLastGenerate;

    void Start()
    {
        timeSinceLastGenerate = Time.time;
    }

    // Quaternion.LookAt() to generate object facing player? 

    void Update()
    {
        if (Time.time - timeSinceLastGenerate >= interval)
        {
            Vector3 pos = new(Random.Range(-maxDistance, maxDistance), 0.5f, Random.Range(-maxDistance, maxDistance));
            Instantiate(enemyPrefab, pos, transform.rotation, transform);
            timeSinceLastGenerate = Time.time;
        }
    }
}
