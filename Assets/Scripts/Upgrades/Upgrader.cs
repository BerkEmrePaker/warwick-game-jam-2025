using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages player upgrades, including displaying upgrade options, applying upgrades, and tracking XP levels.
/// </summary>
public class Upgrader : MonoBehaviour
{
    /// <summary>
    /// Array of sprites representing upgrade options.
    /// </summary>
    public Sprite[] upgradeImages;

    /// <summary>
    /// Array of XP thresholds required to reach each level.
    /// </summary>
    private int[] XPLevels = { 5, 10, 20, 30, 50, 75, 100, 150, 200, 300, 400, 500, 600, 700 };

    /// <summary>
    /// The player's current level.
    /// </summary>
    private int currentLevel = 0;

    /// <summary>
    /// Reference to the upgrade menu GameObject.
    /// </summary>
    public GameObject upgradeMenu;

    [Header("The Upgrade Button")]
    /// <summary>
    /// The prefab for the general upgrade button.
    /// </summary>
    public GameObject generalUpgradeButton;

    /// <summary>
    /// The number of available upgrade options.
    /// </summary>
    private int numberOfUpgradeOptions;

    [Header("Parent Object")]
    /// <summary>
    /// The parent transform for instantiating upgrade buttons.
    /// </summary>
    public Transform parentTransform;

    /// <summary>
    /// The parent GameObject used to manage child objects (e.g., clearing buttons).
    /// </summary>
    public GameObject parent;

    [Tooltip("Insert the GameObject with the Upgrader Script")]
    /// <summary>
    /// Reference to the non-prefab GameObject containing the Upgrader script.
    /// </summary>
    public GameObject nonPrefabObject;

    [SerializeField]
    /// <summary>
    /// Visual representation of the attack radius.
    /// </summary>
    GameObject rangeCircle;

    [Header("Change the amount each upgrade changes stat by: ")]
    [SerializeField]
    /// <summary>
    /// Amount to increase the attack radius per upgrade.
    /// </summary>
    private float attackRadiusChange; // +1

    [SerializeField]
    /// <summary>
    /// Amount to increase the player's max health per upgrade.
    /// </summary>
    private int playerMaxHealthChange; // +40

    [SerializeField]
    /// <summary>
    /// Amount to increase the player's speed per upgrade.
    /// </summary>
    private float playerSpeedChange; // +2

    [SerializeField]
    /// <summary>
    /// Amount to decrease the player's health cooldown per upgrade.
    /// </summary>
    private float playerHealthCooldownChange; // -0.5

    [SerializeField]
    /// <summary>
    /// Amount to increase the attack rate per upgrade.
    /// </summary>
    private float attackRateChange; // +0.5

    [SerializeField]
    /// <summary>
    /// Amount to increase the attack damage per upgrade.
    /// </summary>
    private float attackDamageChange; // +20

    /// <summary>
    /// Retrieves the array of XP levels.
    /// </summary>
    /// <returns>The array of XP thresholds.</returns>
    public int[] GetXPLevels()
    {
        return this.XPLevels;
    }

    /// <summary>
    /// Retrieves the player's current level.
    /// </summary>
    /// <returns>The current level.</returns>
    public int GetCurrentLevel()
    {
        return this.currentLevel;
    }

    /// <summary>
    /// Initializes the number of upgrade options and ensures the upgrade menu is hidden at the start.
    /// </summary>
    void Start()
    {
        numberOfUpgradeOptions = upgradeImages.Length;
        Time.timeScale = 1;
        if (upgradeMenu != null)
            upgradeMenu.SetActive(false);
    }

    /// <summary>
    /// Updates the visual representation of the attack radius based on the current value.
    /// </summary>
    private void Awake()
    {
        rangeCircle.transform.localScale = new Vector3(UpgradeStore.Instance.GetAttackRadius() * 2, UpgradeStore.Instance.GetAttackRadius() * 2, 1);
    }

    /// <summary>
    /// Checks if the player has reached the next XP level and displays the upgrade menu if so.
    /// </summary>
    void Update()
    {
        if (XPStore.Instance.GetXP() >= XPLevels[currentLevel])
        {
            Debug.Log("Level " + (currentLevel + 1) + " reached!");
            if (currentLevel < XPLevels.Length - 1)
            {
                displayUpgradeMenu();
                currentLevel++;
            }
        }
    }

    /// <summary>
    /// Displays upgrade buttons by randomly selecting three unique upgrades.
    /// </summary>
    private void displayUpgradeButtons()
    {
        // Decide which three upgrades to pick
        List<int> indexes = GetUniqueRandomNumbers(3, numberOfUpgradeOptions - 1);
        // Display them onto the screen
        int j = 0;
        foreach (int index in indexes)
        {
            GameObject currentButton = addUpgradeScriptForIndex(index);
            SetPositionOnScreen(j, currentButton);
            j++;
        }
    }

    /// <summary>
    /// Adds the appropriate upgrade script to a button based on the selected index.
    /// </summary>
    /// <param name="index">The index of the upgrade option.</param>
    /// <returns>The instantiated button GameObject.</returns>
    private GameObject addUpgradeScriptForIndex(int index)
    {
        GameObject instantiatedButton = Instantiate(generalUpgradeButton, parentTransform);
        Button buttonComponent = instantiatedButton.GetComponent<Button>();

        switch (index)
        {
            case 0:
                buttonComponent.onClick.AddListener(() =>
                {
                    nonPrefabObject.GetComponent<Upgrader>().upgradeAttackRadius();
                });
                break;
            case 1:
                buttonComponent.onClick.AddListener(() =>
                {
                    nonPrefabObject.GetComponent<Upgrader>().upgradeMaxHealth();
                });
                break;
            case 2:
                buttonComponent.onClick.AddListener(() =>
                {
                    nonPrefabObject.GetComponent<Upgrader>().upgradePlayerSpeed();
                });
                break;
            case 3:
                buttonComponent.onClick.AddListener(() =>
                {
                    nonPrefabObject.GetComponent<Upgrader>().upgradePlayerHealthCooldown();
                });
                break;
            case 4:
                buttonComponent.onClick.AddListener(() =>
                {
                    nonPrefabObject.GetComponent<Upgrader>().upgradeAttackRate();
                });
                break;
            case 5:
                buttonComponent.onClick.AddListener(() =>
                {
                    nonPrefabObject.GetComponent<Upgrader>().upgradeAttackDamage();
                });
                break;
        }
        buttonComponent.image.sprite = upgradeImages[index];

        return instantiatedButton;
    }

    ///////////////////////////////// Upgrade Functions //////////////////////////////////////////

    /// <summary>
    /// Upgrades the player's attack radius and updates the visual representation.
    /// </summary>
    public void upgradeAttackRadius()
    {
        float currentRadius = UpgradeStore.Instance.GetAttackRadius();
        UpgradeStore.Instance.SetAttackRadius(currentRadius + attackRadiusChange);
        rangeCircle.transform.localScale = new Vector3(UpgradeStore.Instance.GetAttackRadius() * 2, UpgradeStore.Instance.GetAttackRadius() * 2, 1);
        Debug.Log("Attack radius now: " + UpgradeStore.Instance.GetAttackRadius());
        closeUpgradeMenu();
    }

    /// <summary>
    /// Upgrades the player's max health.
    /// </summary>
    public void upgradeMaxHealth()
    {
        int current = UpgradeStore.Instance.GetPlayerMaxHealth();
        UpgradeStore.Instance.SetPlayerMaxHealth(current + playerMaxHealthChange);
        Debug.Log("Player Max Health now: " + UpgradeStore.Instance.GetPlayerMaxHealth());
        closeUpgradeMenu();
    }

    /// <summary>
    /// Upgrades the player's speed.
    /// </summary>
    public void upgradePlayerSpeed()
    {
        float current = UpgradeStore.Instance.GetPlayerSpeed();
        UpgradeStore.Instance.SetPlayerSpeed(current + playerSpeedChange);
        Debug.Log("Player Speed now: " + UpgradeStore.Instance.GetPlayerSpeed());
        closeUpgradeMenu();
    }

    /// <summary>
    /// Upgrades the player's health cooldown.
    /// </summary>
    public void upgradePlayerHealthCooldown()
    {
        UpgradeStore.Instance.IncrementPlayerHealthCooldown(playerHealthCooldownChange);
        Debug.Log("Player Health Cooldown now: " + UpgradeStore.Instance.GetPlayerHealthCooldown());
        closeUpgradeMenu();
    }

    /// <summary>
    /// Upgrades the player's attack rate.
    /// </summary>
    public void upgradeAttackRate()
    {
        UpgradeStore.Instance.IncrementAttackRate(attackRateChange);
        Debug.Log("Player Attack Rate now: " + UpgradeStore.Instance.GetAttackRate());
        closeUpgradeMenu();
    }

    /// <summary>
    /// Upgrades the player's attack damage.
    /// </summary>
    public void upgradeAttackDamage()
    {
        UpgradeStore.Instance.IncrementAttackDamage(attackDamageChange);
        Debug.Log("Player Attack Damage now: " + UpgradeStore.Instance.GetAttackDamage());
        closeUpgradeMenu();
    }

    /////////// Helper Functions //////////

    /// <summary>
    /// Closes the upgrade menu and resumes the game.
    /// </summary>
    private void closeUpgradeMenu()
    {
        upgradeMenu.SetActive(false);
        Time.timeScale = 1;
        // Clear the children of the parent object
        DestroyAllChildren(parent);
    }

    /// <summary>
    /// Destroys all child GameObjects of the specified parent GameObject.
    /// </summary>
    /// <param name="parent">The parent GameObject whose children will be destroyed.</param>
    public void DestroyAllChildren(GameObject parent)
    {
        // Loop through each child of the parent GameObject
        foreach (Transform child in parent.transform)
        {
            // Destroy the child GameObject
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Displays the upgrade menu and pauses the game.
    /// </summary>
    private void displayUpgradeMenu()
    {
        Time.timeScale = 0; // Pause the game
        if (upgradeMenu != null)
        {
            upgradeMenu.SetActive(true);
            displayUpgradeButtons();
        }
    }

    /// <summary>
    /// Generates a list of unique random numbers within a specified range.
    /// </summary>
    /// <param name="n">The number of unique random numbers to generate.</param>
    /// <param name="max">The maximum value for the random numbers.</param>
    /// <returns>A list of unique random numbers.</returns>
    public List<int> GetUniqueRandomNumbers(int n, int max)
    {
        if (n > max + 1)
        {
            throw new System.ArgumentException("n must be less than or equal to max + 1.");
        }

        HashSet<int> uniqueNumbers = new HashSet<int>();
        System.Random random = new System.Random();

        while (uniqueNumbers.Count < n)
        {
            int num = random.Next(0, max + 1);
            uniqueNumbers.Add(num);
        }

        return new List<int>(uniqueNumbers);
    }

    /// <summary>
    /// Sets the position of a button on the screen based on its place in the sequence.
    /// </summary>
    /// <param name="place">The position index (0, 1, or 2).</param>
    /// <param name="button">The button GameObject to position.</param>
    private void SetPositionOnScreen(int place, GameObject button)
    {
        switch (place)
        {
            case 0:
                button.transform.localPosition = new Vector3(-500f, -285f, 0f);
                break;
            case 1:
                button.transform.localPosition = new Vector3(0f, -285f, 0f);
                break;
            case 2:
                button.transform.localPosition = new Vector3(500f, -285f, 0f);
                break;
        }
    }
}