using UnityEngine;
using System.Collections.Generic;

public class CraftingStation : MonoBehaviour
{
    [Header("Recipe Settings")]
    public List<string> requiredItems;
    public string craftedItemName;

    [Header("Visuals")]
    public GameObject interactionPrompt;

    private bool playerIsNearby = false;
    private InventoryManager playerInventory;

    void Start()
    {
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
    }

    void Update()
    {
        // This handles manual crafting, like at the Wings table.
        if (playerIsNearby && Input.GetKeyDown(KeyCode.E))
        {
            AttemptCraft();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNearby = true;
            playerInventory = other.GetComponent<InventoryManager>();
            if (interactionPrompt != null) interactionPrompt.SetActive(true);

            // --- THIS IS THE CRITICAL FIX ---
            // For instant pickups like the skates, we must check for the items
            // immediately when the player enters the trigger.
            Debug.Log("Player entered trigger. Attempting craft immediately for instant pickup.");
            AttemptCraft();
            // --- END OF FIX ---
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNearby = false;
            playerInventory = null;
            if (interactionPrompt != null) interactionPrompt.SetActive(false);
        }
    }

    // We put the logic in its own function to avoid repeating code.
    private void AttemptCraft()
    {
        if (playerInventory == null) return; // Safety check

        if (playerInventory.HasAllItems(requiredItems))
        {
            // Get references to the other scripts on the player
            PlayerHealthAndUI health = playerInventory.GetComponent<PlayerHealthAndUI>();
            PlayerBuffs buffs = playerInventory.GetComponent<PlayerBuffs>();

            if (health == null || buffs == null)
            {
                Debug.LogError("Crafting failed: Player is missing Health or Buffs script!");
                return;
            }

            // --- LOGIC FOR EACH ITEM ---
            if (craftedItemName == "Wings")
            {
                if (Input.GetKeyDown(KeyCode.E)) // Only craft wings on key press
                {
                    buffs.GrantWingsBuff();
                    playerInventory.RemoveItems(requiredItems);
                    Debug.Log($"CRAFTING COMPLETE! You made {craftedItemName}!");
                }
            }
            else if (craftedItemName == "IceSkates")
            {
                // This is an instant craft, no key press needed.
                if (health.isIceImmune) return; // Don't craft if we already have it

                health.isIceImmune = true;
                buffs.ShowSkateVisuals();
                playerInventory.RemoveItems(requiredItems);
                Debug.Log($"CRAFTING COMPLETE! You made {craftedItemName}!");

                // Since this is an instant pickup, destroy the item immediately.
                // The CollectibleItem script will also try to do this, which is fine.
                Destroy(gameObject);
            }
        }
    }
}
