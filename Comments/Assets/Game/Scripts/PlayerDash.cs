using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Ghost Effect")]
    public GameObject ghostPrefab; // A prefab for the after-image
    public float ghostInterval = 0.05f;

    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private bool canDash = true;
    private bool isDashing = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        // --- IMPORTANT ---
        // The dash script starts disabled. The PlayerBuffs script will enable it.
        this.enabled = false;
    }

    void Update()
    {
        // When the script is enabled and the player presses Left Shift...
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public bool IsDashing()
    {
        return isDashing;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        // Temporarily disable regular movement control
        playerMovement.enabled = false;

        // Store current gravity and set it to 0 for a straight dash
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // --- THIS IS THE FIX ---
        // Determine dash direction based on MOVEMENT INPUT, not the sprite's scale.
        float dashDirection;
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Check which key is being held down RIGHT NOW.

        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            // If the player is holding left or right, use that direction.
            dashDirection = Mathf.Sign(horizontalInput);
        }
        else
        {
            // If the player is NOT holding any key, use the direction the sprite is facing as a fallback.
            dashDirection = transform.localScale.x;
        }
        // --- END OF FIX ---

        // Apply the dash velocity using the correctly determined direction.
        rb.velocity = new Vector2(dashDirection * dashSpeed, 0f);

        // Start the ghost effect
        StartCoroutine(CreateGhostEffect());

        // Wait for the dash to finish
        yield return new WaitForSeconds(dashDuration);

        // --- Dash is over, clean up ---
        isDashing = false;

        // Stop the player immediately after the dash
        rb.velocity = new Vector2(0f, 0f);

        // Restore gravity and regular movement
        rb.gravityScale = originalGravity;
        playerMovement.enabled = true;

        // Wait for the cooldown to finish
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator CreateGhostEffect()
    {
        while (isDashing)
        {
            // Create a ghost at the player's current position
            GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
            ghost.transform.localScale = transform.localScale; // Match the player's direction
            Destroy(ghost, 0.5f); // The ghost fades and destroys itself

            yield return new WaitForSeconds(ghostInterval);
        }
    }
}
