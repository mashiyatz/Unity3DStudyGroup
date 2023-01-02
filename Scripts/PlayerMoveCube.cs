using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCube : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public int method;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if (method == 1)
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
        else if (method == 2)
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
    }

    private void FixedUpdate()
    {
        if (method == 3)
        {
            Vector3 angle;
            Vector3 direction = Vector3.zero;
            Vector3 moveTowards = transform.position;

            if (Input.GetKey(KeyCode.W)) direction += Vector3.forward; 
            if (Input.GetKey(KeyCode.S)) direction += Vector3.back; 
            if (Input.GetKey(KeyCode.A)) direction += Vector3.left; 
            if (Input.GetKey(KeyCode.D)) direction += Vector3.right; 

            direction.Normalize();

            if (Input.anyKey)
            {
                angle = new Vector3(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0);
                Quaternion deltaRotation = Quaternion.RotateTowards(rb.rotation, Quaternion.Euler(angle), rotationSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(deltaRotation);
            }

            moveTowards += speed * Time.fixedDeltaTime * direction;
            rb.MovePosition(moveTowards);
        }
    }
}
