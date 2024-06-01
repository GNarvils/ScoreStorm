using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderOptions : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Slider musicSlider;
    public Slider soundSlider;
    void Start()
    {
        // Ja nav iestatītās vērtības vai iestatītās vērtības ir skaitlis, kas nav iespējam, tad iestata noklusējuma vērtības bīdņiem
        if (!PlayerPrefs.HasKey("Sensitivity") || PlayerPrefs.GetFloat("Sensitivity") == 0f)
        {
            PlayerPrefs.SetFloat("Sensitivity", 1f);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("Music") || PlayerPrefs.GetFloat("Music") < 0f)
        {
            PlayerPrefs.SetFloat("Music", 1f);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("Sound") || PlayerPrefs.GetFloat("Sound") < 0f)
        {
            PlayerPrefs.SetFloat("Sound", 1f);
            PlayerPrefs.Save();
        }


        // Dabū eksistējošās vērtības bīdņiem
        float sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        sensitivitySlider.value = sensitivity;

        float music = PlayerPrefs.GetFloat("Music", 1f);
        musicSlider.value = music;

        float sound = PlayerPrefs.GetFloat("Sound", 1f);
        soundSlider.value = sound;

        // Ja tiek kustināti izdara metodes, kas nomaina vērtību.
        sensitivitySlider.onValueChanged.AddListener(delegate { OnSensitivityChanged(); });

        musicSlider.onValueChanged.AddListener(delegate { OnMusicChanged(); });

        soundSlider.onValueChanged.AddListener(delegate { OnSoundChanged(); });
    }
    //Maina un saglabā peles kustināšanas ātruma vērtību
    void OnSensitivityChanged()
    {
        float newSensitivity = sensitivitySlider.value;
        PlayerPrefs.SetFloat("Sensitivity", newSensitivity);
        PlayerPrefs.Save();
    }
    //Maina un saglabā mūzikas skaļuma vērtību
    void OnMusicChanged()
    {
        float newMusic = musicSlider.value;
        PlayerPrefs.SetFloat("Music", newMusic);
        PlayerPrefs.Save();
    }
    //Maina un saglabā spēles skaņas skaļuma vērtību
    void OnSoundChanged()
    {
        float newSound = soundSlider.value;
        PlayerPrefs.SetFloat("Sound", newSound);
        PlayerPrefs.Save();
    }
}

