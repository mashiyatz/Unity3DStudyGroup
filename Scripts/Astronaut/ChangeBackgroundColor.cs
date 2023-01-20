using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackgroundColor : MonoBehaviour
{
    private Camera cameraComponent;
    public Color colorA;
    public Color colorB;
    public float period;

    void Start()
    {
        cameraComponent = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraComponent.backgroundColor = Color.Lerp(colorA, colorB, Mathf.Sin(Time.time / period));
    }
}
