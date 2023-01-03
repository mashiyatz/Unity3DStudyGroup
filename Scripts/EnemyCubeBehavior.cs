using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCubeBehavior : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 forward;
    public float speed;
    private float timeOfLeavingPlatform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        forward = transform.forward;
        timeOfLeavingPlatform = Time.fixedTime;
    }

    void FixedUpdate()
    {
        if (Time.fixedTime - timeOfLeavingPlatform > 5.0f)
        {
            Debug.Log(Time.fixedTime - timeOfLeavingPlatform); 
            Destroy(gameObject);
        }
        rb.velocity = forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) Destroy(gameObject);
        if (collision.collider.CompareTag("Platform")) timeOfLeavingPlatform = Time.fixedTime;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Platform")) timeOfLeavingPlatform = Time.fixedTime;
    }
}
