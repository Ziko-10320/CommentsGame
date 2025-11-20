using UnityEngine;

public class DogFollow : MonoBehaviour 
{
    [Header("Follow Settings")]
    [Tooltip("The target the dog will follow (drag the Player object here).")]
    public Transform playerTarget;

    [Tooltip("The specific point on the dog that is used for distance checks (e.g., its nose or feet).")]
    public Transform stoppingPoint; 

    [Tooltip("How fast the dog moves towards the player.")]
    [SerializeField] private float moveSpeed = 5f;

    [Tooltip("The HORIZONTAL distance at which the dog will stop moving.")]
    [SerializeField] private float stoppingDistance = 2f;

    [Header("Animation Settings")]
    [Tooltip("The Animator component on the dog.")]
    [SerializeField] private Animator dogAnimator;

    [Tooltip("The name of the boolean parameter in the Animator (e.g., 'isRunning').")]
    [SerializeField] private string isRunningParameterName = "isRunning";

    // Private variable to hold the reference to the player's transform.
    private Transform target;

    void Start()
    {
        // --- Initialization and Safety Checks ---

        // Use the assigned playerTarget or find the Player by tag.
        if (playerTarget != null)
        {
            target = playerTarget;
        }
        else
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                target = playerObject.transform;
                Debug.Log("DogFollow target not set. Found Player by tag.");
            }
            else
            {
                Debug.LogError("DogFollow Error: Player target is not set and no 'Player' tag found. Disabling script.");
                this.enabled = false;
                return;
            }
        }

        // --- NEW: Safety check for the stoppingPoint ---
        if (stoppingPoint == null)
        {
            Debug.LogWarning("DogFollow Warning: 'stoppingPoint' is not assigned. Defaulting to the dog's main transform. For best results, create and assign a child GameObject as the stopping point.");
            stoppingPoint = this.transform; // Use the dog's own transform as a fallback.
        }

        if (dogAnimator == null)
        {
            Debug.LogError("DogFollow Error: Dog Animator is not assigned. Animations will not work.");
        }
    }

    void Update()
    {
        if (target == null || dogAnimator == null)
        {
            return;
        }

        // --- REVISED: X-AXIS ONLY FOLLOW LOGIC ---

        // 1. Get the positions. We only care about the X coordinate.
        float dogXPosition = stoppingPoint.position.x;
        float playerXPosition = target.position.x;

        // 2. Calculate the HORIZONTAL distance between the dog's stopping point and the player.
        float distanceToPlayerX = Mathf.Abs(dogXPosition - playerXPosition);

        // 3. Check if the dog is outside the horizontal stopping distance.
        if (distanceToPlayerX > stoppingDistance)
        {
            // --- MOVE THE DOG (X-AXIS ONLY) ---

            // Create a target position that has the player's X but the dog's CURRENT Y.
            // This prevents any vertical movement.
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            // Move the dog horizontally towards the target position.
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // --- UPDATE ANIMATION ---
            dogAnimator.SetBool(isRunningParameterName, true);

            // --- FLIP SPRITE ---
            // Calculate horizontal direction to face the player correctly.
            float directionX = playerXPosition - dogXPosition;
            if (directionX > 0)
            {
                // Facing right
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (directionX < 0)
            {
                // Facing left
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            // --- STOP THE DOG ---
            // The dog is close enough horizontally.
            dogAnimator.SetBool(isRunningParameterName, false);
        }
    }

    // This Gizmo now draws from the stoppingPoint to be more accurate.
    void OnDrawGizmosSelected()
    {
        // Use the stoppingPoint's position if it exists, otherwise use the main transform.
        Vector3 center = (stoppingPoint != null) ? stoppingPoint.position : transform.position;

        Gizmos.color = Color.green;
        // Draw lines to visualize the horizontal stopping distance.
        Gizmos.DrawLine(center + new Vector3(-stoppingDistance, -0.5f, 0), center + new Vector3(-stoppingDistance, 0.5f, 0));
        Gizmos.DrawLine(center + new Vector3(stoppingDistance, -0.5f, 0), center + new Vector3(stoppingDistance, 0.5f, 0));
    }
}
