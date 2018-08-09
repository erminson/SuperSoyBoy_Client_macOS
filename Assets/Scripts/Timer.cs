using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    private Text timerText;

    void Awake()
    {
        timerText = GetComponent<Text>();
    }

    void Update()
    {
        timerText.text = Math.Round((decimal)Time.timeSinceLevelLoad, 2).ToString();
    }
}
