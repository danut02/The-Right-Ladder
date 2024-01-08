using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class GameVolume : MonoBehaviour
{
    
    public Slider volumeSlider;
    public TMP_Text volumeText; // Reference to the Text UI element
    public AudioMixer mixer;
    
    private void Start()
    {
        float initialVolume = GetSavedVolume();
        SetVolume(initialVolume);
        volumeSlider.value = initialVolume;
        UpdateVolumeText(initialVolume);
        // Add an event listener to the slider's OnValueChanged event to update the volume
        volumeSlider.onValueChanged.AddListener(AdjustVolume);
    }
    
    public void AdjustVolume(float volume)
    {
        SetVolume(volume);
        SaveVolume(volume);
        UpdateVolumeText(volume);
    }

    private void SetVolume(float volume)
    {
        mixer.SetFloat("GameVol", Mathf.Log10(volume) * 20);
    }

    private float GetSavedVolume()
    {
        if (PlayerPrefs.HasKey("GameVol"))
        {
            return PlayerPrefs.GetFloat("GameVol");
        }
        else
        {
            return 0.2f; // Set an initial volume if there's no saved value
        }
    }

    private void SaveVolume(float volume)
    {
        PlayerPrefs.SetFloat("GameVol", volume);
        PlayerPrefs.Save();
    }

    private void UpdateVolumeText(float volume)          // Update the Text UI element to display the volume percentage
    {
        int volumePercentage = Mathf.RoundToInt(volume * 100);
        if (volumeText != null)
        {
            volumeText.text = volumePercentage + "%";
        }
    }
}
