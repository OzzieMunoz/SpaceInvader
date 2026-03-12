using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartController : MonoBehaviour
{
    public GameObject starterScreen;
    public GameObject enemyFleet;

    public string gameSceneName = "Schmup";

    private bool loading = false;

    void Start()
    {
        if (starterScreen != null)
            starterScreen.SetActive(true);
    }

    public void LoadGame()
    {

        if (loading) return;
        loading = true;

        if (starterScreen != null)
            starterScreen.SetActive(false);

        StartCoroutine(_LoadGame());
    }

    IEnumerator _LoadGame()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(gameSceneName);
        while (!loadOperation.isDone) yield return null;
    }
}