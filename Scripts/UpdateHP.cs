using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHP : MonoBehaviour
{
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = PlayerControl.currentHP / PlayerControl.maxHP;
    }
}
