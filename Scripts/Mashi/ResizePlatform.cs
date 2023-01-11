using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizePlatform : MonoBehaviour
{
    public float maxScale;
    public float minScale;
    public float period;
    private float timerStart;
    private bool isIncreasing;
    private float scale; 

    void Start()
    {
        maxScale = 25f;
        minScale = 5f;
        period = 10f;
        timerStart = Time.fixedTime; 
        transform.localScale = new Vector3(maxScale, maxScale, 1);
        isIncreasing = false;
    }

    void FixedUpdate()
    {
        if (Time.fixedTime - timerStart >= period)
        {
            timerStart = Time.fixedTime;
            isIncreasing = !isIncreasing;
        }

        if (isIncreasing) scale = Mathf.Lerp(minScale, maxScale, (Time.fixedTime - timerStart) / period);
        else if (!isIncreasing) scale = Mathf.Lerp(maxScale, minScale, (Time.fixedTime - timerStart) / period);

        transform.localScale = new Vector3(scale, scale, 1);
    }
}
