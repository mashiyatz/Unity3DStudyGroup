using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScore : MonoBehaviour
{
    public static int score;
    private Text textbox;

    void Start()
    {
        score = 0;
        textbox = GetComponent<Text>();
    }

    void Update()
    {
        textbox.text = score.ToString();
    }
}
