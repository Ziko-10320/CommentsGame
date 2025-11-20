using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("The target the camera will follow (your player).")]
    public Transform target;

    [Tooltip("How quickly the camera will move to the target's position.")]
    public float smoothSpeed = 0.125f;

    [Tooltip("The offset from the target's position (e.g., to keep the camera slightly above or behind).")]
    public Vector3 offset;

    void Start()
    {
        // If the target is not set in the Inspector, try to find the player by tag
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogError("CameraFollow: Target not set and Player object with tag 'Player' not found.");
            }
        }

        // Set the initial offset if it's zero
        if (offset == Vector3.zero && target != null)
        {
            // Assuming a 2D game where Z is the camera distance
            offset = transform.position - target.position;
        }
    }

    // Use LateUpdate to ensure the camera moves after the target has moved in Update
    void LateUpdate()
    {
        if (target == null) return;

        // Calculate the desired position of the camera
        // We only want to track the X position of the target
        Vector3 desiredPosition = new Vector3(target.position.x, transform.position.y, transform.position.z) + new Vector3(offset.x, 0, 0);

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Preserve the camera's original Y and Z position (or the initial offset Y/Z)
        transform.position = new Vector3(smoothedPosition.x, transform.position.y, transform.position.z);
    }
}
