using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject coletavelPrefab;
    public Transform playerTransform;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    public float spawnAreaMinX = -13f;
    public float spawnAreaMaxX = 32f;
    public float spawnAreaMinY = -8f;
    public float spawnAreaMaxY = 8f;
    public float minSpawnDistance = 5f;

    private float elapsedTime = 0f;
    private int currentScore = 0;
    private int currentEnemies = 0;
    public int maxEnemies = 7;
    private int enemiesToSpawn = 0;

    private bool gameEnded = false;

    private Vector2[] wallPositions = new Vector2[]
    {
        new Vector2(18.78f, -0.42f),
        new Vector2(29.07f, -0.30f),
        new Vector2(26.02f, -0.345f)
    };

    private float wallAvoidRadius = 1f;

    private float enemySpeed = 5f;
    private float speedIncreaseInterval = 10f;
    private float speedTimer = 0f;

    public void AddScore(int value)
    {
        currentScore += value;
        UpdateScoreText();

        if (currentEnemies + enemiesToSpawn < maxEnemies)
        {
            enemiesToSpawn++;
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    public void OnEnemyDestroyed()
    {
        currentEnemies = Mathf.Max(0, currentEnemies - 1);
    }

    public void OnFuelEmpty()
    {
        if (gameEnded) return;

        SaveRunTime(); // Salva ANTES de mudar de cena
        gameEnded = true;
        Invoke("GoToGameWin", 0.5f);
    }

    public void SpawnColetavel()
    {
        Vector2 randomPos;
        int attempts = 0;
        bool valid;

        do
        {
            float x = Random.Range(spawnAreaMinX, spawnAreaMaxX);
            float y = Random.Range(spawnAreaMinY, spawnAreaMaxY);
            randomPos = new Vector2(x, y);

            valid = true;

            foreach (Vector2 wallPos in wallPositions)
            {
                if (Vector2.Distance(randomPos, wallPos) < wallAvoidRadius)
                {
                    valid = false;
                    break;
                }
            }

            attempts++;
        }
        while (!valid && attempts < 10);

        Instantiate(coletavelPrefab, randomPos, Quaternion.identity);
    }

    void Update()
    {
        if (gameEnded) return;

        elapsedTime += Time.deltaTime;
        speedTimer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if (timerText != null)
        {
            timerText.text = $"Tempo: {minutes:00}:{seconds:00}";
        }

        if (speedTimer >= speedIncreaseInterval)
        {
            enemySpeed *= 1.1f;
            speedTimer = 0f;
        }

        GameObject[] coletaveis = GameObject.FindGameObjectsWithTag("Coletavel");
        if (coletaveis.Length == 0)
        {
            SaveRunTime(); // Salva ANTES de mudar de cena
            gameEnded = true;
            Invoke("GoToGameWin", 0.5f);
        }

        while (enemiesToSpawn > 0 && currentEnemies < maxEnemies)
        {
            SpawnEnemy();
            enemiesToSpawn--;
        }
    }

    void SaveRunTime()
    {
        PlayerPrefs.SetFloat("LastRunTime", elapsedTime);

        float bestTime = PlayerPrefs.GetFloat("HighScoreTime", 0f);
        if (elapsedTime > bestTime) // ⬅️ quanto MAIOR melhor agora
        {
            PlayerPrefs.SetFloat("HighScoreTime", elapsedTime);
        }

        PlayerPrefs.SetInt("LastScore", currentScore);

        int bestScore = PlayerPrefs.GetInt("HighScorePoints", 0);
        if (currentScore > bestScore)
        {
            PlayerPrefs.SetInt("HighScorePoints", currentScore);
        }

        PlayerPrefs.Save();
    }

    void GoToGameWin()
    {
        SceneManager.LoadScene("GameWin");
    }

    void SpawnEnemy()
    {
        Vector2 randomPos;
        int attempts = 0;

        do
        {
            float x = Random.Range(spawnAreaMinX, spawnAreaMaxX);
            float y = Random.Range(spawnAreaMinY, spawnAreaMaxY);
            randomPos = new Vector2(x, y);
            attempts++;
        }
        while (Vector2.Distance(randomPos, playerTransform.position) < minSpawnDistance && attempts < 10);

        GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
        enemy.GetComponent<EnemyMovement>().SetSpeed(enemySpeed);
        currentEnemies++;
    }
}
