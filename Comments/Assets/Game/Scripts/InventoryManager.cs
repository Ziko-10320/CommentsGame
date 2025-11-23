using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    // A HashSet is used for fast checking of whether an item exists.
    private HashSet<string> collectedItems = new HashSet<string>();

    // Method to add an item to our collection
    public void AddItem(string itemName)
    {
        if (!collectedItems.Contains(itemName))
        {
            collectedItems.Add(itemName);
            Debug.Log($"Collected {itemName}!");
            // Optional: Add UI update logic here to show the player what they collected.
        }
    }

    // Method to check if we have a specific item
    public bool HasItem(string itemName)
    {
        return collectedItems.Contains(itemName);
    }

    // Method to check if we have a list of items (for crafting)
    public bool HasAllItems(List<string> itemNames)
    {
        foreach (string itemName in itemNames)
        {
            if (!collectedItems.Contains(itemName))
            {
                return false; // If any item is missing, return false
            }
        }
        return true; // All items were found
    }

    // Method to remove items after crafting
    public void RemoveItems(List<string> itemNames)
    {
        foreach (string itemName in itemNames)
        {
            if (collectedItems.Contains(itemName))
            {
                collectedItems.Remove(itemName);
                Debug.Log($"Used {itemName} in crafting.");
            }
        }
    }
    public void PrintItems()
    {
        if (collectedItems.Count == 0)
        {
            Debug.Log("Inventory is empty.");
            return;
        }

        string items = "Items in inventory: ";
        foreach (string item in collectedItems)
        {
            items += item + ", ";
        }
        Debug.Log(items);
    }
}
