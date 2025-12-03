using UnityEngine;

public class GoodItem : FallingObject
{
    [Header("Point Settings")]
    [SerializeField]
    [Tooltip("Points awarded when caught by player")]
    private int pointValue = 10;

    /// <summary>
    /// Called when player catches this item.
    /// Adds points and destroys the object.
    /// </summary>
    public override void OnCaught()
    {
        // Add points to the game manager
        if (gameManager != null)
        {
            gameManager.AddScore(pointValue);
        }

        // Destroy this object
        Destroy(gameObject);
    }
}
