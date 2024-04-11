using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionOption : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; 

    private Resolution[] resolutions;

    void Start()
    {
        // Dabū visas iespējamās ekrāna rezolūcijas priekš lietotāja ekrāna
        resolutions = Screen.resolutions;

        // Izdzēš visas opcijas
        resolutionDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(option);
            options.Add(optionData);

            // Pārbauda vai rezolūcija nav tāda pati kā ekrāna rezolūcija
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Pievieno opcijas
        resolutionDropdown.AddOptions(options);

        // Iespējo rezolūciju
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
