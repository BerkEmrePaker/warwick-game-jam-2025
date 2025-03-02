using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Displays the player's high score on the UI, formatted in minutes and seconds.
/// </summary>
public class DisplayHighScore : MonoBehaviour
{
    /// <summary>
    /// Reference to the TextMeshProUGUI component used to display the high score.
    /// </summary>
    public TextMeshProUGUI highScoreText;

    /// <summary>
    /// Initializes the high score display by retrieving and formatting the high score from PlayerPrefs.
    /// </summary>
    void Start()
    {
        DisplayHighScoreText();
    }

    /// <summary>
    /// Retrieves the high score from PlayerPrefs and updates the UI text.
    /// If no high score is found, it defaults to 0.
    /// </summary>
    private void DisplayHighScoreText()
    {
        highScoreText.text = PlayerPrefs.HasKey("HighScore") ? getHighScoreString(PlayerPrefs.GetInt("HighScore")) : getHighScoreString(0);
    }

    /// <summary>
    /// Formats the high score time into a string representation (minutes and seconds).
    /// If the time is 0, it sets the high score in PlayerPrefs to 0.
    /// </summary>
    /// <param name="time">The high score time in seconds.</param>
    /// <returns>A formatted string representing the high score.</returns>
    private String getHighScoreString(int time)
    {
        if (time == 0)
        {
            PlayerPrefs.SetInt("HighScore", 0); // Set high score to 0 if no high score exists
        }

        // Convert time into minutes and seconds
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        // Format the time as "High Score: XXmin YYs"
        return string.Format("High Score: {0:D2}min {1:D2}s", minutes, seconds);
    }
}