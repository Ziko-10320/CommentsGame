using UnityEngine;
using System.Collections;

public class ButterflyWander : MonoBehaviour
{
    [Header("Wander Settings")]
    [Tooltip("How fast the butterfly flies.")]
    public float moveSpeed = 2f;

    [Tooltip("The size of the area the butterfly will wander in.")]
    public float wanderRadius = 5f;

    [Tooltip("How long the butterfly will pause at a destination before picking a new one.")]
    public float pauseDuration = 1.5f;

    private Vector2 startingPoint;
    private Vector2 targetPosition;
    private bool isMoving = true;

    void Start()
    {
        // Store the point where the butterfly starts. It will always wander around this point.
        startingPoint = transform.position;

        // Start the main logic loop.
        StartCoroutine(WanderRoutine());
    }

    void Update()
    {
        // If the butterfly is supposed to be moving, smoothly fly towards the target position.
        if (isMoving)
        {
            // Move from the current position to the target position over time.
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Flip the sprite to face the direction it's moving.
            if (transform.position.x < targetPosition.x)
            {
                // Moving right, face right.
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                // Moving left, face left.
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    private IEnumerator WanderRoutine()
    {
        // This loop will run forever, making the butterfly continuously move.
        while (true)
        {
            // 1. Pick a new random destination.
            PickNewTarget();
            isMoving = true;

            // 2. Wait until the butterfly has reached its destination.
            // We check the distance between the butterfly and its target.
            while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
            {
                // 'yield return null' just means "wait for the next frame".
                yield return null;
            }

            // 3. The butterfly has arrived. Pause for a moment.
            isMoving = false;
            yield return new WaitForSeconds(pauseDuration);
        }
    }

    private void PickNewTarget()
    {
        // Find a random point within a circle around the butterfly's starting point.
        Vector2 randomDirection = Random.insideUnitCircle * wanderRadius;
        targetPosition = startingPoint + randomDirection;
        Debug.Log("Butterfly picking new target: " + targetPosition);
    }

    // This is a helper to let you see the wander area in the editor.
    private void OnDrawGizmosSelected()
    {
        // If the script hasn't started yet, use the current position.
        Vector2 center = (startingPoint == Vector2.zero) ? (Vector2)transform.position : startingPoint;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, wanderRadius);
    }
}
