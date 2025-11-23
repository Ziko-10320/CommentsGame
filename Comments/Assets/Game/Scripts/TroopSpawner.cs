using UnityEngine;

public class TroopSpawner : MonoBehaviour
{
    [Header("Troop Settings")]
    [Tooltip("The prefab of the troop to spawn.")]
    public GameObject troopPrefab;
    [Tooltip("The number of troops to spawn for this cost.")]
    public int troopCount = 1;
    [Tooltip("The resource cost to spawn this troop.")]
    public int troopCost = 2;

    [Header("References")]
    [Tooltip("Reference to the ResourceBarManager script.")]
    public ResourceBarManager resourceManager;

    void Start()
    {
        // Try to find the ResourceBarManager if it wasn't assigned in the Inspector
        if (resourceManager == null)
        {
            resourceManager = FindObjectOfType<ResourceBarManager>();
            if (resourceManager == null)
            {
                Debug.LogError("ResourceBarManager not found in the scene. Spawning will not work.");
            }
        }
    }

    void Update()
    {
        // Check for player input to spawn a troop (e.g., spacebar or mouse click)
        // For simplicity, we'll use the Space key for now.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TrySpawnTroop();
        }
    }

    public void TrySpawnTroop()
    {
        if (troopPrefab == null)
        {
            Debug.LogError("Troop Prefab is not assigned in the Inspector.");
            return;
        }

        if (resourceManager != null)
        {
            // Check if we have enough resources and try to spend them
            if (resourceManager.TrySpendResource(troopCost))
            {
                // Resource spent successfully, now spawn the troop
                SpawnTroop();
            }
            else
            {
                Debug.Log("Not enough resources to spawn troop! Cost: " + troopCost + ", Current: " + resourceManager.CurrentSegments);
            }
        }
    }

    private void SpawnTroop()
    {
        for (int i = 0; i < troopCount; i++)
        {
            // Calculate a slight offset for each troop so they don't spawn exactly on top of each other
            float offsetX = Random.Range(-0.5f, 0.5f);
            float offsetY = Random.Range(-0.5f, 0.5f);
            Vector3 offset = new Vector3(offsetX, offsetY, 0f);

            // Spawn the troop at the spawner's position plus the offset
            Vector3 spawnPosition = transform.position + offset;
            Instantiate(troopPrefab, spawnPosition, Quaternion.identity);
        }

        Debug.Log(troopCount + " troops spawned! Remaining resources: " + resourceManager.CurrentSegments);
    }
}
