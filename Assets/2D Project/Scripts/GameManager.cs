using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public EnemyFleet enemyFleet;

    public string creditsSceneName = "Credits";
    public string mainMenuSceneName = "MainMenu";

    public int winScore = 990;

    private int enemiesRemaining;
    private int totalScore;
    private bool ending = false;

    void Start()
    {
        if (enemyFleet != null)
            enemiesRemaining = enemyFleet.GetComponentsInChildren<Enemy>(true).Length;
        else
            enemiesRemaining = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;

        totalScore = 0;
        
        Enemy.onEnemyDied += OnEnemyDied;
        Player.onPlayerDied += OnPlayerDied;
    }

    void OnDestroy()
    {
        Enemy.onEnemyDied -= OnEnemyDied;
        Player.onPlayerDied -= OnPlayerDied;
    }

    void OnEnemyDied(int score)
    {
        if (ending) return;

        enemiesRemaining--;
        totalScore += score;
        
        if (enemiesRemaining <= 0)
        {
            EndToCredits();
            return;
        }

        if (totalScore >= winScore)
        {
            EndToCredits();
            return;
        }
    }

    void OnPlayerDied()
    {
        if (ending) return;

        EndToCredits();
    }

    void EndToCredits()
    {
        if (ending) return;
        ending = true;

        SceneManager.LoadScene(creditsSceneName);
    }
}