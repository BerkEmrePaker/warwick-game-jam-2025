using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player movement, including input handling, sprite flipping, and animation updates.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Reference to the Rigidbody2D component for movement.
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// Reference to the SpriteRenderer component for flipping the sprite.
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Stores the player's movement input.
    /// </summary>
    private Vector2 moveInput;

    [SerializeField]
    /// <summary>
    /// Reference to the Animator component for controlling animations.
    /// </summary>
    private Animator animator;

    [SerializeField]
    /// <summary>
    /// Reference to the slice GameObject used for attacking.
    /// </summary>
    GameObject slice;

    [SerializeField]
    /// <summary>
    /// Reference to the player GameObject.
    /// </summary>
    private GameObject self;

    /// <summary>
    /// Indicates whether the player is currently attacking.
    /// </summary>
    private bool isAttacking;

    /// <summary>
    /// Reference to the PlayerCollisions component to check if the player is dead.
    /// </summary>
    private PlayerCollisions playerCol;

    /// <summary>
    /// Initializes references to components.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCol = GetComponent<PlayerCollisions>();
    }

    /// <summary>
    /// Updates the player's attacking state every frame.
    /// </summary>
    private void Update()
    {
        isAttacking = self.GetComponent<AttackEnemies>().isAttacking;
    }

    /// <summary>
    /// Handles player movement and animation updates in FixedUpdate for consistent physics behavior.
    /// </summary>
    void FixedUpdate()
    {
        if (playerCol.isDead)
        {
            rb.linearVelocity = new Vector2(0, 0); // Stop movement if the player is dead
        }
        else
        {
            rb.linearVelocity = moveInput * UpgradeStore.Instance.GetPlayerSpeed(); // Move the player
            HandleSpriteFlip(); // Handle sprite flipping based on movement or attack direction

            float speed = moveInput.magnitude * UpgradeStore.Instance.GetPlayerSpeed();
            animator.SetFloat("Speed", speed); // Update animation speed
        }
    }

    /// <summary>
    /// Reads player movement input and normalizes it to prevent faster diagonal movement.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized; // Normalize input to prevent faster diagonal movement
    }

    /// <summary>
    /// Handles flipping the player's sprite based on movement or attack direction.
    /// </summary>
    private void HandleSpriteFlip()
    {
        if (isAttacking) // Check if the player is attacking
        {
            if (slice.transform.localScale.x < 0) // Check if the slice is flipped (attacking left)
            {
                spriteRenderer.flipX = true; // Flip player sprite to face left
            }
            else
            {
                spriteRenderer.flipX = false; // Flip player sprite to face right
            }
        }
        else
        {
            if (moveInput.x != 0) // Check if there is horizontal movement
            {
                spriteRenderer.flipX = moveInput.x < 0; // Flip sprite when moving left
            }
        }
    }
}