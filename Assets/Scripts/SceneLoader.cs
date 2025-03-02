using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Provides static methods for loading different scenes in the game.
/// This class is used to manage scene transitions, such as loading the game scene, main menu, or info scene.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Loads the "GameScene" by name.
    /// This method is typically called to start or restart the game.
    /// </summary>
    public static void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Loads the "MainMenu" scene by name.
    /// This method is typically called to return to the main menu from other parts of the game.
    /// </summary>
    public static void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Loads the "InfoScene" by name.
    /// This method is typically called to display game information, credits, or instructions.
    /// </summary>
    public static void LoadInfoScene()
    {
        SceneManager.LoadScene("InfoScene");
    }
}