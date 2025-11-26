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

        // 1. Spawn the visual explosion effect at the duck's position.
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // 2. Find all colliders within the explosion radius.
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in objectsInRange)
        {
            // 3. Check if any of the found objects are the Player.
            if (col.CompareTag("Player"))
            {
                PlayerHealthAndUI playerHealth = col.GetComponent<PlayerHealthAndUI>();
                if (playerHealth != null)
                {
                    // 4. Deal damage to the player.
                    Debug.Log("Explosion hit the player!");
                    playerHealth.TakeDamage(explosionDamage, "Explosion");
                }
            }
            // You could add checks for other things here, like enemies or breakable walls.
        }

        // 5. Destroy the duck GameObject.
        Destroy(gameObject);
    }

    // This is a helper to see the explosion radius in the editor.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
