using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;

    [Header("Physics Settings")]
    [SerializeField] private float buoyancyForce = 2f;
    [SerializeField] private float waterDrag = 0.95f;
    [SerializeField] private float surfaceDrag = 0.98f;
    [SerializeField] private bool enableBuoyancy = true;

    [Header("Visual Settings")]
    [SerializeField] private float playerScale = 2f;

    [Header("Debug Info")]
    [SerializeField] private Vector2 currentVelocity;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private bool isUnderwater = true;

    private Rigidbody2D rb;
    private PlayerZoneDetector zoneDetector;
    private Vector3 baseScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        zoneDetector = GetComponent<PlayerZoneDetector>();

        // Set initial scale
        baseScale = new Vector3(playerScale, playerScale, 1f);
        transform.localScale = baseScale;
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
    }

    void FixedUpdate()
    {
        UpdateZoneStatus();
        ApplyPhysics();
        MovePlayer();
        currentVelocity = rb.velocity;
    }

    void UpdateZoneStatus()
    {
        if (zoneDetector != null)
        {
            isUnderwater = zoneDetector.IsUnderwater;
        }
    }

    void ApplyPhysics()
    {
        // Apply buoyancy force when underwater
        if (enableBuoyancy && isUnderwater)
        {
            rb.AddForce(Vector2.up * buoyancyForce, ForceMode2D.Force);
        }

        // Apply drag based on zone
        float dragMultiplier = isUnderwater ? waterDrag : surfaceDrag;
        rb.velocity *= dragMultiplier;
    }

    void MovePlayer()
    {
        Vector2 targetVelocity = moveInput * moveSpeed;
        Vector2 newVelocity = Vector2.Lerp(rb.velocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        rb.velocity = newVelocity;

        if (moveInput.magnitude > 0.1f)
        {
            // Track if we're facing left (flipped)
            bool isFacingLeft = transform.localScale.x < 0;

            // Handle horizontal flipping while maintaining base scale
            if (moveInput.x < -0.1f)
            {
                // Moving left - flip to face backward
                transform.localScale = new Vector3(-baseScale.x, baseScale.y, baseScale.z);
                isFacingLeft = true;
            }
            else if (moveInput.x > 0.1f)
            {
                // Moving right - face forward
                transform.localScale = baseScale;
                isFacingLeft = false;
            }

            // Handle vertical tilt (max 45 degrees)
            float tiltAngle = 0f;
            if (Mathf.Abs(moveInput.y) > 0.1f)
            {
                // Normalize vertical input to -1 to 1 range and multiply by 45
                tiltAngle = Mathf.Clamp(moveInput.y, -1f, 1f) * 45f;

                // Invert tilt when facing left so it matches visual direction
                if (isFacingLeft)
                {
                    tiltAngle = -tiltAngle;
                }
            }

            transform.rotation = Quaternion.Euler(0, 0, tiltAngle);
        }
    }

    /// <summary>
    /// Increase move speed (for upgrades)
    /// </summary>
    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;
        Debug.Log($"Move speed increased to {moveSpeed}");
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying && moveInput.magnitude > 0.1f)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)moveInput * 2f);
        }
    }
}
