using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public GameObject enemyCubePrefab;
    public GameObject pointCubePrefab;
    public Transform platform;
    public float interval;
    public Transform player;

    private float timeOfLastIntervalUpdate;
    private float timeOfLastCubeGeneration;
    private int maxNumberOfGenerations;

    void Start()
    {
        timeOfLastIntervalUpdate = Time.fixedTime;
        timeOfLastCubeGeneration = Time.fixedTime;
        maxNumberOfGenerations = 2;
        InvokeRepeating("GeneratePointCube", 2.0f, interval * 1.5f); 
    }

    private void FixedUpdate()
    {
        if (Time.fixedTime - timeOfLastCubeGeneration > interval)
        {
            int random = Random.Range(0, maxNumberOfGenerations);
            for (int i=0; i<=random; i++) GenerateEnemyCube();
            timeOfLastCubeGeneration = Time.fixedTime;
        }

        if (Time.fixedTime - timeOfLastIntervalUpdate > 10f && interval > 0.5f)
        {
            interval -= 0.25f;
            
            if (interval == 4.0f) maxNumberOfGenerations += 1;
            if (interval == 2.5f) maxNumberOfGenerations += 1;
            if (interval == 1.0f) maxNumberOfGenerations += 1;

            timeOfLastIntervalUpdate = Time.fixedTime;
        }
    }

    private void GenerateEnemyCube()
    {
        Vector3 position = new Vector3(
            Random.Range(platform.localScale.x / 2 - 2, platform.localScale.x / 2) * (Random.Range(0, 2) * 2 - 1), 
            0.5f, 
            Random.Range(platform.localScale.y / 2 - 2, platform.localScale.y / 2) * (Random.Range(0, 2) * 2 - 1)
        );
        Quaternion rotation = Quaternion.LookRotation(player.transform.position - position, Vector3.up);
        Instantiate(enemyCubePrefab, position, rotation);
    }

    private void GeneratePointCube()
    {
        Vector3 position = new Vector3(
            Random.Range(-platform.localScale.x / 2, platform.localScale.x / 2),
            0.75f,
            Random.Range(-platform.localScale.y / 2, platform.localScale.y / 2)
        );
        Instantiate(pointCubePrefab, position, transform.rotation);
    }
}
