using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    public float spectrumValue;
    public float bias;
    public float timeStep;
    public float timeToBeat;
    public float restSmoothTime;

    public int spectrumSize;

    public Vector3 beatScale;
    public Vector3 restScale;

    private float previousAudioValue;
    private float audioValue;
    private float timer;
    private bool isBeat;

    void Start()
    {
        if ((int)(Mathf.Log(spectrumSize) / Mathf.Log(2)) != Mathf.Log(spectrumSize) / Mathf.Log(2) || spectrumSize < 64) spectrumSize = 64; 
    }

    void Update()
    {
        float[] spectrum = new float[spectrumSize];
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris); // consider replacing with source to isolate sound
        if (spectrum != null && spectrum.Length > 0) spectrumValue = spectrum[0] * 100;

        OnUpdate();
    }

    private IEnumerator MoveToScale(Vector3 targetScale)
    {
        Vector3 currentScale = transform.localScale;
        Vector3 initialScale = currentScale;
        float timer = 0;

        while (currentScale != targetScale)
        {
            currentScale = Vector3.Lerp(initialScale, targetScale, timer / timeToBeat);
            timer += Time.deltaTime;

            transform.localScale = currentScale;
            yield return null;
        }

        isBeat = false;
    }

    public void OnUpdate()
    {
        previousAudioValue = audioValue;
        audioValue = spectrumValue;

        if ((previousAudioValue > bias && audioValue <= bias) || (previousAudioValue <= bias && audioValue > bias))
        {
            if (timer > timeStep)
            {
                timer = 0;
                isBeat = true;
                StopCoroutine("MoveToScale");
                StartCoroutine("MoveToScale", beatScale);
            }
        }

        timer += Time.deltaTime;

        if (isBeat) return;

        transform.localScale = Vector3.Lerp(transform.localScale, restScale, restSmoothTime * Time.deltaTime);
    }
}
