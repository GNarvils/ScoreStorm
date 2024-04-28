using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void PreGame()
    {
        SceneManager.LoadScene("PreGame", LoadSceneMode.Single);
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Single);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("HomeScreen", LoadSceneMode.Single);
    }
    public void doExitGame()
    {
        StartCoroutine(ExitAfterDelay(3f)); 
    }

    private IEnumerator ExitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.Quit();
    }
    public void Game()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
