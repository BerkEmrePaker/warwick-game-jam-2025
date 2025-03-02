using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the player's health regeneration over time based on the cooldown defined in the UpgradeStore.
/// </summary>
public class HealthRegen : MonoBehaviour
{
    /// <summary>
    /// Reference to the UpgradeStore singleton for retrieving health regeneration cooldown.
    /// </summary>
    private UpgradeStore upgradeStore;

    /// <summary>
    /// Reference to the PlayerHealthStore singleton for managing the player's health.
    /// </summary>
    private PlayerHealthStore playerHealthStore;

    /// <summary>
    /// Initializes references to the UpgradeStore and PlayerHealthStore singletons.
    /// </summary>
    void Awake()
    {
        upgradeStore = UpgradeStore.Instance;
        playerHealthStore = PlayerHealthStore.Instance;
    }

    /// <summary>
    /// Starts the health regeneration coroutine when the script is initialized.
    /// </summary>
    void Start()
    {
        StartCoroutine(healthRegen());
    }

    /// <summary>
    /// Continuously regenerates the player's health at regular intervals defined by the health cooldown.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    IEnumerator healthRegen()
    {
        while (true)
        {
            playerHealthStore.IncrementPlayerHealth(5); // Increment player health by 5
            yield return new WaitForSeconds(upgradeStore.GetPlayerHealthCooldown()); // Wait for the cooldown duration
        }
    }
}