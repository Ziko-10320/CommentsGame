using UnityEngine;

// --- HELPER COMPONENT ---
// This is the simple "brain" we will add to the honey ball when it's created.
public class HoneyBallLogic : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.up * speed;
        }
        Destroy(gameObject, lifetime);
    }

    // --- THIS IS THE NEW FUNCTION ---
    // This function is automatically called by Unity's physics engine
    // whenever this object's collider touches another collider.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // We check the layer of the object we collided with.
        // The layer is represented by a number, so we get its name.
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        // If the name of the layer is "Ground"...
        if (layerName == "Ground")
        {
            Debug.Log("Honey Ball hit the ground. Destroying self.");
            // ...destroy the honey ball immediately.
            Destroy(gameObject);
        }

        // Optional: You could add more checks here later.
        // else if (layerName == "Enemy")
        // {
        //     // Deal damage to the enemy
        //     Destroy(gameObject);
        // }
    }
    // --- END OF NEW FUNCTION ---
}


// --- MAIN SCRIPT (No changes needed here) ---
// This is the component you will put on your Player.
public class SuperMode : MonoBehaviour
{
    [Header("Super Mode Settings")]
    [Tooltip("The SpriteRenderer to enable when Super Mode is activated.")]
    public SpriteRenderer superModeVisual;
    [Tooltip("The key to press to activate Super Mode.")]
    public KeyCode activationKey = KeyCode.F;

    [Header("Shooting Settings")]
    [Tooltip("The Honey Ball prefab to be fired (just the sprite and colliders).")]
    public GameObject honeyBallPrefab;
    [Tooltip("The empty GameObject where the honey ball will spawn from.")]
    public Transform firePoint;
    [Tooltip("The speed of the fired honey ball.")]
    public float honeyBallSpeed = 10f;

    void Start()
    {
        if (superModeVisual != null)
        {
            superModeVisual.enabled = false;
        }
    }

    void Update()
    {
        // Check for activation key press
        if (Input.GetKeyDown(activationKey))
        {
            ActivateSuperMode();
        }

        // Check for shooting input only when Super Mode is active
        if (superModeVisual != null && superModeVisual.enabled)
        {
            if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
            {
                ShootHoneyBall();
            }
        }
    }

    private void ShootHoneyBall()
    {
        if (honeyBallPrefab == null || firePoint == null)
        {
            Debug.LogError("Cannot shoot: Honey Ball Prefab or Fire Point is not assigned!");
            return;
        }

        Debug.Log("Firing Honey Ball!");

        GameObject honeyBallInstance = Instantiate(honeyBallPrefab, firePoint.position, firePoint.rotation);
        HoneyBallLogic logic = honeyBallInstance.AddComponent<HoneyBallLogic>();
        logic.speed = this.honeyBallSpeed;
    }

    private void ActivateSuperMode()
    {
        if (superModeVisual == null) return;

        superModeVisual.enabled = !superModeVisual.enabled;

        if (superModeVisual.enabled)
        {
            Debug.Log("Super Mode ACTIVATED!");
        }
        else
        {
            Debug.Log("Super Mode DEACTIVATED.");
        }
    }
}
