using UnityEngine;

public abstract class FallingObject : MonoBehaviour
{
    // Fall speed - adjustable in inspector
    [Header("Movement Settings")]
    [SerializeField]
    [Tooltip("How fast the object falls downward")]
    protected float fallSpeed = 5f;

    // Reference to game manager
    protected GameManager gameManager;

    /// <summary>
    /// Initialize the falling object with a reference to the game manager.
    /// </summary>
    public virtual void Initialize(GameManager manager)
    {
        gameManager = manager;
    }

    /// <summary>
    /// Update is called once per frame.
    /// Moves the object downward.
    /// </summary>
    protected virtual void Update()
    {
        // Move the object down
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Destroy if it goes below the screen
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Abstract method - each derived class defines what happens when caught.
    /// </summary>
    public abstract void OnCaught();
}