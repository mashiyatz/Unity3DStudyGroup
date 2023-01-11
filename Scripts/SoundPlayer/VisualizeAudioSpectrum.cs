using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizeAudioSpectrum : MonoBehaviour
{
    public GameObject cubePrefab;
    public static float[] spectrum;
    private int spectrumSize;

    public AudioSource audioSource;
    private AudioClip microphoneClip;

    public Toggle micToggle;
    public Button playNextButton;
    public AudioClip[] playlist;

    private int playlistIndex = 0;

    void Start()
    {
        spectrumSize = 64;
        spectrum = new float[spectrumSize];
        playNextButton.onClick.AddListener(PlayNext);
        GenerateCubePlane();
        MicrophoneToAudioClip();

        audioSource.clip = playlist[playlistIndex];
        audioSource.Play();
    }

    void Update()
    {
        if (micToggle.isOn) GetAudioDataFromMic();
        else AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
    }

    void PlayNext()
    {
        playlistIndex += 1;
        if (playlistIndex >= playlist.Length) playlistIndex = 0;
        audioSource.clip = playlist[playlistIndex];
        audioSource.Play();
    }

    void MicrophoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    void GetAudioDataFromMic()
    {
        int clipPosition = Microphone.GetPosition(Microphone.devices[0]);
        int startPosition = clipPosition - spectrumSize;
        if (startPosition < 0) return;
        microphoneClip.GetData(spectrum, startPosition);
    }

    void GenerateCubePlane()
    {
        int index = 0;
        for (int i = -4; i < 4; i++)
        {
            for (int j = -4; j < 4; j++)
            {
                Vector3 pos = new(transform.position.x + i, 0, transform.position.z + j);
                GameObject go = Instantiate(cubePrefab, pos, transform.rotation, transform);
                go.GetComponent<ScaleWithAudio>().index = index;
                index += 1;
            }
        }
    }
}
