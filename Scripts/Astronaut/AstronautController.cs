using UnityEngine;
using System.Collections;

public class AstronautController : MonoBehaviour 
{
	private Animator anim;
	private CharacterController controller;

	public float speed = 5.0f;
	public float turnSpeed = 360.0f;
	private Vector3 moveDirection = Vector3.zero;
	public float gravity = 20.0f;
	public float jumpStrength = 200f;

	void Start () {
		controller = GetComponent<CharacterController>();
		anim = gameObject.GetComponentInChildren<Animator>();
	}

	void Update () {

		if (transform.position.y < -20f)
        {
			transform.position = new Vector3(0, 3f, 0);
			return;
        }

		if (Input.GetKey(KeyCode.Space)) 
        {
			moveDirection.y += jumpStrength * Time.deltaTime;
        }

		if (controller.velocity.y < -3f)
		{
			anim.SetInteger("AnimationPar", 2);
		} 
		else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) 
		{
			anim.SetInteger("AnimationPar", 1);
		} 
		else 
		{
			anim.SetInteger("AnimationPar", 0);
		}

        if (controller.isGrounded)
        {
            moveDirection = Input.GetAxis("Vertical") * speed * transform.forward;
        }

        float turn = Input.GetAxis("Horizontal");
		transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
		controller.Move(moveDirection * Time.deltaTime);
		moveDirection.y -= gravity * Time.deltaTime;
	}

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Rock"))
        {
			Debug.Log("check");
			hit.collider.gameObject.GetComponent<RockCubeBehavior>().TriggerCollision();
        } 
    }
}
