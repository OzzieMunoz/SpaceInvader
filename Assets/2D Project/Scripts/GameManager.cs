using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
       // todo - sign up for notification about enemy death 
       Enemy.onEnemyDied += OnEnemeyDied;
    }

    void OnDestroy()
    {
        Enemy.onEnemyDied -= OnEnemeyDied;
    }

    void OnEnemeyDied(float score)
    {
        Debug.Log($"Killed enemy worth {score}");
    }
}
