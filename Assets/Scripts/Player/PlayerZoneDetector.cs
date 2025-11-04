using UnityEngine;

/// <summary>
/// Attach this to the player to detect which ocean zone they're currently in.
/// This will affect gameplay mechanics like oxygen consumption, movement speed, etc.
/// </summary>
public class PlayerZoneDetector : MonoBehaviour
{
    [Header("Current Zone Status")]
    [Tooltip("The current zone type the player is in")]
    [SerializeField] private ZoneType currentZone = ZoneType.Underwater;

    [Header("Debug")]
    [Tooltip("Log zone changes to console")]
    public bool debugMode = true;

    /// <summary>
    /// Public property to check current zone from other scripts
    /// </summary>
    public ZoneType CurrentZone => currentZone;

    /// <summary>
    /// Check if player is currently at the surface
    /// </summary>
    public bool IsAtSurface => currentZone == ZoneType.Surface;

    /// <summary>
    /// Check if player is currently underwater
    /// </summary>
    public bool IsUnderwater => currentZone == ZoneType.Underwater || currentZone == ZoneType.DeepWater;

    /// <summary>
    /// Called by OceanZone when player enters a zone
    /// </summary>
    public void OnEnterZone(OceanZone zone)
    {
        currentZone = zone.zoneType;

        if (debugMode)
        {
            Debug.Log($"Player entered {zone.zoneType} zone");
        }

        // Here you can trigger zone-specific effects
        HandleZoneEffects(zone.zoneType);
    }

    /// <summary>
    /// Called by OceanZone when player exits a zone
    /// </summary>
    public void OnExitZone(OceanZone zone)
    {
        if (debugMode)
        {
            Debug.Log($"Player exited {zone.zoneType} zone");
        }

        // Additional exit logic can go here
    }

    /// <summary>
    /// Handle effects specific to each zone type
    /// </summary>
    private void HandleZoneEffects(ZoneType zoneType)
    {
        switch (zoneType)
        {
            case ZoneType.Surface:
                // At surface: Restore oxygen, different controls
                // This will be implemented when we add oxygen system
                break;

            case ZoneType.Underwater:
                // Underwater: Normal swimming, consume oxygen
                // This will be implemented with player movement
                break;

            case ZoneType.DeepWater:
                // Deep water: Faster oxygen consumption, pressure effects
                // This will be implemented later
                break;
        }
    }

    /// <summary>
    /// Visual indicator in Scene view
    /// </summary>
    void OnDrawGizmos()
    {
        // Draw a small sphere at player position with color based on zone
        Color gizmoColor = currentZone switch
        {
            ZoneType.Surface => Color.cyan,
            ZoneType.Underwater => Color.blue,
            ZoneType.DeepWater => new Color(0, 0, 0.5f),
            _ => Color.white
        };

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
