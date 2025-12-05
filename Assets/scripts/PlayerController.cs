using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    [Tooltip("How fast the player moves left and right")]
    private float moveSpeed = 10f;

    [Header("Boundary Settings")]
    [SerializeField]
    [Tooltip("Left boundary limit")]
    private float minX = -8f;

    [SerializeField]
    [Tooltip("Right boundary limit")]
    private float maxX = 8f;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Update()
    {
        // Stop moving if game is over
        if (gameManager != null && gameManager.IsGameOver())
        {
            return;
        }

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
    /// Called when another collider enters this trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        FallingObject fallingObject = other.GetComponent<FallingObject>();

        if (fallingObject != null)
        {
            fallingObject.OnCaught();
        }
    }
}