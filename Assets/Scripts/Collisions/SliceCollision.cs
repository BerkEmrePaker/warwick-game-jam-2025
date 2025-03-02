using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects collisions with objects on a specific layer and sends the list of collided objects to the player's AttackEnemies script.
/// </summary>
public class SliceCollision : MonoBehaviour
{
    [Header("Collision Settings")]
    [SerializeField]
    /// <summary>
    /// The layer mask used to filter which objects can be detected.
    /// </summary>
    private LayerMask detectionLayer;

    /// <summary>
    /// A list of GameObjects currently colliding with the slice.
    /// </summary>
    private List<GameObject> collidedObjects = new List<GameObject>();

    [SerializeField]
    /// <summary>
    /// Reference to the player GameObject that contains the AttackEnemies script.
    /// </summary>
    private GameObject player;

    /// <summary>
    /// Detects when an object enters the slice's trigger collider and adds it to the list of collided objects.
    /// </summary>
    /// <param name="other">The collider of the object that entered the trigger.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object is on the correct layer
        if ((detectionLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            if (!collidedObjects.Contains(other.gameObject))
            {
                collidedObjects.Add(other.gameObject); // Add the object to the list
            }
            SendHitEnemiesList(); // Send the updated list to the player
        }
    }

    /// <summary>
    /// Detects when an object exits the slice's trigger collider and removes it from the list of collided objects.
    /// </summary>
    /// <param name="other">The collider of the object that exited the trigger.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (collidedObjects.Contains(other.gameObject))
        {
            collidedObjects.Remove(other.gameObject); // Remove the object from the list
        }
        SendHitEnemiesList(); // Send the updated list to the player
    }

    /// <summary>
    /// Sends the current list of collided objects to the player's AttackEnemies script.
    /// </summary>
    private void SendHitEnemiesList()
    {
        if (player != null)
        {
            AttackEnemies collisionReceiver = player.GetComponent<AttackEnemies>();
            if (collisionReceiver != null)
            {
                collisionReceiver.ReceiveHitEnemiesList(new List<GameObject>(collidedObjects)); // Send the list
            }
        }
    }
}