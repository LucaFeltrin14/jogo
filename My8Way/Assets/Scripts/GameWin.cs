using UnityEngine;
using TMPro;

public class GameWin : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI lastScoreText;
    public TextMeshProUGUI bestScoreText;

    void Start()
    {
        float bestTime = PlayerPrefs.GetFloat("HighScoreTime", 0f);
        float lastRunTime = PlayerPrefs.GetFloat("LastRunTime", 0f);

        int bestMin = Mathf.FloorToInt(bestTime / 60f);
        int bestSec = Mathf.FloorToInt(bestTime % 60f);
        highScoreText.text = $"Melhor Tempo: {bestMin:00}:{bestSec:00}";

        int runMin = Mathf.FloorToInt(lastRunTime / 60f);
        int runSec = Mathf.FloorToInt(lastRunTime % 60f);
        currentTimeText.text = $"Seu Tempo: {runMin:00}:{runSec:00}";

        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        int highScore = PlayerPrefs.GetInt("HighScorePoints", 0);

        lastScoreText.text = $"Seu Score: {lastScore}";
        bestScoreText.text = $"Recorde de Score: {highScore}";
    }
}
