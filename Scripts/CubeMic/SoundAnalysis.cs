using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnalysis : MonoBehaviour
{
    private int spectrumSize;
    private AudioClip microphoneClip;
    private float[] spectrum;

    public static float loudness;

    // public AudioSource audioSource;

    void Start()
    {
        spectrumSize = 64;
        spectrum = new float[spectrumSize];
        microphoneClip = Microphone.Start(Microphone.devices[0], true, 1, AudioSettings.outputSampleRate);
        // audioSource.clip = microphoneClip;
        // audioSource.loop = true;
        // audioSource.Play();
    }

    void Update()
    {
        GetAudioDataFromMic();
        // audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        GetLoudness();
    }

    void GetAudioDataFromMic()
    {
        int clipPosition = Microphone.GetPosition(Microphone.devices[0]);
        int startPosition = clipPosition - spectrumSize;
        if (startPosition < 0) return;
        microphoneClip.GetData(spectrum, startPosition);
    }

    void GetLoudness()
    {
        float totalLoudness = 0;
        for (int i = 0; i < spectrum.Length; i++)
        {
            totalLoudness += Mathf.Abs(spectrum[i]);
        }

        loudness = totalLoudness / spectrumSize;
    }
}
