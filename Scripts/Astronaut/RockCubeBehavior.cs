using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCubeBehavior : MonoBehaviour
{
    public float delay;
    public GameObject particlePrefab;

    void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    public void TriggerCollision()
    {
        Instantiate(particlePrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Instantiate(particlePrefab, transform.position, transform.rotation);
            // PlayerControl.playerAudio.Play();
            // ScoreKeeper.score += 1;
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
