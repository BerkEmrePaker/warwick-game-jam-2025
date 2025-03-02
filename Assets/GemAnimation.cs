using UnityEngine;

/// <summary>
/// Randomly selects and plays one of the gem animations on startup.
/// </summary>
public class GemAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField]
    /// <summary>
    /// Reference to the Animator component for controlling gem animations.
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Randomly selects one of the gem animations to play on startup.
    /// </summary>
    private void Awake()
    {
        animator.SetInteger("Gem", Random.Range(1, 10)); // Set a random animation from the available options
    }
}