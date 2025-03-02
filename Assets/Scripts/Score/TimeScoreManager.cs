using UnityEngine;
using TMPro;

/// <summary>
/// Manages a timer that tracks the player's survival time and displays it on the UI.
/// </summary>
public class TimeScoreManager : MonoBehaviour
{
    /// <summary>
    /// The current timer value in seconds.
    /// </summary>
    private float timer;

    /// <summary>
    /// Indicates whether the timer is currently running.
    /// </summary>
    private bool isRunning;

    /// <summary>
    /// Reference to the TextMeshProUGUI component used to display the timer.
    /// </summary>
    public TextMeshProUGUI timerText;

    /// <summary>
    /// Initializes the timer and starts it running.
    /// </summary>
    void Start()
    {
        timer = 0f; // Initialize the timer to 0
        isRunning = true; // Start the timer
    }

    /// <summary>
    /// Updates the timer every frame if it is running.
    /// </summary>
    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime; // Increment the timer by the time passed since the last frame
            DisplayTime(timer); // Update the displayed time
        }
    }

    /// <summary>
    /// Retrieves the current timer value.
    /// </summary>
    /// <returns>The current timer value in seconds.</returns>
    public float GetTimer()
    {
        return timer;
    }

    /// <summary>
    /// Stops the timer from running.
    /// </summary>
    public void StopTimer()
    {
        isRunning = false;
    }

    /// <summary>
    /// Resets the timer to 0 and starts it running again.
    /// </summary>
    public void ResetTimer()
    {
        timer = 0f; // Reset the timer
        isRunning = true; // Start the timer again
    }

    /// <summary>
    /// Displays the current time on the TextMeshPro UI element.
    /// Formats the time in seconds if under 60 seconds, or in minutes and seconds if over 60 seconds.
    /// </summary>
    /// <param name="time">The current time in seconds.</param>
    private void DisplayTime(float time)
    {
        if (time < 60f)
        {
            // Display in seconds (less than 60 seconds)
            timerText.text = "Time Survived for: " + Mathf.FloorToInt(time).ToString() + "s";
        }
        else
        {
            // Display in minutes and seconds
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            timerText.text = string.Format("Time Survived for: {0:D2}min {1:D2}s", minutes, seconds);
        }
    }
}