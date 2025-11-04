using UnityEngine;

/// <summary>
/// Smoothly follows the player character with the camera.
/// This movement triggers the parallax effect on background layers.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [Tooltip("The target to follow (usually the Player)")]
    public Transform target;

    [Tooltip("How smoothly the camera follows (lower = smoother but slower)")]
    [Range(0.01f, 1f)]
    public float smoothSpeed = 0.125f;

    [Header("Offset")]
    [Tooltip("Offset from the target position")]
    public Vector3 offset = new Vector3(0, 0, -10f);

    [Header("Boundaries (Optional)")]
    [Tooltip("Enable camera boundaries to prevent going outside play area")]
    public bool useBoundaries = false;

    [Tooltip("Minimum X position the camera can reach")]
    public float minX = -20f;

    [Tooltip("Maximum X position the camera can reach")]
    public float maxX = 20f;

    [Tooltip("Minimum Y position the camera can reach")]
    public float minY = -10f;

    [Tooltip("Maximum Y position the camera can reach")]
    public float maxY = 10f;

    void LateUpdate()
    {
        // Don't follow if no target assigned
        if (target == null)
        {
            Debug.LogWarning("CameraFollow: No target assigned!");
            return;
        }

        // Calculate desired position
        Vector3 desiredPosition = target.position + offset;

        // Apply boundaries if enabled
        if (useBoundaries)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }

        // Smoothly interpolate to desired position
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed
        );

        // Update camera position
        transform.position = smoothedPosition;
    }

    /// <summary>
    /// Visualize camera boundaries in Scene view
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (!useBoundaries) return;

        Gizmos.color = Color.yellow;

        // Draw boundary box
        Vector3 center = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 0);

        Gizmos.DrawWireCube(center, size);
    }
}
