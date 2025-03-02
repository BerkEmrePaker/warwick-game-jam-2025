using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the game's pause state, including toggling the pause menu and freezing/unfreezing the game.
/// </summary>
public class PauseManager : MonoBehaviour
{
    [Header("Menu Game Objects")]
    [Tooltip("Reference to the pause menu panel")]
    /// <summary>
    /// The GameObject representing the pause menu.
    /// </summary>
    public GameObject pauseMenu;

    [Tooltip("The Game Object that is required to check against before showing Pause Menu")]
    /// <summary>
    /// The GameObject representing the upgrade menu. The pause menu will not open if the upgrade menu is active.
    /// </summary>
    public GameObject upgradeMenu;

    /// <summary>
    /// Indicates whether the game is currently paused.
    /// </summary>
    private bool isPaused = false;

    /// <summary>
    /// Initializes the game as unpaused and ensures the pause menu is hidden.
    /// </summary>
    void Start()
    {
        // Start game as unpaused
        Time.timeScale = 1;
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Handles the input for toggling the pause state.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (!upgradeMenu.activeSelf)
        {
            // Pause/unpause the game
            TogglePause();
        }
    }

    /// <summary>
    /// Toggles the game's pause state.
    /// If the game is unpaused, it will pause the game and show the pause menu.
    /// If the game is paused, it will unpause the game and hide the pause menu.
    /// </summary>
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0; // Pause the game
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(true); // Show the pause menu
            }
        }
        else
        {
            Time.timeScale = 1; // Resume the game
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false); // Hide the pause menu
            }
        }
    }
}