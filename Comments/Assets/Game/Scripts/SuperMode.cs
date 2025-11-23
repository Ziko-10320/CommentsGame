using UnityEngine;

public class SuperMode : MonoBehaviour
{
    [Header("Super Mode Settings")]
    [Tooltip("The SpriteRenderer to enable when Super Mode is activated (e.g., an aura, a flame effect, etc.).")]
    public SpriteRenderer superModeVisual;

    [Tooltip("The key to press to activate Super Mode.")]
    public KeyCode activationKey = KeyCode.F;

    void Start()
    {
        // Ensure the visual is disabled when the game starts.
        if (superModeVisual != null)
        {
            superModeVisual.enabled = false;
        }
        else
        {
            Debug.LogWarning("SuperMode script is missing a visual! Please assign a SpriteRenderer in the Inspector.");
        }
    }

    void Update()
    {
        // Check if the player presses the activation key.
        if (Input.GetKeyDown(activationKey))
        {
            // Call the function to activate the mode.
            ActivateSuperMode();
        }
    }

    private void ActivateSuperMode()
    {
        // Safety check to make sure a visual has been assigned.
        if (superModeVisual == null)
        {
            Debug.LogError("Cannot activate Super Mode because no visual has been assigned in the Inspector!");
            return;
        }

        // Check if the visual is already enabled.
        if (superModeVisual.enabled)
        {
            // If it's already on, do nothing (or you could make it a toggle here).
            Debug.Log("Super Mode is already active.");
        }
        else
        {
            // If it's off, turn it on.
            Debug.Log("Super Mode ACTIVATED!");
            superModeVisual.enabled = true;
        }
    }
}
