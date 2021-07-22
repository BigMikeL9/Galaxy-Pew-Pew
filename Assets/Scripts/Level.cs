using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{

    [SerializeField] float delayToLoadGameOver = 2;

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene()
    {
        // int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(1);
        FindObjectOfType<GameSession>().ScoreReset();
    }

    public void LoadGameOver()
    {
        StartCoroutine(LoadGameOverDelay());
    }

    private IEnumerator LoadGameOverDelay()
    {
        yield return new WaitForSeconds(delayToLoadGameOver);
        SceneManager.LoadScene("GameOver");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
