using UnityEngine;

/// <summary>
/// Randomly selects and assigns a gem sprite from a predefined array on startup.
/// </summary>
public class GemSprite : MonoBehaviour
{
    [Header("Sprite Settings")]
    [SerializeField]
    /// <summary>
    /// Reference to the SpriteRenderer component for displaying the gem sprite.
    /// </summary>
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    /// <summary>
    /// Array of gem sprites to randomly choose from.
    /// </summary>
    private Sprite[] gemSprites;

    /// <summary>
    /// Randomly selects and assigns a gem sprite from the array on startup.
    /// </summary>
    private void Awake()
    {
        if (gemSprites.Length > 0 && spriteRenderer != null)
        {
            // Assign a random sprite from the array
            spriteRenderer.sprite = gemSprites[Random.Range(0, gemSprites.Length)];
        }
        else
        {
            Debug.LogWarning("GemSprite: Missing SpriteRenderer or no sprites assigned!");
        }
    }
}