using UnityEngine;
using TMPro;

/// <summary>
/// Manages the player's health storage and provides methods to interact with it.
/// Implements the Singleton pattern to ensure only one instance exists in the game.
/// </summary>
public class PlayerHealthStore : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the PlayerHealthStore class.
    /// </summary>
    public static PlayerHealthStore Instance { get; private set; }

    [SerializeField]
    /// <summary>
    /// The player's current health.
    /// </summary>
    private int playerHealth;

    /// <summary>
    /// Initializes the PlayerHealthStore instance and ensures it follows the Singleton pattern.
    /// Sets the player's initial health to 100.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set this as the Singleton instance
            playerHealth = 100; // Initialize player health to 100
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    /// <summary>
    /// Retrieves the player's current health.
    /// </summary>
    /// <returns>The current health value.</returns>
    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    /// <summary>
    /// Sets the player's health to a specific value.
    /// </summary>
    /// <param name="value">The new health value to set.</param>
    public void SetPlayerHealth(int value)
    {
        playerHealth = value;
        // Debug.Log($"Player health updated to: {playerHealth}");
    }

    /// <summary>
    /// Increases the player's health by a specified amount.
    /// Ensures the health does not exceed the player's maximum health.
    /// </summary>
    /// <param name="amount">The amount to add to the player's health.</param>
    public void IncrementPlayerHealth(int amount)
    {
        playerHealth += amount;

        // Clamp the player's health to the maximum health value
        if (playerHealth > UpgradeStore.Instance.GetPlayerMaxHealth())
        {
            playerHealth = UpgradeStore.Instance.GetPlayerMaxHealth();
        }
    }
}