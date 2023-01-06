using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCubeBehavior : MonoBehaviour
{
    public float rotationSpeed;
    public float floatSpeed;
    public float delay;
    public GameObject particlePrefab;

    void Start()
    {
        rotationSpeed = 180f;
        floatSpeed = 5f;
        delay = 5f;
        StartCoroutine(DestroyAfterDelay());
    }

    void FixedUpdate()
    {
        Vector3 position = transform.position;
        Vector3 rotation = transform.eulerAngles;
        position.y = 0.5f * Mathf.Sin(floatSpeed * Time.fixedTime) + 1.0f;
        rotation.y = transform.eulerAngles.y + rotationSpeed * Time.fixedDeltaTime;
        transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Instantiate(particlePrefab, transform.position, transform.rotation);
            PlayerControl.playerAudio.Play();
            UpdateScore.score += 1;
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

}
