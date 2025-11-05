using UnityEngine;
using System;

/// <summary>
/// Manages spear gun shooting mechanics for the player
/// </summary>
public class SpearGun : MonoBehaviour
{
    [Header("Spear Settings")]
    [SerializeField] private GameObject spearPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float spearSpeed = 15f;
    [SerializeField] private float spearLifetime = 3f;

    [Header("Firing Settings")]
    [SerializeField] private float fireRate = 0.5f; // Time between shots
    [SerializeField] private bool useMouseButton = true; // Use mouse button instead of key
    [SerializeField] private KeyCode fireKey = KeyCode.E; // Fallback key if not using mouse

    [Header("Debug Info")]
    [SerializeField] private float lastFireTime = -999f;
    [SerializeField] private bool canFire = true;

    // Events
    public event Action OnSpearFired;
    public event Action OnCooldownStart;
    public event Action OnCooldownEnd;

    // Properties
    public bool CanFire => canFire && Time.time >= lastFireTime + fireRate;
    public float CooldownRemaining => Mathf.Max(0f, (lastFireTime + fireRate) - Time.time);
    public float CooldownProgress => Mathf.Clamp01((Time.time - lastFireTime) / fireRate);

    void Update()
    {
        HandleInput();
        UpdateCooldown();
    }

    void HandleInput()
    {
        bool firePressed = false;

        if (useMouseButton)
        {
            firePressed = Input.GetMouseButtonDown(0); // Left mouse button
        }
        else
        {
            firePressed = Input.GetKeyDown(fireKey);
        }

        if (firePressed && CanFire)
        {
            FireSpear();
        }
    }

    void UpdateCooldown()
    {
        bool wasOnCooldown = !canFire;
        canFire = Time.time >= lastFireTime + fireRate;

        // Fire cooldown end event when cooldown finishes
        if (wasOnCooldown && canFire)
        {
            OnCooldownEnd?.Invoke();
        }
    }

    void FireSpear()
    {
        if (spearPrefab == null)
        {
            Debug.LogError("SpearGun: Spear prefab not assigned!");
            return;
        }

        // Determine fire position and direction
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
        Vector2 fireDirection = GetFireDirection();

        // Create spear
        GameObject spearObj = Instantiate(spearPrefab, spawnPosition, Quaternion.identity);

        // Set spear rotation to match fire direction
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        spearObj.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Get or add Spear component
        Spear spear = spearObj.GetComponent<Spear>();
        if (spear == null)
        {
            spear = spearObj.AddComponent<Spear>();
        }

        // Initialize spear
        spear.Initialize(fireDirection, spearSpeed, spearLifetime);

        // Update cooldown
        lastFireTime = Time.time;
        OnSpearFired?.Invoke();
        OnCooldownStart?.Invoke();
    }

    Vector2 GetFireDirection()
    {
        // Check if player is facing left (flipped)
        bool isFacingLeft = transform.localScale.x < 0 || transform.parent.localScale.x < 0;

        // Get player rotation for tilt
        float rotationZ = transform.rotation.eulerAngles.z;
        if (transform.parent != null)
        {
            rotationZ = transform.parent.rotation.eulerAngles.z;
        }

        // Convert rotation to radians
        float angleRad = rotationZ * Mathf.Deg2Rad;

        // Base direction (right or left)
        Vector2 direction = isFacingLeft ? Vector2.left : Vector2.right;

        // Apply tilt rotation
        float cos = Mathf.Cos(angleRad);
        float sin = Mathf.Sin(angleRad);
        Vector2 rotatedDirection = new Vector2(
            direction.x * cos - direction.y * sin,
            direction.x * sin + direction.y * cos
        );

        return rotatedDirection.normalized;
    }

    /// <summary>
    /// Manually trigger spear fire (for external systems)
    /// </summary>
    public void Fire()
    {
        if (CanFire)
        {
            FireSpear();
        }
    }

    /// <summary>
    /// Change fire rate (for upgrades)
    /// </summary>
    public void SetFireRate(float newFireRate)
    {
        fireRate = Mathf.Max(0.1f, newFireRate);
    }

    /// <summary>
    /// Change spear speed (for upgrades)
    /// </summary>
    public void SetSpearSpeed(float newSpeed)
    {
        spearSpeed = Mathf.Max(1f, newSpeed);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // Draw fire point
        Vector3 firePos = firePoint != null ? firePoint.position : transform.position;
        Gizmos.color = CanFire ? Color.green : Color.red;
        Gizmos.DrawWireSphere(firePos, 0.1f);

        // Draw fire direction
        if (CanFire)
        {
            Gizmos.color = Color.yellow;
            Vector2 direction = GetFireDirection();
            Gizmos.DrawLine(firePos, firePos + (Vector3)direction * 2f);
        }
    }
}
