using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public Transform platform;
    public float interval;
    public Transform player;

    void Start()
    {
        InvokeRepeating("GenerateCube", 2.0f, interval);
    }

    private void GenerateCube()
    {
        Vector3 position = new Vector3(Random.Range(0, platform.localScale.x / 2), 0.5f, Random.Range(0, platform.localScale.y / 2));
        Quaternion rotation = Quaternion.LookRotation(player.transform.position - position, Vector3.up);
        Instantiate(cubePrefab, position, rotation);
    }
}
