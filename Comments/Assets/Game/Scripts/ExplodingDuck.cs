using UnityEngine;
using System.Collections;

public class ExplodingDuck : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("How fast the duck waddles around.")]
    public float moveSpeed = 1f;
    [Tooltip("How long the duck will walk in one direction before changing.")]
    public float directionChangeInterval = 3f;

    [Header("Explosion")]
    [Tooltip("The minimum time in seconds before the duck explodes.")]
    public float minExplosionTime = 5f;
    [Tooltip("The maximum time in seconds before the duck explodes.")]
    public float maxExplosionTime = 15f;
    [Tooltip("The particle system prefab to spawn on explosion.")]
    public GameObject explosionEffectPrefab;
    [Tooltip("The radius of the explosion's damage.")]
    public float explosionRadius = 2f;
    [Tooltip("The amount of damage the explosion deals.")]
    public int explosionDamage = 2;

    private Rigidbody2D rb;
    private float moveDirection = 1f; // 1 for right, -1 for left
    [Tooltip("If checked, the duck will be destroyed after it explodes. If unchecked, it will explode repeatedly.")]
    public bool diesOnExplode = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Start the simple "AI" for waddling back and forth.
        StartCoroutine(WaddleRoutine());

        // Set the random timer for the explosion.
        float explosionTimer = Random.Range(minExplosionTime, maxExplosionTime);
        // The Invoke method calls a function after a specified delay.
        Invoke("Explode", explosionTimer);

        Debug.Log($"This duck will explode in {explosionTimer:F1} seconds!");
    }

    void FixedUpdate()
    {
        // Apply movement velocity.
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    private IEnumerator WaddleRoutine()
    {
        // This loop will run for the entire life of the duck.
        while (true)
        {
            // Wait for a few seconds.
            yield return new WaitForSeconds(directionChangeInterval);

            // Flip the direction.
            moveDirection *= -1;

            // Flip the sprite's visual direction.
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void Explode()
    {
        Debug.Log("DUCK HAS EXPLODED!");

        // 1. Spawn the visual explosion effect (this part is the same).
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // 2. Deal damage (this part is the same).
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D col in objectsInRange)
        {
            if (col.CompareTag("Player"))
            {
                PlayerHealthAndUI playerHealth = col.GetComponent<PlayerHealthAndUI>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(explosionDamage, "Explosion");
                }
            }
        }

        // --- THIS IS THE MODIFICATION ---
        // 3. Check our new boolean.
        if (diesOnExplode)
        {
            // If the box is checked, destroy the duck.
            Destroy(gameObject);
        }
        else
        {
            // If the box is NOT checked, just set a timer for the next explosion.
            float nextExplosionTimer = Random.Range(minExplosionTime, maxExplosionTime);
            Invoke("Explode", nextExplosionTimer);
            Debug.Log($"This duck will explode again in {nextExplosionTimer:F1} seconds!");
        }
        // --- END OF MODIFICATION ---
    }
    private void OnDestroy()
    {
        // Cancel any pending calls to the "Explode" function to prevent errors.
        CancelInvoke("Explode");
    }
    // This is a helper to see the explosion radius in the editor.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
