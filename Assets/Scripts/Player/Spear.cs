using UnityEngine;

/// <summary>
/// Controls spear projectile behavior
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Spear : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    private float speed;
    private float lifetime;
    private float spawnTime;
    private bool hasHit = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // No gravity for spears
    }

    /// <summary>
    /// Initialize the spear with direction, speed, and lifetime
    /// </summary>
    public void Initialize(Vector2 fireDirection, float spearSpeed, float spearLifetime)
    {
        direction = fireDirection.normalized;
        speed = spearSpeed;
        lifetime = spearLifetime;
        spawnTime = Time.time;

        // Set velocity
        rb.velocity = direction * speed;
    }

    void Update()
    {
        // Destroy after lifetime expires
        if (Time.time >= spawnTime + lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Don't collide with player (safe tag check)
        if (HasTag(other, "Player"))
        {
            return;
        }

        // Check if we hit a fish
        if (HasTag(other, "Fish"))
        {
            HitFish(other);
        }
        // Check if we hit terrain
        else if (IsTerrainOrObstacle(other))
        {
            HitTerrain();
        }
    }

    bool HasTag(Collider2D collider, string tag)
    {
        // Check if tag exists and matches
        return collider.tag == tag;
    }

    bool IsTerrainOrObstacle(Collider2D collider)
    {
        // Check common terrain/obstacle tags safely
        return HasTag(collider, "Terrain") ||
               HasTag(collider, "Obstacle") ||
               HasTag(collider, "Wall") ||
               HasTag(collider, "Ground");
    }

    void HitFish(Collider2D fishCollider)
    {
        if (hasHit) return;
        hasHit = true;

        // Try to get fish component and notify it was hit
        var fish = fishCollider.GetComponent<Fish>();
        if (fish != null)
        {
            fish.OnHitBySpear(this);
        }

        // Destroy spear
        Destroy(gameObject);
    }

    void HitTerrain()
    {
        if (hasHit) return;
        hasHit = true;

        // Stop movement
        rb.velocity = Vector2.zero;

        // Optionally: stick to terrain for a moment before disappearing
        Destroy(gameObject, 0.5f);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // Draw spear direction
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(direction * 0.5f));
    }
}
