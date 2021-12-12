using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float timeLeft = 5; //float to define the time left for each turn
    private Text text; //Text component

    /// <summary>
    /// Start method where we subscribe to the OnTurnChange Action to reset the timer
    /// And Invoke the update timer every second
    /// </summary>
    private void Start()
    {
        text = GetComponent<Text>();
        GameplayManager.OnTurnChange += TimerReset;
        InvokeRepeating(nameof(UpdateTimer), 0, 1);
    }

    /// <summary>
    /// Method to update the timer text every second
    /// </summary>
    private void UpdateTimer()
    {
        text.text = timeLeft.ToString();
        timeLeft--;
    }

    /// <summary>
    /// Method to reset the timer to the amount of time a turn takes
    /// </summary>
    private void TimerReset()
    {
        timeLeft = 10;
    }

    /// <summary>
    /// OnDestroy method to unsubscribe from the OnTurnChange action
    /// </summary>
    private void OnDestroy()
    {
        GameplayManager.OnTurnChange -= TimerReset;
    }
}
