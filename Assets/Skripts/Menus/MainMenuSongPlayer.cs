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
        //Pārbauda vai ir vērtības priekš peles ātruma, mūzikas un skaņas skaļuma, ja nē tad uzliek uz noklusējuma vērtību

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
        //Dabū mūzikas skaļumu
        float soundVolume = PlayerPrefs.GetFloat("Music"); 
        audioSource.volume = soundVolume;
        //Dabū mūZikas laiku, ja ir vajadzīgs un tad to iestata
        float savedTime = PlayerPrefs.GetFloat("MusicTime");
        audioSource.time = savedTime;
        
    }

    private void Update()
    {
        //Ja atrodas iestatījumos mūzika dabū skaļumu visu laiku, lai varētu dzīrdēt izmaiņas, kad maina skaļumu
        if (SceneManager.GetActiveScene().name == "Settings")
        {
             float soundVolume = PlayerPrefs.GetFloat("Music");
             audioSource.volume = soundVolume;

        }
    }

    private void Start()
    {
        //Sāk mūzikas atskaņošanu
        audioSource.Play();
    }
    //Kad izlēdz izdara metodi
    private void OnDisable()
    {
        SaveMusicTime();
    }
    //Saglabā mūzikas laiku
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
