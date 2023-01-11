using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureBehavior : MonoBehaviour
{
    public float interval;
    private float timeAtStart;

    void Start()
    {
        timeAtStart = Time.time;
    }

    void Update()
    {
        if (Time.time - timeAtStart >= interval) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            UpdateScore.score += 1;
            Destroy(gameObject);
        }
    }
}
