using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    Resolution[] resolutions;
    public TMP_Dropdown resolutionsDropdown;
    public TMP_Dropdown qualityDropdown;
    

    private void Start()
    {
       
        resolutions = Screen.resolutions;
        resolutionsDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 2;
        qualityDropdown.value=PlayerPrefs.GetInt("QualityIndex");
        PlayerPrefs.SetInt("QualityIndex",qualityDropdown.value);
       
        qualityDropdown.RefreshShownValue();
       
        //QualitySettings.SetQualityLevel(2);
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        if(resolutions == null || resolutionIndex < 0 || resolutionIndex >= resolutions.Length)
        {
            Debug.LogError("Invalid resolution or resolutions array is not initialized!");
            Debug.Log(resolutions);
            return;
        }

        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionWidth", resolution.width);
        PlayerPrefs.SetInt("ResolutionHeight", resolution.height);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("FullScreen", isFullscreen ? 1 : 0);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityIndex", qualityIndex);
        qualityDropdown.value=qualityIndex;
    }
}
