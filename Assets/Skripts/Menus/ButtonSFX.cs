using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSFX : MonoBehaviour
{
    public AudioClip buttonSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        float volume = PlayerPrefs.GetFloat("Sound", 1.0f);
        audioSource.volume = volume;
    }

    public void PlayButtonSound()
    {
            audioSource.PlayOneShot(buttonSound);      
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Settings")
        {
            if (PlayerPrefs.HasKey("Sound"))
            {
                float soundVolume = PlayerPrefs.GetFloat("Sound");
                audioSource.volume = soundVolume;
            }
        }
    }
}
