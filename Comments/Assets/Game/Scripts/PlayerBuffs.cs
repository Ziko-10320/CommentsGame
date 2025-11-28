using UnityEngine;

public class PlayerBuffs : MonoBehaviour
{
    [Header("Wings Buff Settings")]
    public bool hasWings = false;
    

    [Header("Ice Skates Buff Settings")]
    public bool hasIceSkates = false;
    public PhysicsMaterial2D slideMaterial;
    private PhysicsMaterial2D originalPhysicsMaterial;


    [Header("Visuals")]
    [Tooltip("The first wing sprite (e.g., the one in front).")]
    public SpriteRenderer wingVisual1; // Renamed for clarity

    // --- NEW: Add a slot for the second wing sprite ---
    [Tooltip("The second wing sprite (e.g., the one behind).")]
    public SpriteRenderer wingVisual2;
    public SpriteRenderer iceSkateVisualFront;
    public SpriteRenderer iceSkateVisualBack;

    private PlayerMovement playerMovement;
    private Collider2D playerCollider;
    private float originalJumpForce;

    void Awake()
    {
        // --- DETECTIVE CODE ---
        Debug.Log("PlayerBuffs script is AWAKE.");

        playerMovement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<Collider2D>();

        // --- MORE DETECTIVE CODE ---
        if (playerMovement == null)
        {
            Debug.LogError("CRITICAL FAIL in PlayerBuffs: Could not find the 'PlayerMovement' script on this object!");
        }
        else
        {
            Debug.Log("PlayerBuffs successfully found the PlayerMovement script.");
            originalJumpForce = playerMovement.jumpForce;
        }

        if (playerCollider != null)
        {
            originalPhysicsMaterial = playerCollider.sharedMaterial;
        }

        // Hide visuals at start
        if (wingVisual1 != null) wingVisual1.enabled = false;
        if (wingVisual2 != null) wingVisual2.enabled = false;
        if (iceSkateVisualFront != null) iceSkateVisualFront.enabled = false;
        if (iceSkateVisualBack != null) iceSkateVisualBack.enabled = false;
    }

    public void GrantWingsBuff()
    {
        if (hasWings) return;
        hasWings = true;

        // 1. Enable the PlayerDash script.
        PlayerDash dashScript = GetComponent<PlayerDash>();
        if (dashScript != null)
        {
            dashScript.enabled = true;
        }

        // 2. Find the NEW movement script and unlock normal A/D movement.
        SimpleGettingOverIt movementScript = GetComponent<SimpleGettingOverIt>();
        if (movementScript != null)
        {
            movementScript.normalMovementUnlocked = true;
            Debug.Log("WINGS BUFF GRANTED: Normal movement unlocked!");
        }

        // 3. Enable the wing visuals.
        if (wingVisual1 != null) wingVisual1.enabled = true;
        if (wingVisual2 != null) wingVisual2.enabled = true;
    }
    public void ShowSkateVisuals()
    {
        if (iceSkateVisualFront != null) iceSkateVisualFront.enabled = true;
        if (iceSkateVisualBack != null) iceSkateVisualBack.enabled = true;
    }
    // ... (GrantIceSkatesBuff method is unchanged) ...
    public void GrantIceSkatesBuff()
    {
        if (hasIceSkates) return;

        hasIceSkates = true;
        if (playerCollider != null && slideMaterial != null)
        {
            playerCollider.sharedMaterial = slideMaterial;
        }
        if (iceSkateVisualFront != null) iceSkateVisualFront.enabled = true;
        if (iceSkateVisualBack != null) iceSkateVisualBack.enabled = true;
        Debug.Log("Ice Skates buff granted!");
    }
}
