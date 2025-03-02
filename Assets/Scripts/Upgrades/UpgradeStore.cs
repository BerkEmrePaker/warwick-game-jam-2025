using UnityEngine;
using TMPro;
using System.Data;

/// <summary>
/// Manages and stores player upgrade stats such as attack radius, speed, health, and attack properties.
/// Implements the Singleton pattern to ensure only one instance exists in the game.
/// </summary>
public class UpgradeStore : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the UpgradeStore class.
    /// </summary>
    public static UpgradeStore Instance { get; private set; }

    /// <summary>
    /// The player's current attack radius.
    /// </summary>
    private float attackRadius;

    /// <summary>
    /// The player's current movement speed.
    /// </summary>
    private float playerSpeed;

    /// <summary>
    /// The player's current maximum health.
    /// </summary>
    private int playerMaxHealth;

    /// <summary>
    /// The player's current health regeneration cooldown.
    /// </summary>
    private float playerHealthCooldown;

    /// <summary>
    /// The player's current attack rate.
    /// </summary>
    private float attackRate;

    /// <summary>
    /// The player's current attack damage.
    /// </summary>
    private float attackDamage;

    [Header("Initial Values")]
    [SerializeField]
    /// <summary>
    /// The initial attack radius value.
    /// </summary>
    private float initialAttackRadius;

    [SerializeField]
    /// <summary>
    /// The initial player speed value.
    /// </summary>
    private float initialPlayerSpeed;

    [SerializeField]
    /// <summary>
    /// The initial player max health value.
    /// </summary>
    private int initialPlayerMaxHealth;

    [SerializeField]
    /// <summary>
    /// The initial player health cooldown value.
    /// </summary>
    private float initialPlayerHealthCooldown;

    [SerializeField]
    /// <summary>
    /// The initial attack rate value.
    /// </summary>
    private float initialAttackRate;

    [SerializeField]
    /// <summary>
    /// The initial attack damage value.
    /// </summary>
    private float initialAttackDamage;

    /// <summary>
    /// Initializes the UpgradeStore instance and sets initial values for all stats.
    /// Ensures the Singleton pattern is followed by destroying duplicate instances.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set this as the Singleton instance
            attackRadius = initialAttackRadius;
            playerSpeed = initialPlayerSpeed;
            playerMaxHealth = initialPlayerMaxHealth;
            playerHealthCooldown = initialPlayerHealthCooldown;
            attackRate = initialAttackRate;
            attackDamage = initialAttackDamage;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    /// <summary>
    /// Retrieves the player's current attack radius.
    /// </summary>
    /// <returns>The current attack radius.</returns>
    public float GetAttackRadius()
    {
        return attackRadius;
    }

    /// <summary>
    /// Sets the player's attack radius to a specific value.
    /// </summary>
    /// <param name="value">The new attack radius value.</param>
    public void SetAttackRadius(float value)
    {
        attackRadius = value;
    }

    /// <summary>
    /// Retrieves the player's current movement speed.
    /// </summary>
    /// <returns>The current player speed.</returns>
    public float GetPlayerSpeed()
    {
        return playerSpeed;
    }

    /// <summary>
    /// Sets the player's movement speed to a specific value.
    /// </summary>
    /// <param name="value">The new player speed value.</param>
    public void SetPlayerSpeed(float value)
    {
        playerSpeed = value;
    }

    /// <summary>
    /// Retrieves the player's current maximum health.
    /// </summary>
    /// <returns>The current player max health.</returns>
    public int GetPlayerMaxHealth()
    {
        return playerMaxHealth;
    }

    /// <summary>
    /// Sets the player's maximum health to a specific value.
    /// </summary>
    /// <param name="value">The new player max health value.</param>
    public void SetPlayerMaxHealth(int value)
    {
        playerMaxHealth = value;
    }

    /// <summary>
    /// Retrieves the player's current health regeneration cooldown.
    /// </summary>
    /// <returns>The current player health cooldown.</returns>
    public float GetPlayerHealthCooldown()
    {
        return playerHealthCooldown;
    }

    /// <summary>
    /// Increments the player's health regeneration cooldown by a specified amount.
    /// Ensures the cooldown does not go below a minimum value of 0.5.
    /// </summary>
    /// <param name="amount">The amount to increment the cooldown by.</param>
    public void IncrementPlayerHealthCooldown(float amount)
    {
        if (playerHealthCooldown + amount <= 0.5)
        {
            playerHealthCooldown = 0.5f; // Set to minimum value
        }
        else
        {
            playerHealthCooldown += amount;
        }
    }

    /// <summary>
    /// Retrieves the player's current attack rate.
    /// </summary>
    /// <returns>The current attack rate.</returns>
    public float GetAttackRate()
    {
        return attackRate;
    }

    /// <summary>
    /// Sets the player's attack rate to a specific value.
    /// </summary>
    /// <param name="value">The new attack rate value.</param>
    public void SetAttackRate(float value)
    {
        attackRate = value;
    }

    /// <summary>
    /// Increments the player's attack rate by a specified amount.
    /// Ensures the attack rate does not exceed a maximum value of 3.5.
    /// </summary>
    /// <param name="amount">The amount to increment the attack rate by.</param>
    public void IncrementAttackRate(float amount)
    {
        if (attackRate + amount > 3.5f)
        {
            attackRate = 3.5f; // Set to maximum value
        }
        else
        {
            attackRate += amount;
        }
    }

    /// <summary>
    /// Retrieves the player's current attack damage.
    /// </summary>
    /// <returns>The current attack damage.</returns>
    public float GetAttackDamage()
    {
        return attackDamage;
    }

    /// <summary>
    /// Sets the player's attack damage to a specific value.
    /// </summary>
    /// <param name="value">The new attack damage value.</param>
    public void SetAttackDamage(float value)
    {
        attackDamage = value;
    }

    /// <summary>
    /// Increments the player's attack damage by a specified amount.
    /// </summary>
    /// <param name="amount">The amount to increment the attack damage by.</param>
    public void IncrementAttackDamage(float amount)
    {
        attackDamage += amount;
    }
}