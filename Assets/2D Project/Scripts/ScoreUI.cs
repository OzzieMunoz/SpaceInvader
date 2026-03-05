using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text scoreValueText; 
    public TMP_Text highValueText;

    public int digits = 4;
    private int score;
    private int highScore;

    const string HighScoreKey = "HighScore";

    void Awake()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        score = 0;

        RefreshUI();
    }

    void OnEnable()
    {
        Enemy.onEnemyDied += OnEnemyDied;
    }

    void OnDisable()
    {
        Enemy.onEnemyDied -= OnEnemyDied;
    }

    void OnEnemyDied(float points)
    {
        score += Mathf.RoundToInt(points);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
        }

        RefreshUI();
    }

    void RefreshUI()
    {
        string format = "D" + digits;
        if (scoreValueText) scoreValueText.text = score.ToString(format);
        if (highValueText) highValueText.text = highScore.ToString(format);
    }
}