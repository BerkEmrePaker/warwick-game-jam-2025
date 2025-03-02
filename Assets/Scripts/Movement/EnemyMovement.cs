using UnityEngine;

/// <summary>
/// Handles the movement and animation of an enemy, including moving towards the player, flipping the sprite, and adjusting animation speed.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    /// <summary>
    /// The speed at which the enemy moves towards the player.
    /// </summary>
    private float moveSpeed = 1f;

    /// <summary>
    /// Reference to the player's Transform to determine movement direction.
    /// </summary>
    private Transform playerTransform;

    /// <summary>
    /// Reference to the Rigidbody2D component for movement.
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// Reference to the SpriteRenderer component for flipping the sprite.
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Reference to the Animator component for controlling animations.
    /// </summary>
    private Animator animator;

    [Header("Animation Settings")]
    [SerializeField]
    /// <summary>
    /// The base speed at which the animation looks natural.
    /// </summary>
    private float baseMoveSpeed = 1f;

    [SerializeField]
    /// <summary>
    /// The minimum animation speed to prevent the animation from stopping at very low speeds.
    /// </summary>
    private float minAnimationSpeed = 0.2f;

    /// <summary>
    /// Reference to the EnemyHealth component to check if the enemy is dead.
    /// </summary>
    private EnemyHealth enemyHealth;

    /// <summary>
    /// Reference to the player GameObject.
    /// </summary>
    private GameObject player;

    /// <summary>
    /// Reference to the PlayerCollisions component to check if the player is dead.
    /// </summary>
    private PlayerCollisions playerCol;

    /// <summary>
    /// Initializes references to components and finds the player GameObject.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerCol = player.GetComponent<PlayerCollisions>();

        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player object not found. Make sure the Player is defined.");
        }
    }

    /// <summary>
    /// Handles enemy movement and animation updates in FixedUpdate for consistent physics behavior.
    /// </summary>
    private void FixedUpdate()
    {
        if (enemyHealth.isDead || playerCol.isDead)
        {
            rb.linearVelocity = new Vector2(0, 0); // Stop movement if the enemy or player is dead
        }
        else if (playerTransform != null)
        {
            MoveTowardsPlayer();
            HandleSpriteFlip();
            AdjustAnimationSpeed();
        }
    }

    /// <summary>
    /// Moves the enemy towards the player at the defined speed.
    /// </summary>
    private void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        float speed = rb.linearVelocity.magnitude * UpgradeStore.Instance.GetPlayerSpeed();
        animator.SetFloat("Speed", speed);
    }

    /// <summary>
    /// Flips the enemy's sprite based on its movement direction.
    /// </summary>
    private void HandleSpriteFlip()
    {
        if (rb.linearVelocity.x != 0)
        {
            spriteRenderer.flipX = rb.linearVelocity.x < 0;
        }
    }

    /// <summary>
    /// Adjusts the animation speed based on the enemy's movement speed.
    /// Ensures the animation does not stop at very low speeds.
    /// </summary>
    private void AdjustAnimationSpeed()
    {
        float speedMultiplier = rb.linearVelocity.magnitude / baseMoveSpeed;
        animator.speed = Mathf.Max(speedMultiplier, minAnimationSpeed);
    }
}