using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the health of an enemy, handles damage, and destroys the object when health reaches zero.
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField]
    /// <summary>
    /// The maximum health of the enemy.
    /// </summary>
    private float maxHealth = 100f;

    /// <summary>
    /// The current health of the enemy.
    /// </summary>
    private float currentHealth;

    /// <summary>
    /// Reference to the SpriteRenderer component for visual effects (e.g., flashing on damage).
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Indicates whether the enemy is currently on cooldown after taking damage.
    /// </summary>
    private bool isOnCooldown = false;

    /// <summary>
    /// The cooldown duration after taking damage.
    /// </summary>
    private float takeDamageCooldown;

    /// <summary>
    /// Reference to the UpgradeStore singleton for retrieving attack rate.
    /// </summary>
    private UpgradeStore upgradeStore;

    /// <summary>
    /// Reference to the player GameObject.
    /// </summary>
    private GameObject player;

    [Header("Drop Settings")]
    [SerializeField]
    /// <summary>
    /// The health pickup GameObject to drop upon death.
    /// </summary>
    private GameObject extraHealthObject;

    /// <summary>
    /// The parent GameObject for health pickups.
    /// </summary>
    private GameObject extraHealthObjectParent;

    [SerializeField]
    /// <summary>
    /// The tag used to find the parent GameObject for health pickups.
    /// </summary>
    private string extraHealthObjectParentTagToFind;

    [SerializeField]
    /// <summary>
    /// The XP pickup GameObject to drop upon death.
    /// </summary>
    private GameObject XPObject;

    /// <summary>
    /// The parent GameObject for XP pickups.
    /// </summary>
    private GameObject XPObjectParent;

    [SerializeField]
    /// <summary>
    /// The tag used to find the parent GameObject for XP pickups.
    /// </summary>
    private string XPObjectParentTagToFind;

    /// <summary>
    /// Reference to the Animator component for playing animations.
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Indicates whether the enemy is dead.
    /// </summary>
    public bool isDead = false;

    /// <summary>
    /// Initializes the enemy's health and references to components and GameObjects.
    /// </summary>
    private void Awake()
    {
        currentHealth = maxHealth; // Initialize health
        spriteRenderer = GetComponent<SpriteRenderer>();
        upgradeStore = UpgradeStore.Instance;
        player = GameObject.FindGameObjectWithTag("Player");
        extraHealthObjectParent = GameObject.FindGameObjectWithTag(extraHealthObjectParentTagToFind);
        XPObjectParent = GameObject.FindGameObjectWithTag(XPObjectParentTagToFind);
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Reduces health by the given damage amount and triggers death if health drops to zero.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void TakeDamage(float damage)
    {
        if (!isOnCooldown)
        {
            if (damage <= 0f) return; // Ignore non-positive damage

            currentHealth -= damage; // Reduce health

            // Play take hit animation
            animator.SetTrigger("Hit");

            if (currentHealth <= 0f)
            {
                animator.SetTrigger("Die"); // Trigger death animation
                isDead = true; // Mark the enemy as dead
                Collider2D collider = GetComponent<Collider2D>();
                collider.enabled = false; // Disable the collider
            }

            StartCoroutine(DamageCooldownRoutine()); // Start cooldown after taking damage
        }
    }

    /// <summary>
    /// Starts a cooldown routine to temporarily disable damage.
    /// </summary>
    private IEnumerator DamageCooldownRoutine()
    {
        isOnCooldown = true;
        takeDamageCooldown = 0.256f / upgradeStore.GetAttackRate(); // Calculate cooldown based on attack rate
        yield return new WaitForSeconds(takeDamageCooldown + 0.02f); // Wait for cooldown duration
        isOnCooldown = false; // End cooldown
    }

    /// <summary>
    /// Called when the death animation finishes to handle enemy death.
    /// </summary>
    public void DeathAnimationFinished()
    {
        Die();
    }

    /// <summary>
    /// Handles enemy death by dropping health or XP pickups and destroying the GameObject.
    /// </summary>
    private void Die()
    {
        // Chance to drop health
        if (player.GetComponent<AttackEnemies>().ShouldDropHealth())
        {
            Instantiate(extraHealthObject, transform.position, Quaternion.identity, extraHealthObjectParent.transform);
        }
        else
        {
            // Drop an XP chunk
            Instantiate(XPObject, transform.position, Quaternion.identity, XPObjectParent.transform);
        }

        // Destroy the enemy GameObject
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}