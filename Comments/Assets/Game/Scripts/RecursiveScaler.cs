using UnityEngine;

public class RecursiveScaler : MonoBehaviour
{
    [Header("Duplication Settings")]
    [Tooltip("The key to press to trigger the duplication.")]
    public KeyCode duplicationKey = KeyCode.G;

    [Tooltip("How much smaller the new duplicate will be (e.g., 0.5 for half size).")]
    [Range(0.1f, 0.9f)]
    public float scaleDownFactor = 0.75f;

    [Header("Safety Limits (to prevent lag/crash)")]
    [Tooltip("The maximum number of times this object can be duplicated in a chain.")]
    public int maxGenerations = 5;

    // This is a private variable to track our current generation.
    private int currentGeneration = 1;

    void Update()
    {
        // Check if the player presses the duplication key.
        if (Input.GetKeyDown(duplicationKey))
        {
            // Call the function to create the duplicate.
            CreateDuplicate();
        }
    }

    private void CreateDuplicate()
    {
        // --- SAFETY CHECK ---
        // If we have reached our limit, stop right here to prevent a crash.
        if (currentGeneration >= maxGenerations)
        {
            Debug.Log("Maximum duplication depth reached. Stopping to prevent lag.");
            return;
        }

        Debug.Log($"Duplicating! This is generation {currentGeneration + 1}.");

        // 1. Create an exact copy of this GameObject at the same position and rotation.
        GameObject duplicate = Instantiate(gameObject, transform.position, transform.rotation);

        // 2. Scale the new duplicate down.
        duplicate.transform.localScale = transform.localScale * scaleDownFactor;

        // 3. Get the script component on the new duplicate.
        RecursiveScaler duplicateScript = duplicate.GetComponent<RecursiveScaler>();

        // 4. IMPORTANT: Tell the new duplicate that it is the next generation.
        if (duplicateScript != null)
        {
            duplicateScript.currentGeneration = this.currentGeneration + 1;
        }
    }
}
