using UnityEngine;
using System;

/// <summary>
/// Manages player's oxygen levels, depletion, and refilling mechanics
/// </summary>
public class OxygenSystem : MonoBehaviour
{
    [Header("Oxygen Settings")]
    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float currentOxygen = 100f;
    [SerializeField] private float depletionRate = 5f; // Oxygen per second underwater
    [SerializeField] private float deepWaterDepletionMultiplier = 2f;
    [SerializeField] private float refillRate = 20f; // Oxygen per second at surface

    [Header("Low Oxygen Warning")]
    [SerializeField] private float lowOxygenThreshold = 30f;
    [SerializeField] private bool isLowOxygen = false;

    [Header("References")]
    private PlayerZoneDetector zoneDetector;

    // Events for UI updates
    public event Action<float, float> OnOxygenChanged; // current, max
    public event Action OnOxygenDepleted;
    public event Action<bool> OnLowOxygenWarning; // true when low, false when recovered

    // Public properties
    public float CurrentOxygen => currentOxygen;
    public float MaxOxygen => maxOxygen;
    public float OxygenPercentage => currentOxygen / maxOxygen;
    public bool IsLowOxygen => isLowOxygen;
    public bool IsDepleted => currentOxygen <= 0f;

    void Awake()
    {
        zoneDetector = GetComponent<PlayerZoneDetector>();
        currentOxygen = maxOxygen;
    }

    void Update()
    {
        UpdateOxygen();
    }

    void UpdateOxygen()
    {
        if (zoneDetector == null) return;

        if (zoneDetector.IsAtSurface)
        {
            // Refill oxygen at surface
            RefillOxygen(refillRate * Time.deltaTime);
        }
        else if (zoneDetector.IsUnderwater)
        {
            // Deplete oxygen underwater
            float depletionAmount = depletionRate * Time.deltaTime;

            // Increase depletion in deep water
            if (zoneDetector.CurrentZone == ZoneType.DeepWater)
            {
                depletionAmount *= deepWaterDepletionMultiplier;
            }

            DepleteOxygen(depletionAmount);
        }

        // Check for low oxygen warning
        CheckLowOxygenWarning();
    }

    void DepleteOxygen(float amount)
    {
        currentOxygen = Mathf.Max(0f, currentOxygen - amount);
        OnOxygenChanged?.Invoke(currentOxygen, maxOxygen);

        if (currentOxygen <= 0f)
        {
            OnOxygenDepleted?.Invoke();
        }
    }

    void RefillOxygen(float amount)
    {
        currentOxygen = Mathf.Min(maxOxygen, currentOxygen + amount);
        OnOxygenChanged?.Invoke(currentOxygen, maxOxygen);
    }

    void CheckLowOxygenWarning()
    {
        bool wasLowOxygen = isLowOxygen;
        isLowOxygen = currentOxygen <= lowOxygenThreshold;

        // Trigger event only when state changes
        if (wasLowOxygen != isLowOxygen)
        {
            OnLowOxygenWarning?.Invoke(isLowOxygen);
        }
    }

    /// <summary>
    /// Add oxygen (for upgrades or pickups)
    /// </summary>
    public void AddOxygen(float amount)
    {
        RefillOxygen(amount);
    }

    /// <summary>
    /// Increase max oxygen capacity (for upgrades)
    /// </summary>
    public void IncreaseMaxOxygen(float amount)
    {
        maxOxygen += amount;
        OnOxygenChanged?.Invoke(currentOxygen, maxOxygen);
    }

    /// <summary>
    /// Modify depletion rate (for upgrades)
    /// </summary>
    public void SetDepletionRate(float rate)
    {
        depletionRate = rate;
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // Draw oxygen status as a vertical bar above player
        Vector3 barPosition = transform.position + Vector3.up * 1.5f;
        float barWidth = 1f;
        float barHeight = 0.2f;

        // Background (empty bar)
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(barPosition, new Vector3(barWidth, barHeight, 0));

        // Oxygen fill
        float fillAmount = OxygenPercentage;
        Color oxygenColor = isLowOxygen ? Color.red : Color.cyan;
        Gizmos.color = oxygenColor;
        Vector3 fillPosition = barPosition - new Vector3(barWidth * (1f - fillAmount) * 0.5f, 0, 0);
        Gizmos.DrawCube(fillPosition, new Vector3(barWidth * fillAmount, barHeight, 0));
    }
}
