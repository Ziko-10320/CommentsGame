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
        if (other.CompareTag(targetTag))
        {
            // We only need the health script now for this logic
            PlayerHealthAndUI healthSystem = other.GetComponent<PlayerHealthAndUI>();
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (healthSystem != null)
            {
                // 1. Deal damage (this part is correct)
                healthSystem.TakeDamage(damageAmount, damageSourceTag);

                // --- THIS IS THE FINAL FIX ---
                // 2. Check for immunity using the HEALTH script's flag, just like PlayerMovement does.
                bool isImmuneToIce = healthSystem.isIceImmune;

                // Apply knockback ONLY if the source is NOT ice, OR if the player is NOT immune.
                if (rb != null && !(damageSourceTag == "Ice" && isImmuneToIce))
                {
                    // This code will now be SKIPPED correctly when you have the skates.
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    rb.AddForce(Vector2.up * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}
