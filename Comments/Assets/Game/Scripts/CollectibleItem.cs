using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Tooltip("A unique name for this item, e.g., 'Crystal', 'Leaf'. This MUST match the name in the Inventory.")]
    public string itemName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that touched this is the player
        if (other.CompareTag("Player"))
        {
            // Try to get the player's inventory
            InventoryManager inventory = other.GetComponent<InventoryManager>();
            if (inventory != null)
            {
                // Add the item to the inventory and destroy the pickup object
                inventory.AddItem(itemName);
                Destroy(gameObject);
            }
        }
    }
}
