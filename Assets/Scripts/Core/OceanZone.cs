using UnityEngine;

/// <summary>
/// Defines the type of ocean zone
/// </summary>
public enum ZoneType
{
    Surface,
    Underwater,
    DeepWater
}

/// <summary>
/// Represents a zone in the ocean with specific properties.
/// Used to detect when player enters different areas (surface vs underwater).
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class OceanZone : MonoBehaviour
{
    [Header("Zone Settings")]
    [Tooltip("The type of zone this represents")]
    public ZoneType zoneType = ZoneType.Underwater;

    [Header("Visual Feedback (Optional)")]
    [Tooltip("Show the zone boundaries in the Scene view")]
    public bool showGizmos = true;

    [Tooltip("Color of the zone gizmo")]
    public Color gizmoColor = new Color(0, 0.5f, 1f, 0.3f);

    private Collider2D zoneCollider;

    void Awake()
    {
        zoneCollider = GetComponent<Collider2D>();

        // Ensure this is set as a trigger
        if (!zoneCollider.isTrigger)
        {
            Debug.LogWarning($"OceanZone '{gameObject.name}' collider should be a trigger. Setting it now.");
            zoneCollider.isTrigger = true;
        }
    }

    /// <summary>
    /// Called when another collider enters this zone
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering has a PlayerZoneDetector component
        PlayerZoneDetector detector = other.GetComponent<PlayerZoneDetector>();
        if (detector != null)
        {
            detector.OnEnterZone(this);
        }
    }

    /// <summary>
    /// Called when another collider exits this zone
    /// </summary>
    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the object exiting has a PlayerZoneDetector component
        PlayerZoneDetector detector = other.GetComponent<PlayerZoneDetector>();
        if (detector != null)
        {
            detector.OnExitZone(this);
        }
    }

    /// <summary>
    /// Draw the zone boundaries in the Scene view for easier visualization
    /// </summary>
    void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = gizmoColor;

        // Get the collider bounds
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.DrawCube(col.bounds.center, col.bounds.size);
        }
    }
}
