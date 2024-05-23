using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void PreGame()
    {
        StartCoroutine(ChangeSceneAfterDelay("PreGame", 0.2f));
    }

    public void Settings()
    {
        StartCoroutine(ChangeSceneAfterDelay("Settings", 0.2f));
    }

    public void MainMenu()
    {
        StartCoroutine(ChangeSceneAfterDelay("HomeScreen", 0.2f));
    }

    public void doExitGame()
    {
        StartCoroutine(ExitAfterDelay(3f));
    }

    public void Game()
    {
        StartCoroutine(ChangeSceneAfterDelay("Game", 0.2f));
    }

    private IEnumerator ChangeSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private IEnumerator ExitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.Quit();
    }
}
