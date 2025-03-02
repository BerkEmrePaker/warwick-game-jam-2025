using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// Handles camera shake effects using Cinemachine's impulse system.
/// </summary>
public class CameraShake : MonoBehaviour
{
    /// <summary>
    /// Reference to the CinemachineImpulseSource component for generating camera shakes.
    /// </summary>
    private CinemachineImpulseSource impulseSource;

    [Header("Shake Settings")]
    [SerializeField]
    /// <summary>
    /// The force of the camera shake.
    /// </summary>
    private float cameraShakeForce;

    /// <summary>
    /// Initializes the CinemachineImpulseSource component.
    /// </summary>
    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    /// <summary>
    /// Triggers a camera shake with the specified force.
    /// </summary>
    public void ShakeCamera()
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulseWithForce(cameraShakeForce); // Generate a camera shake
        }
    }
}