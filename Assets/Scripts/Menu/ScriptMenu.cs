using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class ScriptMenu : MonoBehaviour
{
    public void Start()
    {
        if (PlayerPrefs.HasKey("ResolutionWidth") && PlayerPrefs.HasKey("ResolutionHeight"))
        {
            var resolutionWidth = PlayerPrefs.GetInt("ResolutionWidth");
            var resolutionHeight = PlayerPrefs.GetInt("ResolutionHeight");
            Screen.SetResolution(resolutionWidth, resolutionHeight, Screen.fullScreen);
        }
        
        if (PlayerPrefs.HasKey("FullScreen"))
        {
            bool isFullScreen = PlayerPrefs.GetInt("FullScreen") == 1;
            Screen.fullScreen = isFullScreen;
        }
        if (PlayerPrefs.HasKey("QualityIndex"))
        {
            int qualityIndex = PlayerPrefs.GetInt("QualityIndex");
            QualitySettings.SetQualityLevel(qualityIndex);
        }
    }

    public void NewGameButton()
    {
        SoundManager.instance.source.Stop();
        SceneManager.LoadSceneAsync(0);
    }
    public void PlayMenu()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void LoadButton()
    {
        SoundManager.instance.source.Stop();
        SceneManager.LoadSceneAsync(0);
        PlayerPrefs.SetInt("LoadSavedGame", 1);
    }
    public void Options()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void Quit()
    {
        Application.Quit();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    public void Back()
    {
        SceneManager.LoadSceneAsync(1);
    }
}