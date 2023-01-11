using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerButtonPress : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Return)) button.onClick.Invoke();
    }
}
