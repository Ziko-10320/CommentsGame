using UnityEngine;
using UnityEngine.UI; // We need this for the Buttons

public class FlyingBook : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("How fast the book flies towards the player.")]
    public float moveSpeed = 3f;
    [Tooltip("How close the book gets to the player before stopping.")]
    public float stoppingDistance = 2f;

    [Header("UI References")]
    [Tooltip("Drag the main UI Panel object here from your scene.")]
    public GameObject questionPanel;
    [Tooltip("Drag the 'Yes' button here.")]
    public Button yesButton;
    [Tooltip("Drag the 'No' button here.")]
    public Button noButton;

    private Transform playerTarget;
    private bool questionHasBeenAsked = false;

    void Start()
    {
        // 1. Find the player in the scene using their tag.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
        else
        {
            Debug.LogError("FlyingBook could not find an object with the 'Player' tag!");
            Destroy(gameObject); // No player to follow, so destroy self.
            return;
        }

        // 2. Hide the question panel at the start.
        if (questionPanel != null)
        {
            questionPanel.SetActive(false);
        }

        // 3. IMPORTANT: Add "listeners" to the buttons.
        // This tells the buttons which function to call when they are clicked.
        if (yesButton != null)
        {
            // When yesButton is clicked, call the "OnYesClicked" function.
            yesButton.onClick.AddListener(OnYesClicked);
        }
        if (noButton != null)
        {
            // When noButton is clicked, call the "OnNoClicked" function.
            noButton.onClick.AddListener(OnNoClicked);
        }
    }

    void Update()
    {
        // If we have a player to follow AND we haven't asked the question yet...
        if (playerTarget != null && !questionHasBeenAsked)
        {
            // Get the distance to the player.
            float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

            // If we are far from the player, move towards them.
            if (distanceToPlayer > stoppingDistance)
            {
                // Move the book towards the player's position.
                transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, moveSpeed * Time.deltaTime);
            }
            // If we are close enough, stop and ask the question.
            else
            {
                AskQuestion();
            }
        }
    }

    private void AskQuestion()
    {
        Debug.Log("Book is asking the question!");
        questionHasBeenAsked = true; // Make sure we only ask once.

        // Show the UI panel.
        if (questionPanel != null)
        {
            questionPanel.SetActive(true);
        }
    }

    // This function will be called when the "YES" button is clicked.
    public void OnYesClicked()
    {
        Debug.Log("Player answered YES. Correct answer!");

        // Hide the panel and destroy the book.
        if (questionPanel != null)
        {
            questionPanel.SetActive(false);
        }
        Destroy(gameObject);
    }

    // This function will be called when the "NO" button is clicked.
    public void OnNoClicked()
    {
        Debug.Log("Player answered NO. WRONG ANSWER!");

        // Find the player's health script and kill them.
        if (playerTarget != null)
        {
            PlayerHealthAndUI playerHealth = playerTarget.GetComponent<PlayerHealthAndUI>();
            if (playerHealth != null)
            {
                // Deal a huge amount of damage to ensure the player dies.
                playerHealth.TakeDamage(9999, "BadTaste");
            }
        }

        // Hide the panel and destroy the book.
        if (questionPanel != null)
        {
            questionPanel.SetActive(false);
        }
        Destroy(gameObject);
    }
}
