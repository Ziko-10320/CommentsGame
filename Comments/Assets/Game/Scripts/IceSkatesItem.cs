using UnityEngine;

public class IceSkatesItem : MonoBehaviour
{
    [Tooltip("The tag of the object that can collect this item (e.g., 'Player').")]
    public string collectorTag = "Player";

    [Header("Sliding Mechanic")]
    [Tooltip("The low-friction Physics Material 2D to apply to the player's collider.")]
    public PhysicsMaterial2D slideMaterial; // NEW: Slot for the slide material

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(collectorTag))
        {
            PlayerHealthAndUI playerHealth = other.GetComponent<PlayerHealthAndUI>();

            if (playerHealth != null)
            {
                // UPDATED: 1. Grant the buff and apply the slide material
                playerHealth.GrantIceSkatesBuff(slideMaterial);

                // 2. Destroy the item so it can't be collected again
                Destroy(gameObject);
            }
        }
    }
}
