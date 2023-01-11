using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    public static int score;
    private TextMeshProUGUI textbox;

    void Start()
    {
        score = 0;
        textbox = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (score < 0) score = 0;
        textbox.text = string.Format("{0}", score);
    }
}
