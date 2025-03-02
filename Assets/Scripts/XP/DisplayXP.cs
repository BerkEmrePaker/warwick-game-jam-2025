using UnityEngine;
using TMPro;
using System.Threading;
using UnityEngine.UI;

/// <summary>
/// Manages the display of the player's XP (experience points) on the UI.
/// This includes updating the XP text and visualizing the XP progress bar.
/// </summary>
public class DisplayXP : MonoBehaviour
{
    [Header("Text Display")]
    /// <summary>
    /// TextMeshProUGUI component to display the player's current XP.
    /// </summary>
    public TextMeshProUGUI playerXPText;

    [Header("Bar Display")]
    /// <summary>
    /// GameObject representing the border of the XP bar.
    /// </summary>
    public GameObject border;

    /// <summary>
    /// GameObject representing the unfilled portion of the XP bar.
    /// </summary>
    public GameObject uncomplete;

    /// <summary>
    /// GameObject representing the filled portion of the XP bar.
    /// </summary>
    public GameObject complete;

    [Header("Upgrade Manager")]
    /// <summary>
    /// Reference to the UpgradeManager GameObject, which contains the Upgrader component.
    /// </summary>
    public GameObject upgradeManager;

    /// <summary>
    /// Reference to the XPStore instance, which stores the player's current XP.
    /// </summary>
    private XPStore xpStore;

    /// <summary>
    /// The player's current XP value.
    /// </summary>
    private float currentXP;

    /// <summary>
    /// Array of XP thresholds required to reach each level.
    /// </summary>
    private int[] XPLevels;

    /// <summary>
    /// The player's current XP level.
    /// </summary>
    private int currentXPLevel;

    /// <summary>
    /// The absolute XP required to reach the next level.
    /// </summary>
    private float nextLevelAbsoluteXP;

    /// <summary>
    /// The absolute XP required to reach the previous level.
    /// </summary>
    private float prevLevelAbsoluteXP;

    /// <summary>
    /// The difference in XP between the current level and the next level.
    /// </summary>
    private float XPGapBetweenTwoLevels;

    /// <summary>
    /// The percentage of the XP bar that should be filled based on the player's progress.
    /// </summary>
    private float fillPC;

    /// <summary>
    /// Initializes the XPStore and retrieves the XP levels from the Upgrader component.
    /// </summary>
    void Awake()
    {
        xpStore = XPStore.Instance;
        XPLevels = upgradeManager.GetComponent<Upgrader>().GetXPLevels();
    }

    /// <summary>
    /// Updates the XP bar display every frame.
    /// </summary>
    void Update()
    {
        DisplayXPBar();
    }

    /// <summary>
    /// Calculates and updates the fill amount of the XP bar based on the player's current XP and level.
    /// </summary>
    private void DisplayXPBar()
    {
        currentXPLevel = upgradeManager.GetComponent<Upgrader>().GetCurrentLevel();
        currentXP = xpStore.GetXP();

        if (currentXPLevel == 0)
        {
            // For the first level, calculate progress from 0 XP.
            nextLevelAbsoluteXP = XPLevels[currentXPLevel];
            prevLevelAbsoluteXP = 0;
            XPGapBetweenTwoLevels = nextLevelAbsoluteXP - prevLevelAbsoluteXP;
            fillPC = (currentXP - prevLevelAbsoluteXP) / XPGapBetweenTwoLevels;
            complete.GetComponent<Image>().fillAmount = fillPC;
        }
        else if (currentXPLevel >= XPLevels.Length)
        {
            // If the player has reached the max level, fill the bar completely.
            complete.GetComponent<Image>().fillAmount = 1f;
        }
        else
        {
            // For intermediate levels, calculate progress between the current and next level.
            nextLevelAbsoluteXP = XPLevels[currentXPLevel];
            prevLevelAbsoluteXP = XPLevels[currentXPLevel - 1];
            XPGapBetweenTwoLevels = nextLevelAbsoluteXP - prevLevelAbsoluteXP;
            fillPC = (currentXP - prevLevelAbsoluteXP) / XPGapBetweenTwoLevels;
            complete.GetComponent<Image>().fillAmount = fillPC;
            Debug.Log("current Xp: " + currentXP + " next XP: " + nextLevelAbsoluteXP + " fill amount: " + fillPC);
        }
    }
}