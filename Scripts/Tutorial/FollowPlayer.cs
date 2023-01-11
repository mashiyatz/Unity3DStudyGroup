using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform cube;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position;
    }

    void Update()
    {
        transform.position = cube.position + offset;
    }
}
