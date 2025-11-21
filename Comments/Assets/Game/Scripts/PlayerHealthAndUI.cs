using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerHealthAndUI : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("The maximum number of health points (hearts).")]
    public int maxHealth = 3;

    [Tooltip("The current number of health points.")]
    public int currentHealth;

    [Header("UI References")]
    [Tooltip("The parent transform for all heart images (e.g., a Horizontal Layout Group).")]
    public Transform heartContainer;
    [Tooltip("The Image prefab for a single heart.")]
    public Image heartPrefab;
    [Tooltip("The sprite to use for a full (red) heart.")]
    public Sprite fullHeartSprite;
    [Tooltip("The sprite to use for an empty (white) heart.")]
    public Sprite emptyHeartSprite;

    private List<Image> hearts = new List<Image>();

    void Start()
    {
        currentHealth = maxHealth;
        InitializeHearts();
    }

    // --- Health Logic ---

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return; // Already dead

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0); // Health cannot go below zero

        UpdateHearts(); // Update UI immediately after taking damage

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log(gameObject.name + " took " + damageAmount + " damage. Remaining health: " + currentHealth);
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died!");

        // For a simple implementation, we can disable the player object
        gameObject.SetActive(false);
    }

    // Optional: Method to restore health
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Health cannot exceed max

        UpdateHearts();
    }

    // --- UI Logic ---

    private void InitializeHearts()
    {
        if (heartContainer == null || heartPrefab == null)
        {
            Debug.LogError("Heart Container or Heart Prefab is not assigned in the Inspector. UI will not display.");
            return;
        }

        // Clear any existing hearts
        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }
        hearts.Clear();

        // Instantiate the heart images
        for (int i = 0; i < maxHealth; i++)
        {
            Image newHeart = Instantiate(heartPrefab, heartContainer);
            hearts.Add(newHeart);
        }

        // Initial update
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
            {
                // This heart is full (Red)
                hearts[i].sprite = fullHeartSprite;
            }
            else
            {
                // This heart is empty (White)
                hearts[i].sprite = emptyHeartSprite;
            }
        }
    }
}
