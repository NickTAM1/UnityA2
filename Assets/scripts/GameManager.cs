using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    [SerializeField]
    [Tooltip("Game time in seconds")]
    private float gameTime = 120f;

    [Header("UI References")]
    [SerializeField]
    [Tooltip("Text element for displaying score")]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    [Tooltip("Text element for displaying remaining lives")]
    private TextMeshProUGUI livesText;

    [SerializeField]
    [Tooltip("Text element for displaying timer")]
    private TextMeshProUGUI timerText;

    [SerializeField]
    [Tooltip("Panel to show when game ends")]
    private GameObject gameOverPanel;

    [SerializeField]
    [Tooltip("Text to show final score")]
    private TextMeshProUGUI finalScoreText;

    // Game state variables
    private int currentScore = 0;
    private int currentLives;
    private float spawnTimer = 0f;
    private float currentTime;
    private bool isGameOver = false;

    void Start()
    {
        currentLives = startingLives;
        currentTime = gameTime;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateUI();
    }


    void Update()
    {
        if (isGameOver) return;

        // Update timer
        currentTime -= Time.deltaTime;

        // Check if time is up
        if (currentTime <= 0)
        {
            currentTime = 0;
            GameOver();
            return;
        }

        // Update spawn timer
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnRate)
        {
            SpawnObject();
            spawnTimer = 0f;
        }

        UpdateUI();
    }

    /// <summary>
    /// Spawns a game object at a random horizontal position within a specified range.
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

    /// <summary>
    /// Removes lives
    /// </summary>
    /// <param name="damage"></param>
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
    /// Updates the UI elements with current score, lives, and time
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

        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Ceil(currentTime);
        }
    }

    /// <summary>
    /// Shows game over panel with final score
    /// </summary>
    private void GameOver()
    {
        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Show final score
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + currentScore;
        }
    }

    /// <summary>
    /// Restarts the game - connect to button
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene("MiniGame");
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

}
