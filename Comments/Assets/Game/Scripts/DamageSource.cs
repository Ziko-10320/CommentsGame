using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [Tooltip("The amount of damage to deal on contact.")]
    public int damageAmount = 1;

    [Tooltip("The vertical force applied to the player on contact (the 'bounce').")]
    public float knockbackForce = 10f; // New variable for knockback force

    [Tooltip("The tag of the object that can be damaged (e.g., 'Player').")]
    public string targetTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has the target tag
        if (other.CompareTag(targetTag))
        {
            // Try to get the HealthSystem component from the colliding object
            PlayerHealthAndUI healthSystem = other.GetComponent<PlayerHealthAndUI>();
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D

            if (healthSystem != null)
            {
                // 1. Deal damage
                healthSystem.TakeDamage(damageAmount);

                // 2. Apply vertical knockback (bounce)
                if (rb != null)
                {
                    // Reset vertical velocity to ensure consistent bounce height
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    // Apply the upward force using Impulse for an instant push
                    rb.AddForce(Vector2.up * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    // Use OnTriggerStay2D for continuous damage (like standing in lava)
    private void OnTriggerStay2D(Collider2D other)
    {
        // For simplicity, we'll only use OnTriggerEnter2D for a single hit.
        // If you want continuous damage, you would implement a timer here
        // and call healthSystem.TakeDamage(damageAmount) every few seconds.
    }
}
