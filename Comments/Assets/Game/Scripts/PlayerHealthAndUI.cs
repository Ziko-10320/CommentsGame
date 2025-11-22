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

    [Header("Buffs")]
    public bool isIceImmune = false;

    [Header("Visuals")]
    [Tooltip("The SpriteRenderer for the FRONT ice skate visual.")]
    public SpriteRenderer iceSkateVisualFront; // Renamed for clarity

    // --- NEW: Add a reference for the back skate ---
    [Tooltip("The SpriteRenderer for the BACK ice skate visual.")]
    public SpriteRenderer iceSkateVisualBack;

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
    private PhysicsMaterial2D originalPhysicsMaterial; // NEW: To store the default material
    private Collider2D playerCollider; // NEW: To access the collider

    void Start()
    {
        currentHealth = maxHealth;
        InitializeHearts();

        // NEW: Store the original physics material and collider
        playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            originalPhysicsMaterial = playerCollider.sharedMaterial;
        }
    }

    // --- Health Logic ---

    public void TakeDamage(int damageAmount, string damageSourceTag = "")
    {
        if (currentHealth <= 0) return;

        // Check for immunity based on the damage source tag
        if (damageSourceTag == "Ice" && isIceImmune)
        {
            Debug.Log("Immune to Ice damage!");
            return;
        }

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHearts();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log(gameObject.name + " took " + damageAmount + " damage from " + damageSourceTag + ". Remaining health: " + currentHealth);
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        gameObject.SetActive(false);
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateHearts();
    }

    // UPDATED: Method to grant the ice skates buff and apply the slide material
    public void GrantIceSkatesBuff(PhysicsMaterial2D slideMaterial)
    {
        isIceImmune = true;
        if (iceSkateVisualFront != null)
        {
            iceSkateVisualFront.enabled = true;
        }
        if (iceSkateVisualBack != null)
        {
            iceSkateVisualBack.enabled = true;
        }
        // NEW: Apply the sliding physics material
        if (playerCollider != null)
        {
            playerCollider.sharedMaterial = slideMaterial;
        }

        Debug.Log(gameObject.name + " collected Ice Skates and is now immune to Ice damage and can slide!");
    }

    // --- UI Logic ---

    private void InitializeHearts()
    {
        if (heartContainer == null || heartPrefab == null)
        {
            Debug.LogError("Heart Container or Heart Prefab is not assigned in the Inspector. UI will not display.");
            return;
        }

        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }
        hearts.Clear();

        for (int i = 0; i < maxHealth; i++)
        {
            Image newHeart = Instantiate(heartPrefab, heartContainer);
            hearts.Add(newHeart);
        }

        UpdateHearts();
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeartSprite;
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite;
            }
        }
    }
}
