using UnityEngine;

public class TennaController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object we collided with is the Player
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player touched Tenna! Let's dance!");
            // Play the dance animation
            animator.Play("Tenna_Dance");
        }
    }
}
