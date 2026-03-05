using UnityEngine;

public class GameStartController : MonoBehaviour
{
    public GameObject starterScreen;
    public GameObject enemyFleet;

    public float screenDuration = 3f; 
    
    private bool screenDismissed = false;
    private float timer = 0f;
    private bool gameStarted = false;

    void Start()
    {
        if (starterScreen != null)
            starterScreen.SetActive(true);

        if (enemyFleet != null)
            enemyFleet.SetActive(false);
    }

    void Update()
    {
        if (gameStarted) return;

        timer += Time.deltaTime;

        bool clicked = UnityEngine.InputSystem.Mouse.current != null &&
                       UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame;

        if (!screenDismissed && (timer >= screenDuration || clicked))
        {
            DismissScreen();
            StartGame();
        }
    }

    void DismissScreen()
    {
        screenDismissed = true;

        if (starterScreen != null)
            starterScreen.SetActive(false);
    }

    void StartGame()
    {
        gameStarted = true;

        if (enemyFleet != null)
            enemyFleet.SetActive(true);
    }
}