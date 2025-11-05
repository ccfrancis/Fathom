using UnityEngine;
using System;

/// <summary>
/// Base class for fish behavior with simple swimming AI
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Fish : MonoBehaviour
{
    [Header("Fish Info")]
    [SerializeField] private string fishName = "Basic Fish";
    [SerializeField] private int value = 10;

    [Header("Movement Settings")]
    [SerializeField] private float swimSpeed = 2f;
    [SerializeField] private float directionChangeTime = 3f;
    [SerializeField] private float turnSpeed = 2f;

    [Header("Boundaries")]
    [SerializeField] private float minX = -20f;
    [SerializeField] private float maxX = 20f;
    [SerializeField] private float minY = -15f;
    [SerializeField] private float maxY = 5f;

    [Header("Debug")]
    [SerializeField] private Vector2 currentDirection;
    [SerializeField] private bool isCaught = false;

    private Rigidbody2D rb;
    private float nextDirectionChangeTime;
    private SpriteRenderer spriteRenderer;

    // Events
    public event Action<Fish> OnCaught;

    // Properties
    public string FishName => fishName;
    public int Value => value;
    public bool IsCaught => isCaught;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Initialize random direction
        currentDirection = GetRandomDirection();
        nextDirectionChangeTime = Time.time + directionChangeTime;
    }

    void FixedUpdate()
    {
        if (!isCaught)
        {
            UpdateMovement();
            CheckBoundaries();
            UpdateVisuals();
        }
    }

    void UpdateMovement()
    {
        // Change direction periodically
        if (Time.time >= nextDirectionChangeTime)
        {
            currentDirection = GetRandomDirection();
            nextDirectionChangeTime = Time.time + directionChangeTime + UnityEngine.Random.Range(-1f, 1f);
        }

        // Move in current direction
        rb.velocity = currentDirection * swimSpeed;
    }

    void CheckBoundaries()
    {
        Vector3 pos = transform.position;
        bool hitBoundary = false;

        // Check boundaries and reverse direction if needed
        if (pos.x < minX || pos.x > maxX)
        {
            currentDirection.x = -currentDirection.x;
            hitBoundary = true;
        }

        if (pos.y < minY || pos.y > maxY)
        {
            currentDirection.y = -currentDirection.y;
            hitBoundary = true;
        }

        // Clamp position to boundaries
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;

        if (hitBoundary)
        {
            // Reset direction change timer when hitting boundary
            nextDirectionChangeTime = Time.time + directionChangeTime;
        }
    }

    void UpdateVisuals()
    {
        // Flip sprite based on direction
        if (spriteRenderer != null && Mathf.Abs(currentDirection.x) > 0.1f)
        {
            spriteRenderer.flipX = currentDirection.x < 0;
        }
    }

    Vector2 GetRandomDirection()
    {
        float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    /// <summary>
    /// Called when this fish is hit by a spear
    /// </summary>
    public void OnHitBySpear(Spear spear)
    {
        if (isCaught) return;

        isCaught = true;
        Debug.Log($"{fishName} caught! Value: {value}");

        // Stop movement
        rb.velocity = Vector2.zero;

        // Fire caught event
        OnCaught?.Invoke(this);

        // Destroy the fish (later we'll add to inventory instead)
        Destroy(gameObject, 0.1f);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // Draw movement direction
        Gizmos.color = isCaught ? Color.red : Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)currentDirection);

        // Draw boundaries (only for one fish to avoid clutter)
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 0);
        Gizmos.DrawWireCube(center, size);
    }
}
