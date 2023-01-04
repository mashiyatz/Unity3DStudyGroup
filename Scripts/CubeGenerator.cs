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

    private float newIntervalTime;
    private float cubeGenerationTime;

    void Start()
    {
        newIntervalTime = Time.fixedTime;
        cubeGenerationTime = Time.fixedTime;
        InvokeRepeating("GeneratePointCube", 2.0f, interval * 1.5f); 
    }

    private void FixedUpdate()
    {
        if (Time.fixedTime - cubeGenerationTime > interval)
        {
            GenerateEnemyCube();
            cubeGenerationTime = Time.fixedTime;
        }

        if (Time.fixedTime - newIntervalTime > 10f)
        {
            interval -= 0.1f;
            newIntervalTime = Time.fixedTime;
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
