using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithSound : MonoBehaviour
{
    public float sensitivity;
    public float threshold;
    public float interval;

    public Vector3 restSize;
    public Vector3 maxSize;

    private Renderer rend;
    public Color restColor;
    public Color maxColor;

    private float timeOfChange;

    private float currentLoudness;
    private float lastLoudness;

    void Start()
    {
        rend = GetComponent<Renderer>(); 
        rend.material.color = restColor;
        timeOfChange = Time.time;
        currentLoudness = 0;
    }

    void Update()
    {
        lastLoudness = currentLoudness;
        currentLoudness = SoundAnalysis.loudness * sensitivity;

        if (Mathf.Abs(currentLoudness - lastLoudness) < threshold)
        {
            currentLoudness = lastLoudness;
        } else
        {
            timeOfChange = Time.time;
        }

        rend.material.color = Color.Lerp(
            restColor, 
            maxColor, 
            Vector3.Distance(transform.localScale, restSize) / Vector3.Distance(maxSize, restSize)
        );

        Vector3 targetScale = Vector3.Lerp(restSize, maxSize, currentLoudness);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, (Time.time - timeOfChange) / interval);
    }
}
