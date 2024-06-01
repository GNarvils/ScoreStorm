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
        //Dabū pogas skaņas skaļuma vertibas.
        audioSource = GetComponent<AudioSource>();
        float volume = PlayerPrefs.GetFloat("Sound", 1.0f);
        audioSource.volume = volume;
    }

    //Metode, kas spēle pogas skaņu.
    public void PlayButtonSound()
    {
            audioSource.PlayOneShot(buttonSound);      
    }

    private void Update()
    {
        //Ja atrodas iestatījuma ainā tad, visu laiku dabū skaņas vērtība, ja varētu dzirdēt izmaiņas.
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
