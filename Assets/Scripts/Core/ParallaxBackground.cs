using UnityEngine;

/// <summary>
/// Creates a parallax scrolling effect by moving the transform relative to the camera movement.
/// Objects closer to the camera (higher parallaxEffect values) move faster, creating depth.
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
    [Header("Parallax Settings")]
    [Tooltip("How much this layer moves relative to camera. 0 = no movement, 1 = moves with camera")]
    [Range(0f, 1f)]
    public float parallaxEffect = 0.5f;

    [Tooltip("Reference to the camera (usually Main Camera)")]
    public Transform cameraTransform;

    private Vector3 previousCameraPosition;

    void Start()
    {
        // If no camera is assigned, find the main camera
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Store the initial camera position
        previousCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        // Calculate how much the camera has moved since last frame
        Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;

        // Move this layer by a fraction of the camera movement
        // Lower parallaxEffect = slower movement (appears further away)
        // Higher parallaxEffect = faster movement (appears closer)
        transform.position += new Vector3(
            deltaMovement.x * parallaxEffect,
            deltaMovement.y * parallaxEffect,
            0f
        );

        // Update previous camera position for next frame
        previousCameraPosition = cameraTransform.position;
    }
}
