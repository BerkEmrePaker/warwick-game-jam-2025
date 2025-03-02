using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Manages the display of the player's health on the UI, including a text display and a health bar.
/// </summary>
public class DisplayPlayerHealth : MonoBehaviour
{
    [Header("Text Display")]
    /// <summary>
    /// Reference to the TextMeshProUGUI component for displaying the player's health.
    /// </summary>
    [SerializeField] private TextMeshProUGUI playerHealthText;

    [Header("Bar Display")]
    /// <summary>
    /// Reference to the red health bar GameObject (background).
    /// </summary>
    public GameObject redHealth;

    /// <summary>
    /// Reference to the green health bar GameObject (foreground).
    /// </summary>
    public GameObject greenHealth;

    /// <summary>
    /// Reference to the PlayerHealthStore singleton for retrieving the player's health.
    /// </summary>
    private PlayerHealthStore playerHealthStore;

    /// <summary>
    /// The player's maximum health value.
    /// </summary>
    private float maxHealth;

    /// <summary>
    /// The player's current health value.
    /// </summary>
    private float health;

    /// <summary>
    /// Initializes the reference to the PlayerHealthStore singleton.
    /// </summary>
    private void Awake()
    {
        playerHealthStore = PlayerHealthStore.Instance;
    }

    /// <summary>
    /// Updates the player's health display every frame.
    /// </summary>
    private void Update()
    {
        if (playerHealthText != null && playerHealthStore != null)
        {
            maxHealth = UpgradeStore.Instance.GetPlayerMaxHealth(); // Get the player's max health
            health = playerHealthStore.GetPlayerHealth(); // Get the player's current health

            ManageHealthBarSize(); // Update the health bar size and position

            // Update the green health bar's fill amount based on the player's current health
            greenHealth.GetComponent<Image>().fillAmount = health / maxHealth;
        }
    }

    /// <summary>
    /// Adjusts the size and position of the health bar based on the player's max health.
    /// </summary>
    private void ManageHealthBarSize()
    {
        float newWidth = 3f * (maxHealth / 100f); // Calculate new width based on max health

        // Adjust the size of the red and green health bars
        redHealth.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth * 0.95f, 0.4f);
        greenHealth.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth * 0.95f, 0.4f);

        // Ensure the health bars are properly aligned
        redHealth.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, redHealth.GetComponent<RectTransform>().anchoredPosition.y);
        greenHealth.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, greenHealth.GetComponent<RectTransform>().anchoredPosition.y);
    }
}