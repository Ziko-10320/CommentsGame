using UnityEngine;

public class TroopFollower : MonoBehaviour
{
    [Tooltip("The speed at which the troop follows the target.")]
    public float moveSpeed = 5f;

    [Tooltip("The distance the troop will try to maintain from the target.")]
    public float stoppingDistance = 1.5f;

    private Transform target;

    void Start()
    {
        // Find the player object by tag. Assumes the player object is tagged "Player".
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Player object with tag 'Player' not found. The troop will not move.");
        }
    }

    void Update()
    {
        if (target != null)
        {
            // Calculate the distance to the target
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // Only move if the troop is further away than the stopping distance
            if (distanceToTarget > stoppingDistance)
            {
                // Calculate the direction to the target
                Vector3 direction = (target.position - transform.position).normalized;

                // Move the troop towards the target
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
    }
}
