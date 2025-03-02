using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Detects and attacks the closest enemy within a specified radius.
/// Implements a cooldown period after each attack.
/// </summary>
public class AttackEnemies : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField]
    /// <summary>
    /// Reference to the player's Transform for positioning the attack.
    /// </summary>
    private Transform player;

    /// <summary>
    /// Indicates whether the player is currently attacking.
    /// </summary>
    public bool isAttacking = false;

    /// <summary>
    /// Reference to the UpgradeStore singleton for retrieving attack stats.
    /// </summary>
    private UpgradeStore upgradeStore;

    [Header("Animation Settings")]
    [SerializeField]
    /// <summary>
    /// Reference to the Animator component for controlling player animations.
    /// </summary>
    private Animator animator;

    [SerializeField]
    /// <summary>
    /// Reference to the slice GameObject used for attacking.
    /// </summary>
    private GameObject slice;

    /// <summary>
    /// Reference to the Collider2D component of the slice for detecting collisions.
    /// </summary>
    private Collider2D sliceCollider;

    /// <summary>
    /// Reference to the Animator component of the slice for controlling slice animations.
    /// </summary>
    private Animator sliceAnimator;

    /// <summary>
    /// Indicates whether the slice animation is currently playing.
    /// </summary>
    private bool isSlicing = false;

    /// <summary>
    /// The closest enemy GameObject within the attack radius.
    /// </summary>
    private GameObject closestEnemy;

    [Header("Health Drop Settings")]
    [SerializeField]
    /// <summary>
    /// The lower bound for the health drop interval.
    /// </summary>
    private int healthDropIntervalLower;

    [SerializeField]
    /// <summary>
    /// The upper bound for the health drop interval.
    /// </summary>
    private int healthDropIntervalUpper;

    /// <summary>
    /// The next time a health drop can occur.
    /// </summary>
    private float nextHealthDropTime = 0f;

    /// <summary>
    /// Indicates whether the player is currently in a hit animation.
    /// </summary>
    private bool isHitAnimation = false;

    /// <summary>
    /// Reference to the PlayerCollisions component for checking player state.
    /// </summary>
    private PlayerCollisions playerCol;

    /// <summary>
    /// Initializes references to components and the UpgradeStore singleton.
    /// </summary>
    private void Awake()
    {
        upgradeStore = UpgradeStore.Instance;
        sliceAnimator = slice.GetComponent<Animator>();
        sliceCollider = slice.GetComponent<Collider2D>();
        playerCol = GetComponent<PlayerCollisions>();
    }

    /// <summary>
    /// Updates the slice position and checks for enemies to attack.
    /// </summary>
    private void Update()
    {
        slice.transform.position = new Vector3(player.position.x, player.position.y); // Constantly sets the position of the slice to the player.
        isSlicing = slice.GetComponent<SetInactiveAfterSlice>().isSlicing; // Constantly check if the slice animation is playing.
        isHitAnimation = GetComponent<PlayerCollisions>().isHitAnimation;

        if (!isAttacking && !isHitAnimation)
        {
            DetectAndAttackEnemy(); // Detect and attack the closest enemy
        }
    }

    /// <summary>
    /// Finds and attacks the closest enemy within the detection radius.
    /// </summary>
    private void DetectAndAttackEnemy()
    {
        closestEnemy = FindClosestEnemyWithinRadius(GetAllEnemies(), upgradeStore.GetAttackRadius());

        if (closestEnemy != null && !isHitAnimation && !playerCol.isDead) // If an enemy is in range and the player is not dead
        {
            // Play attack animation
            isAttacking = true; // Set to true when starting an attack animation
            animator.SetTrigger("Attack1Body"); // Trigger attack animation
        }
    }

    /// <summary>
    /// Retrieves all enemies in the scene.
    /// </summary>
    /// <returns>An array of enemy GameObjects.</returns>
    private GameObject[] GetAllEnemies()
    {
        return GameObject.FindGameObjectsWithTag("PresentEnemy")
            .Concat(GameObject.FindGameObjectsWithTag("PastEnemy"))
            .ToArray();
    }

    /// <summary>
    /// Receives a list of enemies hit by the slice and applies damage to them.
    /// </summary>
    /// <param name="collidedObjects">The list of collided enemy GameObjects.</param>
    public void ReceiveHitEnemiesList(List<GameObject> collidedObjects)
    {
        foreach (GameObject obj in collidedObjects)
        {
            // Damage the enemy
            EnemyHealth enemyHealth = obj.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(upgradeStore.GetAttackDamage()); // Apply damage
            }
            else
            {
                Debug.LogWarning($"{obj.name} is missing an EnemyHealth component!");
            }
        }
    }

    /// <summary>
    /// Called when the attack animation finishes to reset the attack state.
    /// </summary>
    public void OnAttackAnimationFinished()
    {
        isAttacking = false; // Reset to false when the animation finishes
    }

    /// <summary>
    /// Called from the Attack1Body animation to trigger the slice attack.
    /// </summary>
    public void Slice()
    {
        if (!isSlicing && closestEnemy != null)
        {
            isSlicing = true;
            slice.GetComponent<SetInactiveAfterSlice>().isSlicing = true;

            // Rotate slice towards the closest enemy
            Vector2 direction = closestEnemy.transform.position - player.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float zRotation = 0;
            float attackRadius = upgradeStore.GetAttackRadius();
            float sliceScale = attackRadius / 2.75f;
            Vector3 flipSliceX = new Vector3(-sliceScale, sliceScale, sliceScale);
            Vector3 unFlipSliceX = new Vector3(sliceScale, sliceScale, sliceScale);

            if (-90 <= angle && angle <= 75)
            {
                zRotation = angle;
                slice.transform.localScale = unFlipSliceX;
            }
            else if (angle > 75 && angle <= 90)
            {
                zRotation = 75;
                slice.transform.localScale = unFlipSliceX;
            }
            else if (angle < -90)
            {
                zRotation = angle + 180;
                slice.transform.localScale = flipSliceX;
            }
            else if (angle > 90 && angle <= 105)
            {
                zRotation = -75;
                slice.transform.localScale = flipSliceX;
            }
            else
            {
                zRotation = angle - 180;
                slice.transform.localScale = flipSliceX;
            }

            slice.transform.rotation = Quaternion.Euler(0, 0, zRotation);

            sliceAnimator.speed = upgradeStore.GetAttackRate(); // Set animation speed based on attack rate
            sliceCollider.enabled = true; // Enable the slice collider
            sliceAnimator.SetTrigger("Slice"); // Trigger the slice animation
        }
    }

    /// <summary>
    /// Finds the closest enemy within the given radius.
    /// </summary>
    /// <param name="enemies">Array of enemy GameObjects.</param>
    /// <param name="radius">The detection radius.</param>
    /// <returns>The closest enemy GameObject, or null if none are within the radius.</returns>
    private GameObject FindClosestEnemyWithinRadius(GameObject[] enemies, float radius)
    {
        GameObject closest = null;
        float closestDistance = radius;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector3.Distance(player.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    /// <summary>
    /// Determines whether a health drop should occur based on a random interval.
    /// </summary>
    /// <returns>True if a health drop should occur, otherwise false.</returns>
    public bool ShouldDropHealth()
    {
        if (Time.time >= nextHealthDropTime)
        {
            // Use an exponential function to vary drop times significantly
            int randy = Random.Range(healthDropIntervalLower, healthDropIntervalUpper);
            float randomMultiplier = -Mathf.Log(1f - Random.value) * randy;

            nextHealthDropTime = Time.time + randomMultiplier;
            return true; // A health drop happens
        }
        return false; // No drop
    }
}