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
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource nav atrasts.");
            return;
        }

        audioSource.clip = mainMenuMusic;
        audioSource.loop = true;
        if (PlayerPrefs.HasKey("Music"))
        {
            float soundVolume = PlayerPrefs.GetFloat("Music");
            Debug.Log("Mūzikas skaļums: " + soundVolume); 
            audioSource.volume = soundVolume;
        }
        else
        {
            Debug.LogWarning("PlayerPrefs atslēga mūzika nav atrasta");
        }
        if (PlayerPrefs.HasKey("MusicTime"))
        {
            float savedTime = PlayerPrefs.GetFloat("MusicTime");
            audioSource.time = savedTime;
            Debug.Log("Atsāk mūziku no: " + savedTime);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Settings")
        {
            if (PlayerPrefs.HasKey("Music"))
            {
                float soundVolume = PlayerPrefs.GetFloat("Music");
                Debug.Log("Mūzikas skaļums: " + soundVolume);
                audioSource.volume = soundVolume;
            }
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
