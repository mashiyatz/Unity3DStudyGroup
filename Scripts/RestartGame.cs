using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    private Button button;
    public GameObject panel;

    public CubeGenerator generator;
    public PlayerControl player;
    public ResizePlatform resize;
    public FollowPlayer follow;
    public AudioSource canvasAudioSource;
    public GameObject healthBar;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnRetryButtonClick);

        panel.SetActive(true);
        generator.enabled = false;
        player.enabled = false;
        resize.enabled = false;
        follow.enabled = false;
        healthBar.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Return)) button.onClick.Invoke();
    }

    void OnRetryButtonClick()
    {
        canvasAudioSource.Play();
        button.enabled = false;
        StartCoroutine(WaitForAudio());
    }

    IEnumerator WaitForAudio()
    {
        while (canvasAudioSource.isPlaying) yield return null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
