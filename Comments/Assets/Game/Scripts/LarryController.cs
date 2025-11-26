using UnityEngine;
using System.Collections;

public class LarryController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("How fast Larry moves back and forth.")]
    public float moveSpeed = 1.5f;
    [Tooltip("How long Larry will walk in one direction before changing.")]
    public float directionChangeInterval = 4f;

    // --- THE IMPORTANT CHECKBOX ---
    [Header("Behavior")]
    [Tooltip("Check this box if this Larry should deal damage on contact.")]
    public bool isEvil = false;

    [Tooltip("The amount of damage to deal if this Larry is evil.")]
    public int damageAmount = 1;

    private Rigidbody2D rb;
    private float moveDirection = 1f; // 1 for right, -1 for left

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Start the simple "AI" for walking back and forth.
        StartCoroutine(WaddleRoutine());
    }

    void FixedUpdate()
    {
        // Apply movement velocity.
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    private IEnumerator WaddleRoutine()
    {
        // This loop will run for the entire life of Larry.
        while (true)
        {
            // Wait for a few seconds.
            yield return new WaitForSeconds(directionChangeInterval);

            // Flip the movement direction.
            moveDirection *= -1;

            // Flip the sprite's visual direction to match the movement.
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    // This function is called by Unity's physics engine when this object's collider
    // bumps into another solid collider.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // --- DAMAGE LOGIC ---
        // First, check if this is the "evil" version of Larry.
        // If the 'isEvil' box is not checked, this entire block is skipped.
        if (isEvil)
        {
            // Next, check if the object we bumped into is the Player.
            if (collision.gameObject.CompareTag("Player"))
            {
                // Get the player's health script.
                PlayerHealthAndUI playerHealth = collision.gameObject.GetComponent<PlayerHealthAndUI>();
                if (playerHealth != null)
                {
                    // Deal damage to the player.
                    Debug.Log("Evil Larry hit the player!");
                    playerHealth.TakeDamage(damageAmount, "Larry");
                }
            }
        }
    }
}
