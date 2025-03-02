using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the spawning of XP objects at random positions within defined ranges.
/// Ensures that spawned objects are a minimum distance away from the player.
/// </summary>
public class XPSpawner : MonoBehaviour
{
    /// <summary>
    /// The prefab of the object to spawn (e.g., an XP orb).
    /// </summary>
    public GameObject objectToSpawn;

    /// <summary>
    /// The parent GameObject under which spawned objects will be placed.
    /// </summary>
    public Transform parentObject;

    /// <summary>
    /// Reference to the player's Transform to calculate spawn positions relative to the player.
    /// </summary>
    public Transform player;

    /// <summary>
    /// The time interval (in seconds) between each spawn.
    /// </summary>
    public float spawnInterval = 5f;

    /// <summary>
    /// The range of X-axis values for random spawn positions.
    /// </summary>
    public Vector2 xRange = new Vector2(-5f, 5f);

    /// <summary>
    /// The range of Y-axis values for random spawn positions.
    /// </summary>
    public Vector2 yRange = new Vector2(-5f, 5f);

    /// <summary>
    /// The range of Z-axis values for random spawn positions.
    /// </summary>
    public Vector2 zRange = new Vector2(-5f, 5f);

    /// <summary>
    /// The minimum distance a spawned object must be from the player.
    /// </summary>
    public float minDistanceFromPlayer = 2f;

    /// <summary>
    /// Starts the spawning process after a short delay.
    /// </summary>
    void Start()
    {
        StartCoroutine(EnableWithDelay());
    }

    /// <summary>
    /// Waits for a short delay before starting the spawning coroutine.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    private IEnumerator EnableWithDelay()
    {
        // Wait for 3 seconds before starting the spawner
        yield return new WaitForSeconds(3);

        // Start the coroutine to spawn objects repeatedly
        StartCoroutine(SpawnObjects());
    }

    /// <summary>
    /// Continuously spawns objects at random positions within the defined ranges.
    /// Ensures that spawned objects are not too close to the player.
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

            // Spawn the object at the random position and make it a child of the parent object
            GameObject newXpObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity, parentObject);
            newXpObject.transform.localPosition = randomPosition;

            // Wait for the defined spawn interval before spawning the next object
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}