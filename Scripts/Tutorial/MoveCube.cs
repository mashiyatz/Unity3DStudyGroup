using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveCube : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public GameObject gameOverPanel;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // TranslateCube();d
        if (rb.velocity.y <= -20) gameOverPanel.SetActive(true); 
        TurnAndTranslateCube();
    }

    void TurnAndTranslateCube()
    {
        Vector3 linearDirection = Vector3.zero;
        Vector3 angularDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) linearDirection += transform.forward;
        if (Input.GetKey(KeyCode.S)) linearDirection -= transform.forward;

        if (Input.GetKey(KeyCode.A)) angularDirection += Vector3.down;
        if (Input.GetKey(KeyCode.D)) angularDirection += Vector3.up;

        transform.position += speed * Time.fixedDeltaTime * linearDirection;
        transform.eulerAngles += rotationSpeed * Time.fixedDeltaTime * angularDirection;
    }

    void TranslateCube()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) direction += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) direction += Vector3.back;
        if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) direction += Vector3.right;

        direction.Normalize();

        Vector3 deltaPosition = speed * Time.deltaTime * direction;
        transform.position += deltaPosition;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
