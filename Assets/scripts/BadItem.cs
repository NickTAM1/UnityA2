using UnityEngine;

public class BadItem : FallingObject
{
    [Header("Damage Settings")]
    [SerializeField][Tooltip("Lives lost when caught by player")]
    private int damageAmount = 1;

    /// <summary>
    /// Called when player catches this item.
    /// Reduces player lives and destroys the object.
    /// </summary>
    public override void OnCaught()
    {
        // Reduce player lives
        if (gameManager != null)
        {
            gameManager.TakeDamage(damageAmount);
        }

        // Destroy this object
        Destroy(gameObject);
    }
}
