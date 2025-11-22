using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    // --- NEW: Deceleration Settings ---
    [Header("Deceleration Settings")]
    [Tooltip("How quickly the player stops on normal ground. A lower value means a faster stop.")]
    [Range(0f, 1f)]
    public float groundDamping = 0.8f; // Player stops quickly
    [Tooltip("How quickly the player stops on ice. A higher value means a longer slide.")]
    [Range(0f, 1f)]
    public float iceDamping = 0.99f; // Player slides for a while

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.15f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput; // --- MODIFIED: Made this a class-level variable

    // --- NEW: Reference to the health script ---
    private PlayerHealthAndUI playerHealth;

    void Awake() // --- MODIFIED: Changed Start to Awake for reliability
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealthAndUI>(); // Get the health script
    }

    void Update()
    {
        // --- Ground Check (circle) ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // --- Horizontal Input ---
        moveInput = Input.GetAxisRaw("Horizontal"); // Store input

        // --- Jump (Space) ---
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    // --- NEW: Using FixedUpdate for all physics ---
    void FixedUpdate()
    {
        if (moveInput != 0)
        {
            // --- Player is pressing a move key ---
            // This line is the same as your old movement code
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            // --- Player is NOT pressing a move key (this is where sliding happens) ---
            if (isGrounded) // Only apply damping if on the ground
            {
                // Check if the player has the ice skates buff from the health script
                bool hasIceSkates = playerHealth.isIceImmune;

                // Apply the correct damping based on whether we have the skates
                float currentDamping = hasIceSkates ? iceDamping : groundDamping;

                // Reduce the horizontal velocity over time to create a sliding or stopping effect
                rb.velocity = new Vector2(rb.velocity.x * currentDamping, rb.velocity.y);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}
