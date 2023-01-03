using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public PlayerMoveCube controller;
    public Transform player;
    public Transform cameraTransform;
    public Vector3 cameraDistance;
    public Vector3 cameraAngle;

    void Start()
    {
        cameraTransform.SetLocalPositionAndRotation(cameraDistance, Quaternion.Euler(cameraAngle));
    }

    void Update()
    {
        if (controller.method == 2) transform.SetPositionAndRotation(player.position, Quaternion.Euler(player.eulerAngles));
        else
        {
            if (transform.eulerAngles != Vector3.zero) transform.eulerAngles = Vector3.zero; 
            transform.position = player.position;
        }
    }
}
