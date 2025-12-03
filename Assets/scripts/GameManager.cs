using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Controls the whole game - spawning, score, lives, game over
public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    [Tooltip("Prefab for good items that increase score")]
    private GameObject goodItemPrefab;

    [SerializeField]
    [Tooltip("Prefab for bad items that reduce lives")]
    private GameObject badItemPrefab;

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

    [Header("Game Settings")]
    [SerializeField]
    [Tooltip("Number of lives at game start")]
    private int startingLives = 3;

    [Header("UI References")]
    [SerializeField]
    [Tooltip("Text element for displaying score")]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    [Tooltip("Text element for displaying remaining lives")]
    private TextMeshProUGUI livesText;

    [SerializeField]
    [Tooltip("Panel to show when game ends")]
    private GameObject gameOverPanel;

    // Game state variables
    private int currentScore = 0;
    private int currentLives;
    private float spawnTimer = 0f;
    private bool isGameOver = false;

    // Runs at game start
    void Start()
    {
        currentLives = startingLives;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateUI();
    }

    // Runs every frame
    void Update()
    {
        if (isGameOver) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnRate)
        {
            SpawnObject();
            spawnTimer = 0f;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void SpawnObject()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0f);

        // Pick good or bad item
        GameObject prefabToSpawn;
        if (Random.value < badItemChance)
        {
            prefabToSpawn = badItemPrefab;
        }
        else
        {
            prefabToSpawn = goodItemPrefab;
        }

        // Create object and initialize it
        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        FallingObject fallingObject = spawnedObject.GetComponent<FallingObject>();
        if (fallingObject != null)
        {
            fallingObject.Initialize(this);
        }
    }

    /// <summary>
    /// Adds the specified number of points to the current score.
    /// </summary>
    /// <param name="points">The number of points to add to the current score. Must be a non-negative integer.</param>
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateUI();
    }

    // Removes lives
    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        UpdateUI();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Updates the user interface elements to reflect the current score and lives.
    /// </summary>
    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }

        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
    }

    /// <summary>
    /// Ends the current game session and displays the game over panel.
    /// </summary>
    private void GameOver()
    {
        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Restarts the current game by reloading the active scene.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene("MiniGame");
    }
}