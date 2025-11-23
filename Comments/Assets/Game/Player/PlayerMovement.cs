using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Deceleration Settings")]
    [Tooltip("How quickly the player stops on normal ground.")]
    [Range(0f, 1f)]
    public float groundDamping = 0.8f;
    [Tooltip("How quickly the player stops on ice.")]
    [Range(0f, 1f)]
    public float iceDamping = 0.99f;
    // --- NEW: Air Damping ---
    [Tooltip("How quickly the player stops in the air. Lower value = faster stop.")]
    [Range(0f, 1f)]
    public float airDamping = 0.8f; // Make this low for snappy control

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.15f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private PlayerHealthAndUI playerHealth;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealthAndUI>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    // --- RESTRUCTURED FIXEDUPDATE ---
    void FixedUpdate()
    {
        if (!enabled) return; // For the dash script

        // --- Grounded Movement ---
        if (isGrounded)
        {
            if (moveInput != 0)
            {
                // Apply movement directly when on the ground
                rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            }
            else
            {
                // Apply ground/ice deceleration when not moving
                if (playerHealth == null) return;
                bool hasIceSkates = playerHealth.isIceImmune;
                float currentDamping = hasIceSkates ? iceDamping : groundDamping;
                rb.velocity = new Vector2(rb.velocity.x * currentDamping, rb.velocity.y);
            }
        }
        // --- Aerial Movement ---
        else // if (isGrounded == false)
        {
            if (moveInput != 0)
            {
                // Apply movement in the air
                rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            }
            else
            {
                // --- THIS IS THE FIX ---
                // Apply air deceleration when you let go of keys in the air.
                rb.velocity = new Vector2(rb.velocity.x * airDamping, rb.velocity.y);
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
