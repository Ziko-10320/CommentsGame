using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResourceBarManager : MonoBehaviour
{
    [Header("Resource Settings")]
    [Tooltip("The maximum number of resource segments (e.g., 10 for Clash Royale).")]
    public int maxSegments = 10;
    [Tooltip("The rate at which one segment of resource regenerates (in seconds).")]
    public float regenerationRate = 2f;
    [Tooltip("The starting number of resource segments.")]
    public int startingSegments = 4;

    [Header("UI References")]
    [Tooltip("The parent container for all resource segment images.")]
    public Transform segmentContainer;
    [Tooltip("The Image prefab for a single resource segment.")]
    public Image segmentPrefab;
    [Tooltip("The color for an empty (white) segment.")]
    public Color emptyColor = Color.white;
    [Tooltip("The color for a full (blue) segment.")]
    public Color fullColor = Color.blue;

    private float currentResource = 0f;
    private float timeSinceLastRegen = 0f;
    private List<Image> segments = new List<Image>();

    public int CurrentSegments => Mathf.FloorToInt(currentResource);

    void Start()
    {
        // Initialize the UI segments
        for (int i = 0; i < maxSegments; i++)
        {
            Image newSegment = Instantiate(segmentPrefab, segmentContainer);
            segments.Add(newSegment);
        }

        // Set initial resource
        currentResource = Mathf.Clamp(startingSegments, 0, maxSegments);
        UpdateUI();
    }

    void Update()
    {
        // Only regenerate if not at max segments
        if (currentResource < maxSegments)
        {
            timeSinceLastRegen += Time.deltaTime;

            if (timeSinceLastRegen >= regenerationRate)
            {
                // Add one segment of resource
                currentResource = Mathf.Min(currentResource + 1f, maxSegments);
                timeSinceLastRegen = 0f; // Reset timer
                UpdateUI();
            }
        }
    }

    // Call this method from the spawning script to spend resources
    public bool TrySpendResource(int cost)
    {
        if (CurrentSegments >= cost)
        {
            currentResource -= cost;
            // Ensure the resource doesn't drop below 0
            currentResource = Mathf.Max(currentResource, 0f);
            UpdateUI();
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        int fullSegments = CurrentSegments;

        for (int i = 0; i < maxSegments; i++)
        {
            if (i < fullSegments)
            {
                // Segment is full (blue)
                segments[i].color = fullColor;
            }
            else
            {
                // Segment is empty (white)
                segments[i].color = emptyColor;
            }
        }
    }
}
