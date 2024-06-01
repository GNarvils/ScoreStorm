using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionOption : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private int selectedResolutionIndex; // Saglabā atmiņā pēdējo izvēlēto rezolūciju indeksu

    void Start()
    {
        // Dabū visas iespējamās ekrāna rezolūcijas priekš lietotāja ekrāna
        resolutions = Screen.resolutions;

        // Izdzēš visas opcijas
        resolutionDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        HashSet<string> uniqueResolutions = new HashSet<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolutionKey = resolutions[i].width + " x " + resolutions[i].height;

            // Pārbauda, vai rezolūcija jau ir pievienota
            if (!uniqueResolutions.Contains(resolutionKey))
            {
                uniqueResolutions.Add(resolutionKey);

                // Pievieno rezolūciju dropdown opcijām
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(resolutionKey);
                options.Add(optionData);
            }
        }

        // Pievieno opcijas dropdown
        resolutionDropdown.AddOptions(options);

        // Saglabā pēdējo izvēlēto rezolūciju
        selectedResolutionIndex = PlayerPrefs.GetInt("SelectedResolutionIndex", 0);

        // Uzstāda dropdown uz saglabāto rezolūciju
        resolutionDropdown.value = selectedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Sākotnēji uzstāda saglabāto rezolūciju
        SetResolution(selectedResolutionIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < resolutionDropdown.options.Count)
        {
            string[] optionParts = resolutionDropdown.options[resolutionIndex].text.Split(' ');
            int width = int.Parse(optionParts[0]);
            int height = int.Parse(optionParts[2]);

            // Atrod atbilstošo rezolūciju un uzstāda to
            foreach (var resolution in resolutions)
            {
                if (resolution.width == width && resolution.height == height)
                {
                    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
                    Debug.Log("Rezolūcija uzstādīta uz: " + resolution.width + " x " + resolution.height);

                    // Saglabā jauno izvēlēto rezolūciju indeksu
                    selectedResolutionIndex = resolutionIndex;
                    PlayerPrefs.SetInt("SelectedResolutionIndex", selectedResolutionIndex);

                    break;
                }
            }
        }
        else
        {
            Debug.LogWarning("Nederīgs rezolūcijas indekss: " + resolutionIndex);
        }
    }
}