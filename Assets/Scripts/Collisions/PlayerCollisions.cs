using UnityEngine;
using System.Collections;

/// <summary>
/// Handles player collisions with enemies, XP pickups, and extra health.
/// Manages player damage, death, and high score updates.
/// </summary>
public class PlayerCollisions : MonoBehaviour
{
    /// <summary>
    /// Prevents multiple collision detections in the same frame.
    /// </summary>
    private bool isColliding = false;

    /// <summary>
    /// Indicates whether the player is dead.
    /// </summary>
    public bool isDead = false;

    /// <summary>
    /// Indicates whether the player is currently in a hit animation.
    /// </summary>
    public bool isHitAnimation = false;

    /// <summary>
    /// Reference to the Animator component for playing animations.
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Reference to the AttackEnemies component for managing attack states.
    /// </summary>
    private AttackEnemies attackEnemies;

    /// <summary>
    /// Cooldown duration between damage applications from enemies.
    /// </summary>
    public float playerDamageCooldown;

    [SerializeField]
    /// <summary>
    /// Reference to the TimePeriodManager for handling time-switching mechanics.
    /// </summary>
    private TimePeriodManager TPM;

    /// <summary>
    /// Initializes references to components.
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();
        attackEnemies = GetComponent<AttackEnemies>();
    }

    /// <summary>
    /// Detects trigger events (e.g., XP pickups, health pickups).
    /// </summary>
    /// <param name="collision">The collider data.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("XP"))
        {
            Debug.Log("HIT XP");
            Destroy(collision.gameObject);
            XPStore.Instance?.IncrementXP(1); // Increment XP
        }
        else if (collision.CompareTag("ExtraHealth"))
        {
            Debug.Log("Extra Health Gained!");
            Destroy(collision.gameObject);
            PlayerHealthStore.Instance?.IncrementPlayerHealth(20); // Increment player health
        }
    }

    /// <summary>
    /// Checks if the player's health has dropped to zero and triggers death if necessary.
    /// </summary>
    private void Update()
    {
        PlayerHealthStore playerHealth = PlayerHealthStore.Instance;
        if (playerHealth.GetPlayerHealth() <= 0)
        {
            KillPlayer(); // Handle player death
        }
    }

    /// <summary>
    /// Applies damage to the player and triggers the hit animation.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void playerAttacked(int damage)
    {
        if (!isDead)
        {
            PlayerHealthStore playerHealth = PlayerHealthStore.Instance;
            playerHealth.IncrementPlayerHealth(damage); // Apply damage
            animator.SetTrigger("Hit"); // Trigger hit animation
            isHitAnimation = true;
        }
    }

    /// <summary>
    /// Handles player death, updates the high score, and triggers the death animation.
    /// </summary>
    private void KillPlayer()
    {
        if (isDead) return; // Prevent multiple death calls
        isDead = true;

        // Get current time score
        int currentScore = (int)gameObject.GetComponent<TimeScoreManager>().GetTimer();
        // Get high score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Compare current score with high score
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore); // Update high score
            PlayerPrefs.Save(); // Save the high score persistently
        }

        animator.SetTrigger("Death"); // Trigger death animation
    }

    /// <summary>
    /// Waits for the death sound to play before transitioning to the main menu.
    /// </summary>
    /// <param name="delay">Delay in seconds before loading the menu.</param>
    private IEnumerator LoadMainMenuAfterSound(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneLoader.LoadMainMenuScene(); // Transition to the main menu scene
    }

    /// <summary>
    /// Resets the hit animation state and stops attacking.
    /// </summary>
    public void EndOfHitAnimation()
    {
        isHitAnimation = false;
        attackEnemies.isAttacking = false;
    }

    /// <summary>
    /// Called at the end of the death animation to load the main menu.
    /// </summary>
    public void EndOfDeathAnimation()
    {
        StartCoroutine(LoadMainMenuAfterSound(0.25f)); // Load main menu after a short delay
    }

    /// <summary>
    /// Called at the end of the time-switch animation to resume normal gameplay.
    /// </summary>
    public void EndOfTimeSwitchAnimation()
    {
        TPM.TimeJumpAnimationDone(); // Notify TimePeriodManager that the animation is complete
    }

    /// <summary>
    /// Triggers a camera shake during the hit animation.
    /// </summary>
    public void whitePartofHitAnimation()
    {
        // Trigger the camera shake
        GetComponent<CameraShake>().ShakeCamera();
    }
}