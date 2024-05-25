using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSongPlayer : MonoBehaviour
{
    public AudioClip mainMenuMusic;
    private AudioSource audioSource;

    private void Awake()
    {

        if (!PlayerPrefs.HasKey("Sensitivity"))
        {
            PlayerPrefs.SetFloat("Sensitivity", 1f);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetFloat("Music", 1f);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetFloat("Sound", 1f);
            PlayerPrefs.Save();
        }
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource nav atrasts.");
            return;
        }

        audioSource.clip = mainMenuMusic;
        audioSource.loop = true;
        float soundVolume = PlayerPrefs.GetFloat("Music"); 
        audioSource.volume = soundVolume;
        float savedTime = PlayerPrefs.GetFloat("MusicTime");
        audioSource.time = savedTime;
        
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Settings")
        {
             float soundVolume = PlayerPrefs.GetFloat("Music");
             audioSource.volume = soundVolume;

        }
    }

    private void Start()
    {
        audioSource.Play();
    }
    private void OnDisable()
    {
        SaveMusicTime();
    }

    private void SaveMusicTime()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            PlayerPrefs.SetFloat("MusicTime", audioSource.time);
            PlayerPrefs.Save();
            Debug.Log("Saglabātais laiks: " + audioSource.time);
        }
    }
}
