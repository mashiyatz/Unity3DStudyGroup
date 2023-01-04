using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateScore : MonoBehaviour
{
    public static int score;
    private TextMeshProUGUI textbox;


    void Start()
    {
        score = 0;
        textbox = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (score < 0) score = 0;
        textbox.text = string.Format("{0}", score);
    }
}
