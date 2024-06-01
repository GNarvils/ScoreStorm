using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip player1Song;
    public AudioClip player2Song;
    private AudioSource audioSource;
    private GameObject uiGameObject;
    private GameTime gameTimeScript;
    private PlayerHealth playerHealthScript;

    void Start()
    {
        //Dabū komponentus un vajadzīgās vērtības
        audioSource = GetComponent<AudioSource>();
        float savedVolume = PlayerPrefs.GetFloat("Music", 1f);
        audioSource.volume = savedVolume;
        int selectedPlayer = PlayerPrefs.GetInt("SelectedPlayer", 1);
        if (selectedPlayer == 1)
        {
            PlayPlayer1Song();
        }
        else if (selectedPlayer == 2)
        {
            PlayPlayer2Song();
        }
        uiGameObject = GameObject.Find("UI");

        GameObject playerObject = null;
        gameTimeScript = uiGameObject.GetComponent<GameTime>();
        if (PlayerPrefs.GetInt("SelectedPlayer", 1) == 1)
        {
            playerObject = GameObject.Find("Player_1");
        }
        else
        {
            playerObject = GameObject.Find("Player_2");
        }

        playerHealthScript = playerObject.GetComponent<PlayerHealth>();

    }
    void Update()
    {
        //Ja spēlētājs ir miris aptura mūziku
        if (playerHealthScript != null && playerHealthScript.playerHealth <= 0)
            StopMusic();
        //Ja laiks beidzās aptura mūziku vai spēle ir beigusies
        if (gameTimeScript != null && (gameTimeScript.gameIsOver || gameTimeScript.timeIsUp))
            StopMusic();

    }
    //Spēlē spēlētāja 1 mūziku
    void PlayPlayer1Song()
    {
        if (audioSource.clip != player1Song)
        {
            audioSource.clip = player1Song;
            audioSource.Play();
        }
    }
    //Spēlē spēlētāja 2 mūziku
    void PlayPlayer2Song()
    {
        if (audioSource.clip != player2Song)
        {
            audioSource.clip = player2Song;
            audioSource.Play();
        }
    }
    //Apstādina mūziku
    void StopMusic()
    {
        audioSource.Stop();
    }
}
