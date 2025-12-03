using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Prefabs for spawning - assign in inspector
    [Header("Prefabs")]
    [SerializeField]
    [Tooltip("Prefab for good items that increase score")]
    private GameObject goodItemPrefab;
    
    [SerializeField]
    [Tooltip("Prefab for bad items that reduce lives")]
    private GameObject badItemPrefab;

    // Spawn settings - adjustable in inspector
    [Header("Spawn Settings")]
    [SerializeField]
    [Tooltip("How often objects spawn (in seconds)")]
    private float spawnRate = 1.5f;
    
    [SerializeField]
    [Tooltip("Horizontal range for spawn positions")]
    private float spawnRangeX = 7f;
    
    [SerializeField]
    [Tooltip("Height where objects spawn")]
    private float spawnHeight = 6f;
    
    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("Probability of spawning bad items (0-1)")]
    private float badItemChance = 0.3f;

    // Game settings - adjustable in inspector
    [Header("Game Settings")]
    [SerializeField]
    [Tooltip("Number of lives at game start")]
    private int startingLives = 3;

    // UI References - assign in inspector
    [Header("UI References")]
    [SerializeField]
    [Tooltip("Text element for displaying score")]
    private Text scoreText;
    
    [SerializeField]
    [Tooltip("Text element for displaying remaining lives")]
    private Text livesText;
    
    [SerializeField]
    [Tooltip("Panel to show when game ends")]
    private GameObject gameOverPanel;

    // Private variables to track game state
    private int currentScore = 0;
    private int currentLives;
    private float spawnTimer = 0f;
    private bool isGameOver = false;

    
    void Start()
    {
        // Set starting lives
        currentLives = startingLives;

        // Hide game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Update UI
        UpdateUI();
    }

    
    void Update()
    {
        // Don't spawn if game is over
        if (isGameOver) return;

        // Update spawn timer
        spawnTimer += Time.deltaTime;

        // Spawn a new object when timer reaches spawn rate
        if (spawnTimer >= spawnRate)
        {
            SpawnObject();
            spawnTimer = 0f;
        }
    }

    /// <summary>
    /// Spawns a random falling object at the top of the screen.
    /// </summary>
    private void SpawnObject()
    {
        // Calculate random spawn position
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0f);

        // Decide if spawning good or bad item based on chance
        GameObject prefabToSpawn;
        if (Random.value < badItemChance)
        {
            prefabToSpawn = badItemPrefab;
        }
        else
        {
            prefabToSpawn = goodItemPrefab;
        }

        // Spawn the object
        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        // Initialize the falling object with reference to this manager
        FallingObject fallingObject = spawnedObject.GetComponent<FallingObject>();
        if (fallingObject != null)
        {
            fallingObject.Initialize(this);
        }
    }

    /// <summary>
    /// Adds points to the current score.
    /// Called by GoodItem when caught.
    /// </summary>
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateUI();
    }

    /// <summary>
    /// Reduces player lives.
    /// Called by BadItem when caught.
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        UpdateUI();

        // Check for game over
        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Updates the UI text elements.
    /// </summary>
    private void UpdateUI()
    {
        // Update score text
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }

        // Update lives text
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
    }

    /// <summary>
    /// Handles game over state.
    /// </summary>
    private void GameOver()
    {
        isGameOver = true;

        // Show game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Restarts the game.
    /// Called by restart button.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}