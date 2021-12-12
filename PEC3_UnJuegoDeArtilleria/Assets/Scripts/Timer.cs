using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float timeLeft = 5;
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        GameplayManager.OnTurnChange += TimerReset;
        InvokeRepeating(nameof(UpdateTimer), 0, 1);
    }

    private void UpdateTimer()
    {
        text.text = timeLeft.ToString();
        timeLeft--;
    }

    private void TimerReset()
    {
        timeLeft = 10;
    }

    private void OnDestroy()
    {
        GameplayManager.OnTurnChange -= TimerReset;
    }
}
