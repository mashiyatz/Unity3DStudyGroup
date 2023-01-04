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
    public PlayerMoveCube player;
    public ResizePlatform resize;
    public FollowPlayer follow;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnRetryButtonClick);
        panel.SetActive(true);
        generator.enabled = false;
        player.enabled = false;
        resize.enabled = false;
        follow.enabled = false;
    }

    void OnRetryButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
