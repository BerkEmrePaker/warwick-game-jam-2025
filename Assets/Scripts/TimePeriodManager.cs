using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the switching between two time periods (present and past) in the game.
/// Handles enemy visibility, player state, and cooldowns during time switches.
/// </summary>
public class TimePeriodManager : MonoBehaviour
{
    /// <summary>
    /// GameObject containing all enemies in the present time period.
    /// </summary>
    public GameObject presentEnemies;

    /// <summary>
    /// GameObject containing all enemies in the past time period.
    /// </summary>
    public GameObject pastEnemies;

    /// <summary>
    /// Reference to the pause menu GameObject.
    /// </summary>
    public GameObject pauseMenu;

    /// <summary>
    /// Reference to the upgrade menu GameObject.
    /// </summary>
    public GameObject upgradeMenu;

    /// <summary>
    /// Indicates whether the game is currently in the present time period.
    /// </summary>
    public bool present;

    /// <summary>
    /// Determines if the player is allowed to switch time periods.
    /// </summary>
    [SerializeField] private bool switchAllowed;

    /// <summary>
    /// The delay (in seconds) before the player can switch time periods again.
    /// </summary>
    [SerializeField] private float switchDelay;

    /// <summary>
    /// Reference to the player GameObject.
    /// </summary>
    [SerializeField] private GameObject player;

    /// <summary>
    /// Reference to the player's Collider2D component.
    /// </summary>
    private Collider2D playerCollider;

    /// <summary>
    /// Reference to the player's Animator component.
    /// </summary>
    private Animator playerAnimator;

    /// <summary>
    /// Reference to the PlayerCollisions script attached to the player.
    /// </summary>
    private PlayerCollisions playerCol;

    /// <summary>
    /// Reference to the AttackEnemies script attached to the player.
    /// </summary>
    private AttackEnemies attackEn;

    /// <summary>
    /// Initializes the time period manager by setting default values and references.
    /// </summary>
    void Awake()
    {
        present = true;
        presentEnemies.SetActive(true);
        pastEnemies.SetActive(false);
        switchAllowed = true;
        playerCollider = player.GetComponent<Collider2D>();
        playerAnimator = player.GetComponent<Animator>();
        playerCol = player.GetComponent<PlayerCollisions>();
        attackEn = player.GetComponent<AttackEnemies>();
    }

    /// <summary>
    /// Handles the input for switching between time periods.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void OnInteract(InputAction.CallbackContext context)
    {
        // Check if switching is allowed and no menus are active.
        if (switchAllowed && !upgradeMenu.activeSelf && !pauseMenu.activeSelf)
        {
            // Toggle between present and past time periods.
            present = !present;
            presentEnemies.SetActive(present);
            pastEnemies.SetActive(!present);
            switchAllowed = false;

            // Disable player collider and movement during the time switch.
            playerCollider.enabled = false; // Disable the player's collider so they are invincible.
            playerCol.isDead = true; // Pretend the player is dead to disable movement.
            playerAnimator.SetTrigger("TimeSwitch"); // Trigger the time switch animation.
        }
    }

    /// <summary>
    /// Called when the time jump animation is completed.
    /// Re-enables player functionality and starts the cooldown period.
    /// </summary>
    public void TimeJumpAnimationDone()
    {
        playerCollider.enabled = true; // Re-enable the player's collider.
        playerCol.isDead = false; // Re-enable player movement.
        attackEn.isAttacking = false; // Reset the attacking state.
        StartCoroutine(cooldownPeriod(switchDelay)); // Start the cooldown period.
    }

    /// <summary>
    /// Handles the cooldown period after a time switch.
    /// </summary>
    /// <param name="cooldownTime">The duration of the cooldown in seconds.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    private IEnumerator cooldownPeriod(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        switchAllowed = true; // Re-enable time switching after the cooldown.
    }
}