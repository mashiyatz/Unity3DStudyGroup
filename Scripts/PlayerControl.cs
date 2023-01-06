using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public int method;

    public GameObject particlePrefab;
    public GameObject restartButton;
    private Rigidbody rb;

    public static AudioSource playerAudio;

    public static float maxHP = 10f;
    public static float currentHP;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        currentHP = maxHP;
        // rb.isKinematic = true;
        // if kinematic is true, not affected by gravity
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) method = method + 1 > 3 ? 1 : method + 1;

        if (method == 1) MoveCube();
        else if (method == 2) MoveAndRotateCube();
    }

    private void FixedUpdate()
    {
        // For difference between Update() and FixedUpdate() see:
        // https://stackoverflow.com/questions/34447682/what-is-the-difference-between-update-fixedupdate-in-unity
        if (rb.velocity.y <= -20f || currentHP <= 0)
        {
            Instantiate(particlePrefab, transform.position, transform.rotation);
            restartButton.SetActive(true);
            Destroy(gameObject);
        }

        if (method == 3) MoveCubeWithRigidBody();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 vel = collision.gameObject.GetComponent<Rigidbody>().velocity;
            rb.AddForce(vel, ForceMode.Impulse);
        }
    }

    private void MoveCube()
    {
        Vector3 direction = Vector3.zero;

        // Try using GeyKeyDown() instead of GetKey() -- what is the difference, and why? 
        if (Input.GetKey(KeyCode.W)) direction += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) direction += Vector3.back;
        if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) direction += Vector3.right;

        direction.Normalize();
        transform.position += speed * Time.deltaTime * direction;
    }

    private void MoveAndRotateCube()
    {
        Vector3 angle = Vector3.zero;
        Vector3 straight = Vector3.zero;

        if (Input.GetKey(KeyCode.A)) angle += Vector3.up;
        if (Input.GetKey(KeyCode.D)) angle += Vector3.down;
        transform.eulerAngles += rotationSpeed * Time.deltaTime * angle;

        // What happens if we use Vector3.forward and Vector3.background like before?
        if (Input.GetKey(KeyCode.W)) straight += transform.forward;
        if (Input.GetKey(KeyCode.S)) straight += -transform.forward;
        transform.position += speed * Time.deltaTime * straight;
    }

    private void MoveCubeWithRigidBody()
    {
        Vector3 angle;
        Vector3 direction = Vector3.zero;
        Vector3 moveTowards = transform.position;

        if (Input.GetKey(KeyCode.W)) direction += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) direction += Vector3.back;
        if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) direction += Vector3.right;

        direction.Normalize();

        if (direction != Vector3.zero)
        {
            angle = new Vector3(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0);
            Quaternion deltaRotation = Quaternion.RotateTowards(rb.rotation, Quaternion.Euler(angle), rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(deltaRotation);
        }

        moveTowards += speed * Time.fixedDeltaTime * direction;
        rb.MovePosition(moveTowards);
    }
}
