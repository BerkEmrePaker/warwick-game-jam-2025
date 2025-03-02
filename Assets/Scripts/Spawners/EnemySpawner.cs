using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the spawning of enemy objects at random positions within defined ranges.
/// Ensures that spawned enemies are a minimum distance away from the player.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    /// <summary>
    /// The prefab of the enemy object to spawn.
    /// </summary>
    public GameObject objectToSpawn;

    /// <summary>
    /// The parent GameObject under which spawned enemies will be placed.
    /// </summary>
    public Transform parentObject;

    /// <summary>
    /// Reference to the player's Transform to calculate spawn positions relative to the player.
    /// </summary>
    public Transform player;

    /// <summary>
    /// The time interval (in seconds) between each enemy spawn.
    /// </summary>
    public float spawnInterval = 3f;

    [Header("Spawn Position Ranges")]
    /// <summary>
    /// The range of X-axis values for random spawn positions.
    /// </summary>
    public Vector2 xRange = new Vector2(-5f, 5f);

    /// <summary>
    /// The range of Y-axis values for random spawn positions.
    /// </summary>
    public Vector2 yRange = new Vector2(0f, 0f);

    /// <summary>
    /// The range of Z-axis values for random spawn positions.
    /// </summary>
    public Vector2 zRange = new Vector2(-5f, 5f);

    /// <summary>
    /// The minimum distance a spawned enemy must be from the player.
    /// </summary>
    public float minDistanceFromPlayer = 2f;

    /// <summary>
    /// Reference to the coroutine responsible for spawning enemies.
    /// </summary>
    private Coroutine spawnCoroutine;

    /// <summary>
    /// Starts the spawning process when the GameObject is enabled.
    /// </summary>
    private void OnEnable()
    {
        // Start the coroutine to spawn objects after a 2-second delay
        StartCoroutine(EnableWithDelay());
    }

    /// <summary>
    /// Waits for a short delay before starting the spawning coroutine.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    private IEnumerator EnableWithDelay()
    {
        // Wait for 2 seconds before starting the spawner
        yield return new WaitForSeconds(2);

        // Start the coroutine to spawn objects repeatedly
        spawnCoroutine = StartCoroutine(SpawnObjects());
    }

    /// <summary>
    /// Stops the spawning coroutine when the GameObject is disabled to avoid errors.
    /// </summary>
    private void OnDisable()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine); // Stop the coroutine
            spawnCoroutine = null; // Clear the reference
        }
    }

    /// <summary>
    /// Continuously spawns enemy objects at random positions within the defined ranges.
    /// Ensures that spawned enemies are not too close to the player.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            Vector3 randomPosition;

            // Generate random positions until one is sufficiently far from the player
            do
            {
                randomPosition = new Vector3(
                    Random.Range(xRange.x, xRange.y),
                    Random.Range(yRange.x, yRange.y),
                    Random.Range(zRange.x, zRange.y)
                );
            }
            while (Vector3.Distance(randomPosition, player.position) < minDistanceFromPlayer);

            // Spawn the enemy at the random position and make it a child of the parent object
            Instantiate(objectToSpawn, randomPosition, Quaternion.identity, parentObject);

            // Wait for the defined spawn interval before spawning the next enemy
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}