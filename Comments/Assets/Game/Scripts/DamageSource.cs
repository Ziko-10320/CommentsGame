using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [Tooltip("The amount of damage to deal on contact.")]
    public int damageAmount = 1;

    [Tooltip("The vertical force applied to the player on contact (the 'bounce').")]
    public float knockbackForce = 10f;

    [Tooltip("A unique tag for this damage source (e.g., 'Lava', 'Ice').")]
    public string damageSourceTag = "Lava";

    [Tooltip("The tag of the object that can be damaged (e.g., 'Player').")]
    public string targetTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has the target tag
        if (other.CompareTag(targetTag))
        {
            PlayerHealthAndUI healthSystem = other.GetComponent<PlayerHealthAndUI>();
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (healthSystem != null)
            {
                // 1. Deal damage, passing the source tag
                healthSystem.TakeDamage(damageAmount, damageSourceTag);

                // 2. Apply vertical knockback (bounce) - ONLY if not immune to ice or if the source is not ice
                // If the player is immune to ice, we skip the knockback on ice surfaces.
                if (rb != null && !(damageSourceTag == "Ice" && healthSystem.isIceImmune))
                {
                    // Reset vertical velocity to ensure consistent bounce height
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    rb.AddForce(Vector2.up * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // ... (rest of the script remains the same)
    }
}
