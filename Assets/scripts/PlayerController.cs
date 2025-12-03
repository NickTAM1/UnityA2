using UnityEngine;

public class PlayerCon : MonoBehaviour
{

    // Player movement speed - adjustable in inspector
    [Header("Movement Settings")]
    [SerializeField] 
    [Tooltip("How fast the player moves left and right")]
    private float moveSpeed = 10f;

    // Boundary limits for player movement
    [Header("Boundary Settings")]
    [SerializeField]
    [Tooltip("Left boundary limit")]
    private float minX = -8f;
    
    [SerializeField]
    [Tooltip("Right boundary limit")]
    private float maxX = 8f;

    /// <summary>
    /// Update is called once per frame.
    /// Handles player input and movement.
    /// </summary>
    void Update()
    {
        // Get horizontal input (A/D keys or Left/Right arrows)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate new position
        Vector3 newPosition = transform.position;
        newPosition.x += horizontalInput * moveSpeed * Time.deltaTime;

        // Clamp position within boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        // Apply the new position
        transform.position = newPosition;
    }

    /// <summary>
    /// Called when another collider enters this trigger (3D version).
    /// Detects catching falling objects.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is a falling object
        FallingObject fallingObject = other.GetComponent<FallingObject>();

        // If it is, call the OnCaught method
        if (fallingObject != null)
        {
            fallingObject.OnCaught();
        }
    }
}
