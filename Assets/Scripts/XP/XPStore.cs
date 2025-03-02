using UnityEngine;
using TMPro;

/// <summary>
/// Manages the player's XP (experience points) storage and provides methods to interact with it.
/// Implements the Singleton pattern to ensure only one instance exists in the game.
/// </summary>
public class XPStore : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the XPStore class.
    /// </summary>
    public static XPStore Instance { get; private set; }

    /// <summary>
    /// The player's current XP value.
    /// </summary>
    private int xp;

    /// <summary>
    /// Initializes the XPStore instance and ensures it follows the Singleton pattern.
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
    /// Retrieves the player's current XP.
    /// </summary>
    /// <returns>The current XP value.</returns>
    public int GetXP()
    {
        return xp;
    }

    /// <summary>
    /// Sets the player's XP to a specific value.
    /// </summary>
    /// <param name="value">The new XP value to set.</param>
    public void SetXP(int value)
    {
        xp = value;
    }

    /// <summary>
    /// Increases the player's XP by a specified amount.
    /// </summary>
    /// <param name="amount">The amount of XP to add.</param>
    public void IncrementXP(int amount)
    {
        xp += amount;
    }
}