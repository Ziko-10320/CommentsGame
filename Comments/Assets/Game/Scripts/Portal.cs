using UnityEngine;
using UnityEngine.SceneManagement; // This line is ESSENTIAL for scene management!

public class Portal : MonoBehaviour
{
    [Header("Portal Settings")]
    [Tooltip("The EXACT name of the scene this portal should lead to.")]
    public string destinationSceneName;

    [Tooltip("The tag of the object that can use this portal (usually 'Player').")]
    public string targetTag = "Player";

    [Header("Visuals & Sound (Optional)")]
    [Tooltip("An optional particle effect or sprite to show when the portal is used.")]
    public GameObject activationEffect;
    [Tooltip("An optional sound to play when the portal is used.")]
    public AudioClip activationSound;

    private bool isPlayerInRange = false;

    // This function is called by Unity whenever another collider enters this one.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check if the object that entered is the player.
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Player has entered the portal's trigger zone.");
            isPlayerInRange = true;
            // You could add a UI prompt here like "Press E to enter"
        }
    }

    // This function is called by Unity whenever a collider exits this one.
    private void OnTriggerExit2D(Collider2D other)
    {
        // If the player leaves the portal zone, they can't use it anymore.
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Player has left the portal's trigger zone.");
            isPlayerInRange = false;
        }
    }

    // We check for input in the Update function every frame.
    void Update()
    {
        // If the player is in the portal zone AND presses the 'E' key...
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // ...start the teleportation process!
            Teleport();
        }
    }

    private void Teleport()
    {
        // Safety check: make sure a scene name has been entered in the Inspector.
        if (string.IsNullOrEmpty(destinationSceneName))
        {
            Debug.LogError("PORTAL ERROR: Destination Scene Name is not set in the Inspector!");
            return;
        }

        Debug.Log($"Teleporting to scene: {destinationSceneName}");

        // Optional: Play sound and show effects
        if (activationSound != null)
        {
            // This plays the sound at the portal's location.
            AudioSource.PlayClipAtPoint(activationSound, transform.position);
        }
        if (activationEffect != null)
        {
            Instantiate(activationEffect, transform.position, Quaternion.identity);
        }

        // The most important line: Load the new scene!
        SceneManager.LoadScene(destinationSceneName);
    }
}
