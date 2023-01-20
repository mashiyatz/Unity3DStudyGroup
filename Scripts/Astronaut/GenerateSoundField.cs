using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSoundField : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject astronaut;

    public static float[] spectrum;
    private int spectrumSize;
    private AudioClip microphoneClip;

    public GameObject rockPrefab;
    public float interval;
    public float maxDistance;
    private float timeSinceLastRock;

    private void Start()
    {
        GenerateSoundCubeField();
        Instantiate(astronaut, new Vector3(0, 0.5f, 0), Quaternion.Euler(Vector3.zero));

        spectrumSize = 64;
        spectrum = new float[spectrumSize];

        microphoneClip = Microphone.Start(Microphone.devices[0], true, 20, AudioSettings.outputSampleRate);
        timeSinceLastRock = Time.time;
    }

    private void Update()
    {
        GetAudioDataFromMic();
        GenerateRock();
    }

    void GetAudioDataFromMic()
    {
        int clipPosition = Microphone.GetPosition(Microphone.devices[0]);
        int startPosition = clipPosition - spectrumSize;
        if (startPosition < 0) return;
        microphoneClip.GetData(spectrum, startPosition);
    }

    void GenerateRock()
    {
        if (Time.time - timeSinceLastRock >= interval)
        {
            Vector3 pos = new(Random.Range(-maxDistance, maxDistance), 3.0f, Random.Range(-maxDistance, maxDistance));
            Instantiate(rockPrefab, pos, transform.rotation, transform);
            timeSinceLastRock = Time.time;
        }
    }

    void GenerateSoundCubeField()
    {
        int index = 0;
        for (int i = -4; i < 4; i++)
        {
            for (int j = -4; j < 4; j++)
            {
                Vector3 pos = new(transform.position.x + i, 0, transform.position.z + j);
                GameObject go = Instantiate(cubePrefab, pos, transform.rotation, transform);
                go.GetComponent<ScaleWithAudioAstronaut>().index = index;
                index += 1;
            }
        }
    }
}
