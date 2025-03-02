using UnityEngine;
using TMPro;

/// <summary>
/// Manages the player's score storage and provides methods to interact with it.
/// Implements the Singleton pattern to ensure only one instance exists in the game.
/// </summary>
public class ScoreStore : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the ScoreStore class.
    /// </summary>
    public static ScoreStore Instance { get; private set; }

    /// <summary>
    /// The player's current score.
    /// </summary>
    private int score;

    /// <summary>
    /// Initializes the ScoreStore instance and ensures it follows the Singleton pattern.
    /// Destroys any duplicate instances.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set this as the Singleton instance
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    /// <summary>
    /// Retrieves the player's current score.
    /// </summary>
    /// <returns>The current score value.</returns>
    public int GetScore()
    {
        return score;
    }

    /// <summary>
    /// Sets the player's score to a specific value.
    /// </summary>
    /// <param name="value">The new score value to set.</param>
    public void SetScore(int value)
    {
        score = value;
        // Debug.Log($"Score updated to: {score}");
    }

    /// <summary>
    /// Increases the player's score by a specified amount.
    /// </summary>
    /// <param name="amount">The amount to add to the score.</param>
    public void IncrementScore(int amount)
    {
        score += amount;
        // Debug.Log($"Score incremented to: {score}");
    }
}