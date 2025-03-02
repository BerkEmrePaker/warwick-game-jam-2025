using UnityEngine;
using System.Collections;

/// <summary>
/// Handles enemy attack behavior, including triggering attack animations and damaging the player.
/// </summary>
public class EnemyAttack : MonoBehaviour
{
    /// <summary>
    /// Reference to the Animator component for controlling attack animations.
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Indicates whether the enemy is currently attacking.
    /// </summary>
    private bool attacking;

    /// <summary>
    /// The last time the enemy attacked.
    /// </summary>
    private float lastAttackTime;

    /// <summary>
    /// The cooldown duration between attacks, defined by the player's damage cooldown.
    /// </summary>
    private float playerDamageCooldown;

    /// <summary>
    /// Reference to the PlayerCollisions component for applying damage to the player.
    /// </summary>
    private PlayerCollisions playerCol;

    [Header("Attack Settings")]
    [SerializeField]
    /// <summary>
    /// The amount of damage the enemy deals to the player.
    /// </summary>
    private int attackDamage;

    /// <summary>
    /// Initializes references to components and sets up initial attack timing.
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerCol = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollisions>();
        playerDamageCooldown = playerCol.playerDamageCooldown;
        lastAttackTime = -playerDamageCooldown; // Allows immediate attack when spawned
    }

    /// <summary>
    /// Detects when the player enters the enemy's trigger collider and starts an attack if possible.
    /// </summary>
    /// <param name="collision">The collider of the object that entered the trigger.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!attacking && collision.CompareTag("Player") && Time.time >= lastAttackTime + playerDamageCooldown)
        {
            StartAttack(); // Start attack if conditions are met
        }
    }

    /// <summary>
    /// Detects when the player stays within the enemy's trigger collider and starts an attack if possible.
    /// </summary>
    /// <param name="collision">The collider of the object that is staying in the trigger.</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!attacking && collision.CompareTag("Player") && Time.time >= lastAttackTime + playerDamageCooldown)
        {
            StartAttack(); // Start attack if conditions are met
        }
    }

    /// <summary>
    /// Starts the enemy's attack by triggering the attack animation and applying damage to the player.
    /// </summary>
    private void StartAttack()
    {
        attacking = true;
        animator.SetTrigger("Attack"); // Trigger attack animation
        playerCol.playerAttacked(attackDamage); // Apply damage to the player
        lastAttackTime = Time.time; // Update last attack time
    }

    /// <summary>
    /// Called by an animation event when the attack animation ends to reset the attack state.
    /// </summary>
    public void attackAnimationEnd()
    {
        attacking = false; // Reset attack state
    }
}