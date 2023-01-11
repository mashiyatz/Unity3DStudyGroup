using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithAudio : MonoBehaviour
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
    private Color pink;

    private void Start()
    {
        timeOfTransition = Time.time;
        rend = GetComponentInChildren<Renderer>();
        pink = new Color(1, 0.85f, 0.85f, 1);
    }

    void Update()
    {
        previousAudioValue = audioValue;
        audioValue = VisualizeAudioSpectrum.spectrum[index] < 0 ? 0 : VisualizeAudioSpectrum.spectrum[index] * 100;
        
        if (Mathf.Abs(audioValue - previousAudioValue) > threshold)
        {
            if (Time.time - timeOfTransition > interval)
            {
                StopCoroutine(MoveToScale());
                StartCoroutine(MoveToScale());
                timeOfTransition = Time.time;
            }
        }

        rend.material.color = Color.Lerp(pink, Color.red, Mathf.Sqrt(audioValue));
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
