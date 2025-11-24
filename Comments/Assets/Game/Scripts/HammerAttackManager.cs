using UnityEngine;
using System.Collections;

// We will add a new component to the hammer when we spawn it.
// This component contains the hammer's "brain".
public class HammerLogic : MonoBehaviour
{
    // --- Public variables are set by the manager that spawns it ---
    public float moveSpeed;
    public float rotationSpeed;
    public int damageAmount;

    private Transform playerTarget;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
        else
        {
            Destroy(gameObject); // No player to chase
        }
    }

    void FixedUpdate()
    {
        if (playerTarget == null) return;

        // Homing Logic
        Vector2 direction = (Vector2)playerTarget.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -rotateAmount * rotationSpeed;
        rb.velocity = transform.up * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthAndUI playerHealth = other.GetComponent<PlayerHealthAndUI>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount, "Hammer");
            }
            Destroy(gameObject); // Destroy self on impact
        }
    }
}


// This is the main manager script you will place in your scene.
public class HammerAttackManager : MonoBehaviour
{
    [Header("Hammer Prefab")]
    [Tooltip("The visual prefab for the hammer (just the sprite and colliders).")]
    public GameObject hammerVisualPrefab;

    [Header("Spawn Settings")]
    [Tooltip("The empty GameObject where the hammer will be created.")]
    public Transform spawnPoint;
    [Tooltip("The key to press to spawn the hammer.")]
    public KeyCode spawnKey = KeyCode.Y;

    [Header("Hammer Behavior")]
    [Tooltip("How fast the hammer moves towards the player.")]
    public float moveSpeed = 5f;
    [Tooltip("How fast the hammer rotates to face the player.")]
    public float rotationSpeed = 200f;
    [Tooltip("The amount of damage the hammer deals on impact.")]
    public int damageAmount = 1;

    void Update()
    {
        // Listen for the key press.
        if (Input.GetKeyDown(spawnKey))
        {
            SpawnAndLaunchHammer();
        }
    }

    private void SpawnAndLaunchHammer()
    {
        // Safety Checks
        if (hammerVisualPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Hammer Prefab or Spawn Point is not assigned in the HammerAttackManager!");
            return;
        }

        Debug.Log("Spawning a homing hammer!");

        // 1. Create an instance of the hammer's visual prefab.
        GameObject hammerInstance = Instantiate(hammerVisualPrefab, spawnPoint.position, spawnPoint.rotation);

        // 2. Add the "brain" component (HammerLogic) to the new instance.
        HammerLogic logic = hammerInstance.AddComponent<HammerLogic>();

        // 3. Pass the settings from this manager to the new hammer's brain.
        logic.moveSpeed = this.moveSpeed;
        logic.rotationSpeed = this.rotationSpeed;
        logic.damageAmount = this.damageAmount;
    }
}
