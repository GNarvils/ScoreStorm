using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //Pēc 0.2 sekundes pāriet uz pirmsspēles ainu
    public void PreGame()
    {
        StartCoroutine(ChangeSceneAfterDelay("PreGame", 0.2f));
    }
    //Pēc 0.2 sekundes pāriet uz iestatījumu ainu
    public void Settings()
    {
        StartCoroutine(ChangeSceneAfterDelay("Settings", 0.2f));
    }
    //Pēc 0.2 sekundes pāriet uz galvenās izvēlnes ainu
    public void MainMenu()
    {
        StartCoroutine(ChangeSceneAfterDelay("HomeScreen", 0.2f));
    }

    //Pēc vienas sekundes iziet ārā no spēles
    public void doExitGame()
    {
        StartCoroutine(ExitAfterDelay(1f));
    }

    //Pēc 0.2 sekundes pāriet uz spēles ainu
    public void Game()
    {
        StartCoroutine(ChangeSceneAfterDelay("Game", 0.2f));
    }

    //Metode, kas izdara pāriešanu uz citu ainu funkciju
    private IEnumerator ChangeSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    //Mētode, kas iziet ārā no spēles
    private IEnumerator ExitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.Quit();
    }
}
