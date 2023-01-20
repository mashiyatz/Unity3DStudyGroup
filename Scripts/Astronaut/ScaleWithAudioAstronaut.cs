using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithAudioAstronaut : MonoBehaviour
{
    public int index;
    public float threshold;
    public float timeToTransition;
    public float interval;
    public float restSmoothTime;

    private float previousAudioValue;
    private float audioValue;
    private float timeOfTransition;
    private Renderer rend;
    public Color restColor;
    public Color peakColor;

    private void Start()
    {
        timeOfTransition = Time.time;
        rend = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        previousAudioValue = audioValue;
        audioValue = GenerateSoundField.spectrum[index] < 0 ? 0 : GenerateSoundField.spectrum[index] * 100;

        if (Mathf.Abs(audioValue - previousAudioValue) > threshold)
        {
            if (Time.time - timeOfTransition > interval)
            {
                StopCoroutine(MoveToScale());
                StartCoroutine(MoveToScale());
                timeOfTransition = Time.time;
            }
        }

        rend.material.color = Color.Lerp(restColor, peakColor, Mathf.Sqrt(audioValue));
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, restSmoothTime * Time.deltaTime);
    }

    IEnumerator MoveToScale()
    {
        float currentScale = transform.localScale.y;
        float initialScale = currentScale;
        float targetScale = 1 + Mathf.Sqrt(audioValue);
        float timeAtStart = Time.time;

        while (currentScale != targetScale)
        {
            currentScale = Mathf.Lerp(initialScale, targetScale, (Time.time - timeAtStart) / timeToTransition);
            transform.localScale = new Vector3(1, currentScale, 1);
            yield return null;
        }
    }
}
