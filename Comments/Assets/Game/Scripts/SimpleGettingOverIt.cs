using UnityEngine;

public class SimpleGettingOverIt : MonoBehaviour
{
    [Header("Object References")]
    [Tooltip("The empty 'Arm' object that rotates around the player.")]
    public Transform armTransform;
    [Tooltip("The Hammer's collider at the end of the arm.")]
    public Collider2D hammerCollider;
    [Tooltip("The empty object at the VERY TIP of the hammer.")]
    public Transform hammerTipPivot; // The new pivot point
    private DistanceJoint2D distanceJoint;
    [Header("Normal Movement (After Wings)")]
    public float normalMoveSpeed = 5f;
    public float vaultSpeed = 100f;
    [HideInInspector]
    public bool normalMovementUnlocked = false;

    private Rigidbody2D playerRigidbody;
    private Camera mainCamera;

    // --- NEW: We need to store the grip point and state ---
    private bool isGripped = false;
    private Vector2 gripPoint;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        hammerCollider.enabled = false;
        distanceJoint = GetComponent<DistanceJoint2D>();
    }

    void Update()
    {
        // --- GRIP LOGIC ---
        if (!normalMovementUnlocked)
        {
            // When you are HOLDING the left mouse button down...
            if (Input.GetMouseButton(0))
            {
                // The collider is ALWAYS ON while the button is held.
                hammerCollider.enabled = true;

                // If we are not already gripped...
                if (!isGripped)
                {
                    // ...check if we are touching the ground.
                    if (hammerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
                    {
                        Debug.Log("GRIPPED FOR VAULT!");
                        isGripped = true;
                        gripPoint = hammerTipPivot.position;
                        playerRigidbody.gravityScale = 0;
                    }
                }
            }
            // When the mouse button is NOT being held down...
            else
            {
                // The collider is ALWAYS OFF.
                hammerCollider.enabled = false;

                // If we were gripped, release it.
                if (isGripped)
                {
                    Debug.Log("RELEASED VAULT!");
                    isGripped = false;
                    playerRigidbody.gravityScale = 5;
                }
            }
        }

        // --- MOVEMENT AND ROTATION LOGIC ---

        if (isGripped)
        {
            // --- THIS IS THE MANUAL ROTATE AROUND MECHANIC ---
            // 1. We need to know which way to rotate (clockwise or counter-clockwise).
            // We get the vector from the anchor point to the mouse.
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionFromAnchorToMouse = (mousePosition - gripPoint);

            // 2. We get the vector from the anchor point to the PLAYER.
            Vector2 directionFromAnchorToPlayer = ((Vector2)transform.position - gripPoint);

            // 3. We use the Cross Product to determine if the mouse is "to the left" or "to the right" of the player's arm.
            // This gives us a positive or negative value for the rotation direction.
            float rotateDirection = Vector3.Cross(directionFromAnchorToPlayer.normalized, directionFromAnchorToMouse.normalized).z;

            // 4. Use transform.RotateAround() to rotate the PLAYER around the HAMMER TIP.
            // The 'gripPoint' is the center of the circle.
            // Vector3.forward is the axis to rotate around in 2D (the Z-axis).
            // We multiply by our direction and speed.
            transform.RotateAround(gripPoint, Vector3.forward, rotateDirection * vaultSpeed * Time.deltaTime);

            // 5. After rotating, make the arm point back at the grip point to stay rigid.
            armTransform.up = (gripPoint - (Vector2)transform.position).normalized;
        }
        else
        {
            // ... (Aiming logic is the same and correct) ...
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = (mousePosition - (Vector2)armTransform.position).normalized;
            armTransform.up = directionToMouse;

            if (normalMovementUnlocked)
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                playerRigidbody.velocity = new Vector2(horizontalInput * normalMoveSpeed, playerRigidbody.velocity.y);
            }
        }
    }


}
